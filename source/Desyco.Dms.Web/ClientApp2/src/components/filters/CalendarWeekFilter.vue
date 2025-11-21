<script setup lang="ts">

import {useI18n} from "vue-i18n";
import {onMounted, ref, watch} from "vue";
import type {CalendarWeek } from "@models";
import CalendarWeekService from "@/services/calendarWeekService";
import {useToast} from "primevue";
import {ToastLife} from "@/models/common/toastLife";

const { t } = useI18n();
const toast = useToast();
const loading = ref(false);
const selectedCalendarWeek = ref<CalendarWeek | null>(null);
const calendarWeeksOptions = ref<CalendarWeek[]>([]);

async function fetchCalendarWeekData() {
    loading.value = true;

    await CalendarWeekService.getRange(0, 2, {
        onSuccess: (data) => {
            calendarWeeksOptions.value = data;
            selectedCalendarWeek.value = data.find(d=> d.isCurrentWeek) ?? null;
          },
        onError: (e) => {
            toast.add({
                severity: "error",
                summary: t("calendarWeek.errorLoadingFilter"),
                detail: e.message,
                life: ToastLife.error,
            });
        },
    });

    loading.value = false;
}

const emit = defineEmits<(e: "update:modelValue", value: CalendarWeek | null) => void>();

watch(selectedCalendarWeek, (val) => {
    emit("update:modelValue", val);
});

onMounted(() => {
    fetchCalendarWeekData();
});

</script>

<template>
    <FloatLabel class="w-full md:w-250" variant="on">
        <Select
                v-model="selectedCalendarWeek"
                :options="calendarWeeksOptions"
                optionLabel="displayValue"
                :loading="loading"
                show-clear
                reset-filter-on-hide
                class="selectWidth"
                panelClass="selectWidth"
        />
        <label for="on_label">{{t('calendarWeek.title')}}</label>
    </FloatLabel>
</template>

<style scoped>
.selectWidth {
    min-width: 300px;
}
</style>
