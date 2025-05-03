<template></template>

<script>
import axios from "axios";

export default {
  async mounted() {
    try {
      const queryParams = new URLSearchParams(window.location.search);
      const userData = Object.fromEntries(queryParams.entries());
      const response = await axios.get("/api/auth/telegram-callback", { params: userData });

      if (response.data.accessToken && response.data.refreshToken) {
        localStorage.setItem("accessToken", response.data.accessToken);
        localStorage.setItem("refreshToken", response.data.refreshToken);
        const redirect = localStorage.getItem('redirectPath') || '/';
        localStorage.removeItem('redirectPath');
        this.$router.push(redirect);
      } else {
        throw new Error("Не удалось получить токен.");
      }
    } catch (error) {
      alert("Ошибка авторизации. Пожалуйста, попробуйте снова.");
      window.location.href = "/login";
    }
  }
}
</script>
