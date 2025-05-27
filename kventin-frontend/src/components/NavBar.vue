<!-- src/components/NavBar.vue -->
<template>
  <nav class="navbar">
    <!-- Логотип -->
    <div class="logo">
      <img src="/images/header-image.png" alt="КВЕНТИН" class="logo-img" />
    </div>

    <!-- Навигация + кнопка Выйти -->
    <ul class="nav-links">
      <li>
        <router-link to="/grades" active-class="active-link">
          Оценки
        </router-link>
      </li>
      <li>
        <router-link to="/announcements" active-class="active-link">
          Объявления
        </router-link>
      </li>
      <li>
        <router-link to="/messages" active-class="active-link">
          Сообщения
        </router-link>
      </li>

      <!-- Для админа: шаблон расписания -->
      <li v-if="canAdminSchedule">
        <router-link to="/schedule" active-class="active-link">
          Расписание
        </router-link>
      </li>
      <!-- Для школьника/учителя/родителя: своё расписание -->
      <li v-if="canViewUserSchedule">
        <router-link to="/scheduleusers" active-class="active-link">
          Моё расписание
        </router-link>
      </li>

      <li>
        <router-link to="/dashboard" active-class="active-link">
          Личный кабинет
        </router-link>
      </li>

      <!-- Роли/Группы/Предметы только для SuperUser и AdminGroups -->
      <li v-if="canManageRoles">
        <router-link to="/roles" active-class="active-link">
          Роли
        </router-link>
      </li>
      <li v-if="canManageRoles">
        <router-link to="/group" active-class="active-link">
          Группы
        </router-link>
      </li>
      <li v-if="canManageRoles">
        <router-link to="/subject" active-class="active-link">
          Предметы
        </router-link>
      </li>

      <!-- Отступ -->
      <li class="spacer"></li>
      <li>
        <button class="logout-btn" @click="logout">
          <img src="/images/back.jpg" alt="←" class="logout-icon" />
          Выйти
        </button>
      </li>
    </ul>

    <div class="nav-divider"></div>
  </nav>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'
import { useRouter } from 'vue-router'

const router = useRouter()

// роли пользователя
const roles = ref([])

// флаги доступа
const canManageRoles      = ref(false) // SuperUser, AdminGroups
const canAdminSchedule    = ref(false) // SuperUser, AdminSchedule
const canViewUserSchedule = ref(false) // Student, Teacher, Parent, SuperUser

onMounted(async () => {
  try {
    const { data: myRoles } = await axios.get('/api/roles/getMyRoles')
    roles.value = myRoles

    canManageRoles.value =
      roles.value.includes('SuperUser') ||
      roles.value.includes('AdminGroups')

    canAdminSchedule.value =
      roles.value.includes('SuperUser') ||
      roles.value.includes('AdminSchedule')

    canViewUserSchedule.value =
      roles.value.includes('SuperUser') ||
      roles.value.includes('Student')   ||
      roles.value.includes('Teacher')   ||
      roles.value.includes('Parent')

  } catch (e) {
    console.error('Не удалось получить роли', e)
  }
})

function logout() {
  // здесь можно очистить токен из стора/localStorage
  router.push('/')
}
</script>

<style scoped>
.navbar {
  display: flex;
  flex-direction: column;
  align-items: center;
  background: #f9fafb;
  padding: 0 20px;
}
.logo {
  margin: 10px 0;
}
.logo-img {
  height: 50px;
}
.nav-links {
  display: flex;
  align-items: center;
  width: 100%;
  padding: 0;
  list-style: none;
  gap: 30px;
}
.nav-links a {
  text-decoration: none;
  color: #000;
  font-size: 18px;
  font-weight: 300;
  padding: 8px 4px;
}
.active-link {
  font-weight: bold;
  border-bottom: 2px solid #000;
}
.spacer {
  flex: 1;
}
.logout-btn {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 6px 12px;
  background: none;
  border: 1px solid #000;
  border-radius: 6px;
  cursor: pointer;
  font-size: 16px;
  color: #000;
}
.logout-btn:hover {
  background: #eee;
}
.logout-icon {
  width: 18px;
  height: 18px;
}
.nav-divider {
  width: 100%;
  height: 1px;
  background-color: #e0e0e0;
}
</style>
