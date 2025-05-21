<template>
  <div class="group-page">
    <NavBar />

    <h1 class="page-title">Группы:</h1>

    <!-- Если Parent — переключатель между детьми -->
    <div v-if="isParent && childrenList.length" class="child-switcher">
      <label>Выберите ребёнка:</label>
      <select v-model.number="selectedChildId" @change="onChildChange">
        <option
          v-for="c in childrenList"
          :key="c.userId"
          :value="c.userId"
        >
          {{ c.fullName }}
        </option>
      </select>
    </div>

    <div class="groups-container">
      <div
        v-for="g in groups"
        :key="g.studyGroupId"
        class="group-card"
        @click="openEditModal(g.studyGroupId)"
      >
        <div class="group-header">
          <span class="group-name">{{ g.groupName }}</span>
        </div>
        <div class="group-subject">
          Предмет: {{ g.subjectName }}
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

    <!-- Кнопка "Добавить группу" -->
    <button
      v-if="canAddGroup"
      class="btn-add"
      @click="openAddModal"
    >
      Добавить новую группу
    </button>

    <!-- Модалки -->
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
import { ref, computed, onMounted } from 'vue'
import axios from 'axios'
import NavBar from '../components/NavBar.vue'
import AddGroupModal from '../components/AddGroupModal.vue'
import EditGroupModal from '../components/EditGroupModal.vue'

const myId           = ref(null)   // наш userId
const myRoles        = ref([])     // наши роли
const isParent       = computed(() => myRoles.value.includes('Parent'))

const childrenList   = ref([])     // список детей для Parent
const selectedChildId= ref(null)   // текущий выбранный ребёнок

const groups         = ref([])
const teacherList    = ref([])
const allStudents    = ref([])

const showAddModal   = ref(false)
const showEditModal  = ref(false)
const currentGroupId = ref(null)
const editData       = ref({})

// Разрешение на создание групп
const canAddGroup = computed(() =>
  myRoles.value.includes('SuperUser') ||
  myRoles.value.includes('AdminGroups')
)

async function init() {
  // 1) узнаем свой userId
  const { data: id } = await axios.get('/api/user/getMyId')
  myId.value = id

  // 2) получаем свои роли
  const { data: roles } = await axios.get('/api/roles/getMyRoles')
  myRoles.value = roles

  // 3) если Parent — подгружаем список детей и сразу выбираем первого
  if (isParent.value) {
    await loadChildren()
  }

  // 4) загружаем группы (у Parent — уже с учётом выбранного ребёнка)
  await loadGroups()
}

onMounted(init)

// загрузить детей для Parent
async function loadChildren() {
  const { data } = await axios.get(`/api/user/${myId.value}/getChildren`)
  childrenList.value = data
  if (data.length) {
    selectedChildId.value = data[0].userId
    await selectChild(data[0].userId)
  }
}

// при смене ребёнка в селекте
async function onChildChange() {
  if (selectedChildId.value != null) {
    await selectChild(selectedChildId.value)
  }
}

// сообщаем бэку, какого ребёнка показывать
async function selectChild(childId) {
  await axios.post(
    `/api/user/${myId.value}/selectChild/${childId}`
  )
  // после — обновляем группы
  await loadGroups()
}

// запрос групп
async function loadGroups() {
  const { data } = await axios.get('/api/studyGroup/all')
  groups.value = data
}

// AddGroupModal
function openAddModal()   { showAddModal.value = true }
function closeAddModal()  { showAddModal.value = false }
function onGroupCreated() { loadGroups(); closeAddModal() }

// EditGroupModal
async function openEditModal(groupId) {
  currentGroupId.value = groupId
  if (!teacherList.value.length) {
    teacherList.value = (await axios.get('/api/user/getAllTeachers')).data
  }
  if (!allStudents.value.length) {
    allStudents.value = (await axios.get('/api/user/getAllStudents')).data
  }
  editData.value = (await axios.get(`/api/studyGroup/${groupId}`)).data
  showEditModal.value = true
}
function closeEditModal() {
  showEditModal.value = false
  currentGroupId.value = null
  editData.value = {}
}
function handleSaved()   { loadGroups(); closeEditModal() }
function handleDeleted() { loadGroups(); closeEditModal() }
</script>

<style scoped>
.group-page {
  position: relative; z-index: 0; min-height: 100vh;
}
.group-page::before {
  content: ""; position: absolute; inset: 0; z-index: -1;
  background: url('/images/background.png') center/cover no-repeat;
  opacity: 0.5;
}
.page-title {
  text-align: center;
  margin: 2% 0 1%;
  font-size: 24px;
}
/* переключатель ребёнка */
.child-switcher {
  width: 90%;
  margin: 0 auto 16px;
  display: flex;
  align-items: center;
  gap: 8px;
}
.groups-container {
  width: 90%;
  margin: 0 auto;
}
.group-card {
  background: #E1E5ED;
  border: 1px solid #000;
  border-radius: 6px;
  padding: 12px;
  margin-bottom: 12px;
  cursor: pointer;
}
.group-card:hover {
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}
.btn-add {
  display: block;
  max-width: 40%;
  margin: 20px auto;
  padding: 15px 20px;
  background: #F7D4B4;
  border: 1px solid #000;
  border-radius: 4px;
  cursor: pointer;
}
</style>
