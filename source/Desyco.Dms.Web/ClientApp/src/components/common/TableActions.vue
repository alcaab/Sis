<script setup lang="ts">
    import Button from "primevue/button";
    import { useI18n } from "vue-i18n";

    withDefaults(
        defineProps<{
            id: string | number;
            canEdit?: boolean;
            canDelete?: boolean;
            loading?: boolean;
        }>(),
        {
            canEdit: true,
            canDelete: true,
            loading: false,
        },
    );

    const { t } = useI18n();

    const emit = defineEmits(["edit", "delete"]);
</script>

<template>
    <div class="flex justify-end gap-1">
        <!-- Custom Actions (Left) -->
        <slot
            name="left"
            :id="id"
        ></slot>

        <Button
            v-if="canEdit"
            icon="pi pi-pencil"
            outlined
            severity="success"
            :title="t('common.buttons.edit')"
            @click="emit('edit', id)"
        />

        <Button
            v-if="canDelete"
            icon="pi pi-trash"
            outlined
            severity="danger"
            :loading="loading"
            :title="t('common.buttons.delete')"
            @click="emit('delete', id)"
        />

        <!-- Custom Actions (Right) -->
        <slot
            name="right"
            :id="id"
        ></slot>
    </div>
</template>
