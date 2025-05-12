<template>
  <div class="schedule-page">
    <NavBar />

    <div class="schedule-header">
      <label for="date-picker">Выберите дату:</label>
      <input
        id="date-picker"
        type="date"
        v-model="selectedDate"
        @change="fetchWeek"
      />
      <button @click="prevWeek">‹</button>
      <button @click="nextWeek">›</button>
      <button class="add-btn" @click="openModal()">+ Добавить занятие</button>
    </div>

    <table class="schedule-table">
      <thead>
        <tr>
          <th>Номер кабинета</th>
          <th
            v-for="day in weekDays"
            :key="day"
          >{{ formatDay(day) }}</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="room in classrooms" :key="room">
          <td>{{ room }}</td>
          <td v-for="day in weekDays" :key="room + day">
            <div
              v-for="item in itemsBy(room, day)"
              :key="item.id"
              class="lesson-card"
              @click="openModal(item)"
            >
              <div class="time">{{ item.startTime }}–{{ item.endTime }}</div>
              <div class="info">
                {{ item.subjectName }}<br/>
                {{ item.groupName }} · {{ item.teacherName }}
              </div>
            </div>
          </td>
        </tr>
      </tbody>
    </table>

    <div
      v-if="modalVisible"
      class="modal-backdrop"
      @click.self="closeModal"
    >
      <div class="modal">
        <h3>{{ form.id ? 'Редактировать занятие' : 'Добавить занятие' }}</h3>
        <form @submit.prevent="saveItem">
          <label>День недели:
            <select v-model="form.dayOfWeek" required>
              <option
                v-for="(label, index) in ['Пн','Вт','Ср','Чт','Пт','Сб','Вс']"
                :key="index"
                :value="index + 1"
              >
                {{ label }}
              </option>
            </select>
          </label>

          <label>Время начала:
            <input type="time" v-model="form.startTime" required />
          </label>

          <label>Время конца:
            <input type="time" v-model="form.endTime" required />
          </label>

          <label>Кабинет:
            <input v-model="form.classroom" required />
          </label>

          <label>Предмет:
            <select v-model="form.subjectId" required>
              <option
                v-for="s in subjects"
                :key="s.id"
                :value="s.id"
              >{{ s.name }}</option>
            </select>
          </label>

          <label>Группа:
            <select v-model="form.groupId" required>
              <option
                v-for="g in groups"
                :key="g.id"
                :value="g.id"
              >{{ g.name }}</option>
            </select>
          </label>

          <label>Преподаватель:
            <select v-model="form.teacherId" required>
              <option
                v-for="t in teachers"
                :key="t.id"
                :value="t.id"
              >{{ t.name }}</option>
            </select>
          </label>

          <label class="checkbox">
            <input type="checkbox" v-model="form.isOnline" />
            Онлайн
          </label>

          <div class="modal-actions">
            <button type="submit">Сохранить</button>
            <button
              v-if="form.id"
              type="button"
              class="delete-btn"
              @click="deleteItem"
            >Удалить</button>
            <button
              type="button"
              @click="closeModal"
            >Отмена</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import axios from 'axios'
import NavBar from '../components/NavBar.vue'

const selectedDate = ref(new Date().toISOString().substr(0,10))
const scheduleId = ref(null)
const items = ref([])
const classrooms = ref([1,2,3,4,5,6])  // или загрузить из API
const subjects = ref([])
const groups = ref([])
const teachers = ref([])

const modalVisible = ref(false)
const form = ref({})

const weekDays = computed(() => {
  const d = new Date(selectedDate.value)
  const shift = (d.getDay() + 6) % 7
  d.setDate(d.getDate() - shift)
  return Array.from({ length: 7 }, (_, i) => {
    const dd = new Date(d)
    dd.setDate(d.getDate() + i)
    return dd.toISOString().substr(0,10)
  })
})

function formatDay(iso) {
  const d = new Date(iso)
  return d.toLocaleDateString('ru-RU', { weekday: 'short', day: 'numeric', month: 'numeric' })
}

