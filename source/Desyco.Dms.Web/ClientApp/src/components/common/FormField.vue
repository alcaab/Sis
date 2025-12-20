<script setup lang="ts">
    defineProps<{
        label: string;
        id?: string;
        required?: boolean;
        error?: string | boolean;
        hint?: string;
    }>();
</script>

<template>
    <div class="grid grid-cols-12 gap-2 items-center">
        <label
            :for="id"
            class="col-span-12 md:col-span-2 font-medium text-surface-700 dark:text-surface-0"
        >
            {{ label }}
            <span
                v-if="required"
                class="text-red-500"
                >*</span
            >
        </label>
        <div class="col-span-12 md:col-span-10">
            <slot></slot>
            <small
                v-if="hint"
                class="text-surface-500 block mt-1"
                >{{ hint }}</small
            >
            <small
                v-if="error && typeof error === 'string' && error !== 'required'"
                class="text-red-500 block mt-1"
                >{{ error }}</small
            >
        </div>
    </div>
</template>
