<!-- src/components/EditItemModal.vue -->
<template>
  <div class="modal-backdrop" @click.self="close">
    <div class="modal large">
      <h2 class="modal-title">
        {{ form.scheduleItemId ? 'Редактировать элемент' : 'Новый элемент' }}
      </h2>

      <div class="modal-body">
        <!-- День недели -->
        <label>
          День недели:
          <select v-model.number="form.dayOfWeek">
            <option disabled value="">— выберите —</option>
            <option
              v-for="(w, i) in weekdays.slice(1)"
              :key="i+1"
              :value="i+1"
            >
              {{ w }}
            </option>
          </select>
        </label>

        <!-- Время начала / конца -->
        <label>
          Начало:
          <input v-model="form.startTime" type="time" />
        </label>
        <label>
          Конец:
          <input v-model="form.endTime" type="time" />
        </label>

        <!-- Кабинет -->
        <label>
          Кабинет:
          <input v-model="form.classroom" type="text" placeholder="№ кабинета" />
        </label>

        <!-- Учитель -->
        <label>
          Учитель:
          <select v-model.number="form.teacherId">
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

        <!-- Группа -->
        <label>
          Группа:
          <select v-model.number="form.groupId">
            <option disabled value="">— выберите —</option>
            <option
              v-for="g in groups"
              :key="g.studyGroupId"
              :value="g.studyGroupId"
            >
              {{ g.groupName }}
            </option>
          </select>
        </label>

        <!-- Предмет -->
        <label>
          Предмет:
          <select v-model.number="form.subjectId">
            <option disabled value="">— выберите —</option>
            <option
              v-for="s in subjects"
              :key="s.subjectId"
              :value="s.subjectId"
            >
              {{ s.subjectName }}
            </option>
          </select>
        </label>

        <!-- Онлайн -->
        <label class="checkbox">
          <input v-model="form.isOnline" type="checkbox" />
          Онлайн
        </label>
      </div>

      <div class="modal-actions">
        <button class="btn-cancel" @click="close">Отмена</button>
        <button
          class="btn-save"
          :disabled="!canSave"
          @click="save"
        >
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
  initial:    { type: Object, required: true },
  scheduleId: { type: Number, required: true },
  teachers:   { type: Array,  required: true },
  subjects:   { type: Array,  required: true },
  groups:     { type: Array,  required: true },
});

const emit = defineEmits(['close', 'saved']);

const weekdays = [
  '—',
  'Понедельник',
  'Вторник',
  'Среда',
  'Четверг',
  'Пятница',
  'Суббота',
  'Воскресенье'
];

// Форма
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

// При открытии копируем initial
watch(
  () => props.initial,
  (v) => {
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
  },
  { immediate: true }
);

// Кнопка Save доступна, когда обязательные поля заполнены
const canSave = computed(() =>
  form.value.dayOfWeek &&
  form.value.startTime &&
  form.value.endTime &&
  form.value.classroom.trim() &&
  form.value.teacherId &&
  form.value.groupId &&
  form.value.subjectId
);

async function save() {
  const dto = {
    dayOfWeek:  form.value.dayOfWeek,
    startTime:  form.value.startTime,
    endTime:    form.value.endTime,
    classroom:  form.value.classroom,
    teacherId:  form.value.teacherId,
    groupId:    form.value.groupId,
    subjectId:  form.value.subjectId,
    scheduleId: props.scheduleId,
    isOnline:   form.value.isOnline
  };

  if (form.value.scheduleItemId) {
    // редактирование
    await axios.post(
      `/api/schedule/updateItem/${form.value.scheduleItemId}`,
      dto
    );
  } else {
    // создание
    await axios.post(`/api/schedule/addItem`, dto);
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
  position: fixed;
  inset: 0;
  background: rgba(0,0,0,0.4);
  display: flex;
  align-items: center;
  justify-content: center;
}
.modal {
  background: #fff;
  border-radius: 8px;
  width: 420px;
  padding: 20px;
  box-shadow: 0 2px 10px rgba(0,0,0,0.2);
}
.modal-title {
  text-align: center;
  margin-bottom: 16px;
}
.modal-body {
  display: flex;
  flex-direction: column;
  gap: 10px;
  max-height: 60vh;
  overflow-y: auto;
}
.modal-body label {
  display: flex;
  flex-direction: column;
  font-size: 14px;
}
.modal-body select,
.modal-body input[type="text"],
.modal-body input[type="time"] {
  margin-top: 4px;
  padding: 6px;
  border: 1px solid #ccc;
  border-radius: 4px;
}
.checkbox {
  flex-direction: row;
  align-items: center;
}
.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 16px;
}
.btn-cancel {
  background: #ccc;
  color: #333;
  border: none;
  padding: 6px 14px;
  border-radius: 4px;
  cursor: pointer;
}
.btn-save {
  background: #5cb85c;
  color: #fff;
  border: none;
  padding: 6px 14px;
  border-radius: 4px;
  cursor: pointer;
}
.btn-save:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
</style>
