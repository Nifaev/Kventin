<template>
  <div class="modal-backdrop" @click.self="close">
    <div class="modal">
      <h2>Добавить предмет</h2>
      <div class="modal-body">
        <label>
          Название предмета:
          <input v-model="name" type="text" />
        </label>
        <!-- Показываем ошибку, если дубликат -->
        <p v-if="error" class="error">{{ error }}</p>
      </div>
      <div class="modal-actions">
        <button class="btn-cancel" @click="close">Отмена</button>
        <button
          class="btn-save"
          :disabled="!canCreate"
          @click="onCreate"
        >
          Добавить
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue';
import axios from 'axios';

const props = defineProps({
  existingSubjects: {
    type: Array,
    default: () => []
  }
});
const emit = defineEmits(['close','created']);

const name  = ref('');
const error = ref('');

// Когда `name` меняется — сбрасываем текст ошибки
watch(name, () => {
  error.value = '';
});

const isDuplicate = computed(() => {
  const v = name.value.trim().toLowerCase();
  return v && props.existingSubjects.some(s => s.subjectName.toLowerCase() === v);
});

const canCreate = computed(() =>
  name.value.trim().length > 0 && !isDuplicate.value
);

async function onCreate() {
  if (isDuplicate.value) {
    error.value = 'Предмет с таким названием уже существует';
    return;
  }
  try {
    await axios.post('/api/subject/create', null, {
      params: { subjectName: name.value.trim() }
    });
    emit('created');
    close();
  } catch (e) {
    error.value = 'Ошибка при создании';
    console.error(e);
  }
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
.warning {
  margin-top: 4px;
  color: #d9534f;
  font-size: 13px;
}
.btn-save:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
.error {
  color: #d9534f;
  margin-top: 6px;
  font-size: 13px;
}
.modal h2 {
  margin: 0 0 16px; text-align: center;
}
.modal-body label {
  display: block; margin-bottom: 14px; font-weight: 500;
}
.modal-body input {
  width: 100%; padding: 8px; margin-top: 6px;
  border-radius: 6px; border: 1px solid #ccc;
}
.required {
  color: red; margin-left: 4px;
}
.modal-actions {
  display: flex; justify-content: flex-end; gap: 10px; margin-top: 20px;
}
.btn-cancel {
  background: #ccc; color: #333; padding: 8px 16px; border-radius: 8px; border: none; cursor: pointer;
}
.btn-cancel:hover { background: #b3b3b3; }
.btn-save {
  background: #5cb85c; color: #fff; padding: 8px 16px; border-radius: 8px; border: none; cursor: pointer;
}
.btn-save:disabled { background: #8fd19a; cursor: not-allowed; }
.btn-save:hover:not(:disabled) { background: #4cae4c; }
</style>
