<!-- src/components/CreateScheduleModal.vue -->
<template>
  <div class="modal-backdrop" @click.self="close">
    <div class="modal">
      <h2>{{ initial ? 'Изменить расписание' : 'Создать расписание' }}</h2>
      <div class="modal-body">
        <label>
          <span>Год начала:</span>
          <input
            v-model.number="form.startYear"
            type="number"
            min="2000"
            max="2100"
          />
        </label>
        <label>
          <span>Год окончания:</span>
          <input
            :value="form.startYear + 1"
            type="number"
            readonly
          />
        </label>
      </div>
      <div class="modal-actions">
        <button class="btn-cancel" @click="close">Отмена</button>
        <button class="btn-save" :disabled="!canSave" @click="save">
          {{ initial ? 'Сохранить' : 'Создать' }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue';
import axios from 'axios';

const props = defineProps({
  initial: { type: Object, default: null }
});
const emit = defineEmits(['close','saved']);

// Начальное значение формы
const form = ref({
  startYear: props.initial?.startYear || new Date().getFullYear()
});

// Если initial обновился — подхватываем его
watch(
  () => props.initial,
  v => {
    if (v) {
      form.value.startYear = v.startYear;
    }
  }
);

// Кнопка доступна, когда год в диапазоне и не пуст
const canSave = computed(() =>
  form.value.startYear >= 2000 &&
  form.value.startYear <= 2100
);

async function save() {
  const dto = {
    startYear: form.value.startYear,
    endYear:   form.value.startYear + 1
  };
  // Всегда POST на /api/schedule/create
  await axios.post('/api/schedule/create', dto);
  emit('saved');
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
  display: flex; align-items: center; justify-content: center;
}
.modal {
  background: #fff; border-radius: 8px;
  width: 320px; padding: 20px;
  box-shadow: 0 4px 12px rgba(0,0,0,0.15);
}
.modal h2 {
  text-align: center; margin-bottom: 16px;
}
.modal-body label {
  display: block;
  margin-bottom: 12px;
  font-weight: 500;
}
.modal-body label span {
  display: block;
  margin-bottom: 4px;
}
.modal-body input {
  width: 100%;
  padding: 6px;
  box-sizing: border-box;
  border: 1px solid #ccc;
  border-radius: 4px;
}
.modal-body input[readonly] {
  background: #f5f5f5;
}
.modal-actions {
  display: flex; justify-content: flex-end; gap:10px; margin-top:16px;
}
.btn-cancel {
  background: #ccc; color:#333; border:none;
  border-radius:4px; padding:6px 12px; cursor:pointer;
}
.btn-save {
  background: #5cb85c; color:#fff; border:none;
  border-radius:4px; padding:6px 12px; cursor:pointer;
}
.btn-save:disabled {
  opacity: 0.6; cursor: not-allowed;
}
</style>
