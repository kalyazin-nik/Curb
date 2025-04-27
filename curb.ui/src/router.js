import { createRouter, createWebHistory } from 'vue-router';
import TheWelcome from './components/TheWelcome.vue';
import WeatherForecast from './components/WeatherForecast.vue';
import Login from './components/Login.vue';
import Auth from './components/Auth.vue';

const routes = [
  { path: '/', component: TheWelcome },
  { path: '/weather', component: WeatherForecast, meta: { requiresAuth: true } },
  { path: '/login', component: Login },
  { path: '/auth', component: Auth },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token');
  if (to.meta.requiresAuth && !token) {
    localStorage.setItem('redirectPath', to.fullPath);
    next('/login');
  } else {
    next();
  }
});

export default router;
