<script setup lang="ts">
import {computed, onMounted, ref, watch} from "vue";
import ArticleDisplayService from "@/services/articleDisplayService";
import ArticleCard from "@/components/articles/ArticleCard.vue";
import type {PageState} from "primevue";
import type {ArticlePreviewDisplay, CalendarWeek, StoreDto} from "@models";
import type {Filters, RequestParamsPayload} from "@/utils/queryOptions/queryOptionModels";
import ProgressSpinner from 'primevue/progressspinner';
import type {ProductArea} from "@/models/productArea/productArea";
import LoadingView from "@/views/LoadingView.vue";
import {useNotifications} from "@/services/notificationService";
import {useI18n} from "vue-i18n";

const props = defineProps<{
  calendarWeek?: CalendarWeek | null;
  store?: StoreDto | null;
  productArea?: ProductArea | null;
  articleNumbers?: number[] | null;
}>()

const selectedFilters = computed((): Filters => {
  const filters: Filters = {};
  if (props.calendarWeek != null) {
    filters['ValidFrom'] = {
      value: new Date(props.calendarWeek.calendarWeekEnd),
      matchMode: 'le',
      type: 'date'
    };
    filters['ValidTo'] = {
      value: new Date(props.calendarWeek.calendarWeekStart),
      matchMode: 'ge',
      type: 'date'
    };
  }
  
  if (props.store != null) {
    filters['storeNumber'] = {
      value: props.store.number,
      matchMode: 'eq',
      type: 'number'
    };
  }

  if (props.productArea != null) {
    filters['article.mainGroupNumber'] = {
      value: props.productArea.areaNumbers,
      matchMode: 'in',
      type: 'number'
    };
  }
  
  if (props.articleNumbers && props.articleNumbers.length > 0) {
    filters['articleNumber'] = {
      value: props.articleNumbers,
      matchMode: 'in',
      type: 'number'
    };
  }

  return filters;
});

const { showError } = useNotifications();
const { t } = useI18n();
const paginationData = ref({ skip: 0, take: 28 });
const Articles = ref<ArticlePreviewDisplay | null>(null);
const loading = ref(false);
const error = ref<string | null>(null);
const first = ref(0);

async function loadArticles(): Promise<void> {
  loading.value = true;
  error.value = null;
  try {
    const payload: RequestParamsPayload = {
      skip: paginationData.value.skip,
      take: paginationData.value.take,
      showCount: true,
      filters: selectedFilters.value,
    };
    Articles.value = await ArticleDisplayService.getPreviewArticles(payload);
  } catch (err) {
    showError(t("common.errorMessage"));
  } finally {
    loading.value = false;
  }
}

onMounted(async () => {
  if(Object.keys(selectedFilters.value).length > 0){
    await loadArticles();
  }
});

watch([() => props.calendarWeek, () => props.store, () => props.productArea, () => props.articleNumbers], async() => {
  paginationData.value.skip = 0;
  first.value = 0;
  await loadArticles();
});

const ItemsPerPage = computed(() => {
  if (!Articles.value) 
    return 28; 
  
  return Articles.value.rows * Articles.value.columns;
});

async function onPageChange(event: PageState): Promise<void> {
  paginationData.value.skip = ItemsPerPage.value * event.page;
  paginationData.value.take = ItemsPerPage.value;
  first.value = event.first;
  await loadArticles();
}

const gridStyle = computed(() => ({
  '--base-columns': Articles.value?.columns || 4 
}));

const articleItems = computed(() => {
  return Articles.value?.results?.items || [];
});

const totalRecords = computed(() => {
  return Articles.value?.results?.totalItems || 0;
});
</script>
<template>
  <loading-view v-if="loading">
  </loading-view>
  <div>
    <div class="articles-grid grid gap-4 w-full box-border" :style="gridStyle" >
      <ArticleCard
        v-for="article in articleItems"
        :number="article.number"
        :title="article.text"
        :image="article.image"
      />
    </div>
    <div class="mt-4">
      <Paginator
        class="custom-paginator"
        :rows="ItemsPerPage"
        :totalRecords="totalRecords"
        :first="first"
        @page="onPageChange"
      />
    </div>
  </div>
</template>

<style scoped>

.articles-grid {
  grid-template-columns: repeat(var(--base-columns), 1fr);
}

@media (max-width: 1536px) {
  .articles-grid { grid-template-columns: repeat(calc(var(--base-columns) - 1), 1fr); }
}
@media (max-width: 1280px) {
  .articles-grid { grid-template-columns: repeat(calc(var(--base-columns) - 2), 1fr); }
}
@media (max-width: 1024px) {
  .articles-grid { grid-template-columns: repeat(calc(var(--base-columns) - 3), 1fr); }
}
@media (max-width: 768px) {
  .articles-grid { grid-template-columns: repeat(calc(var(--base-columns) - 4), 1fr); }
}
@media (max-width: 640px) {
  .articles-grid { grid-template-columns: repeat(calc(var(--base-columns) - 5), 1fr); }
}
</style>
