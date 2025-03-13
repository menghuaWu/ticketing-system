<script setup>
const {data, pending, error,refresh} = useTickets()
const tickets = computed(() => data?.value || []);
onMounted(() => {
    console.log("🔄 網頁重新整理後，強制刷新 tickets : ", tickets);
});

const{createOrder} = useOrders();

const orderTicket = async (ticketId) => {
    console.log("🟢 ticketId 的類型是:", typeof ticketId);
    const response = await createOrder({ ticketId, userId: 999 });
    if (response) {
      alert('Order created!');
    } else {
      alert('Failed to create order');
    }
}
</script>

<template>
    <div v-if="pending">Loading tickets...</div>
    <div v-else-if="error">Failed to load tickets</div>
    <div v-else>
        <ul>
            <li v-for="ticket in tickets" :key="ticket.mssqlId">
                {{ ticket.title }} - {{ ticket.price }}$
                <button class="btn btn-primary" @click="orderTicket(Number(ticket.mssqlId))">Order</button>
            </li>
        </ul>
    </div>
</template>