<script setup lang="ts">
    import { onMounted, ref } from "vue";
    import { useDayOfWeekStore } from "@/stores/dayOfWeekStore";
    import WeeklySchedule from "./WeeklySchedule.vue";
    import { WeeklyScheduleDto } from "@/types/days-of-week";
    import { useNotification } from "@/composables/useNotification";
    import { useI18n } from "vue-i18n";
    import Button from "primevue/button";

    const store = useDayOfWeekStore();
    const { showSuccess, showError } = useNotification();
    const { t } = useI18n();
    const ws = ref<InstanceType<typeof WeeklySchedule> | null>(null);

    onMounted(() => {
        store.fetchDays();
    });

    const handleSaveAll = async (model: WeeklyScheduleDto) => {
        try {
            await store.updateAll(model);
            showSuccess(t("common.notifications.updateSuccess"));
        } catch (error) {
            showError(error, t("common.notifications.operationErrorTitle"));
        }
    };

    const handleError = (message: string) => {
        showError({ message }, t("common.notifications.operationErrorTitle"));
    };
</script>

<template>
    <div class="card w-full animate fade-in">
        <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4 mb-2">
            <h4 class="m-0 text-xl font-semibold text-surface-900 dark:text-surface-0">
                {{ t("schoolSettings.dayOfWeek.title") }}
            </h4>
            <div class="flex items-center gap-2">
                <Button
                    :label="t('common.buttons.saveAll')"
                    icon="pi pi-save"
                    @click="ws?.onSaveAll()"
                    :loading="store.saving"
                />
            </div>
        </div>
        <WeeklySchedule
            ref="ws"
            :days="store.days"
            :saving="store.saving"
            @saveAll="handleSaveAll"
            @error="handleError"
        />
    </div>
</template>
