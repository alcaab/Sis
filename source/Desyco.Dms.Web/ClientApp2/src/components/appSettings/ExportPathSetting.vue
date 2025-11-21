<script setup lang="ts">
import ExportPathInput from "@/components/common/ExportPathInput.vue";

interface Props {
  modelValue: string;
  writable: boolean | null;
  error?: string;
  label: string;
  hint: string;
}

const props = defineProps<Props>();
const emit = defineEmits(["update:modelValue", "update:writable"]);
</script>

<template>
  <div class="mt-4">
    <label>{{ label }}</label><br />
    <small class="netto:text-gray-500">{{ hint }}</small>
    <export-path-input
      :error="error"
      :writable="writable"
      v-model="props.modelValue"
      @update:modelValue="emit('update:modelValue', $event)"
      @update:writable="emit('update:writable', $event)"
    />
  </div>
</template>
