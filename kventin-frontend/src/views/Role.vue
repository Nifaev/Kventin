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
                <span v-for="r in u.roles" :key="r" class="role-chip">
                  {{ r }}
                  <button 
                    class="icon-btn delete-btn" 
                    @click="removeRole(u.user.userId, r)"
                    title="Удалить роль"
                  >
                    <img src="/images/delete.png" alt="×" />
                  </button>
                </span>
              </div>
              <em v-else class="no-role">Нет роли</em>
            </td>
            <td>
              <select
                v-model="selectedRoles[u.user.userId]"
                multiple
                class="role-select"
              >
                <option v-for="r in allRoles" :key="r" :value="r">
                  {{ r }}
                </option>
              </select>
              <button
                class="icon-btn save-btn"
                @click="assignRoles(u.user.userId)"
                :disabled="!selectedRoles[u.user.userId]?.length"
                title="Сохранить"
              >
                <img src="/images/save.png" alt="Сохранить" />
              </button>
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
  const selectedRoles = ref({});
  const message = ref('');
  const error = ref('');
  
  const sortedUsers = computed(() => [
    ...users.value.filter(u => u.roles.length === 0),
    ...users.value.filter(u => u.roles.length > 0)
  ]);
  
  async function fetchData() {
    message.value = error.value = '';
    try {
      const { data: usersData } = await axios.post('/api/roles/getUsersRolesInfo');
      users.value = usersData;
      const { data: rolesData } = await axios.get('/api/roles/getAllRoles');
      allRoles.value = rolesData;
  
      // подготовить массив выбранных ролей
      users.value.forEach(u => {
        selectedRoles.value[u.user.userId] = [...u.roles];
      });
    } catch (e) {
      error.value = 'Ошибка загрузки данных.';
    }
  }
  
  async function assignRoles(userId) {
    const roles = selectedRoles.value[userId] || [];
    if (!roles.length) return;
    message.value = error.value = '';
    try {
      await axios.post(`/api/roles/${userId}/setRoles`, roles);
      message.value = 'Роли успешно обновлены';
      await fetchData();
    } catch (e) {
      error.value = 'Ошибка сохранения ролей';
    }
  }
  
  async function removeRole(userId, role) {
    if (!confirm(`Удалить роль "${role}"?`)) return;
    message.value = error.value = '';
    try {
      await axios.post(`/api/roles/${userId}/deleteRoles`, [role]);
      message.value = `Роль "${role}" удалена`;
      await fetchData();
    } catch (e) {
      error.value = 'Ошибка удаления роли';
    }
  }
  
  onMounted(fetchData);
  </script>
  
  <style scoped>
  .admin-page {
    padding: 20px;
    background: #f9fafb;
  }
  
  .page-title {
    text-align: center;
    margin-bottom: 16px;
  }
  
  .users-table {
    width: 100%;
    table-layout: fixed;
    border-collapse: collapse;
    background: white;
  }
  
  .users-table th,
  .users-table td {
    border: 1px solid #ccc;
    padding: 12px;
    vertical-align: top;
  }
  
  .col-name   { width: 30%; }
  .col-roles  { width: 45%; }
  .col-action { width: 25%; text-align: center; }
  
  .roles-list {
    display: flex;
    flex-wrap: wrap;
    gap: 6px;
  }
  
  .role-chip {
    background: #e0f7fa;
    border-radius: 16px;
    padding: 4px 8px;
    display: inline-flex;
    align-items: center;
    font-size: 0.9em;
  }
  
  .role-select {
    width: 100%;
    height: 80px;
    padding: 4px;
    border-radius: 4px;
    border: 1px solid #999;
    box-sizing: border-box;
  }
  
  .icon-btn {
    background: none;
    border: none;
    cursor: pointer;
    vertical-align: middle;
    margin-left: 6px;
  }
  
  .save-btn img,
  .delete-btn img {
    width: 20px;
    height: 20px;
  }
  
  .save-btn:disabled {
    opacity: 0.5;
    cursor: not-allowed;
  }
  
  .message {
    margin-top: 16px;
    color: green;
    text-align: center;
  }
  
  .error {
    margin-top: 16px;
    color: red;
    text-align: center;
  }
  
  .no-role {
    font-style: italic;
    color: #c00;
  }
  </style>
  