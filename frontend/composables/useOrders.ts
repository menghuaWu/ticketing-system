export const useOrders = () => {
    const config = useRuntimeConfig();
  
    const createOrder = async (orderData: { ticketId: number; userId: number }) => {
      try {
        console.log("🚀 Sending Order:", orderData); // 確保資料正確
        const response = await $fetch(`${config.public.apiBase}/orders`, {
          method: 'POST',
          headers: { "Content-Type": "application/json" },
          body: orderData,
        });
        console.log("呼叫 API : ", `${config.public.apiBase}/orders`); // 確保 API 正確呼叫
        console.log("🎯 API 回傳的資料：", response); // 確保 API 有回傳資料
        return response;
      } catch (error) {
        console.error('Error creating order:', error);
        return null;
      }
    };
    
    return { createOrder };
  };