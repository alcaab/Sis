<script setup lang="ts">
import { Dialog, Button } from "primevue";
import { useI18n } from "vue-i18n";

const { t } = useI18n();

const props = defineProps<{ 
  visible: boolean; 
  content: string;
  data: any;
}>();
const emit = defineEmits(["update:visible", "delete"]);

function changeVisibility(value: boolean): void {
  emit("update:visible", value);
}

function accept(): void {
  emit("delete", props.data);
  changeVisibility(false);
}
</script>

<template>
  <Dialog modal :visible="visible" @update:visible="changeVisibility($event)">
    <template #container>
      <div
        class="flex flex-col items-center p-8 bg-surface-0 dark:bg-surface-900 rounded-border">
        <div
          class="rounded-full bg-primary text-primary-contrast inline-flex justify-center items-center h-16 w-16 -mt-16">
          <i :class="`pi pi-question text-xl`"></i>
        </div>

        <span class="font-bold text-2xl block mb-2 mt-6">
          {{ t("common.confirm") }}
        </span>

        <div class="flex flex-wrap">
          {{ t("common.deleteConfirm", { content: content }) }}
        </div>
        <div class="flex items-center gap-2 mt-6">
          <Button
            autofocus
            :label="t('common.yes')"
            @click="accept"
            icon="pi pi-check"></Button>

          <Button
            :label="t('common.no')"
            @click="changeVisibility(false)"
            icon="pi pi-times"
            severity="secondary"></Button>
        </div>
      </div>
    </template>
  </Dialog>
</template>

<style scoped></style>
