<template>
  <div class="lesson-details-page">
    <NavBar />
    <button @click="$router.back()" class="back-button">← Назад</button>

    <div class="lesson-content" v-if="lessonData">
      <div class="lesson-header">
        <div><strong>Дата занятия:</strong> {{ formatDate(lessonData.date) }}</div>
        <div><strong>Время:</strong> {{ lessonData.startTime }}–{{ lessonData.endTime }}</div>
        <div><strong>Кабинет:</strong> №{{ lessonData.classroom }}</div>
        <div><strong>Преподаватель:</strong> {{ lessonData.teacher.fullName }}</div>
        <div><strong>Предмет:</strong> {{ lessonData.subjectName }}</div>
        <div><strong>Группа:</strong> {{ lessonData.groupName }}</div>
      </div>

      <div class="students-table">
        <table>
          <thead>
            <tr>
              <th>ФИО ученика</th>
              <th>Оценки за занятие</th>
              <th v-if="canManageMarks">Добавить оценку</th>
              <th v-if="canManageMarks">Редактировать оценки</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="student in lessonData.students" :key="student.userId">
              <td>{{ student.fullName }}</td>
              <td>
                <span v-if="student.marks.length">
                  {{ student.marks.map(m => m.markValue).join(' / ') }}
                </span>
                <span v-else>—</span>
              </td>
              <td v-if="canManageMarks">
                <select v-model="student.newMark">
                  <option disabled value="">—</option>
                  <option v-for="val in ['1','2','3','4','5','Н/А']" :key="val" :value="val">{{ val }}</option>
                </select>
                <button class="add-btn" @click="addMark(student)">
                  <img src="/images/save.png" alt="Добавить">
                </button>
              </td>
              <td v-if="canManageMarks">
                <div v-for="mark in student.marks" :key="mark.markId" class="mark-edit">
                  <select v-model="mark.markValue">
                    <option v-for="val in ['1','2','3','4','5','Н/А']" :key="val" :value="val">{{ val }}</option>
                  </select>
                  <button class="update-btn" @click="updateMark(mark)">
                    <img src="/images/redactirovat.png" alt="Изменить">
                  </button>
                  <button class="delete-btn" @click="deleteMark(mark)">
                    <img src="/images/delete.png" alt="Удалить">
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="file-upload" v-if="canManageMarks">
        <h3>Добавить задания на урок</h3>
        <input type="file" @change="handleFileUpload" />
        <button @click="uploadFile">Загрузить файл</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import axios from 'axios';
import NavBar from '../components/NavBar.vue';

const route = useRoute();
const lessonId = route.params.lessonId;

const lessonData = ref(null);
const currentUserRoles = ref([]);
const selectedFile = ref(null);

const canManageMarks = computed(() =>
  currentUserRoles.value.some(role => ['Teacher', 'AdminLessons', 'SuperUser'].includes(role))
);

async function fetchLessonDetails() {
  try {
    const { data } = await axios.get(`/api/lessons/${lessonId}`);
    lessonData.value = {
      ...data,
      students: data.students.map(student => ({
        ...student,
        newMark: '',
      })),
    };
  } catch (e) {
    console.error(e);
  }
}

async function fetchCurrentUserRoles() {
  try {
    const { data } = await axios.get('/api/roles/getMyRoles');
    currentUserRoles.value = data;
  } catch (e) {
    console.error(e);
  }
}

async function addMark(student) {
  const markValue = parseInt(student.newMark, 10);

  if (!markValue) return alert('Выберите оценку.');

  const payload = {
    lessonId: Number(lessonId),
    studentMarks: [{
      studentId: student.userId,
      marks: [{ markValue: markValue, teacherComment: '' }]
    }]
  };

  try {
    await axios.post('/api/mark/assignMarksForLesson', payload);
    alert('Оценка добавлена.');
    student.newMark = '';
    await fetchLessonDetails();
  } catch (e) {
    alert('Ошибка добавления.');
  }
}

async function updateMark(mark) {
  const payload = { markValue: parseInt(mark.markValue, 10), teacherComment: '' };

  try {
    await axios.post(`/api/mark/${mark.markId}/update`, payload);
    alert('Оценка изменена.');
    await fetchLessonDetails();
  } catch (e) {
    alert('Ошибка изменения.');
  }
}

async function deleteMark(mark) {
  try {
    await axios.delete(`/api/mark/${mark.markId}/delete`);
    alert('Оценка удалена.');
    await fetchLessonDetails();
  } catch (e) {
    alert('Ошибка удаления.');
  }
}

function formatDate(iso) {
  const [y, m, d] = iso.split('-');
  return `${d}.${m}.${y}`;
}

function handleFileUpload(event) {
  selectedFile.value = event.target.files[0];
}

async function uploadFile() {
  if (!selectedFile.value) return alert('Выберите файл для загрузки.');

  const formData = new FormData();
  formData.append('file', selectedFile.value);

  try {
    await axios.post('http://localhost:7269/api/file/upload', formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
      withCredentials: true, // добавлено для поддержки CORS
    });
    alert('Файл успешно загружен.');
  } catch (e) {
    alert('Ошибка загрузки файла.');
  }
}

onMounted(async () => {
  await fetchCurrentUserRoles();
  await fetchLessonDetails();
});
</script>

<style scoped>
.lesson-details-page {
  position: relative; min-height: 100vh;
}

.lesson-details-page::before {
  content: ""; position: absolute; inset: 0; z-index: -1;
  background: url('/images/background.png') center/cover no-repeat; opacity: 0.5;
}

.back-button {
  padding: 6px 12px;
  cursor: pointer;
  background: #F7D4B4;
  box-shadow: 0 2px 4px rgba(0,0,0,0.2);
  border-radius: 5px;
  width: 10%;
  margin-left: 2%;
}

.lesson-content {
  background: #fff;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  margin: 0 2%;
}

.students-table table {
  width: 100%; 
  border-collapse: collapse;
  margin-top: 2%;
}

.students-table th, .students-table td {
  border: 1px solid #352f2f;
  padding: 10px;
  text-align: center;
  vertical-align: middle; /* добавлено вертикальное выравнивание */
}

.students-table th {
  background-color: #7fcdd3a6;
}

select {
  padding: 4px;
  text-align: center;
}

.add-btn, .update-btn, .delete-btn {
  cursor: pointer;
  border: none;
  background: none;
  padding: 2px;
  width: 24px;
  height: 24px;
  display: inline-flex; /* изменено с flex на inline-flex */
  align-items: center;
  justify-content: center;
}

.add-btn img, .update-btn img, .delete-btn img {
  width: 100%;
  height: 100%;
  object-fit: contain;
}

.mark-edit {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 5px;
  margin-bottom: 5px;
}

.add-btn:hover, .update-btn:hover, .delete-btn:hover {
  opacity: 0.8;
}
</style>

