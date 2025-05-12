import { createRouter, createWebHistory } from 'vue-router';

// Импорт страниц
import Grades from './views/Grades.vue';
import Announcements from './views/Announcements.vue';
import Messages from './views/Messages.vue';
import Schedule from './views/Schedule.vue';
import Login from './views/Login.vue';
import Dashboard from './views/Dashboard.vue';
import Register from './views/Register.vue';
import Role from './views/Role.vue';         // ← новый импорт

const routes = [
  { path: '/register',    component: Register },
  { path: '/',            component: Login    },
  { path: '/dashboard',   component: Dashboard},
  { path: '/grades',      component: Grades   },
  { path: '/announcements', component: Announcements },
  { path: '/messages',    component: Messages },
  { path: '/schedule',    component: Schedule },
  { path: '/roles',       component: Role },   // ← новая страница
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
