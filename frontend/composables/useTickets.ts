export const useTickets = () => {
    const config = useRuntimeConfig();
    const { data, pending, error, refresh } = useFetch(`${config.public.apiBase}/api/tickets`);
  
    return { data, pending, error, refresh };
  };