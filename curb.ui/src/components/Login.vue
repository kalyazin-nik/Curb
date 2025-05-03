<template>
  <div>
    <h1>Добро пожаловать!</h1>
    <div id="telegram-login"></div>
  </div>
</template>

<script>

  export default {
    mounted() {
      const script = document.createElement("script");
      script.src = "https://telegram.org/js/telegram-widget.js?22";
      script.async = true;
      script.setAttribute("data-telegram-login", "Curb_authenticate_bot");
      script.setAttribute("data-size", "large");
      script.setAttribute("data-auth-url", "https://indirectly-unified-cardinal.ngrok-free.app/auth");
      script.setAttribute("data-request-access", "write");
      document.getElementById("telegram-login").appendChild(script);
    }
  }
</script>
