import { createApp } from 'vue';
import App from './App.vue';
import router from './router'; // Подключаем роутер
import VueTheMask from 'vue-the-mask';

const app = createApp(App);

app.use(router); // Используем Vue Router
app.use(VueTheMask);
app.mount('#app');
