<script setup lang="ts">
    import { onMounted, ref, watch } from "vue";
    import { useForm } from "vee-validate";
    import * as yup from "yup";
    import { useI18n } from "vue-i18n";
    import type { UserDto } from "@/types/user";
    import FormField from "@/components/common/FormField.vue";
    import { roleService } from "@/service/roleService.ts";

    const props = defineProps<{
        initialData?: Partial<UserDto>;
        isEditing: boolean;
        loading: boolean;
    }>();

    const emit = defineEmits(["submit", "cancel"]);
    const { t } = useI18n();

    const validationSchema = yup.object({
        userName: yup.string().trim().required(),
        email: yup.string().trim().email(t("common.validations.email")).required(),
        phoneNumber: yup.string().trim().nullable(),
        firstName: yup.string().trim().nullable(),
        lastName: yup.string().trim().nullable(),
        lockoutEnabled: yup.boolean().required(),
    });

    const { defineField, handleSubmit, resetForm, errors } = useForm<UserDto>({
        validationSchema,
        initialValues: props.initialData,
    });

    const [userName] = defineField("userName");
    const [email] = defineField("email");
    const [phoneNumber] = defineField("phoneNumber");
    const [firstName] = defineField("firstName");
    const [lastName] = defineField("lastName");
    const [lockoutEnabled] = defineField("lockoutEnabled");
    const [roles] = defineField("roles");

    const roleOptions = ref<string[]>([]);

    // Watch for changes in initialData when in edit mode
    watch(
        () => props.initialData,
        (newData) => {
            if (newData) {
                resetForm({ values: newData });
            }
        },
        { deep: true, immediate: true },
    );

    onMounted(async () => {
        roleOptions.value = await roleService.getNames();
    });

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
    >
        <FormField
            :label="t('admin.users.userName')"
            required
        >
            <InputText
                id="userName"
                v-model="userName"
                :invalid="!!errors.userName"
                class="w-full"
            />
        </FormField>
        <FormField
            :label="t('admin.users.email')"
            :error="errors.email"
            required
        >
            <InputText
                id="email"
                v-model="email"
                :invalid="!!errors.email"
                class="w-full"
            />
        </FormField>
        <FormField
            :label="t('admin.users.firstName')"
            class="w-full"
        >
            <InputText
                id="firstName"
                v-model="firstName"
                :invalid="!!errors.firstName"
                class="w-full"
            />
        </FormField>
        <FormField
            :label="t('admin.users.lastName')"
            :error="errors.lastName"
        >
            <InputText
                id="lastName"
                v-model="lastName"
                :invalid="!!errors.lastName"
                class="w-full"
            />
        </FormField>
        <FormField
            :label="t('admin.users.phoneNumber')"
            :error="errors.phoneNumber"
        >
            <InputMask
                id="phoneNumber"
                v-model="phoneNumber"
                mask="(999) 999-9999"
                :invalid="!!errors.phoneNumber"
            />
        </FormField>
        <FormField :label="t('admin.users.lockoutEnabled')">
            <ToggleSwitch
                id="lockoutEnabled"
                v-model="lockoutEnabled"
            />
        </FormField>
        <FormField :label="t('admin.users.roles')">
            <MultiSelect
                v-model="roles"
                display="chip"
                :options="roleOptions"
                filter
                :placeholder="t('common.placeholders.select')"
                class="w-full"
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

<style scoped></style>
