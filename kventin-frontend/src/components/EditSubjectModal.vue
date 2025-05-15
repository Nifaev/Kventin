<!-- src/components/EditItemModal.vue -->
<template>
  <div class="modal-backdrop" @click.self="close">
    <div class="modal large">
      <h2>{{ form.scheduleItemId ? 'Редактировать элемент' : 'Новый элемент' }}</h2>

      <div class="modal-body">
        <label>
          День недели:
          <select v-model.number="form.dayOfWeek">
            <option disabled value="">— выберите —</option>
            <option v-for="(w,i) in weekdays" :key="i" :value="i">
              {{ w }}
            </option>
          </select>
        </label>

        <label>
          Время начала:
          <input v-model="form.startTime" type="time" />
        </label>
        <label>
          Время конца:
          <input v-model="form.endTime" type="time" />
        </label>

        <label>
          Кабинет:
          <input v-model="form.classroom" type="text" placeholder="№ кабинета" />
        </label>

        <label>
          Учитель:
          <select v-model.number="form.teacherId">
            <option disabled value="">— выберите —</option>
            <option v-for="t in teachers" :key="t.userId" :value="t.userId">
              {{ t.fullName }}
            </option>
          </select>
        </label>

        <label>
          Группа:
          <select v-model.number="form.groupId">
            <option disabled value="">— выберите —</option>
            <option v-for="g in groups" :key="g.studyGroupId" :value="g.studyGroupId">
              {{ g.groupName }}
            </option>
          </select>
        </label>

        <label>
          Предмет:
          <select v-model.number="form.subjectId">
            <option disabled value="">— выберите —</option>
            <option v-for="s in subjects" :key="s.subjectId" :value="s.subjectId">
              {{ s.subjectName }}
            </option>
          </select>
        </label>

        <label class="checkbox">
          <input v-model="form.isOnline" type="checkbox" />
          Онлайн
        </label>
      </div>

      <div class="modal-actions">
        <button class="btn-cancel" @click="close">Отмена</button>
        <button class="btn-save" :disabled="!canSave" @click="save">
          {{ form.scheduleItemId ? 'Сохранить' : 'Добавить' }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, watch, computed } from 'vue';
import axios from 'axios';

const props = defineProps({
  // если edit — сюда придёт объект с полями scheduleItemId, dayOfWeek, startTime, endTime, classroom,
  // teacherId, groupId, subjectId, isOnline
  initial:    { type: Object, default: () => ({}) },
  scheduleId: { type: Number, required: true },
  teachers:   { type: Array,  required: true },
  subjects:   { type: Array,  required: true },
  groups:     { type: Array,  required: true },
});
const emit = defineEmits(['close','saved']);

// названия дней недели
const weekdays = [
  '—','Понедельник','Вторник','Среда',
  'Четверг','Пятница','Суббота','Воскресенье'
];

// локальная форма
const form = ref({
  scheduleItemId: null,
  dayOfWeek:      null,
  startTime:      '',
  endTime:        '',
  classroom:      '',
  teacherId:      null,
  groupId:        null,
  subjectId:      null,
  isOnline:       false
});

// при открытии копируем initial
watch(() => props.initial, v => {
  form.value = {
    scheduleItemId: v.scheduleItemId || null,
    dayOfWeek:      v.dayOfWeek      || null,
    startTime:      v.startTime      || '',
    endTime:        v.endTime        || '',
    classroom:      v.classroom      || '',
    teacherId:      v.teacherId      || null,
    groupId:        v.groupId        || null,
    subjectId:      v.subjectId      || null,
    isOnline:       !!v.isOnline
  };
}, { immediate: true });

// можно сохранить, когда все ключевые поля заполнены
const canSave = computed(() =>
  form.value.dayOfWeek > 0 &&
  form.value.startTime &&
  form.value.endTime &&
  form.value.classroom.trim() &&
  form.value.teacherId &&
  form.value.groupId &&
  form.value.subjectId
);

// отправка на сервер
async function save() {
  const dto = {
    dayOfWeek:   form.value.dayOfWeek,
    startTime:   form.value.startTime,
    endTime:     form.value.endTime,
    classroom:   form.value.classroom,
    teacherId:   form.value.teacherId,
    groupId:     form.value.groupId,
    subjectId:   form.value.subjectId,
    scheduleId:  props.scheduleId,
    isOnline:    form.value.isOnline
  };

  if (form.value.scheduleItemId) {
    // редактирование
    await axios.post(
      `/api/schedule/updateItem/${form.value.scheduleItemId}`,
      dto
    );
  } else {
    // создание
    await axios.post('/api/schedule/addItem', dto);
  }

  emit('saved');
  emit('close');
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
.modal.large { width: 420px; }
.modal {
  background: #fff; border-radius: 8px; padding: 20px;
}
.modal h2 {
  text-align: center; margin-bottom: 12px;
}
.modal-body {
  display: flex; flex-direction: column; gap: 10px;
  max-height: 60vh; overflow-y: auto;
}
.modal-body label {
  display: flex; flex-direction: column;
  font-size: 14px;
}
.modal-body input[type="text"],
.modal-body input[type="time"],
.modal-body select {
  margin-top: 4px;
  padding: 6px;
  border: 1px solid #ccc;
  border-radius: 4px;
}
.modal-body .checkbox {
  flex-direction: row; align-items: center;
}
.modal-actions {
  display: flex; justify-content: flex-end; gap: 10px; margin-top: 16px;
}
.btn-cancel {
  background: #ccc; color: #333;
  border: none; padding: 6px 12px; border-radius: 4px; cursor: pointer;
}
.btn-save {
  background: #5cb85c; color: #fff;
  border: none; padding: 6px 12px; border-radius: 4px; cursor: pointer;
}
.btn-save:disabled {
  opacity: 0.6; cursor: not-allowed;
}
</style>
