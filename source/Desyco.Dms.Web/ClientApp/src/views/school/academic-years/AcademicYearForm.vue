<script setup lang="ts">
import { watch } from 'vue';
import { AcademicYearStatus, type AcademicYearDto } from '@/types/academic-year';
import FormField from '@/components/common/FormField.vue';
import { useForm } from 'vee-validate';
import * as yup from 'yup';
import { toTypedSchema } from '@vee-validate/yup';

const props = defineProps<{
    initialData?: Partial<AcademicYearDto>;
    isEditing: boolean;
    loading: boolean;
}>();

const emit = defineEmits(['submit', 'cancel']);

const schema = toTypedSchema(
    yup.object({
        id: yup.number().optional(),
        name: yup.string().required('Name is required'),
        startDate: yup.date().typeError('Start Date is required').required('Start Date is required'),
        endDate: yup.date().typeError('End Date is required').required('End Date is required'),
        status: yup.number().required('Status is required')
    })
);

const { defineField, handleSubmit, resetForm, errors } = useForm<AcademicYearDto>({
    validationSchema: schema,
    initialValues: {
        name: '',
        status: AcademicYearStatus.Upcoming,
        startDate: null,
        endDate: null
    }
});

const [name] = defineField('name');
const [startDate] = defineField('startDate');
const [endDate] = defineField('endDate');
const [status] = defineField('status');

const parseLocalDate = (dateVal: string | Date | null | undefined): Date | null => {
    if (!dateVal) return null;
    if (dateVal instanceof Date) return dateVal;

    // Handle YYYY-MM-DD string to prevent UTC conversion issues
    const strVal = dateVal.toString();
    if (/^\d{4}-\d{2}-\d{2}$/.test(strVal)) {
        const [year, month, day] = strVal.split('-').map(Number);
        return new Date(year, month - 1, day);
    }

    return new Date(dateVal);
};

// Watch for initialData changes to repopulate form
watch(
    () => props.initialData,
    (newVal) => {
        if (newVal) {
            resetForm({
                values: {
                    id: newVal.id,
                    name: newVal.name || '',
                    status: newVal.status ?? AcademicYearStatus.Upcoming,
                    startDate: parseLocalDate(newVal.startDate),
                    endDate: parseLocalDate(newVal.endDate)
                }
            });
        }
    },
    { immediate: true, deep: true }
);

const statuses = [
    { label: 'Upcoming', value: AcademicYearStatus.Upcoming },
    { label: 'Current', value: AcademicYearStatus.Current },
    { label: 'Closed', value: AcademicYearStatus.Closed }
];

const onSubmit = handleSubmit((values) => {
    emit('submit', values);
});

const handleCancel = () => {
    emit('cancel');
};
</script>

<template>
    <div class="card">
        <div class="font-semibold text-xl mb-6">{{ isEditing ? 'Edit' : 'Create' }} Academic Year</div>

        <form @submit="onSubmit" class="flex flex-col gap-4">
            <FormField label="Name" id="name" required :error="errors.name">
                <InputText id="name" v-model="name" class="w-full" :invalid="!!errors.name" />
            </FormField>

            <FormField label="Start Date" id="startDate" required :error="errors.startDate">
                <DatePicker id="startDate" v-model="startDate" showIcon dateFormat="yy-mm-dd" :invalid="!!errors.startDate" />
            </FormField>

            <FormField label="End Date" id="endDate" required :error="errors.endDate">
                <DatePicker id="endDate" v-model="endDate" showIcon dateFormat="yy-mm-dd" :invalid="!!errors.endDate" />
            </FormField>

            <FormField label="Status" id="status" required :error="errors.status">
                <SelectButton v-model="status" :options="statuses" optionLabel="label" optionValue="value" class="w-full md:w-auto" :invalid="!!errors.status" />
            </FormField>

            <div class="flex justify-end gap-2 mt-4">
                <Button label="Cancel" severity="secondary" outlined @click="handleCancel" />
                <Button label="Save" type="submit" :loading="loading" icon="pi pi-check" />
            </div>
        </form>
    </div>
</template>
