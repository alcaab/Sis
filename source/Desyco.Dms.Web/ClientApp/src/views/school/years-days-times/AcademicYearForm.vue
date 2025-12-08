<script setup lang="ts">
import { ref, watch } from 'vue';
import { AcademicYearStatus, type AcademicYearDto } from '@/types/academic-year';
import { useRouter } from 'vue-router'; // Necesario para el router.push en handleCancel

const props = defineProps<{
    initialData?: Partial<AcademicYearDto>;
    isEditing: boolean;
    loading: boolean;
}>();

const emit = defineEmits(['submit', 'cancel']);

const router = useRouter(); // Inicializar router

// Interfaz local para el estado del formulario, asegurando que las fechas sean Date o null (no string) para el componente Calendar
interface FormState extends Omit<AcademicYearDto, 'startDate' | 'endDate'> {
    id?: number; // Add id to FormState as it will be present for edits
    startDate: Date | null;
    endDate: Date | null;
}

const form = ref<FormState>({
    name: '',
    status: AcademicYearStatus.Upcoming,
    startDate: null,
    endDate: null
});

const submitted = ref(false); // Para mostrar errores solo después del submit

// Watch for initialData changes to populate the form
watch(
    () => props.initialData,
    (newVal) => {
        if (newVal) {
            form.value.id = newVal.id; // Assign ID for edit
            form.value.name = newVal.name || '';
            form.value.status = newVal.status ?? AcademicYearStatus.Upcoming;
            form.value.startDate = newVal.startDate instanceof Date ? newVal.startDate : typeof newVal.startDate === 'string' && newVal.startDate ? new Date(newVal.startDate) : null;
            form.value.endDate = newVal.endDate instanceof Date ? newVal.endDate : typeof newVal.endDate === 'string' && newVal.endDate ? new Date(newVal.endDate) : null;
        }
    },
    { immediate: true, deep: true }
); // Trigger immediately on mount and watch deeply for changes

const statuses = ref([
    { label: 'Upcoming', value: AcademicYearStatus.Upcoming },
    { label: 'Current', value: AcademicYearStatus.Current },
    { label: 'Closed', value: AcademicYearStatus.Closed }
]);

const handleSubmit = () => {
    submitted.value = true; // Marca que se ha intentado enviar el formulario

    // Validaciones
    if (form.value.name?.trim() && form.value.startDate && form.value.endDate && form.value.status !== undefined) {
        emit('submit', form.value);
    }
};

const handleCancel = () => {
    router.back(); // Volver a la página anterior
};
</script>

<template>
    <div class="card">
        <div class="font-semibold text-xl mb-6">{{ isEditing ? 'Edit' : 'Create' }} Academic Year</div>

        <form @submit.prevent="handleSubmit" class="flex flex-col gap-4">
            <!-- Name Field -->
            <div class="grid grid-cols-12 gap-2 items-center">
                <label for="name" class="col-span-12 md:col-span-2 font-medium text-surface-700 dark:text-surface-0"> Name <span class="text-red-500">*</span> </label>
                <div class="col-span-12 md:col-span-10">
                    <InputText id="name" v-model="form.name" class="w-full" :invalid="submitted && !form.name" required />
                    <small class="text-red-500" v-if="submitted && !form.name">Name is required.</small>
                </div>
            </div>

            <!-- Start Date Field -->
            <div class="grid grid-cols-12 gap-2 items-center">
                <label for="startDate" class="col-span-12 md:col-span-2 font-medium text-surface-700 dark:text-surface-0"> Start Date <span class="text-red-500">*</span> </label>
                <div class="col-span-12 md:col-span-10">
                    <Calendar id="startDate" v-model="form.startDate" showIcon dateFormat="yy-mm-dd" :invalid="submitted && !form.startDate" required />
                    <small class="text-red-500" v-if="submitted && !form.startDate">Start Date is required.</small>
                </div>
            </div>

            <!-- End Date Field -->
            <div class="grid grid-cols-12 gap-2 items-center">
                <label for="endDate" class="col-span-12 md:col-span-2 font-medium text-surface-700 dark:text-surface-0"> End Date <span class="text-red-500">*</span> </label>
                <div class="col-span-12 md:col-span-10">
                    <Calendar id="endDate" v-model="form.endDate" showIcon dateFormat="yy-mm-dd" :invalid="submitted && !form.endDate" required />
                    <small class="text-red-500" v-if="submitted && !form.endDate">End Date is required.</small>
                </div>
            </div>

            <!-- Status Field -->
            <div class="grid grid-cols-12 gap-2 items-center">
                <label for="status" class="col-span-12 md:col-span-2 font-medium text-surface-700 dark:text-surface-0"> Status <span class="text-red-500">*</span> </label>
                <div class="col-span-12 md:col-span-10">
                    <SelectButton v-model="form.status" :options="statuses" optionLabel="label" optionValue="value" class="w-full md:w-auto" :invalid="submitted && form.status === undefined" required />
                    <small class="text-red-500" v-if="submitted && form.status === undefined">Status is required.</small>
                </div>
            </div>

            <!-- Actions -->
            <div class="flex justify-end gap-2 mt-4">
                <Button label="Cancel" severity="secondary" outlined @click="handleCancel" />
                <Button label="Save" type="submit" :loading="loading" icon="pi pi-check" />
            </div>
        </form>
    </div>
</template>
