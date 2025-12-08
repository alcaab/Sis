<script setup lang="ts">
    import Button from "primevue/button";

    withDefaults(
        defineProps<{
            id: number;
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

    const emit = defineEmits(["edit", "delete"]);
</script>

<template>
    <div class="flex justify-end gap-2">
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
            @click="emit('edit', id)"
        />

        <Button
            v-if="canDelete"
            icon="pi pi-trash"
            outlined
            severity="danger"
            :loading="loading"
            @click="emit('delete', id)"
        />

        <!-- Custom Actions (Right) -->
        <slot
            name="right"
            :id="id"
        ></slot>
    </div>
</template>
