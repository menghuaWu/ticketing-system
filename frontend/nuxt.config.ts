// https://nuxt.com/docs/api/configuration/nuxt-config
import { defineNuxtConfig } from 'nuxt/config';

export default defineNuxtConfig({
  imports: {
    dirs: ['composables'] // 確保 Nuxt 自動導入 composables
  },
  modules: ['@pinia/nuxt'],
  runtimeConfig: {
    public: {
      apiBase: process.env.API_BASE || 'http://localhost:8080/api' // API Gateway
    }
  },
  devtools: { enabled: true },
  ssr: true, // 確保啟用 SSR
  css: [
    '@/assets/css/tailwind.css'
  ],

  buildDir: '.nuxt',

  postcss: {
    plugins: {
      tailwindcss: {},
      autoprefixer: {},
    },
  },
  nitro: {
    preset: 'node-server', // 使用 Node.js 預設模式
    serveStatic: true // 確保靜態文件能正確載入
  },
  vite: {
    build: {
      sourcemap: false, // 防止 source map 載入錯誤
    }
  },
  compatibilityDate: '2025-03-04'
})