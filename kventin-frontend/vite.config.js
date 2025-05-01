import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  server: {
    proxy: {
      '/api': {
        target: 'https://localhost:7269',  // Укажите HTTPS-порт .NET сервера
        changeOrigin: true,
        secure: false,  // Отключаем проверку SSL
      }
    }
  }
})
