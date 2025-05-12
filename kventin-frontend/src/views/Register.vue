<template>
  <div class="register-container">
    <div class="register-box">
      <h2>Регистрация</h2>
      <form @submit.prevent="register">
        <div class="input-group">
          <label for="firstName">Имя</label>
          <input v-model="registerData.firstName" type="text" id="firstName" required />
        </div>
        <div class="input-group">
          <label for="lastName">Фамилия</label>
          <input v-model="registerData.lastName" type="text" id="lastName" required />
        </div>
        <div class="input-group">
          <label for="middleName">Отчество</label>
          <input v-model="registerData.middleName" type="text" id="middleName" required />
        </div>
        <div class="input-group">
          <label for="phoneNumber">Телефон</label>
          <input
            v-model="registerData.phoneNumber"
            type="tel"
            id="phoneNumber"
            required
          />
        </div>
        <div class="input-group">
          <label for="email">Email</label>
          <input v-model="registerData.email" type="email" id="email" required />
        </div>
        <div class="input-group">
          <label for="password">Пароль</label>
          <input v-model="registerData.password" type="password" id="password" required />
        </div>
        <div class="input-group">
          <label for="passwordConfirmation">Повторите пароль</label>
          <input
            v-model="registerData.passwordConfirmation"
            type="password"
            id="passwordConfirmation"
            required
          />
        </div>
        <button type="submit">Зарегистрироваться</button>
      </form>

      <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
      <p v-if="successMessage" class="success">{{ successMessage }}</p>
    </div>
  </div>
</template>

<script setup>
import { reactive, ref } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';

const router = useRouter();
const errorMessage   = ref('');
const successMessage = ref('');

// Тело запроса соответствует Swagger RegisterDto
const registerData = reactive({
  firstName:            '',
  lastName:             '',
  middleName:           '',
  phoneNumber:          '',
  password:             '',
  passwordConfirmation: '',
  email:                ''
});

const register = async () => {
  errorMessage.value   = '';
  successMessage.value = '';

  try {
    await axios.post('/api/auth/register', registerData);
    // после успешной регистрации переходим на логин с флагом ожидания
    router.push({ path: '/', query: { pendingConfirm: '1' } });
  } catch (err) {
    const data = err.response?.data;
    if (data?.errors) {
      // ASP.NET-стиль ValidationErrors
      const arr = Object.values(data.errors).flat();
      errorMessage.value = arr.join('. ');
    } else if (data?.message) {
      errorMessage.value = data.message;
    } else {
      errorMessage.value = 'Ошибка регистрации. Проверьте данные.';
    }
    console.error(err);
  }
};
</script>

<style scoped>
.register-container {
  display: flex;
  height: 100vh;
  justify-content: center;
  align-items: center;
  background: #f9fafb;
}
.register-box {
  background: #fff;
  padding: 24px;
  border-radius: 10px;
  width: 360px;
  text-align: center;
  box-shadow: 0 4px 6px rgba(0,0,0,0.1);
}
.input-group {
  margin-bottom: 16px;
  text-align: left;
}
.input-group label {
  display: block;
  font-weight: 600;
  margin-bottom: 4px;
}
.input-group input {
  width: 100%;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 5px;
}
button[type="submit"] {
  width: 100%;
  padding: 10px;
  background: #4caf50;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}
button[type="submit"]:hover {
  background: #45a049;
}
.error {
  margin-top: 12px;
  color: #d32f2f;
}
.success {
  margin-top: 12px;
  color: #388e3c;
}
</style>
