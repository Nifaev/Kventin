<template>
  <div class="schedule-page">
    <NavBar />

    <div class="container">
      <h1 class="page-title">Шаблон расписания</h1>

      <!-- Селектор учебного года -->
      <section class="year-selector">
        <label>
          Начало учебного года:
          <select v-model.number="selectedStart">
            <option v-for="y in availableYears" :key="y" :value="y">
              {{ y }}
            </option>
          </select>
        </label>
        <span class="dash">—</span>
        <label>
          Конец учебного года:
          <select v-model.number="selectedEnd">
            <option v-for="y in availableYears" :key="y" :value="y + 1">
              {{ y + 1 }}
            </option>
          </select>
        </label>
        <button class="btn-load" @click="loadSchedule">Загрузить</button>
      </section>

      <!-- Шапка шаблона -->
      <section class="schedule-header" v-if="!loadingTemplate">
        <div v-if="!currentSchedule">
          <p>Для {{ selectedStart }}–{{ selectedEnd }} шаблон не найден.</p>
          <button class="btn-add" @click="showCreateSchedule = true">
            Создать расписание
          </button>
        </div>
        <div v-else>
          <p>
            Шаблон для {{ currentSchedule.startYear }}–{{ currentSchedule.endYear }}:
          </p>
          <button class="btn-edit" @click="showCreateSchedule = true">
            Изменить года
          </button>
        </div>
      </section>
      <div v-else class="loading">Загрузка...</div>

      <!-- Список элементов расписания -->
      <section v-if="currentSchedule" class="items-section">
        <header class="items-header">
          <h2>Элементы расписания</h2>
          <button class="btn-add" @click="openItemModal()">Добавить элемент</button>
        </header>

        <table class="items-table">
          <thead>
            <tr>
              <th>День недели</th>
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
            <tr v-for="it in items" :key="it.scheduleItemId">
              <td>{{ it.dayOfWeek }}</td>
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
            <tr v-if="!items.length">
              <td colspan="8" class="empty">Пока нет ни одного элемента.</td>
            </tr>
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
import { ref, onMounted, computed } from 'vue';
import axios from 'axios';
import NavBar               from '../components/NavBar.vue';
import CreateScheduleModal  from '../components/CreateScheduleModal.vue';
import EditItemModal        from '../components/EditItemModal.vue';

const today = new Date().getFullYear();
const availableYears = computed(() =>
  Array.from({ length: 6 }, (_, i) => today - i)
);

const selectedStart = ref(today);
const selectedEnd   = ref(today + 1);

const currentSchedule    = ref(null);
const items              = ref([]);
const loadingTemplate    = ref(false);
const teachers           = ref([]);
const subjects           = ref([]);
const groups             = ref([]);

const showCreateSchedule = ref(false);
const showItemModal      = ref(false);
const editingItem        = ref(null);

const weekdays = [
  '—','Понедельник','Вторник','Среда',
  'Четверг','Пятница','Суббота','Воскресенье'
];

// загрузка списков для выпадающих
async function loadRefs() {
  const [t,s,g] = await Promise.all([
    axios.get('/api/user/getAllTeachers'),
    axios.get('/api/subject/all'),
    axios.get('/api/studyGroup/all'),
  ]);
  teachers.value = t.data;
  subjects.value = s.data;
  groups.value   = g.data;
}

// загрузка шаблона и сразу его элементов
async function loadSchedule() {
  loadingTemplate.value = true;
  currentSchedule.value = null;
  items.value = [];
  try {
    const { data } = await axios.post('/api/schedule', {
      startYear: selectedStart.value,
      endYear:   selectedEnd.value
    });
    currentSchedule.value = data;
    items.value = data.scheduleItems || [];
  } catch (err) {
    if (!(err.response && err.response.status === 400)) {
      console.error(err);
    }
  } finally {
    loadingTemplate.value = false;
  }
}

// после создания/редактирования шаблона
function onTemplateSaved() {
  showCreateSchedule.value = false;
  loadSchedule();
}

// открываем форму добавления/редактирования элемента
function openItemModal(item = null) {
  editingItem.value = item
    ? { ...item }
    : { dayOfWeek:1, startTime:'08:00', endTime:'09:30',
        classroom:'', teacherId:null, subjectId:null, groupId:null, isOnline:false };
  showItemModal.value = true;
}

// после сохранения элемента
function onItemSaved() {
  showItemModal.value = false;
  loadSchedule();
}

// удаляем элемент и перезагружаем список
async function deleteItem(id) {
  if (!confirm('Удалить элемент?')) return;
  await axios.delete(`/api/schedule/deleteItem/${id}`);
  loadSchedule();
}

onMounted(async () => {
  await loadRefs();
  await loadSchedule();
});
</script>

<style scoped>
.schedule-page {
  padding: 20px;
  background: #f5f7fa;
  min-height: 100vh;
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
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 12px;
  margin-bottom: 24px;
}
.year-selector label {
  display: flex;
  flex-direction: column;
  font-size: 14px;
}
.year-selector select {
  margin-top: 4px;
  padding: 6px;
  border: 1px solid #ccc;
  border-radius: 4px;
}
.dash {
  font-size: 20px;
  line-height: 1.6;
}
.btn-load {
  background: #007bff;
  color: #fff;
  padding: 8px 16px;
  border-radius: 6px;
  border: none;
  cursor: pointer;
}
.btn-load:hover { background: #0069d9; }

.schedule-header {
  background: #fff;
  padding: 16px;
  border-radius: 8px;
  margin-bottom: 24px;
  text-align: center;
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
  background: #5cb85c;
  color: #fff;
  padding: 8px 14px;
  border: none;
  border-radius: 6px;
  cursor: pointer;
}
.btn-edit {
  background: #f0ad4e;
  color: #fff;
  padding: 8px 14px;
  border: none;
  border-radius: 6px;
  cursor: pointer;
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
