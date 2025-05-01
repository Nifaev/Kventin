import { createRouter, createWebHistory } from 'vue-router';


// Импортируем компоненты страниц
import Grades from './views/Grades.vue';
import Announcements from './views/Announcements.vue';
import Messages from './views/Messages.vue';
import Schedule from './views/Schedule.vue';
import Login from './views/Login.vue';
import Dashboard from './views/Dashboard.vue';
import Register from './views/Register.vue';


// Определяем маршруты
const routes = [
  { path: '/register', component: Register },
  { path: '/', component: Login }, // Главная страница — Login.vue
  { path: '/dashboard', component: Dashboard },// Личный кабинет
  { path: '/grades', component: Grades },
  { path: '/announcements', component: Announcements },
  { path: '/messages', component: Messages },
  { path: '/schedule', component: Schedule },
];

// Создаём роутер
const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;

