<script setup lang="ts">
    import { ref, onMounted } from "vue";
    import { useSpecialDayStore } from "@/stores/specialDayStore";
    import { useAcademicYearStore } from "@/stores/academicYearStore";
    import { useEvaluationPeriodStore } from "@/stores/evaluationPeriodStore";
    import { useNotification } from "@/composables/useNotification";
    import { useDeleteConfirm } from "@/composables/useDeleteConfirm";
    import { useI18n } from "vue-i18n";
    import { SpecialDayType, type SpecialDayDto } from "@/types/special-days";
    import SpecialDayForm from "./SpecialDayForm.vue";
    import { format, parseISO, isSameDay, differenceInMonths } from "date-fns";

    const specialDayStore = useSpecialDayStore();
    const academicYearStore = useAcademicYearStore();
    const evaluationPeriodStore = useEvaluationPeriodStore();
    const notify = useNotification();
    const { confirmDelete, deletingItemId } = useDeleteConfirm();
    const { t } = useI18n();

    // State
    const dateRange = ref<Date[]>([]); // For DatePicker selection mode
    const minDate = ref(new Date());
    const maxDate = ref(new Date());
    const numberOfMonths = ref(1);

    const itemDialog = ref(false);
    const selectedItem = ref<SpecialDayDto | null>(null);
    const selectedDate = ref<Date | null>(null);
    const selectedEvaluationPeriodId = ref<number>(0);
    const submitted = ref(false);

    // Initial Load
    onMounted(async () => {
        try {
            await academicYearStore.fetchAcademicYears();
            // Find current academic year or use first
            // Assuming we want the 'current' or first available for now.
            // In a real app, we might have a global selector context.
            // Check if there is an academic year marked as Current? Dto doesn't show it but Status enum exists.
            // Let's just pick the first one for now or loop to find one with status 'Current' (value 1) if exposed.
            // AcademicYearDto has status.

            const currentYear = academicYearStore.academicYears.find(y => y.status === 1 /* Current */)
                             || academicYearStore.academicYears[0];

            if (currentYear) {
                // Set dates
                const start = parseISO(currentYear.startDate as string);
                const end = parseISO(currentYear.endDate as string);
                minDate.value = start;
                maxDate.value = end;

                // Calculate months
                numberOfMonths.value = differenceInMonths(end, start) + 2; // +1 to include end month, +1 buffer

                // Fetch data
                await evaluationPeriodStore.fetchEvaluationPeriods(); // Assuming get all
                // Filter periods for this year if store returns all,
                // but usually fetchByAcademicYear is better if available.
                // Assuming fetchEvaluationPeriods fetches all, we filter in memory or we need a specific call.
                // Let's just fetch special days for this year.
                await specialDayStore.loadByAcademicYear(currentYear.id as number);
            }
        } catch (error) {
            notify.showError(error, t("common.notifications.loadError"));
        }
    });

    // Helpers
    const getSpecialDay = (date: any) => {
        // PrimeVue DatePicker passes date object in slot props
        const d = date.year !== undefined ? new Date(date.year, date.month, date.day) : date;
        return specialDayStore.specialDays.find(sd => isSameDay(parseISO(sd.date), d));
    };

    const getEvaluationPeriodForDate = (date: Date) => {
        // Find period that covers this date
        // evaluationPeriodStore.evaluationPeriods is the list.
        // Needs proper date comparison
        const dStr = format(date, 'yyyy-MM-dd');
        return evaluationPeriodStore.evaluationPeriods.find(ep =>
             ep.startDate as string <= dStr && ep.endDate as string >= dStr
        );
    };

    // Actions
    const openNew = (date: Date) => {
        const period = getEvaluationPeriodForDate(date);
        if (!period) {
             notify.showError(t("schoolSettings.specialDay.noPeriodFound"));
             return;
        }

        selectedItem.value = null;
        selectedDate.value = date;
        selectedEvaluationPeriodId.value = period.id as number;
        submitted.value = false;
        itemDialog.value = true;
    };

    const editItem = (item: SpecialDayDto) => {
        selectedItem.value = { ...item };
        selectedDate.value = parseISO(item.date);
        selectedEvaluationPeriodId.value = item.evaluationPeriodId;
        itemDialog.value = true;
    };

    const hideDialog = () => {
        itemDialog.value = false;
        submitted.value = false;
    };

    const saveItem = async (data: any) => {
        submitted.value = true;
        try {
            if (selectedItem.value && selectedItem.value.id > 0) {
                await specialDayStore.update(selectedItem.value.id, data);
                notify.showSuccess(t("common.notifications.updateSuccess"));
            } else {
                await specialDayStore.create(data);
                notify.showSuccess(t("common.notifications.createSuccess"));
            }
            hideDialog();
        } catch (error) {
            notify.showError(error, t("common.notifications.operationError"));
        }
    };

    const onConfirmDelete = (item: SpecialDayDto) => {
        confirmDelete({
            item: item,
            deleteAction: specialDayStore.remove,
            onSuccess: () => {}, // Store updates reactive list automatically
        });
    };

    // Visuals
    const getCircleClass = (type: SpecialDayType) => {
        switch (type) {
            case SpecialDayType.SchoolClosure:
                return 'bg-red-500 text-white';
            case SpecialDayType.TimingChange:
                return 'bg-orange-500 text-white';
            default:
                return 'bg-primary text-primary-contrast';
        }
    };

