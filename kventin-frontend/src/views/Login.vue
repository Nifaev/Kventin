<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import { useRouter, useRoute } from 'vue-router';

const router = useRouter();
const route  = useRoute();

const usePhoneNumber = ref(true);
const loginData = ref({
  phoneNumber: '',
  email: '',
  password: ''
});
const errorMessage     = ref('');
const showPendingModal = ref(false);

onMounted(() => {
  // Если пришли с регистрации — показываем модалку
  if (route.query.pendingConfirm === '1') {
    showPendingModal.value = true;
  }
});

const login = async () => {
  errorMessage.value = '';
  try {
    const requestData = { password: loginData.value.password };
    if (usePhoneNumber.value) {
      requestData.phoneNumber = loginData.value.phoneNumber;
    } else {
      requestData.email = loginData.value.email;
    }
    const response = await axios.post('/api/auth/login', requestData);
    if (response.status === 200) {
      router.push('/dashboard');
    }
  } catch (err) {
    if (err.response?.status === 403) {
      errorMessage.value = 'Дождитесь, пока администратор подтвердит Ваш аккаунт';
    } else {
      errorMessage.value = 'Ошибка входа. Проверьте данные и попробуйте снова';
    }
  }
};
</script>

<template>
  <div class="login-page">
    <div class="login-container">
      <div class="form-section">
        <div class="login-box">
          <div class="toggle-container">
            <span :class="{ active: usePhoneNumber }" @click="usePhoneNumber = true">Телефон</span>
            <span :class="{ active: !usePhoneNumber }" @click="usePhoneNumber = false">Email</span>
          </div>
          
          <input
            v-if="usePhoneNumber"
            v-model="loginData.phoneNumber"
            class="otstyp"
            type="text"
            placeholder="Введите номер телефона"
          />
          <input
            v-if="!usePhoneNumber"
            v-model="loginData.email"
            class="otstyp"
            type="text"
            placeholder="Введите email"
          />
          <input
            v-model="loginData.password"
            type="password"
            class="otstyp"
            placeholder="Введите пароль"
          />
          <button class="button" @click="login">Вход</button>
        </div>
        <button class="button" @click="router.push('/register')">Регистрация</button>
        <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
      </div>
    </div>

    <div class="image-container">
      <div class="info-section">
        <img src="/images/header-image.png" alt="Информация" class="header-image" />
        <p class="info-text">
          Интенсивные подготовительные курсы для учеников 5-11 классов по подготовке к
          выпускным экзаменам (ОГЭ / ЕГЭ) и ВПР
        </p>
        <img src="/images/map.png" alt="Карта" class="map-image" />
      </div>
    </div>

    <!-- Модальное окно ожидания подтверждения -->
    <div
      v-if="showPendingModal"
      class="modal-backdrop"
      @click="showPendingModal = false"
    >
      <div class="modal" @click.stop>
        <p>Дождитесь подтверждения регистрации</p>
        <button @click="showPendingModal = false">Закрыть</button>
      </div>
    </div>
  </div>
</template>

<style>
.button {
  color: #000;
  background-color: #fff;
  border: 2px solid #000;
}

.login-page {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 30px;
  background-color: #f9fafb;
}

.image-container {
  display: flex;
  width: 100%;
  max-width: 500px;
}

.login-container {
  display: flex;
  width: 50%;
  max-width: 500px;
  background: #f1ece6;
  border-radius: 10px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  overflow: hidden;
}

.otstyp {
  display: block;
  width: 100%;         /* займет всю доступную ширину контейнера */
  max-width: 440px;    /* но не будет шире 400px */
  padding: 10px;
  margin: 0 0 10px;
  border: 1px solid #151515;
  border-radius: 5px;
}
.form-section {
  flex: 1;
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 20px;
}

.login-box {
  width: 100%;
  text-align: center;
}

.info-section {
  padding: 10px;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: flex-start;
}

.header-image {
  width: 100%;
  max-width: 200px;
  margin-bottom: 15px;
}

.map-image {
  width: 90%;
  border: 2px solid #000;
  max-width: 500px;
  border-radius: 10px;
  margin-top: 15px;
}

.info-text {
  text-align: center;
  font-size: 16px;
  font-weight: 500;
  margin-bottom: 10px;
}

.toggle-container {
  display: flex;
  justify-content: center;
  margin-bottom: 10px;
}

.toggle-container span {
  cursor: pointer;
  padding: 5px 15px;
  border-bottom: 2px solid transparent;
  transition: 0.3s;
}

.toggle-container span.active {
  font-weight: bold;
  border-bottom: 2px solid #fff;
}

/* input{
  display: block;
  width: 550px;
  padding: 10px;
  margin: 0px 0;
  border: 1px solid #151515;
  border-radius: 5px;
} */

button {
  width: 100%;
  padding: 10px;
  margin: 5px 0;
  background-color: #fff;
  color: #000;
  border: 2px solid #000;
  cursor: pointer;
  border-radius: 5px;
}

.button:hover {
  background-color: #9f8e9972;
}

.error {
  color: red;
  margin-top: 10px;
}

.modal-backdrop {
  position: fixed;
  inset: 0;
  background: rgba(0,0,0,0.4);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal {
  background: #fff;
  padding: 20px 30px;
  border-radius: 8px;
  text-align: center;
}

.modal button {
  margin-top: 10px;
  padding: 6px 12px;
}
</style>
