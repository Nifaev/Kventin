<!-- src/views/GroupPage.vue -->
<template>
  <div class="group-page">
    <NavBar />

    <h1 class="page-title">Группы:</h1>

    <div class="groups-container">
      <div
        v-for="g in groups"
        :key="g.studyGroupId"
        class="group-card"
        @dblclick="openEditModal(g.studyGroupId)"
      >
        <div class="group-header">
          <span class="group-name">{{ g.groupName }}</span>
        </div>
        <div class="group-subject">
            <span class="group-subject">Предмет:{{ g.subjectName }}</span>
        </div>
        <div class="group-teacher">
          Преподаватель: {{ g.teacher.fullName }}
        </div>
        <p v-if="g.description" class="group-desc">
          {{ g.description }}
        </p>
      </div>

      <p v-if="!groups.length" class="empty">
        Пока нет ни одной группы.
      </p>
    </div>

    <button class="btn-add" @click="openAddModal">
      Добавить новую группу
    </button>

    <AddGroupModal
      v-if="showAddModal"
      @close="closeAddModal"
      @created="onGroupCreated"
    />

    <EditGroupModal
      v-if="showEditModal"
      :group-id="currentGroupId"
      :initial-data="editData"
      :teacher-list="teacherList"
      :all-students="allStudents"
      @saved="handleSaved"
      @deleted="handleDeleted"
      @close="closeEditModal"
    />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';

import NavBar from '../components/NavBar.vue';
import AddGroupModal from '../components/AddGroupModal.vue';
import EditGroupModal from '../components/EditGroupModal.vue';

const groups         = ref([]);
const teacherList    = ref([]);
const allStudents    = ref([]);

const showAddModal   = ref(false);
const showEditModal  = ref(false);
const currentGroupId = ref(null);
const editData       = ref({});

// Загрузка списка всех групп
async function fetchGroups() {
  const { data } = await axios.get('/api/studyGroup/all');
  groups.value = data;
}

// Открыть модалку редактирования (после загрузки всех данных)
async function openEditModal(groupId) {
  currentGroupId.value = groupId;

  // Загрузить справочные данные один раз
  if (!teacherList.value.length) {
    const { data } = await axios.get('/api/user/getAllTeachers');
    teacherList.value = data;
  }
  if (!allStudents.value.length) {
    const { data } = await axios.get('/api/user/getAllStudents');
    allStudents.value = data;
  }

  // Загрузить данные конкретной группы
  const { data } = await axios.get(`/api/studyGroup/${groupId}`);
  editData.value = data;

  // И только после этого показать модалку
  showEditModal.value = true;
}

// Обработчики событий из EditGroupModal
function handleSaved() {
  fetchGroups();
  closeEditModal();
}
function handleDeleted() {
  fetchGroups();
  closeEditModal();
}

function closeEditModal() {
  showEditModal.value = false;
  currentGroupId.value = null;
  editData.value = {};
}

function openAddModal() {
  showAddModal.value = true;
}
function closeAddModal() {
  showAddModal.value = false;
}

function onGroupCreated() {
  fetchGroups();
  closeAddModal();
}

onMounted(fetchGroups);
</script>

<style scoped>
html, body {
  margin: 0; padding: 0; height: 100%;
  background: #f9fafb;
}
.group-page {
  background: #f5f7fa;
  min-height: 100vh;
  padding: 20px;
}
.page-title {
  margin-top: 3%;   /* вот это новое */
  text-align: center;
  margin-bottom: 16px;
  font-size: 25px;
}
.groups-container {
  margin: 0 auto;
}
.group-card {
  background: #E1E5ED;
  border: 1px solid rgb(0, 0, 0);
  border-radius: 6px;
  padding: 12px;
  margin: 0 auto 1%;
  cursor: pointer;
  max-width: 90%;
}
.group-card:hover {
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}
.group-header {
  font-size: 16px;
  margin-bottom: 10px;
}
.group-subject {
  font-size: 16px;
  margin-bottom: 10px;
  color: #666;
}
.group-name {
  font-weight: bold;
  font-size: 18px;
}

.group-teacher {
  font-style: italic;
  margin-bottom: 8px;
}
.group-desc {
  line-height: 1.4;
  color: #333;
}
.empty {
  text-align: center;
  color: #888;
  font-style: italic;
}

.btn-add {
  display: block;
  max-width: 50%;
  margin: 0 auto ;
  padding: 10px 20px;
  background: #F7D4B4;
  border: 1px solid rgb(0, 0, 0);
  border-radius: 4px;
  color: #000000;
  cursor: pointer;
}
.btn-add:hover {
  background: rgba(207, 145, 69, 0.612);
}
</style>
