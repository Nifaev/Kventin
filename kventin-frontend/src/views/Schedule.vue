<template>
  <div class="schedule-page">
    <NavBar />

    <div class="container">
      <h1 class="page-title">Шаблон расписания</h1>

      <!-- Селектор учебного года -->
      <section class="year-selector">
        <label>
          Учебный год:
          <select v-model="selectedYearPair">
            <option
              v-for="pair in yearPairs"
              :key="pair.value"
              :value="pair.value"
            >
              {{ pair.label }}
            </option>
          </select>
        </label>
        <button class="btn-load" @click="loadSchedule">Загрузить</button>
      </section>

      <!-- Шапка шаблона -->
      <section class="schedule-header" v-if="!loadingTemplate">
        <div v-if="!currentSchedule" class="header-row">
          <span>Для {{ currentPairLabel }} шаблон не найден.</span>
          <button class="btn-add-small" @click="createSchedule">
            Создать расписание
          </button>
        </div>
        <div v-else class="header-row">
          <span>Добавить расписание: {{ currentSchedule.startYear }}{{ currentSchedule.endYear }}:</span>
          <button class="btn-edit" @click="createSchedule">
            Добавить
          </button>
        </div>
      </section>
      <div v-else class="loading">Загрузка...</div>

      <!-- Список элементов расписания -->
      <section v-if="currentSchedule" class="items-section">
        <header class="items-header">
          <h2>Расписание</h2>
          <button class="btn-add" @click="openItemModal()">Добавить элемент</button>
        </header>
        <table class="items-table">
          <thead>
            <tr>
              <th>Время</th>
              <th>Предмет</th>
              <th>Преподаватель</th>
              <th>Группа</th>
              <th>Кабинет</th>
              <th>Онлайн?</th>
              <th>Действия</th>
            </tr>
          </thead>
          <tbody>
            <!-- Если нет ни одного элемента -->
            <tr v-if="!items.length">
              <td colspan="8" class="empty">Пока нет ни одного элемента.</td>
            </tr>

            <!-- Группируем по дням недели -->
            <template v-else>
              <template v-for="group in groupedItems" :key="group.day">
                <!-- Заголовок дня -->
                <tr class="day-header">
                  <td colspan="8">{{ group.day }}</td>
                </tr>
                <!-- Строки с парами -->
                <tr
                  v-for="it in group.items"
                  :key="it.scheduleItemId"
                >
                  <td>{{ it.startTime }}–{{ it.endTime }}</td>
                  <td>{{ it.subjectName }}</td>
                  <td>{{ it.teacher.fullName }}</td>
                  <td>{{ it.groupName }}</td>
                  <td>{{ it.classroom }}</td>
                  <td>{{ it.isOnline ? 'Да' : 'Нет' }}</td>
                  <td>
                    <button class="btn-small" @click="openItemModal(it)">✎</button>
                    <button class="btn-small danger" @click="deleteItem(it.scheduleItemId)">×</button>
                  </td>
                </tr>
              </template>
            </template>
          </tbody>
        </table>
      </section>
    </div>

    <!-- Модалки -->
    <CreateScheduleModal
      v-if="showCreateSchedule"
      :initial="currentSchedule"
      @close="showCreateSchedule = false"
      @saved="onTemplateSaved"
    />
    <EditItemModal
      v-if="showItemModal"
      :initial="editingItem"
      :schedule-id="currentSchedule.scheduleId"
      :teachers="teachers"
      :subjects="subjects"
      :groups="groups"
      @close="showItemModal = false"
      @saved="onItemSaved"
    />
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import axios from 'axios'
import NavBar              from '../components/NavBar.vue'
import CreateScheduleModal from '../components/CreateScheduleModal.vue'
import EditItemModal       from '../components/EditItemModal.vue'

const yearPairs         = ref([])
const selectedYearPair = ref(null)

const currentSchedule = ref(null)
const items           = ref([])
const loadingTemplate = ref(false)

const teachers      = ref([])
const subjects      = ref([])
const groups        = ref([])

const showCreateSchedule = ref(false)
const showItemModal      = ref(false)
const editingItem        = ref(null)

// Жёстко заданный порядок дней:
const weekdays = [
  'Понедельник','Вторник','Среда',
  'Четверг','Пятница','Суббота','Воскресенье'
]

const currentPair = computed(() =>
  yearPairs.value.find(p => p.value === selectedYearPair.value)
)
const currentPairLabel = computed(() => currentPair.value?.label || '')

// Новая группировка
const groupedItems = computed(() => {
  const map = items.value.reduce((acc, it) => {
    const day = it.dayOfWeek || '—'
    if (!acc[day]) acc[day] = []
    acc[day].push(it)
    return acc
  }, {})
  return Object.entries(map)
    .map(([day, arr]) => ({ day, items: arr }))
    .sort((a, b) => {
      const i1 = weekdays.indexOf(a.day)
      const i2 = weekdays.indexOf(b.day)
      if (i1 === -1 && i2 === -1) return 0
      if (i1 === -1) return 1
      if (i2 === -1) return -1
      return i1 - i2
    })
})

