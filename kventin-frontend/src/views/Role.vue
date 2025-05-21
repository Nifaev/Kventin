<template>
  <div class="admin-page">
    <NavBar />

    <h2 class="page-title">Управление ролями пользователей</h2>

    <table class="users-table">
      <thead>
        <tr>
          <th class="col-name">ФИО</th>
          <th class="col-roles">Текущие роли</th>
          <th class="col-action">Действия</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="u in sortedUsers" :key="u.user.userId">
          <td>{{ u.user.fullName }}</td>
          <td>
            <div v-if="u.roles.length" class="roles-list">
              <span
                v-for="r in u.roles"
                :key="r"
                class="role-chip"
              >
                {{ r }}
                <button
                  class="icon-btn delete-btn"
                  @click="removeRole(u.user.userId, r)"
                  title="Удалить"
                >×</button>
              </span>
            </div>
            <em v-else class="no-role">Нет роли</em>
          </td>
          <td class="col-action">
            <div class="action-cell">
              <select
                v-model="selectedRole[u.user.userId]"
                class="role-select"
                title="Выберите роль"
              >
                <option disabled value="">— выбрать роль —</option>
                <option
                  v-for="r in allRoles"
                  :key="r"
                  :value="r"
                >{{ r }}</option>
              </select>
              <button
                class="icon-btn save-btn"
                @click="addRole(u.user.userId, selectedRole[u.user.userId])"
                :disabled="!selectedRole[u.user.userId]"
                title="Сохранить роль"
              >
                <img src="/images/save.png" alt="Save" />
              </button>
            </div>
          </td>
        </tr>
      </tbody>
    </table>

    <p v-if="message" class="message">{{ message }}</p>
    <p v-if="error" class="error">{{ error }}</p>

    <!-- Модалка привязки детей -->
    <div v-if="showChildModal" class="modal-backdrop">
      <div class="modal">
        <h3>Привязать детей</h3>
        <p>Дважды кликните по ребёнку, чтобы добавить его:</p>

        <select
          class="child-select"
          size="5"
          @dblclick="onChildDblClick"
        >
          <option
            v-for="s in students"
            :key="s.userId"
            :value="s.userId"
          >
            {{ s.fullName }}
          </option>
        </select>

        <div class="selected-children">
          <span
            v-for="c in selectedChildren"
            :key="c.id"
            class="child-chip"
          >
            {{ c.fullName }}
          </span>
        </div>

        <button
          class="btn-confirm"
          @click="confirmChildren"
          :disabled="!selectedChildren.length"
        >
          Сохранить
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import axios from 'axios'
import NavBar from '../components/NavBar.vue'

const users            = ref([])
const allRoles         = ref([])
const selectedRole     = ref({})
const message          = ref('')
const error            = ref('')

// состояние модалки
const showChildModal   = ref(false)
const currentParentId  = ref(null)
const students         = ref([])
const selectedChildren = ref([]) // { id, fullName }[]

const sortedUsers = computed(() => [
  ...users.value.filter(u => u.roles.length === 0),
  ...users.value.filter(u => u.roles.length > 0)
])

// загрузка пользователей + ролей
async function fetchData() {
  message.value = ''
  error.value = ''
  try {
    const { data: udata } = await axios.post('/api/roles/getUsersRolesInfo')
    users.value = udata
    const { data: rdata } = await axios.get('/api/roles/getAllRoles')
    allRoles.value = rdata
    users.value.forEach(u => {
      selectedRole.value[u.user.userId] = ''
    })
  } catch {
    error.value = 'Не удалось загрузить данные'
  }
}

// назначить роль или открыть модалку для Parent
async function addRole(userId, role) {
  if (!role) return
  message.value = ''
  error.value = ''
  try {
    await axios.post(`/api/roles/${userId}/setRoles`, [role])
    if (role === 'Parent') {
      currentParentId.value = userId
      showChildModal.value = true
      selectedChildren.value = []
      const { data } = await axios.get('/api/user/getAllStudents')
      students.value = data
    } else {
      await fetchData()
    }
  } catch {
    error.value = 'Ошибка назначения роли'
  }
}

