<script>
// const {data: tickets, pending, error} = useTickets()
const{createOrder} = useOrders();

const orderTicket = async (title, price) => {
    const response = await createOrder({title, price});
    if (response) {
      alert('Order created!');
    } else {
      alert('Failed to create order');
    }
}
</script>

<template>
    <!-- <div v-if="pending">Loading tickets...</div>
    <div v-else-if="error">Failed to load tickets</div>
    <div v-else>
        <ul>
            <li v-for="ticket in tickets" :key="ticket.id">
                {{ ticket.title }} - {{ ticket.price }}$
            </li>
        </ul>
    </div> -->
    <div>
        <ul>
            <li v-for="(index) in 8" :key="index">
                基本通關{{index}} - 250$
                <button @click="orderTicket('基本通關{{index}}',250)">Order</button>
            </li>
        </ul>
    </div>
</template>