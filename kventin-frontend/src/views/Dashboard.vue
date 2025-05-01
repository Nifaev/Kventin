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
            <input v-model="userData.lastName" :disabled="!editable" />
            <button v-if="editable" @click="editField('lastName')" class="edit-icon">
              <img src="/images/redactirovat.png" alt="Редактировать" />
            </button>
          </div>

          <label>Имя</label>
          <div class="editable">
            <input v-model="userData.firstName" :disabled="!editable" />
            <button v-if="editable" @click="editField('firstName')" class="edit-icon">
              <img src="/images/redactirovat.png" alt="Редактировать" />
            </button>
          </div>

          <label>Отчество</label>
          <div class="editable">
            <input v-model="userData.middleName" :disabled="!editable" />
            <button v-if="editable" @click="editField('middleName')" class="edit-icon">
              <img src="/images/redactirovat.png" alt="Редактировать" />
            </button>
          </div>
        </div>

        <!-- Средняя колонка -->
        <div class="column">
          <label>Почтовый адрес</label>
          <div class="editable">
            <input v-model="userData.email" :disabled="!editable" />
            <button v-if="editable" @click="editField('email')" class="edit-icon">
              <img src="/images/redactirovat.png" alt="Редактировать" />
            </button>
          </div>

          <label>Номер телефона</label>
          <div class="editable">
            <input v-model="userData.phoneNumber" :disabled="!editable" />
          </div>

          <label>VK</label>
          <div class="editable">
            <input v-model="userData.vk" :disabled="!editable" />
            <button v-if="editable" @click="editField('vk')" class="edit-icon">
              <img src="/images/redactirovat.png" alt="Редактировать" />
            </button>
          </div>

          <label>Telegram</label>
          <div class="editable">
            <input v-model="userData.telegram" :disabled="!editable" />
            <button v-if="editable" @click="editField('telegram')" class="edit-icon">
              <img src="/images/redactirovat.png" alt="Редактировать" />
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
import { ref } from 'vue';
import NavBar from '../components/NavBar.vue';

const editable = ref(true); // Возможность редактирования (заглушка)
const userData = ref({
  lastName: 'Нифаев',
  firstName: 'Иван',
  middleName: 'Алексеевич',
  email: 'nadodelat@gmail.com',
  phoneNumber: '8-953-526-25-43',
  vk: 'https://m.vk.com/kssenia8',
  telegram: '',
  contractNumber: '234567654321234'
});

const editField = (field) => {
  console.log(`Редактирование поля: ${field}`);
};
</script>

<style>
.dashboard {
  background-color: #f9fafb;
  height: 100vh;
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
  max-width: 80%;
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
  gap: 10px; /* Отступ между полем и кнопкой */
}
p{
}

.edit-icon {
  background: none;
  border: 1px solid #ccc;
  width: 30px; /* Квадратная кнопка */
  height: 30px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 5px; /* Делаем углы немного закругленными */
}
.edit-icon img {
  width: 10px; /* Размер иконки */
  height: 10px;
}
input {
  width: 100%; /* Растягиваем поле */
  max-width: 400px; /* Ограничиваем максимальную ширину */
  height: 20px; /* Увеличиваем высоту */
  padding: 10px;
  font-size: 16px;
  border: 1px solid #ccc;
  border-radius: 5px;
}
.edit-icon:hover {
  background: #ccc; /* Отключает изменение цвета */
}

</style>
