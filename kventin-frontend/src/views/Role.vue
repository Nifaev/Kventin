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

          <!-- текущие роли -->
          <td>
            <div v-if="u.roles.length" class="roles-list">
              <span v-for="r in u.roles" :key="r" class="role-chip">
                {{ r }}
                <button class="icon-btn delete-btn"
                        @click="removeRole(u.user.userId, r)"
                        title="Удалить">
                  ×
                </button>
              </span>
            </div>
            <em v-else class="no-role">Нет роли</em>
          </td>

          <!-- одиночный дропдаун + кнопка сохранить -->
          <td class="col-action">
            <div class="action-cell">
              <select
                v-model="selectedRole[u.user.userId]"
                class="role-select"
                title="Выберите роль"
              >
                <option disabled value="">— выбрать роль —</option>
                <option v-for="r in allRoles" :key="r" :value="r">
                  {{ r }}
                </option>
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
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import axios from 'axios';
import NavBar from '../components/NavBar.vue';

const users = ref([]);
const allRoles = ref([]);
const selectedRole = ref({});
const message = ref('');
const error = ref('');

const sortedUsers = computed(() => [
  ...users.value.filter(u => u.roles.length === 0),
  ...users.value.filter(u => u.roles.length > 0)
]);

async function fetchData() {
  message.value = '';
  error.value = '';
  try {
    const { data: usersData } = await axios.post('/api/roles/getUsersRolesInfo');
    users.value = usersData;
    const { data: rolesData } = await axios.get('/api/roles/getAllRoles');
    allRoles.value = rolesData;
    users.value.forEach(u => {
      selectedRole.value[u.user.userId] = '';
    });
  } catch {
    error.value = 'Не удалось загрузить данные';
  }
}

async function addRole(userId, role) {
  if (!role) return;
  message.value = '';
  error.value = '';
  try {
    await axios.post(`/api/roles/${userId}/setRoles`, [role]);
    message.value = `Роль "${role}" назначена.`;
    await fetchData();
  } catch {
    error.value = 'Ошибка назначения роли';
  }
}

async function removeRole(userId, role) {
  if (!confirm(`Удалить роль "${role}"?`)) return;
  message.value = '';
  error.value = '';
  try {
    await axios.post(`/api/roles/${userId}/deleteRoles`, [role]);
    message.value = `Роль "${role}" удалена.`;
    await fetchData();
  } catch {
    error.value = 'Ошибка удаления роли';
  }
}

onMounted(fetchData);
</script>

<style scoped>
html,
body {
  margin: 0;
  padding: 0;
  height: 100%;
  background-color: #f9fafb; /* тот же цвет, что и у .admin-page */
}

.page-title {
  text-align: center;
  margin-bottom: 16px;
}
.users-table {
  width: 90%;
  margin: 0 auto;
  background: white;

  /* переключили модель ячеек */
  border-collapse: separate;
  border-spacing: 0;

  /* рамка вокруг всей таблицы */
  border: 1px solid #ccc;

  /* скругления + тень */
  border-radius: 10px;
  box-shadow: 0 4px 6px rgba(0,0,0,0.1);
  overflow: hidden;
}

.users-table th,
.users-table td {
  border-bottom: 1px solid #000000;
  border-right: 1px solid #000000;
  padding: 12px;
}

/* у последнего столбца и последней строки убираем лишние бордеры */
.users-table th:last-child,
.users-table td:last-child {
  border-right: none;
}
.users-table tr:last-child td {
  border-bottom: none;
}

.col-name { width: 30%; }
.col-roles { width: 45%; }
.col-action { width: 25%; text-align: center; }

.roles-list {
  display: flex;
  flex-wrap: wrap;
  gap: 6px;
}
.role-chip {
  background: #e0f7fa;
  padding: 3px 2px;
  border-radius: 10px;
  display: inline-flex;
  align-items: center;
  font-size: 0.9em;
}
.delete-btn {
  margin-left: 4px;
  background: none;
  border: none;
  cursor: pointer;
  color: #900;
}
.no-role {
  font-style: italic;
  color: #c00;
}

.action-cell {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  gap: 8px;
}

.role-select {
  width:100%;
  padding: 6px;
  border: 1px solid #2ecccfa0;
  border-radius: 5px;
  background: rgba(255, 255, 255, 0.614);
  cursor: pointer;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.icon-btn {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 1.2em;
  line-height: 1;
}

.save-btn {
  flex-shrink: 0;
  width: 24px;
  height: 24px;
  padding: 0;
  background: none;
  border: none;
  cursor: pointer;
}
.save-btn img {
  width: 100%;
  height: 100%;
  display: block;
}
.save-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.message {
  margin-top: 16px;
  text-align: center;
  color: green;
}
.error {
  margin-top: 16px;
  text-align: center;
  color: red;
}
</style>
