export const useApi = () => {
    const fetchApi = async (url: string, options: any = {}) => {
      try {
        const config = { baseURL: "http://localhost:5000/api", ...options };
        const response = await $fetch(url, config);
        return response;
      } catch (error) {
        console.error("API 錯誤:", error);
        throw error;
      }
    };
  
    return { fetchApi };
  };
  