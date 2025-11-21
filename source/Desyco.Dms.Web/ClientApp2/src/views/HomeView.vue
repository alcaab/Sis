<script setup lang="ts">
import { ref, computed } from "vue";
import { useI18n } from "vue-i18n";
import ArticlesPreview from "@/components/articles/ArticlesPreview.vue";
import MainViewFilters from "../components/filters/MainViewFilters.vue";
import type {CalendarWeek, StoreDto} from "@models";
import {BakeOff, type ProductArea} from "@/models/productArea/productArea";
import BakeOffTabs from "@/components/productAreaTabs/BakeOffTabs.vue";

const { t } = useI18n();
const viewTitle = ref(t("articlesPreview"));
const selectedCalendarWeek = ref<CalendarWeek | null>(null);
const selectedStore = ref<StoreDto | null>(null);
const selectedProductArea = ref<ProductArea | null>(null);

const isBakeOff = computed(() => {
  return selectedProductArea.value?.areaName === BakeOff;
});

function handleCalendarWeekChange(calendarWeek: CalendarWeek | null) {
  selectedCalendarWeek.value = calendarWeek;
}

function handleStoreChange(store: StoreDto | null) {
  selectedStore.value = store;
}

function handleProductAreaChange(area: ProductArea | null) {
  selectedProductArea.value = area;
}

function handleFiltersChanged(filters: { calendarWeek: CalendarWeek | null; store: StoreDto | null; productArea: ProductArea | null }) {
  selectedStore.value = filters.store;
  selectedCalendarWeek.value = filters.calendarWeek;
  selectedProductArea.value = filters.productArea;
}
</script>

<template>
  <Card>
    <template #title>
      {{ viewTitle }}
    </template>
    <template #content>
      <MainViewFilters
        @update:calendarWeek="handleCalendarWeekChange"
        @update:store="handleStoreChange"
        @update:productArea="handleProductAreaChange"
        @filtersChanged="handleFiltersChanged"
      />

      <BakeOffTabs
        v-if="isBakeOff"
        :calendar-week="selectedCalendarWeek"
        :store="selectedStore"
        :product-area="selectedProductArea"
      />

      <ArticlesPreview
        v-else
        :calendar-week="selectedCalendarWeek"
        :store="selectedStore"
        :product-area="selectedProductArea"
      />
    </template>
  </Card>
</template>

<style scoped></style>
