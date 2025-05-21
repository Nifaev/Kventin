<template>
  <div class="dashboard">
    <NavBar />

    <div class="content">
      <h2>Личный кабинет</h2>
      <div class="info-box">
        <!-- Левая колонка -->
        <div class="column">
          <label>Фамилия</label>
          <div class="editable">
            <input v-model="userData.lastName" :disabled="!editable.lastName" />
            <button v-if="!editable.lastName" @click="toggleEdit('lastName')" class="edit-icon">
              <img src="/images/redactirovat.png" alt="Редактировать" />
            </button>
            <button v-else @click="saveField('lastName')" class="edit-icon">
              <img src="/images/save.png" alt="Сохранить" />
            </button>
          </div>

          <label>Имя</label>
          <div class="editable">
            <input v-model="userData.firstName" :disabled="!editable.firstName" />
            <button v-if="!editable.firstName" @click="toggleEdit('firstName')" class="edit-icon">
              <img src="/images/redactirovat.png" alt="Редактировать" />
            </button>
            <button v-else @click="saveField('firstName')" class="edit-icon">
              <img src="/images/save.png" alt="Сохранить" />
            </button>
          </div>

          <label>Отчество</label>
          <div class="editable">
            <input v-model="userData.middleName" :disabled="!editable.middleName" />
            <button v-if="!editable.middleName" @click="toggleEdit('middleName')" class="edit-icon">
              <img src="/images/redactirovat.png" alt="Редактировать" />
            </button>
            <button v-else @click="saveField('middleName')" class="edit-icon">
              <img src="/images/save.png" alt="Сохранить" />
            </button>
          </div>
        </div>

        <!-- Средняя колонка -->
        <div class="column">
          <label>Почтовый адрес</label>
          <div class="editable">
            <input v-model="userData.email" :disabled="!editable.email" />
            <button v-if="!editable.email" @click="toggleEdit('email')" class="edit-icon">
              <img src="/images/redactirovat.png" alt="Редактировать" />
            </button>
            <button v-else @click="saveField('email')" class="edit-icon">
              <img src="/images/save.png" alt="Сохранить" />
            </button>
          </div>

          <label>Номер телефона</label>
          <div class="editable">
            <input v-model="userData.phoneNumber" :disabled="!editable.phoneNumber" />
            <button v-if="!editable.phoneNumber" @click="toggleEdit('phoneNumber')" class="edit-icon">
              <img src="/images/redactirovat.png" alt="Редактировать" />
            </button>
            <button v-else @click="saveField('phoneNumber')" class="edit-icon">
              <img src="/images/save.png" alt="Сохранить" />
            </button>
          </div>

          <label>VK</label>
          <div class="editable">
            <input v-model="userData.vk" :disabled="!editable.vk" />
            <button v-if="!editable.vk" @click="toggleEdit('vk')" class="edit-icon">
              <img src="/images/redactirovat.png" alt="Редактировать" />
            </button>
            <button v-else @click="saveField('vk')" class="edit-icon">
              <img src="/images/save.png" alt="Сохранить" />
            </button>
          </div>

          <label>Telegram</label>
          <div class="editable">
            <input v-model="userData.telegram" :disabled="!editable.telegram" />
            <button v-if="!editable.telegram" @click="toggleEdit('telegram')" class="edit-icon">
              <img src="/images/redactirovat.png" alt="Редактировать" />
            </button>
            <button v-else @click="saveField('telegram')" class="edit-icon">
              <img src="/images/save.png" alt="Сохранить" />
            </button>
          </div>
        </div>

        <!-- Правая колонка -->
        <div class="column">
          <label>Номер договора</label>
          <div>
            <input v-model="userData.contractNumber" disabled />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { reactive, ref, onMounted } from 'vue';
import axios from 'axios';
import NavBar from '../components/NavBar.vue';

const userId = ref(null);
const userData = reactive({
  lastName: '',
  firstName: '',
  middleName: '',
  email: '',
  phoneNumber: '',
  vk: '',
  telegram: '',
  contractNumber: ''
});

// track which field is in edit-mode
const editable = reactive({
  lastName:    false,
  firstName:   false,
  middleName:  false,
  email:       false,
  phoneNumber: false,
  vk:          false,
  telegram:    false
});

// 1) Получаем свой ID
// 2) Загружаем accountInfo
onMounted(async () => {
  try {
    const { data: id } = await axios.get('/api/user/getMyId');
    userId.value = id;

    const { data: info } = await axios.get(`/api/account/${id}/getAccountInfo`);
    // мапим ключи
    userData.lastName       = info.lastName;
    userData.firstName      = info.firstName;
    userData.middleName     = info.middleName;
    userData.email          = info.email;
    userData.phoneNumber    = info.phoneNumber;
    userData.vk             = info.vkLink;
    userData.telegram       = info.tgLink;
    userData.contractNumber = info.contractNumber;
  } catch (e) {
    console.error('Не удалось загрузить данные профиля', e);
  }
});

// Переключаем режим редактирования для одного поля
function toggleEdit(field) {
  editable[field] = true;
}

// Сохраняем одно поле на сервере
async function saveField(field) {
  if (userId.value == null) return;
  // строим DTO по схеме UpdateUserAccountInfoDto
  const dto = {
    email:          userData.email,
    phoneNumber:    userData.phoneNumber,
    vkLink:         userData.vk,
    tgLink:         userData.telegram,
    contractNumber: userData.contractNumber
  };
  try {
    await axios.post(`/api/account/${userId.value}/updateAccountInfo`, dto);
    editable[field] = false;
  } catch (e) {
    console.error(`Ошибка при сохранении ${field}`, e);
    // можно показать уведомление об ошибке
  }
}
</script>

<style>
.dashboard {
  position: relative;
  min-height: 100vh;
  /* чтобы дочерний контент был над фоном */
  z-index: 0;
}

/* Псевдо-элемент с фоном */
.dashboard::before {
  content: "";
  position: absolute;
  inset: 0;
  z-index: -1; /* за основным контентом */
  background: url('/images/background.png') center/cover no-repeat;
  opacity: 0.5; /* здесь регулируете прозрачность */
}
.content {
  padding: 20px;
  text-align: center;
}
.info-box {
  display: flex;
  justify-content: space-around;
  background: white;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  max-width: 90%;
  margin: auto;
}
.column {
  flex: 1;
  text-align: left;
  padding: 20px;
}
.editable {
  display: flex;
  align-items: center;
  gap: 10px;
}
.edit-icon {
  background: none;
  border: 1px solid #ccc;
  width: 30px;
  height: 30px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 5px;
  cursor: pointer;
}
.edit-icon img {
  width: 14px;
  height: 14px;
}
input {
  width: 100%;
  max-width: 400px;
  padding: 8px;
  font-size: 16px;
  border: 1px solid #ccc;
  border-radius: 5px;
}
.edit-icon:hover {
  background: #eee;
}
</style>
