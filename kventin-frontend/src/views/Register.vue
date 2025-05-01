<template>
  <div class="register-container">
    <div class="register-box">
      <h2>Регистрация</h2>
      <form @submit.prevent="register">
        <div class="input-group">
          <label for="firstName">Имя</label>
          <input v-model="firstName" type="text" id="firstName" required />
        </div>
        <div class="input-group">
          <label for="lastName">Фамилия</label>
          <input v-model="lastName" type="text" id="lastName" required />
        </div>
        <div class="input-group">
          <label for="middleName">Отчество</label>
          <input v-model="middleName" type="text" id="middleName" required />
        </div>
        <div class="input-group">
          <label for="phoneNumber">Телефон</label>
          <input v-model="phoneNumber" v-mask="'+7 (###) ###-##-##'" type="tel" id="phoneNumber" required />
        </div>
        <div class="input-group">
          <label for="email">Email</label>
          <input v-model="email" type="email" id="email" required />
        </div>
        <div class="input-group">
          <label for="password">Пароль</label>
          <input v-model="password" type="password" id="password" required />
        </div>
        <div class="input-group">
          <label for="confirmPassword">Повторите пароль</label>
          <input v-model="confirmPassword" type="password" id="confirmPassword" required />
        </div>
        <button type="submit">Зарегистрироваться</button>
      </form>
      <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
      <p v-if="successMessage" class="success">{{ successMessage }}</p>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      firstName: '',
      lastName: '',
      middleName: '',
      phoneNumber: '',
      email: '',
      password: '',
      confirmPassword: '',
      errorMessage: '',
      successMessage: ''
    };
  },
  methods: {
    async register() {
      // Проверка совпадения пароля
      if (this.password !== this.confirmPassword) {
        this.errorMessage = 'Пароли не совпадают!';
        this.successMessage = '';
        return;
      }

      try {
        const response = await axios.post('/api/auth/register', {
          firstName: this.firstName,
          lastName: this.lastName,
          middleName: this.middleName,
          phoneNumber: this.phoneNumber,
          email: this.email,
          password: this.password
        });
        console.log('Регистрация успешна:', response.data);
        this.successMessage = "Вы успешно зарегистрировались!";
        this.errorMessage = "";
        // Перенаправление на личный кабинет
        setTimeout(() => this.$router.push('/dashboard'), 2000);
      } catch (error) {
        this.errorMessage = 'Ошибка регистрации. Проверьте данные.';
        this.successMessage = "";
        console.error(error);
      }
    }
  }
};
</script>

<style>
.register-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background: #f9fafb;
}

.register-box {
  background: white;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  width: 350px;
  text-align: center;
}

.input-group {
  margin-bottom: 15px;
}

.input-group label {
  display: block;
  font-weight: bold;
  margin-bottom: 5px;
}

.input-group input {
  width: 100%;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 5px;
}

button {
  background: #4caf50;
  color: white;
  padding: 10px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  width: 100%;
}

button:hover {
  background: #45a049;
}

.error {
  color: red;
  margin-top: 10px;
}

.success {
  color: green;
  margin-top: 10px;
}
</style>
