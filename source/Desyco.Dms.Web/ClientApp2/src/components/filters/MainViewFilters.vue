<script setup lang="ts">
import { ref } from "vue";
import type { CalendarWeek, StoreDto } from "@models";
import CalendarWeekFilter from "./CalendarWeekFilter.vue";
import StoreFilter from "./StoreFilter.vue";
import { useI18n } from "vue-i18n";
import ProductAreaFilter from "./ProductAreaFilter.vue";
import type {ProductArea} from "@/models/productArea/productArea";

const { t } = useI18n();

const selectedCalendarWeek = ref<CalendarWeek | null>(null);
const selectedStore = ref<StoreDto | null>(null);
const selectedProductArea = ref<ProductArea | null>(null);

const emit = defineEmits<{
  (e: "update:calendarWeek", value: CalendarWeek | null): void;
  (e: "update:store", value: StoreDto | null): void;
  (e: "update:productArea", value: ProductArea | null): void;
  (e: "filtersChanged", filters: { calendarWeek: CalendarWeek | null; store: StoreDto | null; productArea: ProductArea | null }): void;
}>();

function handleCalendarWeekChange(value: CalendarWeek | null) {
  selectedCalendarWeek.value = value;
  emit("update:calendarWeek", value);
  emit("filtersChanged", {
    calendarWeek: value ?? null,
    productArea: selectedProductArea.value,
    store: selectedStore.value
  });
}

function handleStoreChange(value: StoreDto | null) {
  selectedStore.value = value;
  emit("update:store", value);
  emit("filtersChanged", {
    calendarWeek: selectedCalendarWeek.value,
    productArea: selectedProductArea.value,
    store: value ?? null
  });
}

function handleProductAreaChange(value: ProductArea | null) {
  selectedProductArea.value = value;
  emit("update:productArea", value);
  emit("filtersChanged", {
    calendarWeek: selectedCalendarWeek.value,
    productArea: value ?? null,
    store: selectedStore.value
  });
}

</script>

<template>
  <div class="main-view-filters">
    <div class="filters-container">
      <div class="filter-item">
        <CalendarWeekFilter
          @update:modelValue="handleCalendarWeekChange"
          :modelValue="selectedCalendarWeek"
        />
      </div>

      <div class="filter-item">
        <StoreFilter
          @update:modelValue="handleStoreChange"
          :modelValue="selectedStore"
        />
      </div>

      <div class="filter-item">
        <ProductAreaFilter
          @update:modelValue="handleProductAreaChange"
          :modelValue="selectedProductArea"
        />
      </div>
    </div>
  </div>
</template>

<style scoped>
.main-view-filters {
  padding: 1rem;
  border-radius: 8px;
  margin-bottom: 2rem;
}

.filters-container {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
  align-items: flex-end;
}

.filter-item {
  flex: 1 1 200px;
  min-width: 200px;
  max-width: 400px;
}

@media (min-width: 1300px) {
  .filter-item {
    flex: 1 1 calc(33.333% - 1rem);
  }
}

@media (min-width: 690px) and (max-width: 1299px) {
  .filter-item {
    flex: 1 1 calc(50% - 0.5rem);
  }
}

@media (max-width: 689px) {
  .filter-item {
    flex: 1 1 100%;
    max-width: 100%;
  }
}
</style>
