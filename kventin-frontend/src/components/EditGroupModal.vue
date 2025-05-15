<!-- src/components/EditGroupModal.vue -->
<template>
  <div class="modal-backdrop" @click.self="close">
    <div class="modal">
      <h2>Редактировать группу</h2>

      <div class="modal-body">
        <label>
          Название группы:
          <input v-model="localForm.groupName" type="text" />
        </label>

        <label>
          Преподаватель:
          <select v-model="localForm.teacherId">
            <option disabled value="">— выберите —</option>
            <option
              v-for="t in teacherList"
              :key="t.userId"
              :value="t.userId"
            >
              {{ t.fullName }}
            </option>
          </select>
        </label>

        <label>
          Добавить участника:
          <div class="add-student-row">
            <select v-model="newStudentId">
              <option disabled value="">— выберите —</option>
              <option
                v-for="s in allStudents"
                :key="s.userId"
                :value="s.userId"
              >
                {{ s.fullName }}
              </option>
            </select>
            <button class="btn-small add" @click="addStudent" :disabled="!newStudentId">+</button>
          </div>
        </label>
                <div class="participants">
          <p><strong>Участники:</strong></p>
          <ul>
            <li v-for="s in localForm.students" :key="s.userId">
              {{ s.fullName }}
                         <!-- добавили @click.stop -->
             <button
               class="btn-small remove"
               @click.stop="removeStudent(s.userId)"
             >×</button>
            </li>
          </ul>
        </div>
      </div>

      <div class="modal-actions">
        <button class="btn-save" @click="onSave" :disabled="!canSave">
          Сохранить
        </button>
        <button class="btn-cancel" @click="close">
          Отмена
        </button>
        <button class="btn-delete" @click="onDelete">
          Удалить
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue';
import axios from 'axios';

const props = defineProps({
  groupId:        { type: Number, required: true },
  initialData:    { type: Object,  required: true },
  teacherList:    { type: Array,   required: true },
  allStudents:    { type: Array,   required: true },
});
const emit = defineEmits(['saved', 'deleted', 'close']);

const localForm = ref({
  studyGroupId: props.initialData.studyGroupId,
  groupName:    props.initialData.groupName,
  teacherId:    props.initialData.teacher.userId,
  students:     [...props.initialData.students],
});
const newStudentId = ref(null);

const canSave = computed(() =>
  localForm.value.groupName.trim() &&
  localForm.value.teacherId !== ''
);

watch(() => props.initialData, val => {
  localForm.value = {
    studyGroupId: val.studyGroupId,
    groupName:    val.groupName,
    teacherId:    val.teacher.userId,
    students:     [...val.students],
  };
  newStudentId.value = null;
});

async function addStudent() {
  await axios.post(
    `/api/studyGroup/${localForm.value.studyGroupId}/addStudent`,
    null,
    { params: { studentId: newStudentId.value } }
  );
  const student = props.allStudents.find(s => s.userId === newStudentId.value);
  localForm.value.students.push(student);
  newStudentId.value = null;
}
async function removeStudent(studentId) {
  await axios.delete(
    `/api/studyGroup/${localForm.value.studyGroupId}/deleteStudent`,
    { params: { studentId } }
  );
  localForm.value.students = localForm.value.students.filter(s => s.userId !== studentId);
}

async function onSave() {
  await axios.post(
    `/api/studyGroup/${localForm.value.studyGroupId}/update`,
    {
      groupName: localForm.value.groupName,
      teacherId: localForm.value.teacherId
    }
  );
  emit('saved');
}
async function onDelete() {
  if (!confirm('Точно удалить эту группу?')) return;
  await axios.delete(`/api/studyGroup/${localForm.value.studyGroupId}/delete`);
  emit('deleted');
}
function close() {
  emit('close');
}
</script>

<style scoped>
.modal-backdrop {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.3);
  display: flex;
  align-items: center;
  justify-content: center;
}
.modal {
  background: #f0f4f8;  /* новый светлый фон */
  border-radius: 12px;   /* более скруглённые углы */
  width: 360px;
  padding: 20px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}
.modal h2 {
  margin-top: 0;
  text-align: center;
}
.modal-body label {
  display: block;
  margin-bottom: 14px;
}
.modal-body input,
.modal-body select {
  width: 100%;
  padding: 8px;
  margin-top: 6px;
  box-sizing: border-box;
  border-radius: 6px;
  border: 1px solid #ccc;
}
.participants ul {
  list-style: none;
  padding: 0;
}
.participants li {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 6px;
}
.add-student-row {
  display: flex;
  gap: 8px;
  align-items: center;
}

/* маленькие квадратные кнопки для × и + */
.btn-small {
  width: 24px;
  height: 24px;
  padding: 0;
  line-height: 1;
  text-align: center;
  font-size: 16px;
  border: 1px solid #888;
  border-radius: 4px;
  background: #fff;
  cursor: pointer;
}
.btn-small:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

/* кнопки модалки */
.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 20px;
}
.modal-actions button {
  padding: 8px 16px;
  border-radius: 8px;  /* скругление для всех кнопок */
  border: none;
  cursor: pointer;
  font-size: 14px;
  border: 1px solid rgb(0, 0, 0);
}
.modal-actions button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
.btn-save {
  background: #5cb85c85;
  color: #000000;
  
}
.btn-save:hover {
  background: #4cae4c;
}
.btn-cancel {
  background: #ccc;
  color: #000000;
}
.btn-cancel:hover {
  background: #b3b3b3;
}
.btn-delete {
  background: #d9544f99;
  color: #000000;
}
.btn-delete:hover {
  background: #c9302c;
}
</style>
