<script setup lang="ts">
    import { onMounted, watch, computed } from "vue";
    import type { EvaluationPeriodDto } from "@/types/evaluation-period";
    import { useAcademicYearStore } from "@/stores/academicYearStore";
    import { useEducationalLevelStore } from "@/stores/educationalLevelStore";
    import FormField from "@/components/common/FormField.vue";
    import { useForm } from "vee-validate";
    import * as yup from "yup";
    import { toTypedSchema } from "@vee-validate/yup";
    import { useI18n } from "vue-i18n";

    const { t } = useI18n();
    const academicYearStore = useAcademicYearStore();
    const educationalLevelStore = useEducationalLevelStore();

    const props = defineProps<{
        initialData?: Partial<EvaluationPeriodDto>;
        isEditing: boolean;
        loading: boolean;
    }>();

    const emit = defineEmits(["submit", "cancel"]);

    const schema = toTypedSchema(
        yup.object({
            id: yup.number().optional(),
            academicYearId: yup.number().required(t("common.validations.required")),
            levelTypeId: yup.number().required(t("common.validations.required")),
            name: yup.string().required(t("common.validations.required")),
            shortName: yup.string().required(t("common.validations.required")),
            startDate: yup.date().typeError(t("common.validations.required")).required(),
            endDate: yup.date().typeError(t("common.validations.required")).required(),
            sequence: yup.number().nullable().optional(),
            weight: yup.number().min(0).max(100).required(t("common.validations.required")),
        }),
    );

    const { defineField, handleSubmit, resetForm, errors } = useForm<EvaluationPeriodDto>({
        validationSchema: schema,
        initialValues: {
            academicYearId: 0,
            levelTypeId: 1,
            name: "",
            shortName: "",
            startDate: null,
            endDate: null,
            weight: 0,
            sequence: null,
        },
    });

    const [academicYearId] = defineField("academicYearId");
    const [levelTypeId] = defineField("levelTypeId");
    const [name] = defineField("name");
    const [shortName] = defineField("shortName");
    const [startDate] = defineField("startDate");
    const [endDate] = defineField("endDate");
    const [sequence] = defineField("sequence");
    const [weight] = defineField("weight");

    const parseLocalDate = (dateVal: string | Date | null | undefined): Date | null => {
        if (!dateVal) return null;
        if (dateVal instanceof Date) return dateVal;

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

    onMounted(async () => {
        if (academicYearStore.academicYears.length === 0) {
            await academicYearStore.fetchAcademicYears();
        }
        if (educationalLevelStore.educationalLevels.length === 0) {
            await educationalLevelStore.fetchEducationalLevels();
        }
    });

    watch(
        () => props.initialData,
        (newVal) => {
            if (newVal) {
                resetForm({
                    values: {
                        ...newVal,
                        startDate: parseLocalDate(newVal.startDate),
                        endDate: parseLocalDate(newVal.endDate),
                    } as any,
                });
            }
        },
        { immediate: true, deep: true },
    );

    const onSubmit = handleSubmit((values) => {
        // Ensure dates are sent as YYYY-MM-DD strings for DateOnly compatibility
        const payload = {
            ...values,
            startDate:
                values.startDate instanceof Date ? values.startDate.toLocaleDateString("en-CA") : values.startDate,
            endDate: values.endDate instanceof Date ? values.endDate.toLocaleDateString("en-CA") : values.endDate,
        };
        emit("submit", payload);
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
            :label="t('schoolSettings.evaluationPeriod.academicYear')"
            id="academicYearId"
            required
        >
            <Select
                id="academicYearId"
                v-model="academicYearId"
                :options="academicYearStore.academicYears"
                optionLabel="name"
                optionValue="id"
                :placeholder="t('common.placeholders.select')"
                class="w-sm"
                :invalid="!!errors.academicYearId"
            />
        </FormField>

        <FormField
            :label="t('schoolSettings.evaluationPeriod.levelType')"
            id="levelTypeId"
            required
        >
            <Select
                id="levelTypeId"
                v-model="levelTypeId"
                :options="educationalLevelStore.educationalLevels"
                optionLabel="name"
                optionValue="id"
                :placeholder="t('common.placeholders.select')"
                class="w-sm"
                :invalid="!!errors.levelTypeId"
            />
        </FormField>

        <FormField
            :label="t('schoolSettings.evaluationPeriod.name')"
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
            :label="t('schoolSettings.evaluationPeriod.shortName')"
            id="shortName"
            required
        >
            <InputText
                id="shortName"
                v-model="shortName"
                class="w-sm"
                :invalid="!!errors.shortName"
            />
        </FormField>

        <FormField
            :label="t('schoolSettings.evaluationPeriod.startDate')"
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
            :label="t('schoolSettings.evaluationPeriod.endDate')"
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
            :label="t('schoolSettings.evaluationPeriod.weight')"
            id="weight"
            required
        >
            <InputNumber
                id="weight"
                v-model="weight"
                suffix="%"
                :min="0"
                :max="100"
                :minFractionDigits="2"
                :maxFractionDigits="3"
                :invalid="!!errors.weight"
            />
        </FormField>

        <FormField
            :label="t('schoolSettings.evaluationPeriod.sequence')"
            id="sequence"
        >
            <InputNumber
                id="sequence"
                v-model="sequence"
                :useGrouping="false"
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
