<template>
  <div class="subject-page">
    <NavBar />

    <h1 class="page-title">Предметы:</h1>

    <div class="cards-container">
      <div
        v-for="subj in subjects"
        :key="subj.subjectId"
        class="card"
        @dblclick="openEditModal(subj.subjectId)"
      >
        {{ subj.subjectName }}
      </div>

      <p v-if="!subjects.length" class="empty">
        Пока нет ни одного предмета.
      </p>
    </div>

    <button class="btn-add" @click="openAddModal">
      Добавить предмет
    </button>

    <!-- здесь передаём массив subjects -->
    <AddSubjectModal
      v-if="showAdd"
      :existing-subjects="subjects"
      @close="showAdd = false"
      @created="reload"
    />

    <EditSubjectModal
      v-if="showEdit"
      :subject-id="currentId"
      :initial="editData"
      :existing-subjects="subjects"
      @close="showEdit = false"
      @saved="reload"
      @deleted="reload"
    />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import NavBar from '../components/NavBar.vue';
import AddSubjectModal from '../components/AddSubjectModal.vue';
import EditSubjectModal from '../components/EditSubjectModal.vue';

const subjects   = ref([]);
const showAdd    = ref(false);
const showEdit   = ref(false);
const currentId  = ref(null);
const editData   = ref({});

async function loadAll() {
  const { data } = await axios.get('/api/subject/all');
  subjects.value = data;
}

function reload() {
  loadAll();
  showAdd.value = false;
  showEdit.value = false;
  currentId.value = null;
  editData.value = {};
}

function openAddModal() {
  showAdd.value = true;
}

async function openEditModal(id) {
  const { data } = await axios.get(`/api/subject/${id}`);
  editData.value = data;
  currentId.value = id;
  showEdit.value = true;
}

onMounted(loadAll);
</script>


<style scoped>
html, body {
  margin: 0; padding: 0; height: 100%;
  background: #f9fafb;

}
.subject-page {
  position: relative; z-index: 0; min-height: 100vh;
}
.subject-page::before {
  content: ""; position: absolute; inset: 0; z-index: -1;
  background: url('/images/background.png') center/cover no-repeat;
  opacity: 0.5;
}
.page-title {
  margin-top: 3%; text-align: center; margin-bottom: 16px;
}
.cards-container {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(160px, 1fr));
  gap: 12px;
  max-width: 90%;
  margin: 0 0 0 5% ;


}
.card {
  border: 1px solid #ccd;
  border-radius: 6px;
  padding: 12px;
  text-align: center;
  cursor: pointer;
  user-select: none;
  background: #E1E5ED;
}
.card:hover {
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}
.empty {
  text-align: center;
  color: #888;
  font-style: italic;
}
.btn-add {
  display: block;
  max-width: 20%;
  padding: 10px 20px;
  background: #F7D4B4;
  border: 1px solid rgb(0, 0, 0);
  border-radius: 4px;
  color: #000000;
  cursor: pointer;
  margin: 1% 0 0 5% ;

}
.btn-add:hover {
  background: #e3b183;
}
</style>