async function loadYearPairs() {
  try {
    const { data: years } = await axios.get('/api/schedule/getSchoolYears')
    yearPairs.value = years.map(pairStr => {
      const [start, end] = pairStr.split('/').map(Number)
      return { value: pairStr, label: pairStr, start, end }
    })
    if (yearPairs.value.length) {
      selectedYearPair.value = yearPairs.value[0].value
    }
  } catch (e) {
    console.error('Не удалось загрузить учебные года', e)
  }
}

async function loadRefs() {
  const [tRes, sRes, gRes] = await Promise.all([
    axios.get('/api/user/getAllTeachers'),
    axios.get('/api/subject/all'),
    axios.get('/api/studyGroup/all'),
  ])
  teachers.value = tRes.data
  subjects.value = sRes.data
  groups.value   = gRes.data
}

async function loadSchedule() {
  if (!currentPair.value) return
  loadingTemplate.value = true
  currentSchedule.value = null
  items.value = []
  try {
    const { start, end } = currentPair.value
    const { data } = await axios.post('/api/schedule', { startYear: start, endYear: end })
    currentSchedule.value = data
    items.value = data.scheduleItems || []
  } catch (err) {
    if (!(err.response && err.response.status === 400)) {
      console.error(err)
    }
  } finally {
    loadingTemplate.value = false
  }
}

function createSchedule() {
  showCreateSchedule.value = true
}
function onTemplateSaved() {
  showCreateSchedule.value = false
  loadSchedule()
}

function openItemModal(item = null) {
  editingItem.value = item
    ? { ...item }
    : { dayOfWeek:'Понедельник', startTime:'08:00', endTime:'09:30',
        classroom:'', teacherId:null, subjectId:null, groupId:null, isOnline:false }
  showItemModal.value = true
}
function onItemSaved() {
  showItemModal.value = false
  loadSchedule()
}

async function deleteItem(id) {
  if (!confirm('Удалить элемент?')) return
  await axios.delete(`/api/schedule/deleteItem/${id}`)
  loadSchedule()
}

onMounted(async () => {
  await loadYearPairs()
  await loadRefs()
  await loadSchedule()
})
</script>

<style scoped>
.container {
  max-width: 900px;
  margin: auto;
}

.items-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 1em;
}

.items-table th,
.items-table td {
  padding: 8px 12px;
  border: 1px solid #ddd;
}

.items-table .day-header td {
  background-color: #f5f7fa;
  font-weight: bold;
  text-align: center;
}

.items-table .empty {
  text-align: center;
  color: #666;
}
/* ваш существующий CSS без изменений */
.schedule-page {
  position: relative; z-index: 0; min-height: 100vh;
}
.schedule-page::before {
  content: ""; position: absolute; inset: 0; z-index: -1;
  background: url('/images/background.png') center/cover no-repeat;
  opacity: 0.5;
}
.container {
  max-width: 1000px;
  margin: 0 auto;
}
.page-title {
  text-align: center;
  margin-bottom: 24px;
  font-size: 28px;
}
.year-selector {
  justify-content: center;
  gap: 12px;
  margin: 10px ;
}
.year-selector label {
  flex-direction: column;
  font-size: 14px;
}
.year-selector select {
  margin-top: 4px;
  padding: 6px;
  border: 1px solid #ccc;
  border-radius: 4px;
}
.btn-load {
  background: #7fcdd3b3;
  border: 1px solid rgb(0, 0, 0);
  color: #000000;
  padding: 8px 16px;
  border-radius: 6px;
  margin-left: 10px;
  width: 150px;
  cursor: pointer;
}
.btn-load:hover { background: #0069d9; }

.schedule-header {
  padding: 16px;
  border-radius: 8px;
}
.loading {
  text-align: center;
  padding: 16px;
  color: #666;
}

.items-section {
  background: #fff;
  padding: 16px;
  border-radius: 8px;
}
.items-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 12px;
}
.items-table {
  width: 100%;
  border-collapse: collapse;
}
.items-table th,
.items-table td {
  border: 1px solid #ddd;
  padding: 8px;
}
.empty {
  text-align: center;
  color: #888;
  font-style: italic;
}
.btn-add {
  background: #5cb85ca4;
  color: #000000;
  padding: 8px 14px;
  width: 150px;
  border: 1px solid rgb(0, 0, 0);
  border-radius: 6px;
  cursor: pointer;
}
.btn-edit {
  background: #7fcdd3b3;
  display: flex;
  color: #000000;
  padding: 8px 14px;
  border-radius: 6px;
  border: 1px solid rgb(0, 0, 0);
  cursor: pointer;
  margin-left: 10px;
  text-align: center;
  width: 150px;
}
.schedule-header .header-row {
  display: flex;
  font-size: 15px;
  align-items: center;
  border-radius: 8px;
}
.btn-small {
  padding: 4px 6px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  background: #ccc;
  margin: 0 2px;
}
.btn-small.danger {
  background: #d9534f;
  color: #fff;
}
</style>
