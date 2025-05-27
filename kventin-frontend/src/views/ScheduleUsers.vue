<!-- src/views/ScheduleUsers.vue -->
<template>
  <div class="schedule-users-page">
    <!-- Общий NavBar -->
    <NavBar />

    <!-- Навигация по неделям с настоящими датами -->
    <header class="week-navigation">
      <button @click="changeWeek(-1)">‹ Предыдущая</button>
      <div class="week-range">
        {{ weekInfo.startOfWeek ? formatDate(weekInfo.startOfWeek) : '—' }}
        &nbsp;–&nbsp;
        {{ weekInfo.endOFWeek   ? formatDate(weekInfo.endOFWeek)   : '—' }}
      </div>
      <button @click="changeWeek(+1)">Следующая ›</button>
    </header>

    <!-- Сетка карточек дней -->
    <div class="days-grid">
      <div
        v-for="day in daysList"
        :key="day.dayOfWeek"
        class="day-card"
      >
        <!-- Голубой заголовок -->
        <div class="day-header">
          {{ day.name }} {{ day.date ? formatDate(day.date) : '—' }}
        </div>

                <!-- Персиковая «оболочка» -->
        <div class="lessons-shell">
        <!-- Белые карточки-строки уроков -->
        <router-link
            v-for="(lesson, idx) in day.lessons"
            :key="lesson.lessonId"
            :to="`/lessons/${lesson.lessonId}`"
            class="lesson-row"
        >
            <div class="lesson-main">
            {{ idx + 1 }}. {{ lesson.subjectName }}
            </div>
            <div class="lesson-room">
            Каб. {{ lesson.classroom || '—' }}
            </div>
            <div class="lesson-time">
            {{ lesson.startTime }}–{{ lesson.endTime }}
            </div>
        </router-link>

        <!-- Если уроков нет -->
        <div v-if="!day.lessons.length" class="no-lessons">
            Нет уроков
        </div>
</div>

      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch, computed } from 'vue'
import axios from 'axios'
import NavBar from '../components/NavBar.vue'

// смещение недель: 0 = текущая, –1 = предыдущая, +1 = следующая
const skipWeeksCount = ref(0)

// объект с ответом API
const weekInfo = ref({
  startOfWeek: '',
  endOFWeek:   '',
  schoolDays:  []
})

// названия дней по номеру dayOfWeek (1–7)
const weekdays = [
  '', 'Понедельник','Вторник','Среда',
  'Четверг','Пятница','Суббота','Воскресенье'
]

// собираем ровно 7 карточек, даже если для дня нет записей
const daysList = computed(() => {
  const map = {}
  weekInfo.value.schoolDays.forEach(d => {
    map[d.dayOfWeek] = d
  })
  return weekdays.slice(1).map((name, i) => {
    const dayOfWeek = i + 1
    const rec = map[dayOfWeek] || { date: '', lessons: [] }
    return { dayOfWeek, name, date: rec.date, lessons: rec.lessons }
  })
})

// запрос к API
async function fetchWeek() {
  try {
    const res = await axios.get('/api/lessons/getSchoolWeek', {
      params: { skipWeeksCount: skipWeeksCount.value }
    })
    weekInfo.value = res.data
  } catch (e) {
    console.error('Ошибка загрузки расписания:', e)
  }
}

// смена недели
function changeWeek(delta) {
  skipWeeksCount.value += delta
}

// формат YYYY-MM-DD → DD.MM.YYYY
function formatDate(iso) {
  if (!iso) return '—'
  const [y, m, d] = iso.split('-')
  return `${d}.${m}.${y}`
}

onMounted(fetchWeek)
watch(skipWeeksCount, fetchWeek)
</script>

<style scoped>

/* ваш существующий CSS без изменений */
.schedule-users-page {
  position: relative; z-index: 0; min-height: 100vh;
}
.schedule-users-page::before {
  content: ""; position: absolute; inset: 0; z-index: -1;
  background: url('/images/background.png') center/cover no-repeat;
  opacity: 0.5;
}

/* Навигация по неделям */
.week-navigation {
  display: flex;
  align-items: center;
  gap: 16px;
  margin-bottom: 24px;
}
.week-navigation button {
  flex: 1;
  background: none;
  margin-left: 2%;
  border: 1px solid #888;
  padding: 8px;
  border-radius: 4px;
  cursor: pointer;
  max-width: 200px;
  align-items: center;

}
.week-navigation button:hover {
  background: #f0f0f0;
}
.week-navigation .week-range {
  flex: 0 1 auto;
  min-width: 200px;
  text-align: center;
  font-weight: 600;
  font-size: 1rem;
}

/* Сетка карточек */
.days-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(400px, 1fr));
  gap: 20px;
}

/* Карточка дня */
.day-card {
  display: flex;
  margin-left: 2%;
  margin-right: 2%;
  flex-direction: column;
  background: #f8d5b1c2; /* персик */
  border-radius: 8px;
  overflow: hidden;
}

/* Голубой заголовок */
.day-header {
  background: #bfd4e9c9;
  padding: 12px;
  text-align: center;
  font-weight: 600;
  font-size: 1.1rem;
}

/* Персиковая оболочка для уроков */
.lessons-shell {
  padding: 12px;
  display: flex;
  flex-direction: column;
  gap: 8px;
}

/* Белая строка урока */
.lesson-row {
  display: grid;
  grid-template-columns: 1fr auto auto;
  background: #fff;
  border-radius: 4px;
  padding: 12px;
  align-items: center;
  column-gap: 12px;
    text-decoration: none;
  color: inherit; /* цвет наследуется от родителя */
}

/* Разделители между колонками */
.lesson-room,
.lesson-time {
  border-left: 1px solid #ccc;
  padding-left: 12px;
  text-align: right;
  color: #333;
  font-size: 0.95rem;
}

.lesson-main {
  color: #333;
  font-size: 1rem;
}

/* Когда нет уроков */
.no-lessons {
  text-align: center;
  font-style: italic;
  color: #666;
  padding: 12px 0;
}
</style>
