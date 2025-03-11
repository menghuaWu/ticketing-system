export const useOrders = () => {
    const config = useRuntimeConfig();
  
    const createOrder = async (orderData: { title: string; price: number }) => {
      try {
        const response = await $fetch(`${config.public.apiBase}/api/orders`, {
          method: 'POST',
          body: orderData,
        });
        return response;
      } catch (error) {
        console.error('Error creating order:', error);
        return null;
      }
    };
  
    return { createOrder };
  };