# 使用 Node.js 官方映像作為基礎映像
FROM node:18 AS build

# 設定工作目錄
WORKDIR /app

# 複製 package.json 和 package-lock.json
COPY package*.json ./

# 安裝依賴
RUN npm install

# 複製專案所有檔案
COPY . .

# 建立 Nuxt 專案
RUN npm run build

# 使用較小的 Nginx 映像來部署靜態檔案
FROM nginx:alpine

# 設定工作目錄
WORKDIR /app

# 複製建置後的檔案到 Nginx 的公開資料夾
COPY --from=build /app/.output/public /usr/share/nginx/html

# 開放 80 埠
EXPOSE 80

# 啟動 Nginx 服務
CMD ["nginx", "-g", "daemon off;"]
