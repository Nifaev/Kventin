<template>
  <nav class="navbar">
    <!-- Логотип -->
    <div class="logo">
      <img src="/images/header-image.png" alt="КВЕНТИН" class="logo-img" />
    </div>

    <!-- Навигация + кнопка Выйти -->
      <!-- Ссылки + spacer + кнопка Выйти -->
      <ul class="nav-links">
        <li><router-link to="/grades"       active-class="active-link">Оценки</router-link></li>
        <li><router-link to="/announcements" active-class="active-link">Объявления</router-link></li>
        <li><router-link to="/messages"     active-class="active-link">Сообщения</router-link></li>
        <li><router-link to="/schedule"     active-class="active-link">Расписание</router-link></li>
        <li><router-link to="/dashboard"    active-class="active-link">Личный кабинет</router-link></li>
        <li v-if="isAdminOrSuper">
          <router-link to="/roles" active-class="active-link">Роли</router-link>
        </li>
        <li><router-link to="/group"    active-class="active-link">Группы</router-link></li>
        <li><router-link to="/subject"    active-class="active-link">Предмет</router-link></li>
        <!-- этот li растягивается и «отодвигает» кнопку вправо -->
        <li class="spacer"></li>
        <li>
          <button class="logout-btn" @click="logout">
            <img src="/images/back.jpg" alt="←" class="logout-icon" />
            Выйти
          </button>
        </li>
      </ul>
    <!-- разделитель -->
    <div class="nav-divider"></div>
  </nav>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import axios from 'axios';

const router = useRouter();
const isAdminOrSuper = ref(false);

onMounted(async () => {
  try {
    const { data } = await axios.get('/api/roles/getMyRoles');
    isAdminOrSuper.value = data.includes('AdminRegistration')
                         || data.includes('SuperUser');
  } catch (e) {
    console.error('Не удалось узнать роли', e);
  }
});

function logout() {
  // Здесь можно добавить логику очистки токена
  router.push('/');
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


/* сам список ссылок */
.nav-links {
  display: flex;
  align-items: center;
  width: 100%;
  margin: 0 auto 0 0 ;
  padding: 0;
  list-style: none;
  gap: 30px;

}

/* обычные пункты меню */
.nav-links a {
  text-decoration: none;
  color: #000;
  font-size: 18px;
  font-weight: 300;
  padding: 8px 4px;
}
.nav-links .active-link {
  font-weight: bold;
  border-bottom: 2px solid #000;
}

/* этот li растягивается, чтобы отодвинуть кнопку
.spacer {
  flex: 0.4;
} */

/* кнопка «Выйти» */
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

/* разделитель */
.nav-divider {
  width: 100%;
  height: 1px;
  background-color: #e0e0e0;
}
</style>