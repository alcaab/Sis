<script setup lang="ts">
import { useI18n } from "vue-i18n";
import { onMounted, ref, watch } from "vue";
import DataTable, { type DataTableFilterMeta, type DataTableSortEvent } from "primevue/datatable";
import Column from "primevue/column";
import { storeToRefs } from "pinia";
import { useDebounce } from "@/utils/useDebounce";
import { useDataTableUtils } from "@utils";
import DeletionDialog from "@/components/DeletionDialog.vue";
import { useGtinArticleStore } from "@/stores/gtinArticleStore";
import type { RequestParamsPayload } from "@/utils/queryOptions/queryOptionModels";
import type { ArticleDisplayDto } from "@models";
import { useNotifications } from "@/services/notificationService";

const { t } = useI18n();
const gtinArticleStore = useGtinArticleStore();
const { items, totalItems, loading, availableArticles } = storeToRefs(gtinArticleStore);
const { value, debouncedValue, isDebouncing } = useDebounce("", 750);
const { paginate } = useDataTableUtils(handlePagination);
const articleTable = ref();
const selectedArticle = ref<ArticleDisplayDto | null>(null);
const deletedArticle = ref<ArticleDisplayDto | null>(null);
const filters = ref<Record<string, DataTableFilterMeta>>({});
const searchTerm = ref("");
const dialogVisible = ref(false);
const defaultOptions = ref({
  first: 0,
  rows: 10,
  sortField: "number",
  sortOrder: -1,
} as DataTableSortEvent);
const notify = useNotifications();

watch(debouncedValue, (newValue) => {
  onSearch(newValue);
});

function onSearch(value: string){
  searchTerm.value = value;
  articleTable.value.filter({});
}

function onSort(event: DataTableSortEvent) 
{
  if (defaultOptions.value.sortOrder == 1) 
  {
    defaultOptions.value.sortOrder = -1;
  } 
  else 
  {
    defaultOptions.value.sortOrder = 1;
  }

  paginate(event);
}

async function handlePagination(event: RequestParamsPayload) 
{
  const cloneEvent = JSON.parse(JSON.stringify(event)) as RequestParamsPayload;
  
  const args: RequestParamsPayload = {
    ...cloneEvent,
    search: searchTerm.value,
  };

  await gtinArticleStore.loadArticlesList(args, notify);
}

function deleteArticle(model: ArticleDisplayDto) {
  deletedArticle.value = model;
  dialogVisible.value = true;
}

async function addArticle() {
  if (!selectedArticle.value) return;

  const success = await gtinArticleStore.addArticle(selectedArticle.value, notify);

  if (success) {
    selectedArticle.value = null;
  }
}

async function handleDelete(article: ArticleDisplayDto) 
{
  await gtinArticleStore.removeArticle(article, notify);

  deletedArticle.value = null;
}

onMounted(() => {
  gtinArticleStore.loadAvailableArticles(notify);
  onSearch("");
});
</script>

<template>
  <Panel>
    <h1>{{ t("gtinArticle.title") }}</h1>
    <label>{{ t("gtinArticle.description") }}</label>
    <div>
      <div class="grid grid-cols-12 mb-4 mt-2">
        <div class="col-span-12 md:col-span-6">
          <FloatLabel variant="on">
            <div class="flex gap-2 items-center">
              <Select
                v-model="selectedArticle"
                :options="availableArticles"
                optionLabel="displayText"
                :loading="loading"
                filter
                show-clear
                reset-filter-on-hide
                fluid
              />
            </div>
            <label for="on_label">{{ t("gtinArticle.select") }}</label>
          </FloatLabel>
        </div>
        <div class="col-span-12 md:col-span-6">
          <div class="flex justify-end">
            <Button
              :label="t('gtinArticle.addArticle')"
              icon="pi pi-plus"
              :disabled="!selectedArticle"
              @click="addArticle()"
            />
          </div>
        </div>
      </div>

      <div class="grid grid-cols-12 mb-4 mt-2">
        <div class="col-span-12 md:col-span-6">
          <InputGroup>
            <InputText v-model="value" :placeholder="t('gtinArticle.search')" class="w-full" />
            <InputGroupAddon>
              <Button
                icon="pi pi-search"
                :loading="isDebouncing"
                severity="secondary"
                variant="text"
                @click="onSearch(value)"
              />
            </InputGroupAddon>
          </InputGroup>
        </div>
      </div>

      <DataTable
        ref="articleTable"
        :value="items"
        :loading="loading"
        lazy
        paginator
        dataKey="number"
        striped-rows
        :rows="25"
        :totalRecords="totalItems"
        :filters="filters"
        :sortField="defaultOptions.sortField"
        :sortOrder="defaultOptions.sortOrder === null ? undefined : defaultOptions.sortOrder"
        :rowsPerPageOptions="[25, 50, 100]"
        currentPageReportTemplate="{first} bis {last} von {totalRecords}"
        paginatorTemplate="FirstPageLink PrevPageLink CurrentPageReport NextPageLink LastPageLink RowsPerPageDropdown"
        @page="paginate($event)"
        @sort="onSort($event)"
        @filter="paginate($event)"
      >
        <Column
          field="displayText"
          :header="t('gtinArticle.displayText')"
          :sortable="true"
          sortField="number"
          header-class="w-[50%]"
        />
        <Column :header="t('common.actions')" header-class="w-[8rem]">
          <template #body="{ data }">
            <Button
              icon="pi pi-trash"
              severity="danger"
              variant="text"
              v-tooltip="t('common.buttons.delete')"
              @click="deleteArticle(data)"
            />
          </template>
        </Column>

        <template #paginatorstart>
          <Button type="button" icon="pi pi-refresh" text @click="onSearch(value)" />
        </template>

        <template #empty>
          {{ t("common.noEntries") }}
        </template>
      </DataTable>

      <DeletionDialog
        v-model:visible="dialogVisible"
        :data="deletedArticle"
        :content="t('customGroups')"
        @delete="handleDelete($event)"
      />
    </div>
  </Panel>
</template>

<style scoped></style>
