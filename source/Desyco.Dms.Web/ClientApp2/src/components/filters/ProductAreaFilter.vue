<script setup lang="ts">
import {onMounted, ref, watch} from "vue";
import {useI18n} from "vue-i18n";
import {useToast} from "primevue";
import ProductAreaService from "@/services/productAreaService";
import type {ProductArea} from "@/models/productArea/productArea";
import {ToastLife} from "@/models/common/toastLife";

const { t } = useI18n();
const toast = useToast();
const loading = ref(false);
const selectedArea = ref<ProductArea | null>(null);
const areaOptions = ref<ProductArea[]>([]);

async function fetchProductAreas() {
  loading.value = true;

  await ProductAreaService.getProductAreas( {
    onSuccess: (data) => {
      areaOptions.value = data;
    },
    onError: (e) => {
      toast.add({
        severity: "error",
        summary: t("ProductArea.errorLoadingFilter"),
        detail: e.message,
        life: ToastLife.error,
      });
    },
  });
  loading.value = false;
}

const emit = defineEmits<(e: "update:modelValue", value: ProductArea | null) => void>();

watch(selectedArea, (val) => {
  emit("update:modelValue", val);
});

onMounted(() => {
  fetchProductAreas();
});

</script>

<template>
  <FloatLabel class="w-full md:w-250" variant="on">
    <Select
      v-model="selectedArea"
      :options="areaOptions"
      optionLabel="description"
      filter
      :filterFields="['name']"
      :loading="loading"
      show-clear
      reset-filter-on-hide
      class="selectWidth"
      panelClass="selectWidth"
    />
    <label for="on_label">{{t('ProductArea.title')}}</label>
  </FloatLabel>
</template>

<style scoped>
.selectWidth {
  min-width: 300px;
}
</style>

