<script setup>
import { computed } from 'vue';
import { useTickets } from '~/composables/useTickets';
import { useOrders } from '~/composables/useOrders';

const {data, pending, error} = useTickets()
const tickets = computed(() => data?.value || []);

const{createOrder} = useOrders();

const orderTicket = async (ticketId) => {
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
            <li v-for="ticket in tickets" :key="ticket.id">
                {{ ticket.title }} - {{ ticket.price }}$
                <button class="btn btn-primary" @click="orderTicket(ticket.id)">Order</button>
            </li>
        </ul>
    </div>
</template>