</script>

<template>
    <div class="card w-full animate fade-in">
        <DatePicker
            v-model="dateRange"
            :numberOfMonths="numberOfMonths"
            :minDate="minDate"
            :maxDate="maxDate"
            :disabledDays="[0, 6]"
            selectionMode="multiple"
            inline
            class="w-full shadow-sm border-round-xl special-days-calendar"
            fluid
            :pt="{
                title: { class: 'flex justify-start items-center gap-2 ml-2 w-full font-bold' },
                previousButton: { class: 'hidden' },
                nextButton: { class: 'hidden' },
                dayView: { class: 'text-sm' },
                header: { class: 'border-none bg-transparent pb-3' },
            }"
        >
            <template #date="slotProps">
                 <!-- Disabled Days (Weekends) -->
                 <template v-if="[0, 6].includes(new Date(slotProps.date.year, slotProps.date.month, slotProps.date.day).getDay())">
                    <div class="p-2"></div>
                 </template>
                 <!-- Valid Days -->
                 <template v-else>
                     <div class="flex flex-col items-center justify-center gap-1 h-full w-full p-1 relative group cursor-pointer"
                          style="min-height: 4rem;">

                        <!-- Day Number Circle -->
                        <div
                            :class="[
                                'w-8 h-8 flex items-center justify-center rounded-full text-sm font-bold transition-all',
                                getSpecialDay(slotProps.date) ? getCircleClass(getSpecialDay(slotProps.date)!.type) : 'bg-surface-100 text-surface-600 dark:bg-surface-700 dark:text-surface-400'
                            ]"
                        >
                            {{ slotProps.date.day }}
                        </div>

                        <!-- Hover Actions -->
                        <div class="flex gap-1 opacity-0 group-hover:opacity-100 transition-opacity absolute -bottom-1 z-10 bg-surface-0 dark:bg-surface-900 rounded-full shadow-md p-0.5">
                            <template v-if="getSpecialDay(slotProps.date)">
                                <Button
                                    icon="pi pi-pencil"
                                    text
                                    rounded
                                    severity="warn"
                                    size="small"
                                    class="!w-6 !h-6 !p-0"
                                    @click.stop="editItem(getSpecialDay(slotProps.date)!)"
                                />
                                <Button
                                    icon="pi pi-trash"
                                    text
                                    rounded
                                    severity="danger"
                                    size="small"
                                    class="!w-6 !h-6 !p-0"
                                    :loading="deletingItemId === getSpecialDay(slotProps.date)!.id"
                                    @click.stop="onConfirmDelete(getSpecialDay(slotProps.date)!)"
                                />
                            </template>
                            <template v-else>
                                <Button
                                    icon="pi pi-plus"
                                    text
                                    rounded
                                    severity="success"
                                    size="small"
                                    class="!w-6 !h-6 !p-0"
                                    @click.stop="openNew(new Date(slotProps.date.year, slotProps.date.month, slotProps.date.day))"
                                />
                            </template>
                        </div>
                     </div>
                 </template>
            </template>
        </DatePicker>
    </div>

    <Dialog
        v-model:visible="itemDialog"
        :style="{ width: '50vw' }"
        :breakpoints="{ '960px': '75vw', '640px': '90vw' }"
        :header="selectedItem ? t('schoolSettings.specialDay.editHeader') : t('schoolSettings.specialDay.newHeader')"
        :modal="true"
        class="p-fluid"
    >
        <SpecialDayForm
            :initialData="selectedItem"
            :date="selectedDate"
            :evaluationPeriodId="selectedEvaluationPeriodId"
            :submitted="submitted"
            :loading="specialDayStore.loading"
            @submit="saveItem"
            @cancel="hideDialog"
        />
    </Dialog>
    <ConfirmDialog />
</template>

<style scoped lang="scss">
    :deep(.p-datepicker-calendar-container) {
        display: flex;
        flex-direction: column;
        gap: 1rem;
    }

    :deep(.p-datepicker-calendar-container .p-datepicker-calendar) {
        border-inline-start: 0;
    }
</style>
