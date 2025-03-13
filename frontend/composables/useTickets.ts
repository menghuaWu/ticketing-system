export const useTickets = () => {
    const config = useRuntimeConfig();
    const { data, pending, error, refresh } = useAsyncData('tickets', () =>
      $fetch(`${config.public.apiBase}/tickets`)
    );
    console.log("呼叫 API : ", `${config.public.apiBase}/tickets`); // 確保 API 正確呼叫
    console.log("🎯 API 回傳的資料：", data.value); // 確保 API 有回傳資料
    return { data, pending, error, refresh };
  };