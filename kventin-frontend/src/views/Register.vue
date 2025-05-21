<template>
  <div class="register-container">
    <div class="register-box">
      <h2>Регистрация</h2>
      <form @submit.prevent="register">
        <div class="input-group">
          <label for="firstName">
            Имя <span class="required">✱</span>
          </label>
          <input
            v-model="registerData.firstName"
            type="text"
            id="firstName"
            required
          />
        </div>

        <div class="input-group">
          <label for="lastName">
            Фамилия <span class="required">✱</span>
          </label>
          <input
            v-model="registerData.lastName"
            type="text"
            id="lastName"
            required
          />
        </div>

        <div class="input-group">
          <label for="middleName">
            Отчество
          </label>
          <input
            v-model="registerData.middleName"
            type="text"
            id="middleName"
            placeholder="необязательно"
          />
        </div>

        <div class="input-group">
          <label for="phoneNumber">
            Телефон <span class="required">✱</span>
          </label>
          <input
            v-model="registerData.phoneNumber"
            @input="formatPhone"
            type="tel"
            id="phoneNumber"
            placeholder="+7 (___) ___-__-__"
          />
        </div>

        <div class="input-group">
          <label for="email">
            Email 
          </label>
          <input
            v-model="registerData.email"
            type="email"
            id="email"
            placeholder="пример@domain.com"
          />
        </div>

        <div class="input-group">
          <label for="password">
            Пароль <span class="required">✱</span>
          </label>
          <input
            v-model="registerData.password"
            type="password"
            id="password"
            required
          />
        </div>

        <div class="input-group">
          <label for="passwordConfirmation">
            Повторите пароль <span class="required">✱</span>
          </label>
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

// DTO для регистрации
const registerData = reactive({
  firstName:            '',
  lastName:             '',
  middleName:           '',
  phoneNumber:          '',
  email:                '',
  password:             '',
  passwordConfirmation: ''
});

// Функция форматирования телефона
function formatPhone(e) {
  // берём цифры
  let digits = e.target.value.replace(/\D/g, '');
  // если ввели без кода, добавляем 7
  if (!digits.startsWith('7')) {
    digits = '7' + digits;
  }
  // обрезаем до +7XXXXXXXXXX
  digits = digits.slice(0, 11);
  
  // разбиваем на части
  const part1 = digits.slice(1, 4);
  const part2 = digits.slice(4, 7);
  const part3 = digits.slice(7, 9);
  const part4 = digits.slice(9, 11);
  
  // строим строку
  let formatted = '+7';
  if (part1) {
    formatted += ' (' + part1;
    if (part1.length === 3) formatted += ')';
  }
  if (part2) {
    formatted += ' ' + part2;
  }
  if (part3) {
    formatted += '-' + part3;
  }
  if (part4) {
    formatted += '-' + part4;
  }
  
  registerData.phoneNumber = formatted;
}

const register = async () => {
  errorMessage.value   = '';
  successMessage.value = '';

  // клиентская валидация: надо хотя бы один из контактов
  const hasPhone = registerData.phoneNumber.replace(/\D/g, '').length === 11;
  const hasEmail = !!registerData.email.trim();
  if (!hasPhone && !hasEmail) {
    errorMessage.value = 'Укажите телефон или email.';
    return;
  }

  // Подчищаем телефон к отправке
  // Подчищаем телефон к отправке и middleName
  const payload = {
    firstName:            registerData.firstName.trim(),
    lastName:             registerData.lastName.trim(),
    middleName:           registerData.middleName.trim() || null, // <-- вот здесь
    phoneNumber:          hasPhone
                            ? registerData.phoneNumber.replace(/\D/g, '')
                            : null,                              // если нет телефона — null
    email:                hasEmail
                            ? registerData.email.trim()
                            : null,
    password:             registerData.password,
    passwordConfirmation: registerData.passwordConfirmation
  };
  try {
    await axios.post('/api/auth/register', payload);
    successMessage.value = 'Регистрация прошла успешно! Перенаправление...';
    setTimeout(() => {
      router.push({ path: '/', query: { pendingConfirm: '1' } });
    }, 1500);
  } catch (err) {
    const data = err.response?.data;
    if (data?.errors) {
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
  
  background-image: url('/images/background.png');
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
  display: flex;
  align-items: center;
  font-weight: 600;
  margin-bottom: 4px;
}
.input-group .required {
  color: #d32f2f;
  margin-left: 4px;
  font-size: 10px;
  line-height: 1;
}
.input-group input {
  width: 100%;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 5px;
  box-sizing: border-box;
}
button[type="submit"] {
  width: 100%;
  padding: 10px;
  font-size: 15px;
  background: #BFD4E9;
  color: rgb(0, 0, 0);
  border: 1px color(srgb-linear rgb(0, 0, 0) green blue);
  border-radius: 5px;
  cursor: pointer;
}
button[type="submit"]:hover {
  background: #a7c6e5;
}
.error {
  margin-top: 12px;
  color: #d32f2f;
}
.success {
  margin-top: 12px;
  color: #FFBA7B;
}
</style>