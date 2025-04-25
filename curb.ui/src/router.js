//import { createRouter, createWebHistory } from 'vue-router';
//import WeatherForecast from './components/WeatherForecast.vue';

//const routes = [
//  { path: '/weather', component: WeatherForecast }
//];

//const router = createRouter({
//  history: createWebHistory(),
//  routes,
//});

//export default router;

import { createRouter, createWebHistory } from 'vue-router';
import TheWelcome from './components/TheWelcome.vue';
import WeatherForecast from './components/WeatherForecast.vue';

const routes = [
  { path: '/', component: TheWelcome }, // Главная страница
  { path: '/weather', component: WeatherForecast }, // Страница с прогнозом погоды
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
