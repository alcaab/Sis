<script setup lang="ts">
import { ref, watch } from "vue";
import MultiSelect from "primevue/multiselect";
import { useI18n } from "vue-i18n";

const { t } = useI18n();

const props = defineProps<{
  modelValue: number;
  error?: string;
}>();

const emit = defineEmits<{
  (e: "update:modelValue", value: number): void;
}>();

const weekDays = [
  { name: t("days.monday"), value: 1 << 0 },
  { name: t("days.tuesday"), value: 1 << 1 },
  { name: t("days.wednesday"), value: 1 << 2 },
  { name: t("days.thursday"), value: 1 << 3 },
  { name: t("days.friday"), value: 1 << 4 },
  { name: t("days.saturday"), value: 1 << 5 },
  { name: t("days.sunday"), value: 1 << 6 },
];

const selectedDays = ref<number[]>([]);

watch(
  () => props.modelValue,
  (val) => {
    selectedDays.value = weekDays
      .filter(day => (val & day.value) !== 0)
      .map(day => day.value);
  },
  { immediate: true }
);

watch(selectedDays, (arr) => {
  const bitmask = arr.reduce((acc, val) => acc | val, 0);
  emit("update:modelValue", bitmask);
});
</script>

<template>
  <div class="flex flex-col gap-2">
    <MultiSelect
      v-model="selectedDays"
      :options="weekDays"
      optionLabel="name"
      optionValue="value"
      :placeholder="t('days.selectDay')"
      display="chip"
      :class="{ 'p-invalid': !!error }"
    />
    <small v-if="error" class="text-red-500 text-xs">{{ error }}</small>
  </div>
</template>
