<script setup lang="ts">
import DayOfWeekPicker from "@/components/common/DayOfWeekPicker.vue";
import TimePicker from "@/components/common/TimePicker.vue";

interface Props {
  weekDays: number;
  weekDaysError?: string;
  time: string;
  timeError?: string;
  label: string;
  hint: string;
}

const props = defineProps<Props>();
const emit = defineEmits(["update:weekDays", "update:time"]);
</script>

<template>
  <div class="flex items-start gap-6 mt-4">
    <div class="flex-1 mt-2">
      <label class="block">{{ label }}</label>
      <small class="netto:text-gray-500 block mb-1">{{ hint }}</small>
      <day-of-week-picker
        v-model="props.weekDays"
        :error="weekDaysError"
        @update:modelValue="emit('update:weekDays', $event)"
      />
    </div>
    <div class="flex-1 flex flex-col gap-2 mt-14">
      <time-picker
        v-model="props.time"
        :error="timeError"
        @update:modelValue="emit('update:time', $event)"
      />
    </div>
  </div>
</template>
