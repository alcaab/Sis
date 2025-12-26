<script setup lang="ts">
import { onMounted } from 'vue';
import { useDayOfWeekStore } from '@/stores/dayOfWeekStore';
import WeeklySchedule from './WeeklySchedule.vue';
import type { UpdateDayOfWeekCommand } from '@/types/days-of-week';
import { useToast } from 'primevue/usetoast';

const store = useDayOfWeekStore();
const toast = useToast();

onMounted(() => {
    store.fetchDays();
});

const handleSave = async (id: number, command: UpdateDayOfWeekCommand) => {
    try {
        await store.updateDay(id, command);
        toast.add({ severity: 'success', summary: 'Success', detail: 'Schedule updated successfully', life: 3000 });
    } catch (error) {
        toast.add({ severity: 'error', summary: 'Error', detail: 'Failed to update schedule', life: 3000 });
    }
};

const handleError = (message: string) => {
    toast.add({ severity: 'warn', summary: 'Validation', detail: message, life: 3000 });
};
</script>

<template>
    <div>
        <h1 class="text-2xl font-bold mb-4">Days of the Week</h1>
        <div v-if="store.loading" class="flex justify-center p-8">
            <i class="pi pi-spin pi-spinner text-4xl"></i>
        </div>
        <WeeklySchedule 
            v-else 
            :days="store.days" 
            :saving="store.saving" 
            @save="handleSave" 
            @error="handleError"
        />
    </div>
</template>
