// https://nuxt.com/docs/api/configuration/nuxt-config
import { defineNuxtConfig } from 'nuxt/config';

export default defineNuxtConfig({
  modules: ['@nuxt/http', '@pinia/nuxt'],
  runtimeConfig: {
    public: {
      apiBase: 'http://localhost:8080' // API Gateway
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