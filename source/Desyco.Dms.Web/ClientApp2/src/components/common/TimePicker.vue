<script setup lang="ts">
import DatePicker from "primevue/datepicker";
import {useI18n} from "vue-i18n";

defineProps<{
  modelValue: string;
  error?: string | undefined;
}>();

const { t } = useI18n();

const emit = defineEmits<{
  (e: "update:modelValue", value: string | null): void;
}>();

function onChange(value: unknown) {
  if (!(value instanceof Date)) {
    emit("update:modelValue", null);
    return;
  }
  const hours = value.getHours().toString().padStart(2, "0");
  const minutes = value.getMinutes().toString().padStart(2, "0");
  emit("update:modelValue", `${hours}:${minutes}`);
}

function toDate(value: string | null): Date | null {
  if (!value) return null;
  const [h, m] = value.split(":").map(Number);
  const d = new Date();
  d.setHours(h, m, 0, 0);
  return d;
}
</script>

<template>
  <div class="flex flex-col gap-2">
    <DatePicker
      :placeholder="t('time')"
      showIcon
      icon="pi pi-clock"
      iconDisplay="input"
      :model-value="toDate(modelValue)"
      @update:model-value="onChange"
      dateFormat="HH:mm"
      time-only
      hourFormat="24"
      class="w-32"
    />
    <small v-if="error" class="text-red-500 text-xs">{{ error }}</small>
  </div>
</template>
