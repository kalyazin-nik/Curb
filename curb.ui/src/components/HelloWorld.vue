<template>
  <div class="greetings">
    <h1 class="green">Проект на VUE JS</h1>
    <h3>
      <a class="button" href="/">На главную</a>
      <a class="button" href="/weather">Посмотреть погоду</a>
    </h3>
  </div>
</template>

<style scoped>
h1 {
  font-weight: 500;
  font-size: 2.6rem;
  position: relative;
  top: -10px;
}

h3 {
  font-size: 1.2rem;
}

.greetings h1,
.greetings h3 {
  text-align: center;
}

a, .button {
  padding-right: 3rem;
}

@media (min-width: 1024px) {
  .greetings h1,
  .greetings h3 {
    text-align: left;
  }
}
</style>
