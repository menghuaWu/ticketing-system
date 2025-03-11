<script>
const {data: tickets, pending, error} = useTickets()
</script>

<template>
    <div v-if="pending">Loading tickets...</div>
    <div v-else-if="error">Failed to load tickets</div>
    <div v-else>
        <ul>
            <li v-for="ticket in tickets" :key="ticket.id">
                {{ ticket.title }} - {{ ticket.price }}$
            </li>
        </ul>
    </div>
</template>