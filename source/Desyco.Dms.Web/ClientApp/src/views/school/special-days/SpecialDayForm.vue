<script setup lang="ts">
import { computed, watch } from "vue";
    import { useI18n } from "vue-i18n";
    import { useForm } from "vee-validate";
    import * as yup from "yup";
    import { toTypedSchema } from "@vee-validate/yup";
    import { SpecialDayType, type SpecialDayDto } from "@/types/special-days";
    import FormField from "@/components/common/FormField.vue";
    import { enumService } from "@/service/EnumService";
    import { format } from "date-fns";

    const { t } = useI18n();

    const props = defineProps<{
        initialData?: SpecialDayDto | null;
        date: Date | null;
        evaluationPeriodId: number;
        submitted: boolean;
        loading: boolean;
    }>();

    const emit = defineEmits(["submit", "cancel"]);

    const schema = toTypedSchema(
        yup.object({
            id: yup.number().optional(),
            evaluationPeriodId: yup.number().required(),
            type: yup.number().required(t("common.validations.required")),
            name: yup.string().required(t("common.validations.required")).max(100),
            description: yup.string().optional(),
            date: yup.string().required(),
            openTime: yup.string().nullable().optional(),
            startTime: yup.string().nullable().optional(),
            endTime: yup.string().nullable().optional(),
            closeTime: yup.string().nullable().optional(),
        }),
    );

    const { defineField, handleSubmit, resetForm, errors } = useForm<any>({
        validationSchema: schema,
    });

    const [type] = defineField("type");
    const [name] = defineField("name");
    const [description] = defineField("description");
    const [openTime] = defineField("openTime");
    const [startTime] = defineField("startTime");
    const [endTime] = defineField("endTime");
    const [closeTime] = defineField("closeTime");

    // Helper to parse "HH:mm:ss" string to Date for DatePicker
    const parseTime = (timeStr?: string) => {
        if (!timeStr) return undefined;
        const [hours, minutes] = timeStr.split(":").map(Number);
        const d = new Date();
        d.setHours(hours, minutes, 0, 0);
        return d;
    };

    // Helper to format Date to "HH:mm:ss"
    const formatTime = (d?: Date) => {
        if (!d) return undefined;
        return format(d, "HH:mm:ss");
    };

    // Proxies for Time DatePickers
    const openTimeProxy = computed({
        get: () => parseTime(openTime.value),
        set: (val) => openTime.value = formatTime(val)
    });

    const startTimeProxy = computed({
        get: () => parseTime(startTime.value),
        set: (val) => startTime.value = formatTime(val)
    });

    const endTimeProxy = computed({
        get: () => parseTime(endTime.value),
        set: (val) => endTime.value = formatTime(val)
    });

    const closeTimeProxy = computed({
        get: () => parseTime(closeTime.value),
        set: (val) => closeTime.value = formatTime(val)
    });


    watch(
        () => [props.initialData, props.date, props.evaluationPeriodId],
        () => {
            if (props.initialData) {
                resetForm({ values: { ...props.initialData } });
            } else if (props.date) {
                resetForm({
                    values: {
                        id: 0,
                        evaluationPeriodId: props.evaluationPeriodId,
                        type: SpecialDayType.SchoolClosure,
                        name: "",
                        description: "",
                        date: format(props.date, "yyyy-MM-dd"),
                        openTime: null,
                        startTime: null,
                        endTime: null,
                        closeTime: null,
                    },
                });
            }
        },
        { immediate: true },
    );

    const onSubmit = handleSubmit((values) => {
        emit("submit", values);
    });

    // Read-only formatted date
    const displayDate = computed(() => {
        if (props.initialData) return props.initialData.date;
        if (props.date) return format(props.date, "yyyy-MM-dd");
        return "";
    });

</script>

<template>
    <form @submit="onSubmit" class="flex flex-col gap-4">

        <div class="flex flex-col gap-2">
            <label class="font-bold block">{{ t('schoolSettings.specialDay.date') }}</label>
            <InputText :modelValue="displayDate" readonly class="w-full bg-surface-100" />
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
             <FormField
                :label="t('schoolSettings.specialDay.name')"
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
                :label="t('schoolSettings.specialDay.type')"
                id="type"
                required
                :error="errors.type"
            >
                <Select
                    id="type"
                    v-model="type"
                    :options="enumService.specialDayTypes"
                    optionLabel="description"
                    optionValue="id"
                    class="w-full"
                    :invalid="!!errors.type"
                />
            </FormField>
        </div>

        <FormField
            :label="t('schoolSettings.specialDay.description')"
            id="description"
            :error="errors.description"
        >
            <Textarea
                id="description"
                v-model="description"
                rows="3"
                class="w-full"
            />
        </FormField>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
             <FormField :label="t('schoolSettings.specialDay.openTime')" id="openTime">
                <DatePicker v-model="openTimeProxy" timeOnly fluid />
             </FormField>
             <FormField :label="t('schoolSettings.specialDay.closeTime')" id="closeTime">
                <DatePicker v-model="closeTimeProxy" timeOnly fluid />
             </FormField>
             <FormField :label="t('schoolSettings.specialDay.startTime')" id="startTime">
                <DatePicker v-model="startTimeProxy" timeOnly fluid />
             </FormField>
             <FormField :label="t('schoolSettings.specialDay.endTime')" id="endTime">
                <DatePicker v-model="endTimeProxy" timeOnly fluid />
             </FormField>
        </div>

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
