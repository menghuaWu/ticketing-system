export const useTickets = () => {
    const config = useRuntimeConfig();
    const { data, pending, error, refresh } = useAsyncData('tickets', async () => {
      try {
        console.log("🚀 發送 API 請求:", `${config.public.apiBase}/tickets`); // 確保 API 正確呼叫
        const response = await $fetch(`${config.public.apiBase}/tickets`)
        console.log("🎯 API 回傳的資料：", response); // 確保 API 有回傳資料
        return response; // 確保這裡 return 正確的資料
      }catch (error) {
        console.error("❌ API 錯誤:", error);
        return null; // 確保發生錯誤時 return null
      }
    });
    
    
    return { data, pending, error, refresh };
  };