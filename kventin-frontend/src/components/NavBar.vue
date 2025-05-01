<template>
  <nav class="navbar">
    <div class="logo">
      <img src="/images/header-image.png" alt="КВЕНТИН" class="logo-img" />
    </div>
    <ul class="nav-links">
      <li>
        <router-link to="/grades" active-class="active-link">Оценки</router-link>
      </li>
      <li>
        <router-link to="/announcements" active-class="active-link">Объявления</router-link>
      </li>
      <li>
        <router-link to="/messages" active-class="active-link">Сообщения</router-link>
      </li>
      <li>
        <router-link to="/schedule" active-class="active-link">Расписание</router-link>
      </li>
      <li>
        <router-link to="/dashboard" active-class="active-link">Личный кабинет</router-link>
      </li>
    </ul>
    <div class="nav-divider"></div>
  </nav>
</template>

<style>
.navbar {
  display: flex;
  flex-direction: column;
  align-items: center;
  background: #f9fafb;
  padding: 0px 20px;
  border-bottom: 1px solid #e0e0e0;
}
.logo {
  display: flex;
  justify-content: center;
}
.logo-img {
  height: 50px;
}
.nav-links {
  list-style: none;
  display: flex;
  gap: 30px;
  padding: 0px 0;
}
.nav-links li {
  display: inline;
}
.nav-links a {
  text-decoration: none;
  color: rgb(0, 0, 0);
  font-size: 18px;
  font-weight: 300;
  padding-bottom: 5px;
}
.nav-links .active-link {
  font-weight: bold;
  border-bottom: 1px solid black; /* Подчеркивание активной страницы */
}
.nav-divider {
  width: 100%;
  height: 1px;
  background-color: rgb(61, 61, 61);
  margin-top: 0px;
}
</style>
