<script setup lang="ts">
    import { ref, onMounted, watch } from "vue";
    import { useSpecialDayStore } from "@/stores/specialDayStore";
    import { useNotification } from "@/composables/useNotification";
    import { useI18n } from "vue-i18n";
    import { type SpecialDayDto } from "@/types/special-days";
    import SpecialDayForm from "./SpecialDayForm.vue";
    import { AcademicYearDto, AcademicYearStatus } from "@/types/academic-year.ts";
    import { AcademicYearService } from "@/service/AcademicYearService.ts";
    import CalendarDay from "@/views/school/special-days/CalendarDay.vue";
    import DatePicker from "primevue/datepicker";

    const specialDayStore = useSpecialDayStore();
    const notify = useNotification();
    const { t } = useI18n();
    const itemDialog = ref(false);
    const selectedItem = ref<SpecialDayDto | null>(null);
    const selectedDate = ref<Date | null>(null);
    const selectedEvaluationPeriodId = ref<number>(0);
    const submitted = ref(false);
    const academicYears = ref<AcademicYearDto[]>([]);
    const selectedAcademicYear = ref<number>(0);

    watch(selectedAcademicYear, async (newValue) => {
        await specialDayStore.loadByAcademicYear(newValue);
    });

    // Initial Load
    onMounted(async () => {
        try {
            academicYears.value = (await AcademicYearService.getAcademicYears()).data.results;

            const currentYear =
                academicYears.value.find((y) => y.status === AcademicYearStatus.Current) || academicYears.value[0];

            if (currentYear) {
                selectedAcademicYear.value = currentYear.id as number;
            }
        } catch (error) {
            notify.showError(error, t("common.notifications.loadError"));
        }
    });

    const events = ref([
        {
            date: "2025-01-10",
            name: "Día especial",
            id: 1,
        },
    ]);

    function getDayMeta(date: any) {
        const iso = `${date.year}-${String(date.month + 1).padStart(2, "0")}-${String(date.day).padStart(2, "0")}`;
        return events.value.find((e) => e.date === iso);
    }

    const datePickerPt = {
        day: { class: "h-10 w-10 border-gray-300 md:h-20 md:w-20 md:border-green-500 rounded-md" },
        dayCell: { class: "p-1" },
        panel: { class: "border-none" },
        title: { class: "flex items-center justify-start gap-2 ml-2 w-full font-bold text-xl" },
        pcNextButton: { class: "hidden" },
        previousButton: {
            class: "border-none bg-transparent hover:bg-emphasis p-2 rounded-full transition-all duration-200",
        },
        nextButton: {
            class: "border-none bg-transparent hover:bg-emphasis p-2 rounded-full transition-all duration-200",
        },
        dayView: { class: "text-sm md:text-xl" },
        header: { class: "border-none bg-transparent pb-3 " },
        //tableHeader: { class: "bg-gray-300 rounded-md" },
    };

    function onCreate(date: any) {
        console.log("Crear en:", date);
    }

    function onEdit(event: any) {
        console.log("Editar:", event);
    }

    function onDelete(event: any) {
        console.log("Eliminar:", event);
    }

    const dateRange = ref(null);
    // const minDate = ref(new Date(2025, 11, 29));
    // const maxDate = ref(new Date(2026, 4, 15));

    const parseLocalDate = (dateVal: string | Date | null | undefined): Date => {
        if (!dateVal) return new Date();
        if (dateVal instanceof Date) return dateVal;

        const strVal = dateVal.toString();
        if (/^\d{4}-\d{2}-\d{2}$/.test(strVal)) {
            const [year, month, day] = strVal.split("-").map(Number);
            return new Date(year, month - 1, day);
        }

        return new Date(dateVal);
    };
</script>

<template>
    <div class="card w-full animate fade-in">
        <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4 mb-2">
            <h4 class="m-0 text-xl font-semibold text-surface-900 dark:text-surface-0">
                {{ t("schoolSettings.specialDays.title") }}
            </h4>
            <div class="flex items-center gap-2">
                <Select
                    id="academicYearId"
                    v-model="selectedAcademicYear"
                    :options="academicYears"
                    optionLabel="name"
                    optionValue="id"
                    :placeholder="t('common.placeholders.select')"
                    class="w-3xs"
                />
            </div>
        </div>
        <div class="flex flex-col md:flex-row">
            <Tabs
                :value="1"
                class="w-full"
            >
                <TabList>
                    <Tab
                        v-for="tab in specialDayStore.specialDayGroups"
                        :key="tab.id"
                        :value="tab.id"
                        >{{ tab.name }}</Tab
                    >
                </TabList>
                <TabPanels>
                    <TabPanel
                        v-for="tab in specialDayStore.specialDayGroups"
                        :key="tab.id"
                        :value="tab.id"
                        class="w-ful"
                    >
                        <div class="flex flex-col gap-2.5">
                            <Panel
                                v-for="period in tab.periods"
                                :key="period.id"
                                :value="String(period.id)"
                                toggleable
                            >
                                <template #header>
                                    <div class="flex items-center gap-2">
                                        <i class="pi pi-calendar" />
                                        <h5 class="font-bold uppercase">{{ period.name }}:</h5>
                                        <span>from {{ period.startDate }} to {{ period.endDate }}</span>
                                    </div>
                                </template>
                                <DatePicker
                                    v-model="dateRange"
                                    selectionMode="multiple"
                                    inline
                                    :numberOfMonths="period.numberOfMonths"
                                    :minDate="parseLocalDate(period.startDate)"
                                    :maxDate="parseLocalDate(period.endDate)"
                                    :viewDate="parseLocalDate(period.startDate)"
                                    :showOtherMonths="false"
                                    :showButtonBar="false"
                                    :hideOnDateTimeSelect="true"
                                    :disabledDays="[0, 6]"
                                    :pt="datePickerPt"
                                    :prevIcon="undefined"
                                    :nextIcon="undefined"
                                    fluid
                                    class="border-none"
                                >
                                    <template #date="slotProps">
                                        <CalendarDay
                                            :date="slotProps.date"
                                            :meta="getDayMeta(slotProps.date)"
                                            :disabledDays="[0, 6]"
                                            @create="onCreate"
                                            @edit="onEdit"
                                            @delete="onDelete"
                                        />
                                    </template>
                                </DatePicker>
                            </Panel>
                        </div>
                    </TabPanel>
                </TabPanels>
            </Tabs>
        </div>
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
        />
    </Dialog>
    <ConfirmDialog />
</template>

<style scoped lang="scss">
    //:deep(.p-datepicker-calendar-container) {
    //    display: grid;
    //    grid-template-columns: repeat(2, 1fr);
    //    gap: 1rem;
    //}
    :deep(.p-datepicker-calendar-container) {
        display: flex;
        flex-direction: column;
        gap: 1rem;
    }

    :deep(.p-datepicker-calendar-container .p-datepicker-calendar) {
        border-inline-start: 0;
    }

    :deep(.p-tabpanels) {
        padding: 0;
    }
</style>
