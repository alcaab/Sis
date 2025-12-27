<script setup lang="ts">
    import { ref, computed, watchEffect } from "vue";
    import type { DayOfWeekDto, UpdateWeeklyScheduleCommand, DayOfWeekScheduleUpdateDto } from "@/types/days-of-week";
    import SelectButton from "primevue/selectbutton";
    import DatePicker from "primevue/datepicker";
    import Button from "primevue/button";
    import Accordion from "primevue/accordion";
    import AccordionPanel from "primevue/accordionpanel";
    import AccordionHeader from "primevue/accordionheader";
    import AccordionContent from "primevue/accordioncontent";
    import { enumService } from "@/service/EnumService";
    import { useI18n } from "vue-i18n";
    import FormField from "@/components/common/FormField.vue";

    const { t } = useI18n();

    const props = defineProps<{
        days: DayOfWeekDto[];
        saving: boolean;
    }>();

    const emit = defineEmits<{
        (e: "saveAll", command: UpdateWeeklyScheduleCommand): void;
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

    // Expand all by default
    const expandedDays = computed(() => props.days.map((d) => String(d.id)));

    const onSaveAll = () => {
        const updates: DayOfWeekScheduleUpdateDto[] = [];

        // Validate all days
        for (const day of props.days) {
            const form = forms.value[day.id];
            if (!form) continue;

            if (form.isSchoolDay) {
                const start = form.startTime;
                const end = form.endTime;
                const open = form.openTime;
                const close = form.closeTime;

                if (!start || !end || !open || !close) {
                    emit("error", `${getDayHeader(day.id)}: ${t("common.validations.required")}`);
                    return;
                }

                if (start >= end) {
                    emit("error", `${getDayHeader(day.id)}: Start Time must be before End Time.`);
                    return;
                }
            }

            updates.push({
                id: form.id,
                isSchoolDay: form.isSchoolDay,
                openTime: toTimeStr(form.openTime),
                startTime: toTimeStr(form.startTime),
                endTime: toTimeStr(form.endTime),
                closeTime: toTimeStr(form.closeTime),
            });
        }

        const command: UpdateWeeklyScheduleCommand = {
            days: updates,
        };
        emit("saveAll", command);
    };
</script>

<template>
    <div class="card flex flex-col gap-4">
        <div class="flex justify-end mb-2">
            <Button
                :label="t('common.buttons.saveAll')"
                icon="pi pi-save"
                @click="onSaveAll"
                :loading="saving"
            />
        </div>

        <Accordion
            :value="expandedDays"
            multiple
        >
            <AccordionPanel
                v-for="(day, idx) in days"
                :key="day.id"
                :value="String(day.id)"
            >
                <AccordionHeader>{{ getDayHeader(day.id) }}</AccordionHeader>
                <AccordionContent>
                    <div
                        v-if="forms[day.id]"
                        class="flex flex-col gap-4"
                    >
                        <FormField
                            :label="t('schoolSettings.dayOfWeek.schoolDay')"
                            :id="`isSchoolDay-${idx}`"
                            required
                        >
                            <SelectButton
                                v-model="forms[day.id].isSchoolDay"
                                :options="schoolDayOptions"
                                optionLabel="label"
                                optionValue="value"
                            />
                        </FormField>

                        <div
                            v-if="forms[day.id].isSchoolDay"
                            class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4"
                        >
                            <FormField
                                :label="t('schoolSettings.dayOfWeek.openTime')"
                                :id="`openTime-${idx}`"
                            >
                                <DatePicker
                                    v-model="forms[day.id].openTime"
                                    timeOnly
                                    showIcon
                                    hourFormat="24"
                                />
                            </FormField>

                            <FormField
                                :label="t('schoolSettings.dayOfWeek.startTime')"
                                :id="`startTime-${idx}`"
                            >
                                <DatePicker
                                    v-model="forms[day.id].startTime"
                                    timeOnly
                                    showIcon
                                    hourFormat="24"
                                />
                            </FormField>
                            <FormField
                                :label="t('schoolSettings.dayOfWeek.endTime')"
                                :id="`endTime-${idx}`"
                            >
                                <DatePicker
                                    v-model="forms[day.id].endTime"
                                    timeOnly
                                    showIcon
                                    hourFormat="24"
                                />
                            </FormField>
                            <FormField
                                :label="t('schoolSettings.dayOfWeek.closeTime')"
                                :id="`closeTime-${idx}`"
                            >
                                <DatePicker
                                    v-model="forms[day.id].closeTime"
                                    timeOnly
                                    showIcon
                                    hourFormat="24"
                                />
                            </FormField>
                        </div>
                    </div>
                </AccordionContent>
            </AccordionPanel>
        </Accordion>

        <div class="flex justify-end mt-2">
            <Button
                :label="t('common.buttons.saveAll')"
                icon="pi pi-save"
                @click="onSaveAll"
                :loading="saving"
            />
        </div>
    </div>
</template>
