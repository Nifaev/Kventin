<!-- src/components/AddSubjectModal.vue -->
<template>
  <div class="modal-backdrop" @click.self="close">
    <div class="modal">
      <h2>Добавить предмет</h2>
      <div class="modal-body">
        <label>
          Название предмета:
          <input v-model="name" type="text" />
        </label>
      </div>
      <div class="modal-actions">
        <button class="btn-cancel" @click="close">Отмена</button>
        <button class="btn-save" :disabled="!canCreate" @click="onCreate">
          Добавить
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import axios from 'axios';

const emit = defineEmits(['close','created']);

const name = ref('');

const canCreate = computed(() => name.value.trim().length > 0);

async function onCreate() {
  await axios.post('/api/subject/create', null, {
    params: { subjectName: name.value }
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
  position: fixed; inset: 0;
  background: rgba(0,0,0,0.3);
  display: flex; align-items: center; justify-content: center;
}
.modal {
  background: #f0f4f8;
  border-radius: 12px;
  width: 360px;
  padding: 20px;
  box-shadow: 0 4px 12px rgba(0,0,0,0.15);
}
.modal h2 {
  margin: 0 0 16px;
  text-align: center;
}
.modal-body label {
  display: block;
  margin-bottom: 14px;
  font-weight: 500;
}
.modal-body input {
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
}
.btn-cancel {
  background: #ccc; color: #333;
}
.btn-cancel:hover { background: #b3b3b3; }
.btn-save {
  background: #5cb85c; color: #fff;
}
.btn-save:disabled { background: #8fd19a; cursor: not-allowed; }
.btn-save:hover:not(:disabled) { background: #4cae4c; }
</style>
