<script setup lang="ts">
import { onMounted } from 'vue';
import { useDayOfWeekStore } from '@/stores/dayOfWeekStore';
import WeeklySchedule from './WeeklySchedule.vue';
import type { UpdateWeeklyScheduleCommand } from '@/types/days-of-week';
import { useNotification } from '@/composables/useNotification';
import { useI18n } from 'vue-i18n';

const store = useDayOfWeekStore();
const { showSuccess, showError } = useNotification();
const { t } = useI18n();

onMounted(() => {
    store.fetchDays();
});

const handleSaveAll = async (command: UpdateWeeklyScheduleCommand) => {
    try {
        await store.updateAll(command);
        showSuccess(t('common.notifications.updateSuccess'));
    } catch (error) {
        showError(error, t('common.notifications.operationErrorTitle'));
    }
};

const handleError = (message: string) => {
    // Custom validation error from child
    showError({ message }, t('common.notifications.operationErrorTitle'));
};
</script>

<template>
    <div>
        <h1 class="text-2xl font-bold mb-4">{{ t('schoolSettings.panelGroups.yearsDaysTimes.daysOfTheWeek') }}</h1>
        <div v-if="store.loading" class="flex justify-center p-8">
            <i class="pi pi-spin pi-spinner text-4xl"></i>
        </div>
        <WeeklySchedule 
            v-else 
            :days="store.days" 
            :saving="store.saving" 
            @saveAll="handleSaveAll" 
            @error="handleError"
        />
    </div>
</template>