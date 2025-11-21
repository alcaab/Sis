<script setup lang="ts">
import InputText from "primevue/inputtext";
import { useI18n } from "vue-i18n";
const { t } = useI18n();

defineProps<{
  modelValue: string;
  writable: boolean | null;
  error?: string | undefined;
}>();

const emit = defineEmits<{
  (e: "update:modelValue", value: string): void;
  (e: "update:writable", value: boolean | null): void;
}>();

function onInput(e: Event) {
  emit("update:writable", null);
  emit("update:modelValue", (e.target as HTMLInputElement).value);
}

</script>

<template>
  <div class="flex flex-col gap-2">
    <InputText
      :value="modelValue"
      @input="onInput"
      :class="{ 'p-invalid': !!error || (writable !== null && !writable) }"
      required
    />
    <small v-if="error" class="text-red-500 text-xs">
      {{ error }}
    </small>
    <small v-if="writable !== null && !writable" class="text-red-500 text-xs">
      {{ t('appSettings.exportPath.pathNotWritable') }}
    </small>
  </div>
</template>
