<!-- src/components/AddGroupModal.vue -->
<template>
  <div class="modal-backdrop" @click.self="close">
    <div class="modal">
      <h2>Добавить группу</h2>

      <div class="modal-body">
        <label>
          Название группы:
          <input v-model="form.groupName" type="text" />
        </label>

        <label>
          Предмет:
          <select v-model="form.subjectId">
            <option disabled value="">— выберите —</option>
            <option
              v-for="subj in subjects"
              :key="subj.subjectId"
              :value="subj.subjectId"
            >
              {{ subj.subjectName }}
            </option>
          </select>
        </label>

        <label>
          Преподаватель:
          <select v-model="form.teacherId">
            <option disabled value="">— выберите —</option>
            <option
              v-for="t in teachers"
              :key="t.userId"
              :value="t.userId"
            >
              {{ t.fullName }}
            </option>
          </select>
        </label>
      </div>

      <div class="modal-actions">
        <button class="btn-cancel" @click="close">
          Отмена
        </button>
        <button
          class="btn-save"
          :disabled="!canCreate"
          @click="onSubmit"
        >
          Добавить группу
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import axios from 'axios';

const emit = defineEmits(['close', 'created']);

const form = ref({
  groupName: '',
  subjectId: '',
  teacherId: ''
});

const subjects = ref([]);
const teachers = ref([]);

const canCreate = computed(() =>
  form.value.groupName.trim() &&
  form.value.subjectId &&
  form.value.teacherId
);

onMounted(async () => {
  const [{ data: subjList }, { data: teachList }] = await Promise.all([
    axios.get('/api/subject/all'),
    axios.get('/api/user/getAllTeachers')
  ]);
  subjects.value = subjList;
  teachers.value = teachList;
});

async function onSubmit() {
  await axios.post('/api/studyGroup/create', {
    groupName: form.value.groupName,
    subjectId: form.value.subjectId,
    teacherId: form.value.teacherId
  });

  emit('created');
  close();
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
  background: #f0f4f8;
  border-radius: 12px;
  width: 400px;
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
  font-weight: 500;
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
.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 20px;
}
.modal-actions button {
  padding: 8px 16px;
  border-radius: 8px;
  border: none;
  cursor: pointer;
  font-size: 14px;
  border: 1px solid rgb(0, 0, 0);
}
.modal-actions button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
.btn-cancel {
  background: #ccc;
  color: #333;
}
.btn-cancel:hover {
  background: #b3b3b3;
}
.btn-save {
  background: #5cb85c;
  color: #000000;
}
.btn-save:disabled {
  background: #8fd19a;
}
.btn-save:hover:not(:disabled) {
  background: #4cae4c;
}
</style>
