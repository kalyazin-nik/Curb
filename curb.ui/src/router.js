import { createRouter, createWebHistory } from 'vue-router';
import TheWelcome from './components/TheWelcome.vue';
import WeatherForecast from './components/WeatherForecast.vue';

const routes = [
  { path: '/', component: TheWelcome },
  { path: '/weather', component: WeatherForecast },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