function itemsBy(room, day) {
  const dow = new Date(day).getDay()
  return items.value.filter(i => i.classroom == room && i.dayOfWeek == dow)
}

async function ensureSchedule() {
  if (!scheduleId.value) {
    const dto = { startYear: new Date().getFullYear(), endYear: new Date().getFullYear()+1 }
    const res = await axios.post('/api/schedule/create', dto)
    scheduleId.value = res.data.id
  }
}

async function fetchWeek() {
  await ensureSchedule()
  // получаем расписание на год (если нужно)
  await axios.post('/api/schedule', { startYear:0, endYear:0 })
    .then(r => scheduleId.value = r.data.id)

  const payload = {
    scheduleId: scheduleId.value,
    startDate: weekDays.value[0],
    endDate:   weekDays.value[6]
  }
  await axios.post('/api/schedule/getItems', payload)
    .then(r => items.value = r.data)
}

async function loadLookups() {
  const [subRes, grpRes, tchRes] = await Promise.all([
    axios.get('/api/subjects'),
    axios.get('/api/groups'),
    axios.get('/api/users/teachers')
  ])
  subjects.value = subRes.data
  groups.value   = grpRes.data
  teachers.value = tchRes.data
}

function prevWeek() {
  const d = new Date(selectedDate.value)
  d.setDate(d.getDate() - 7)
  selectedDate.value = d.toISOString().substr(0,10)
  fetchWeek()
}
function nextWeek() {
  const d = new Date(selectedDate.value)
  d.setDate(d.getDate() + 7)
  selectedDate.value = d.toISOString().substr(0,10)
  fetchWeek()
}

function openModal(item = {}) {
  form.value = { ...item }
  modalVisible.value = true
}
function closeModal() {
  modalVisible.value = false
}

async function saveItem() {
  if (form.value.id) {
    await axios.post(`/api/schedule/updateItem/${form.value.id}`, form.value)
  } else {
    await axios.post('/api/schedule/addItem', { ...form.value, scheduleId: scheduleId.value })
  }
  closeModal()
  fetchWeek()
}

async function deleteItem() {
  await axios.delete(`/api/schedule/deleteItem/${form.value.id}`)
  closeModal()
  fetchWeek()
}

onMounted(() => {
  fetchWeek()
  loadLookups()
})
</script>

<style scoped>
.schedule-page {
  background: #f2f5f9;
  min-height: 100vh;
}
.schedule-header {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 16px;
  background: #fff;
}
.schedule-header label {
  font-weight: bold;
}
.schedule-header input[type="date"] {
  padding: 4px;
}
.schedule-header button {
  padding: 4px 8px;
  cursor: pointer;
}
.add-btn {
  margin-left: auto;
  background: #28a745;
  color: #fff;
  border: none;
  padding: 6px 12px;
  border-radius: 4px;
}
.schedule-table {
  width: 95%;
  margin: 0 auto;
  border-collapse: collapse;
}
.schedule-table th,
.schedule-table td {
  border: 1px solid #ccc;
  padding: 8px;
  vertical-align: top;
}
.lesson-card {
  background: #ffdca8;
  margin: 4px 0;
  padding: 6px;
  border-radius: 4px;
  cursor: pointer;
}
.lesson-card .time {
  font-weight: bold;
}
.modal-backdrop {
  position: fixed;
  top: 0; left: 0; right: 0; bottom: 0;
  background: rgba(0,0,0,0.5);
  display: flex; align-items: center; justify-content: center;
}
.modal {
  background: #fff;
  padding: 20px;
  border-radius: 8px;
  width: 300px;
}
.modal h3 {
  margin-top: 0;
  text-align: center;
}
.modal form label {
  display: block;
  margin: 8px 0;
}
.modal .checkbox {
  display: flex;
  align-items: center;
  gap: 4px;
}
.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 8px;
  margin-top: 12px;
}
.modal-actions button {
  padding: 6px 12px;
  cursor: pointer;
}
.delete-btn {
  background: #dc3545;
  color: #fff;
  border: none;
}
</style>
