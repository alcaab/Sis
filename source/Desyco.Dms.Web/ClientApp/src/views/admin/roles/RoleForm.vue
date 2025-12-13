<script setup lang="ts">
    import { watch } from "vue";
    import { useForm } from "vee-validate";
    import * as yup from "yup";
    import { toTypedSchema } from "@vee-validate/yup";
    import type { RoleDto, CreateRoleDto } from "@/types/role";
    import FormField from "@/components/common/FormField.vue";

    const props = defineProps<{
        initialData?: RoleDto | null;
        submitted: boolean;
        loading: boolean;
    }>();

    const emit = defineEmits(["submit", "cancel"]);

    const schema = toTypedSchema(
        yup.object({
            name: yup.string().required("Name is required"),
            description: yup.string().nullable().optional(),
        }),
    );

    const { defineField, handleSubmit, resetForm, errors } = useForm<CreateRoleDto>({
        validationSchema: schema,
        initialValues: {
            name: "",
            description: null,
        },
    });

    const [name] = defineField("name");
    const [description] = defineField("description");

    // Watch for changes in initialData to populate the form
    watch(
        () => props.initialData,
        (newVal) => {
            if (newVal) {
                resetForm({
                    values: {
                        name: newVal.name,
                        description: newVal.description,
                    },
                });
            } else {
                resetForm({
                    values: {
                        name: "",
                        description: null,
                    },
                });
            }
        },
        { immediate: true },
    );

    const onSubmit = handleSubmit((values) => {
        emit("submit", values);
    });

    const handleCancel = () => {
        emit("cancel");
    };
</script>

<template>
    <form @submit="onSubmit">
        <div class="flex flex-col gap-4">
            <FormField
                label="Name"
                id="name"
                required
                :error="errors.name"
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
                label="Description"
                id="description"
                :error="errors.description"
            >
                <Textarea
                    id="description"
                    v-model="description"
                    rows="3"
                    class="w-full"
                    :invalid="!!errors.description"
                />
            </FormField>
        </div>

        <div class="flex justify-end gap-2 mt-6">
            <Button
                label="Cancel"
                icon="pi pi-times"
                text
                @click="handleCancel"
            />
            <Button
                label="Save"
                icon="pi pi-check"
                type="submit"
                :loading="loading"
            />
        </div>
    </form>
</template>
