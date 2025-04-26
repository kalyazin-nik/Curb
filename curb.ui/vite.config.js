import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';

export default defineConfig({
  plugins: [plugin()],
  server: {
    port: 80,
    proxy: {
      '/api': {
        target: 'http://localhost:5124/api/',
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/api/, '')
      }
    }
  }
});
