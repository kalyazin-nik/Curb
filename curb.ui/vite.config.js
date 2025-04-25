//import { defineConfig } from 'vite';
//import plugin from '@vitejs/plugin-vue';

//// https://vitejs.dev/config/
//export default defineConfig({
//    plugins: [plugin()],
//    server: {
//        port: 53987,
//    }
//})

import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [plugin()],
  server: {
    port: 80, // Порт для UI
    proxy: {
      '/api': { // Прокси для запросов на API
        target: 'http://localhost:5124/api/', // Адрес API
        changeOrigin: true, // Меняет заголовок Host на origin
        rewrite: (path) => path.replace(/^\/api/, ''), // Перезапись пути, если требуется
      },
    },
  },
});
