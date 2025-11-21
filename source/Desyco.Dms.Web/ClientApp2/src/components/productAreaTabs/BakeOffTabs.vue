<script setup lang="ts">
import { computed, onMounted, ref, watch } from "vue";
import ArticlesPreview from "@/components/articles/ArticlesPreview.vue";
import TabView from 'primevue/tabview';
import TabPanel from 'primevue/tabpanel';
import type { CalendarWeek, CustomGroupDto, StoreDto } from "@models";
import type { ProductArea } from "@/models/productArea/productArea";
import type { RequestParamsPayload } from "@/utils/queryOptions/queryOptionModels";
import { useNotifications } from "@/services/notificationService";
import { useI18n } from "vue-i18n";
import { useCustomGroupStore } from "@/stores/customGroupStore";

const props = defineProps<{
  calendarWeek?: CalendarWeek | null;
  store?: StoreDto | null;
  productArea?: ProductArea | null;
}>();

const { showError } = useNotifications();
const { t } = useI18n();
const customGroupStore = useCustomGroupStore();
const activeTab = ref(0);
const selectedArticleNumbers = ref<number[] | null>(null);
let bakeOffGroups = ref<CustomGroupDto[]>([]);
async function loadTabGroups() {
  try {
    const args: RequestParamsPayload = {
      skip: 0,
      take: 5,
      sorts: [{field: 'name', order: 'asc'}],
      extras: {
        mainGroupNumber: props.productArea?.areaNumbers[0]
      }
    }
    await customGroupStore.loadCustomGroups(args);
    bakeOffGroups.value = customGroupStore.items;
  }
  catch(error) {
    showError(t("common.errorMessage"));
  }
  finally {
    updateArticleNumbers()
  }
}

function onTabChanged(index: number) {
  activeTab.value = index;
  updateArticleNumbers();
}

function updateArticleNumbers() {
  const group = bakeOffGroups.value[activeTab.value];
  if (group.articleNames && group) {
    selectedArticleNumbers.value = group.articleNames as number[];
  } else {
    selectedArticleNumbers.value = null;
  }
}

watch([() => props.calendarWeek, () => props.store, () => props.productArea], () => {
  activeTab.value = 0;
});

onMounted(() => {
  loadTabGroups();
})

const showArticles = computed(() =>
  (selectedArticleNumbers.value?.length ?? 0) > 0
)
</script>

<template>
  <TabView
    v-model:activeIndex="activeTab"
    class="mb-4"
    @tab-change="onTabChanged($event.index)"
  >
    <TabPanel
      v-for="(group, index) in bakeOffGroups"
      :key="group.id"
      :header="group.name"
    >
      <ArticlesPreview
        v-if="showArticles && activeTab === index"
        :calendar-week="calendarWeek"
        :store="store"
        :product-area="productArea"
        :article-numbers="selectedArticleNumbers"
      />
    </TabPanel>
  </TabView>
</template>

<style scoped>
:deep(.p-tabview-ink-bar) {
  background: #28292f !important;
}
</style>
