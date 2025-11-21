<script setup lang="ts">
import {onMounted, ref, watch} from "vue";
import StoreService from "@/services/storeService";
import type {StoreDto} from "@models";
import {useI18n} from "vue-i18n";
import {useToast} from "primevue";
import {ToastLife} from "@/models/common/toastLife";

const { t } = useI18n();
const toast = useToast();
const loading = ref(false);
const selectedStore = ref<StoreDto | null>(null);
const storeOptions = ref<StoreDto[]>([]);

async function fetchStoreData() {
  loading.value = true;

  await StoreService.getByDisplayName( {
    onSuccess: (data) => {
      storeOptions.value = data;
    },
    onError: (e) => {
      toast.add({
        severity: "error",
        summary: t("store.errorLoadingFilter"),
        detail: e.message,
        life: ToastLife.error,
      });
    },
  });
  loading.value = false;
}

const emit = defineEmits<(e: "update:modelValue", value: StoreDto | null) => void>();

watch(selectedStore, (val) => {
  emit("update:modelValue", val);
});

onMounted(() => {
  fetchStoreData();
});

</script>

<template>
  <FloatLabel class="w-full md:w-250" variant="on">
    <Select
      v-model="selectedStore"
      :options="storeOptions"
      optionLabel="displayName"
      filter
      :filterFields="['number']"
      :virtualScrollerOptions="{ itemSize: 40 }"
      :loading="loading"
      show-clear
      reset-filter-on-hide
      class="selectWidth"
      panelClass="selectWidth"
    />
    <label for="on_label">{{t('store.title')}}</label>
  </FloatLabel>
</template>

<style scoped>
.selectWidth {
  min-width: 300px;
  max-width: 300px;
}
</style>
