<script setup>
const { fetchApi } = useApi();
const tickets = ref([]);

const loadTickets = async () => {
  tickets.value = await fetchApi("/tickets");
};

onMounted(() => {
  loadTickets();
});
</script>

<template>
  <div class="p-6">
    <h1 class="text-3xl font-bold">票券列表</h1>
    <ul v-if="tickets.length" class="mt-4">
      <li v-for="ticket in tickets" :key="ticket.id" class="p-4 bg-white shadow rounded-md mb-2">
        <h2 class="text-xl font-semibold">{{ ticket.title }}</h2>
        <p>價格：${{ ticket.price }}</p>
      </li>
    </ul>
    <p v-else class="text-gray-500 mt-4">載入中...</p>
  </div>
</template>
