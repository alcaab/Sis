<script setup lang="ts">
    import { ref, watch, computed } from "vue";
    import { useI18n } from "vue-i18n";
    import { useForm } from "vee-validate";
    import * as yup from "yup";
    import { toTypedSchema } from "@vee-validate/yup";
    import { EducationalLevelType, type EducationalLevelDto } from "@/types/educational-level";
    import FormField from "@/components/common/FormField.vue";

    const { t } = useI18n();

    const props = defineProps<{
        initialData?: EducationalLevelDto | null;
        submitted: boolean;
        loading: boolean;
    }>();

    const emit = defineEmits(["submit", "cancel"]);

    const schema = toTypedSchema(
        yup.object({
            id: yup.number().optional(),
            name: yup.string().required(t("common.validations.required")).max(100),
            levelTypeId: yup.number().required(t("common.validations.required")),
            isActive: yup.boolean().required(),
        }),
    );

    const { defineField, handleSubmit, resetForm, errors } = useForm<EducationalLevelDto>({
        validationSchema: schema,
        initialValues: {
            id: 0,
            name: "",
            levelTypeId: EducationalLevelType.Primary,
            isActive: true,
        },
    });

    const [name] = defineField("name");
    const [levelTypeId] = defineField("levelTypeId");
    const [isActive] = defineField("isActive");

    const levelTypes = computed(() => [
        {
            label: t("schoolSettings.educationalLevel.types.earlyChildhood"),
            value: EducationalLevelType.EarlyChildhood,
        },
        { label: t("schoolSettings.educationalLevel.types.primary"), value: EducationalLevelType.Primary },
        { label: t("schoolSettings.educationalLevel.types.secondary"), value: EducationalLevelType.Secondary },
    ]);

    watch(
        () => props.initialData,
        (newVal) => {
            if (newVal) {
                resetForm({ values: newVal });
            } else {
                resetForm({
                    values: {
                        id: 0,
                        name: "",
                        levelTypeId: EducationalLevelType.Primary,
                        isActive: true,
                    },
                });
            }
        },
        { immediate: true },
    );

    const onSubmit = handleSubmit((values) => {
        emit("submit", values);
    });
</script>

<template>
    <form
        @submit="onSubmit"
        class="flex flex-col gap-4"
    >
        <FormField
            :label="t('schoolSettings.educationalLevel.name')"
            id="name"
            required
            :error="errors.name"
        >
            <InputText
                id="name"
                v-model="name"
                :invalid="!!errors.name"
                class="w-full"
                autofocus
            />
        </FormField>

        <FormField
            :label="t('schoolSettings.educationalLevel.levelType')"
            id="levelTypeId"
            required
            :error="errors.levelTypeId"
        >
            <Select
                id="levelTypeId"
                v-model="levelTypeId"
                :options="levelTypes"
                optionLabel="label"
                optionValue="value"
                class="w-sm"
                :placeholder="t('common.placeholders.select')"
                :invalid="!!errors.levelTypeId"
            />
        </FormField>

        <FormField
            :label="t('schoolSettings.educationalLevel.isActive')"
            id="isActive"
        >
            <ToggleSwitch
                id="isActive"
                v-model="isActive"
            />
        </FormField>

        <div class="flex justify-end gap-2 mt-4">
            <Button
                type="button"
                :label="t('common.buttons.cancel')"
                severity="secondary"
                outlined
                @click="emit('cancel')"
            />
            <Button
                type="submit"
                :label="t('common.buttons.save')"
                :loading="loading"
                icon="pi pi-check"
            />
        </div>
    </form>
</template>
