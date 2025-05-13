<template></template>

<script setup>
import { onMounted } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';

const router = useRouter();
onMounted(async () => {
    let accessToken = localStorage.getItem('accessToken');
    let refreshToken = localStorage.getItem('refreshToken');

    if (!refreshToken || !accessToken) {
      return router.push('/');
    }

    const tokenPayload = JSON.parse(atob(accessToken.split('.')[1]));
    const userId = tokenPayload.userId;
    const role = tokenPayload.role;
    axios.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;
    await axios.post('/api/auth/logout', { refreshToken, id: userId, role });

    localStorage.removeItem('redirectPath');
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    router.push('/');
});
</script>
