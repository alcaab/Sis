<script setup lang="ts">
    import { ref, computed, watchEffect } from "vue";
    import { DayOfWeekDto, WeeklyScheduleDto } from "@/types/days-of-week";
    import SelectButton from "primevue/selectbutton";
    import DatePicker from "primevue/datepicker";
    import { enumService } from "@/service/EnumService";
    import { useI18n } from "vue-i18n";
    import FormField from "@/components/common/FormField.vue";

    const { t } = useI18n();

    const props = defineProps<{
        days: DayOfWeekDto[];
        saving: boolean;
    }>();

    const emit = defineEmits<{
        (e: "saveAll", model: WeeklyScheduleDto): void;
        (e: "error", message: string): void;
    }>();

    const schoolDayOptions = computed(() => [
        { label: t("common.yes"), value: true },
        { label: t("common.no"), value: false },
    ]);

    // Helper to convert TimeOnly string "HH:mm:ss" to Date
    const toDate = (timeStr: string | null): Date | null => {
        if (!timeStr) return null;
        const [hours, minutes, seconds] = timeStr.split(":").map(Number);
        const date = new Date();
        date.setHours(hours, minutes, seconds || 0, 0);
        return date;
    };

    // Helper to convert Date to TimeOnly string "HH:mm:ss"
    const toTimeStr = (date: Date | null): string | null => {
        if (!date) return null;
        const hours = date.getHours().toString().padStart(2, "0");
        const minutes = date.getMinutes().toString().padStart(2, "0");
        const seconds = date.getSeconds().toString().padStart(2, "0");
        return `${hours}:${minutes}:${seconds}`;
    };

    const forms = ref<Record<number, any>>({});

    watchEffect(() => {
        props.days.forEach((day) => {
            if (!forms.value[day.id]) {
                forms.value[day.id] = {
                    id: day.id,
                    isSchoolDay: day.isSchoolDay,
                    openTime: toDate(day.openTime),
                    startTime: toDate(day.startTime),
                    endTime: toDate(day.endTime),
                    closeTime: toDate(day.closeTime),
                };
            }
        });
    });

    const getDayHeader = (dayId: number) => {
        return enumService.getDescription("DayOfWeek", dayId);
    };

    const onSaveAll = () => {
        const updates: DayOfWeekDto[] = [];

        for (const day of props.days) {
            const form = forms.value[day.id];
            if (!form) continue;

            if (form.isSchoolDay) {
                const start = form.startTime;
                const end = form.endTime;
                // const open = form.openTime;
                // const close = form.closeTime;

                // if (!start || !end || !open || !close) {
                //     emit("error", `${getDayHeader(day.id)}: ${t("common.validations.required")}`);
                //     return;
                // }

                // if (start >= end) {
                //     emit("error", `${getDayHeader(day.id)}: Start Time must be before End Time.`);
                //     return;
                // }
            }

            updates.push({
                id: form.id,
                name: "",
                shortName: "",
                isSchoolDay: form.isSchoolDay,
                openTime: toTimeStr(form.openTime),
                startTime: toTimeStr(form.startTime),
                endTime: toTimeStr(form.endTime),
                closeTime: toTimeStr(form.closeTime),
            });
        }

        const model: WeeklyScheduleDto = {
            days: updates,
        };
        emit("saveAll", model);
    };

    defineExpose({
        onSaveAll,
    });
</script>

<template>
    <div class="flex flex-col gap-1">
        <Panel
            v-for="day in days"
            :key="day.id"
            :value="String(day.id)"
            :collapsed="!forms[day.id].isSchoolDay"
        >
            <template #header>
                <div class="flex items-center gap-2">
                    <i class="pi pi-clock" />
                    <span class="font-bold">{{ getDayHeader(day.id) }}</span>
                </div>
            </template>
            <template #icons>
                <SelectButton
                    v-model="forms[day.id].isSchoolDay"
                    optionLabel="label"
                    optionValue="value"
                    :options="schoolDayOptions"
                    :allowEmpty="false"
                />
            </template>
            <div
                v-if="forms[day.id] && forms[day.id].isSchoolDay"
                class="flex flex-col gap-4"
            >
                <FormField
                    :label="t('schoolSettings.dayOfWeek.openTime')"
                    :id="`openTime-${day.id}`"
                    align="right"
                >
                    <DatePicker
                        v-model="forms[day.id].openTime"
                        timeOnly
                        showIcon
                        showClear
                        icon="pi pi-clock"
                        class="w-2/12"
                        hourFormat="24"
                        iconDisplay="input"
                        fluid
                    />
                </FormField>
                <FormField
                    :label="t('schoolSettings.dayOfWeek.startTime')"
                    :id="`startTime-${day.id}`"
                    align="right"
                >
                    <DatePicker
                        v-model="forms[day.id].startTime"
                        timeOnly
                        showIcon
                        showClear
                        icon="pi pi-clock"
                        class="w-2/12"
                        hourFormat="24"
                        iconDisplay="input"
                        fluid
                    />
                </FormField>
                <FormField
                    :label="t('schoolSettings.dayOfWeek.endTime')"
                    :id="`endTime-${day.id}`"
                    align="right"
                >
                    <DatePicker
                        v-model="forms[day.id].endTime"
                        timeOnly
                        showIcon
                        showClear
                        icon="pi pi-clock"
                        class="w-2/12"
                        hourFormat="24"
                        iconDisplay="input"
                        fluid
                    />
                </FormField>
                <FormField
                    :label="t('schoolSettings.dayOfWeek.closeTime')"
                    :id="`closeTime-${day.id}`"
                    align="right"
                >
                    <DatePicker
                        v-model="forms[day.id].closeTime"
                        timeOnly
                        showIcon
                        showClear
                        icon="pi pi-clock"
                        class="w-2/12"
                        hourFormat="24"
                        iconDisplay="input"
                        fluid
                    />
                </FormField>
            </div>
        </Panel>
    </div>
</template>
<style scoped>
    :deep(.p-panel-header) {
        padding: 0.5rem 1rem;
    }
</style>
