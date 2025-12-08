<script setup lang="ts">
import { ref, watch } from 'vue';
import { AcademicYearStatus, type AcademicYearDto } from '@/types/academic-year';
import FormField from '@/components/common/FormField.vue';
import { useRouter } from 'vue-router'; // Necesario para el router.push en handleCancel

const props = defineProps<{
    initialData?: Partial<AcademicYearDto>;
    isEditing: boolean;
    loading: boolean;
}>();

const emit = defineEmits(['submit', 'cancel']);

const router = useRouter(); // Inicializar router

interface FormState extends Omit<AcademicYearDto, 'startDate' | 'endDate'> {
    id?: number;
    startDate: Date | null;
    endDate: Date | null;
}

const form = ref<FormState>({
    name: '',
    status: AcademicYearStatus.Upcoming,
    startDate: null,
    endDate: null
});

const submitted = ref(false);

// Watch for initialData changes to populate the form
watch(
    () => props.initialData,
    (newVal) => {
        if (newVal) {
            form.value.id = newVal.id;
            form.value.name = newVal.name || '';
            form.value.status = newVal.status ?? AcademicYearStatus.Upcoming;
            form.value.startDate = newVal.startDate instanceof Date ? newVal.startDate : typeof newVal.startDate === 'string' && newVal.startDate ? new Date(newVal.startDate) : null;
            form.value.endDate = newVal.endDate instanceof Date ? newVal.endDate : typeof newVal.endDate === 'string' && newVal.endDate ? new Date(newVal.endDate) : null;
        }
    },
    { immediate: true, deep: true }
);

const statuses = ref([
    { label: 'Upcoming', value: AcademicYearStatus.Upcoming },
    { label: 'Current', value: AcademicYearStatus.Current },
    { label: 'Closed', value: AcademicYearStatus.Closed }
]);

const handleSubmit = () => {
    submitted.value = true;

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
            <FormField label="Name" id="name" required :error="submitted && !form.name ? 'Name is required.' : ''">
                <InputText id="name" v-model="form.name" class="w-full" :invalid="submitted && !form.name" />
            </FormField>

            <FormField label="Start Date" id="startDate" required :error="submitted && !form.startDate ? 'Start Date is required.' : ''">
                <Calendar id="startDate" v-model="form.startDate" showIcon dateFormat="yy-mm-dd" :invalid="submitted && !form.startDate" />
            </FormField>

            <FormField label="End Date" id="endDate" required :error="submitted && !form.endDate ? 'End Date is required.' : ''">
                <Calendar id="endDate" v-model="form.endDate" showIcon dateFormat="yy-mm-dd" :invalid="submitted && !form.endDate" />
            </FormField>

            <FormField label="Status" id="status" required :error="submitted && form.status === undefined ? 'Status is required.' : ''">
                <SelectButton v-model="form.status" :options="statuses" optionLabel="label" optionValue="value" class="w-full md:w-auto" :invalid="submitted && form.status === undefined" />
            </FormField>

            <div class="flex justify-end gap-2 mt-4">
                <Button label="Cancel" severity="secondary" outlined @click="handleCancel" />
                <Button label="Save" type="submit" :loading="loading" icon="pi pi-check" />
            </div>
        </form>
    </div>
</template>
