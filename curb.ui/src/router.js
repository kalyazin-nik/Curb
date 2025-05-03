import { createRouter, createWebHistory } from 'vue-router';
import TheWelcome from './components/TheWelcome.vue';
import WeatherForecast from './components/WeatherForecast.vue';
import Login from './components/Login.vue';
import Auth from './components/Auth.vue';
import AuthRefresh from './components/AuthRefresh.vue';
import Logout from './components/Logout.vue';

const routes = [
  { path: '/', component: TheWelcome },
  { path: '/weather', component: WeatherForecast, meta: { requiresAuth: true } },
  { path: '/login', component: Login },
  { path: '/auth', component: Auth },
  { path: '/auth/refresh', component: AuthRefresh },
  { path: '/logout', component: Logout }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  let accessToken = localStorage.getItem('accessToken');

  if (to.meta.requiresAuth) {
    if (!accessToken) {
      localStorage.setItem('redirectPath', to.fullPath);
      return next('/login');
    }

    try {
      const tokenPayload = JSON.parse(atob(accessToken.split('.')[1]));
      const tokenExpiration = tokenPayload.exp * 1000;

      if (Date.now() >= tokenExpiration) {
        localStorage.setItem('redirectPath', to.fullPath);
        return next('/auth/refresh');
      }
    } catch (error) {
      console.error("Ошибка декодирования accessToken:", error);
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      return next('/login');
    }
  }

  next();
});

export default router;
