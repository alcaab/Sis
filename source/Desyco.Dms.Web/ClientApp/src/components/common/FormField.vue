<script setup lang="ts">
defineProps<{
    label: string;
    id?: string; // Para el 'for' del label
    required?: boolean;
    error?: string | boolean; // El mensaje de error (o booleano false si no hay error)
}>();
</script>

<template>
    <div class="grid grid-cols-12 gap-2 items-center">
        <label :for="id" class="col-span-12 md:col-span-2 font-medium text-surface-700 dark:text-surface-0"> {{ label }} <span v-if="required" class="text-red-500">*</span> </label>
        <div class="col-span-12 md:col-span-10">
            <slot></slot>
            <small v-if="error && typeof error === 'string'" class="text-red-500 block mt-1">{{ error }}</small>
        </div>
    </div>
</template>