// удалить роль
async function removeRole(userId, role) {
  if (!confirm(`Удалить роль «${role}»?`)) return
  message.value = ''
  error.value = ''
  try {
    await axios.post(`/api/roles/${userId}/deleteRoles`, [role])
    message.value = `Роль «${role}» удалена`
    await fetchData()
  } catch {
    error.value = 'Ошибка удаления роли'
  }
}

// двойной клик выбирает ребёнка в modal
function onChildDblClick(e) {
  const id = Number(e.target.value)
  const student = students.value.find(s => s.userId === id)
  if (student && !selectedChildren.value.some(c => c.id === id)) {
    selectedChildren.value.push({
      id: student.userId,
      fullName: student.fullName
    })
  }
}

// сохраняем связь Parent→Children
async function confirmChildren() {
  try {
    const childIds = selectedChildren.value.map(c => c.id)
    console.log('Отправляем на /api/user/' + currentParentId.value + '/addChildren:', childIds)
    await axios.post(
      `/api/user/${currentParentId.value}/addChildren`,
      childIds
    )
    showChildModal.value = false
    await fetchData()
  } catch (e) {
    console.error(e)
    alert('Ошибка привязки детей')
  }
}

onMounted(fetchData)
</script>

<style scoped>
html, body {
  margin: 0; padding: 0; height: 100%;
  background: #f9fafb;
}
.admin-page {
  position: relative; z-index: 0; min-height: 100vh;
}
.admin-page::before {
  content: ""; position: absolute; inset: 0; z-index: -1;
  background: url('/images/background.png') center/cover no-repeat;
  opacity: 0.5;
}
.page-title {
  margin-top: 3%; text-align: center; margin-bottom: 16px;
}
.users-table {
  width: 90%; margin: 0 auto; background: white;
  border: 1px solid #ccc; border-radius: 10px;
  box-shadow: 0 4px 6px rgba(0,0,0,0.1); overflow: hidden;
}
.users-table th, .users-table td {
  border-bottom: 1px solid #ccc; border-right: 1px solid #ccc;
  padding: 12px; vertical-align: middle;
}
.users-table th:last-child,
.users-table td:last-child { border-right: none; }
.users-table tr:last-child td { border-bottom: none; }
.col-name   { width: 30%; }
.col-roles  { width: 45%; }
.col-action { width: 25%; text-align: center; }
.roles-list { display: flex; flex-wrap: wrap; gap: 6px; }
.role-chip {
  background: #e0f7fa; padding: 4px 8px; border-radius: 16px;
  display: inline-flex; align-items: center; font-size: .9em;
}
.delete-btn {
  margin-left: 4px; background: none; border: none;
  cursor: pointer; color: #900;
}
.no-role { font-style: italic; color: #c00; }
.action-cell { display: flex; align-items: center; justify-content: flex-end; gap: 8px; }
.role-select {
  width: 100%; padding: 6px; border: 1px solid #2ecccfa0;
  border-radius: 5px; background: rgba(255,255,255,0.6);
  cursor: pointer; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
}
.save-btn {
  flex-shrink: 0; width:24px; height:24px; background:none; border:none; cursor:pointer; padding:0;
}
.save-btn img { width:100%; height:100%; display:block; }
.save-btn:disabled { opacity:.5; cursor:not-allowed; }

.message { margin-top:16px; text-align:center; color:green; }
.error   { margin-top:16px; text-align:center; color:red; }

/* Модалка */
.modal-backdrop {
  position:fixed; top:0; left:0; right:0; bottom:0;
  background:rgba(0,0,0,0.4); display:flex; align-items:center; justify-content:center;
}
.modal {
  background:white; padding:20px; border-radius:8px; width:300px;
  box-shadow:0 2px 8px rgba(0,0,0,0.2);
}
.child-select { width:100%; box-sizing:border-box; margin-bottom:12px; }
.selected-children { display:flex; flex-wrap:wrap; gap:6px; margin-bottom:12px; }
.child-chip {
  background:#e0f7fa; padding:4px 8px; border-radius:12px; font-size:.9em;
}
.btn-confirm { margin-top:12px; padding:8px 12px; }
.btn-confirm:disabled { opacity:.5; cursor:not-allowed; }
</style>
