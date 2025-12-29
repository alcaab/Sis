<script setup lang="ts">
    import { ref } from "vue";

    const dateRange = ref(null);
    const minDate = ref(new Date(2025, 11, 29));
    const maxDate = ref(new Date(2026, 4, 15));
</script>

<template>
    <div class="card w-full animate fade-in">
        <DatePicker
            v-model="dateRange"
            :numberOfMonths="4"
            :minDate="minDate"
            :maxDate="maxDate"
            :defaultValue="minDate"
            :disabledDays="[0, 6]"
            selectionMode="multiple"
            inline
            class="w-full shadow-sm border-round-xl"
            fluid
            :pt="{
                // Alinea el contenedor del título (Mes Año) a la izquierda
                title: { class: 'flex justify-start items-center gap-2 ml-2 w-full font-bold' },

                // Estilo minimalista para los botones de navegación
                previousButton: {
                    class: 'border-none bg-transparent hover:bg-emphasis p-2 rounded-full transition-all duration-200',
                },
                nextButton: {
                    class: 'border-none bg-transparent hover:bg-emphasis p-2 rounded-full transition-all duration-200',
                },

                // Reduce el tamaño de los nombres de los días para mayor elegancia
                dayView: { class: 'text-sm' },
                header: { class: 'border-none bg-transparent pb-3' },
            }"
        >
            <template #previcon>
                <span class="pi pi-chevron-left text-xs" />
            </template>
            <template #nexticon>
                <span class="pi pi-chevron-right text-xs" />
            </template>
        </DatePicker>
    </div>
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
