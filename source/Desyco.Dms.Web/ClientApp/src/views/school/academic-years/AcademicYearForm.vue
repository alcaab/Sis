<script setup lang="ts">
    import { watch, computed } from "vue";
    import { AcademicYearStatus, type AcademicYearDto } from "@/types/academic-year";
    import FormField from "@/components/common/FormField.vue";
    import { useForm } from "vee-validate";
    import * as yup from "yup";
    import { toTypedSchema } from "@vee-validate/yup";
    import { useI18n } from "vue-i18n";

    const { t } = useI18n();
    const props = defineProps<{
        initialData?: Partial<AcademicYearDto>;
        isEditing: boolean;
        loading: boolean;
    }>();

    const emit = defineEmits(["submit", "cancel"]);

    const schema = toTypedSchema(
        yup.object({
            id: yup.number().optional(),
            name: yup.string().required(),
            startDate: yup.date().typeError(t("common.validations.required")).required(),
            endDate: yup.date().typeError(t("common.validations.required")).required(),
            status: yup.number().required(),
        }),
    );

    const { defineField, handleSubmit, resetForm, errors } = useForm<AcademicYearDto>({
        validationSchema: schema,
        initialValues: {
            name: "",
            status: AcademicYearStatus.Upcoming,
            startDate: null,
            endDate: null,
        },
    });

    const [name] = defineField("name");
    const [startDate] = defineField("startDate");
    const [endDate] = defineField("endDate");
    const [status] = defineField("status");

    const parseLocalDate = (dateVal: string | Date | null | undefined): Date | null => {
        if (!dateVal) return null;
        if (dateVal instanceof Date) return dateVal;

        // Handle YYYY-MM-DD string to prevent UTC conversion issues
        const strVal = dateVal.toString();
        if (/^\d{4}-\d{2}-\d{2}$/.test(strVal)) {
            const [year, month, day] = strVal.split("-").map(Number);
            return new Date(year, month - 1, day);
        }

        return new Date(dateVal);
    };

    const startDateProxy = computed({
        get: () => parseLocalDate(startDate.value),
        set: (val) => {
            startDate.value = val;
        },
    });

    const endDateProxy = computed({
        get: () => parseLocalDate(endDate.value),
        set: (val) => {
            endDate.value = val;
        },
    });

    // Watch for initialData changes to repopulate form
    watch(
        () => props.initialData,
        (newVal) => {
            if (newVal) {
                resetForm({
                    values: {
                        id: newVal.id,
                        name: newVal.name || "",
                        status: newVal.status ?? AcademicYearStatus.Upcoming,
                        startDate: parseLocalDate(newVal.startDate),
                        endDate: parseLocalDate(newVal.endDate),
                    },
                });
            }
        },
        { immediate: true, deep: true },
    );

    const statuses = computed(() => [
        { label: t("schoolSettings.academicYear.statusLabels.upcoming"), value: AcademicYearStatus.Upcoming },
        { label: t("schoolSettings.academicYear.statusLabels.current"), value: AcademicYearStatus.Current },
        { label: t("schoolSettings.academicYear.statusLabels.closed"), value: AcademicYearStatus.Closed },
    ]);

    const onSubmit = handleSubmit((values) => {
        emit("submit", values);
    });

    const handleCancel = () => {
        emit("cancel");
    };
</script>

<template>
    <form
        @submit="onSubmit"
        class="flex flex-col gap-4"
        v-focustrap
    >
        <FormField
            :label="t('schoolSettings.academicYear.name')"
            id="name"
            required
        >
            <InputText
                id="name"
                v-model="name"
                class="w-full"
                :invalid="!!errors.name"
                autofocus
            />
        </FormField>

        <FormField
            :label="t('schoolSettings.academicYear.startDate')"
            id="startDate"
            required
        >
            <DatePicker
                id="startDate"
                v-model="startDateProxy"
                showIcon
                dateFormat="yy-mm-dd"
                :invalid="!!errors.startDate"
            />
        </FormField>

        <FormField
            :label="t('schoolSettings.academicYear.endDate')"
            id="endDate"
            required
        >
            <DatePicker
                id="endDate"
                v-model="endDateProxy"
                showIcon
                dateFormat="yy-mm-dd"
                :invalid="!!errors.endDate"
            />
        </FormField>

        <FormField
            :label="t('schoolSettings.academicYear.status')"
            id="status"
            required
        >
            <SelectButton
                v-model="status"
                :options="statuses"
                optionLabel="label"
                optionValue="value"
                class="w-full md:w-auto"
                :invalid="!!errors.status"
            />
        </FormField>

        <div class="flex justify-end gap-2 mt-4">
            <Button
                :label="t('common.buttons.cancel')"
                severity="secondary"
                outlined
                @click="handleCancel"
            />
            <Button
                :label="t('common.buttons.save')"
                type="submit"
                :loading="loading"
                icon="pi pi-check"
            />
        </div>
    </form>
</template>
