// https://nuxt.com/docs/api/configuration/nuxt-config
import { defineNuxtConfig } from 'nuxt/config';

export default defineNuxtConfig({
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
    preset: 'node', // 使用 Node.js 預設模式
    output: {
      dir: '.output'
    },
    serveStatic: true, // 確保靜態文件能正確載入
    logLevel: 3, // 設置日誌級別，方便偵錯
  },
  vite: {
    build: {
      sourcemap: false, // 防止 source map 載入錯誤
    }
  },
  compatibilityDate: '2025-03-04'
})