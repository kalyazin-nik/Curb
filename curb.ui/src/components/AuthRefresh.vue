<template></template>

<script setup>
import { onMounted } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';

const router = useRouter();

onMounted(async () => {
  try {
    let accessToken = localStorage.getItem('accessToken');
    let refreshToken = localStorage.getItem('refreshToken');

    if (!refreshToken || !accessToken) {
      return router.push('/login');
    }

    axios.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;
    const tokenPayload = JSON.parse(atob(accessToken.split('.')[1]));
    const userId = tokenPayload.userId;
    const role = tokenPayload.role;

    // const response = await axios.put('/api/auth/refresh-token', {
    //   refreshToken,
    //   id: userId,
    //   role
    // }, {
    //   headers: {
    //     Authorization: `Bearer ${accessToken}`,
    //   }
    // });
    const response = await axios.put('/api/auth/refresh-token', { refreshToken, id: userId, role });

    localStorage.setItem('accessToken', response.data.accessToken);
    const redirect = localStorage.getItem('redirectPath') || '/';
    localStorage.removeItem('redirectPath');
    router.push(redirect);
  } catch (error) {
    console.error('Ошибка обновления токена:', error);
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    router.push('/login');
  }
});
</script>
