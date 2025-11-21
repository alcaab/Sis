<script setup lang="ts">
import { onMounted, ref, defineProps, watch} from "vue";
import CustomGroupService from "@/services/customGroupService";
import { useCustomGroupStore } from "@/stores/customGroupStore";
import { storeToRefs } from "pinia";
import { useDataTableUtils } from "@utils";
import { useDialog } from "primevue/usedialog";
import CustomGroupDialog from "@/components/customGroups/CustomGroupDialog.vue";
import type { CustomGroupDto } from "@models";
import type { DynamicDialogCloseOptions } from "primevue/dynamicdialogoptions";
import type { DataTableFilterMeta, DataTableSortEvent } from "primevue/datatable";
import DeletionDialog from "@/components/DeletionDialog.vue";
import { useI18n } from "vue-i18n";
import { useDebounce } from "@/utils/useDebounce";
import type { RequestParamsPayload } from "@/utils/queryOptions/queryOptionModels";
import { useNotifications } from "@/services/notificationService";

const props = defineProps<{
  header: string;
  mainGroupNumber: number;
}>();

const { showError } = useNotifications();
const dialog = useDialog();
const { t } = useI18n();
const customGroupStore = useCustomGroupStore();
const { items, totalItems, loading } = storeToRefs(customGroupStore);
const { value, debouncedValue, isDebouncing } = useDebounce("", 750);
const { paginate } = useDataTableUtils(handlePagination);
const filters = ref<Record<string, DataTableFilterMeta>>({});
const selectedFilter: Record<string, DataTableFilterMeta> = {};

const defaultOptions = ref({
  first: 0,
  rows: 10,
  sortField: "name",
  sortOrder: -1,
} as DataTableSortEvent);

const dt = ref();
const dialogVisible = ref(false);
const dialogData = ref({});

watch(debouncedValue, (newValue) => {
  onSearch(newValue);
});

onMounted(() => onSearch(""));

function onSearch(searchTerm: string){
  selectedFilter.name = { value: searchTerm, matchMode: "contains" };
  dt.value.filter({});
}

async function onAddGroup(){
  await openDialog(t("customGroup.newHeader"));
}

async function onEditGroup({ id }: CustomGroupDto){
  await openDialog(t("customGroup.editHeader"), id);
}

function onDeleteGroup(model: CustomGroupDto){
  dialogData.value = model;
  dialogVisible.value = true;
}

async function handleDelete({ id }: CustomGroupDto) {
  try {
    await CustomGroupService.deleteGroup(id);

    dt.value.filter({});
  } catch {
    showError(t("common.deleteErrorMessage"));
  }
}

async function handlePagination(event: RequestParamsPayload) {
  try {
    const args: RequestParamsPayload = {
      ...event,
      filters: selectedFilter,
      extras: {
        mainGroupNumber: props.mainGroupNumber,
      },
    };
    await customGroupStore.loadCustomGroups(args);
    items.value.forEach((item) => {
      item.articleNames = (item.articleNames as number[]).join(", ");
    })
  } catch {
    showError(t("common.errorMessage"));
  }
}

async function openDialog(header: string, id?: string | undefined){
  try {
    await customGroupStore.loadGroup(id, props.mainGroupNumber);
    dialog.open(CustomGroupDialog, {
      data: {
        isNew: id === undefined,
      },
      props: {
        header,
        style: {
          width: "50vw",
        },
        modal: true
      },
      onClose: async (opt: DynamicDialogCloseOptions) => {
        if (!opt.data) return;

        try {
          const group: CustomGroupDto = opt.data;

          if (group.id) {
            await CustomGroupService.updateGroup(group.id, group);
          } else {
            await CustomGroupService.createGroup(group);
          }

          dt.value.filter({});
        } catch (e) {
          if (
            typeof e === "object" &&
            e !== null &&
            "status" in e &&
            e.status === 409
          ) {
            showError(t("errors.conflict"));
          } else {
            showError(t("common.saveErrorMessage"));
          }
        }
      },
    });
  } catch {
    showError(t("common.errorMessage"));
  }
}
</script>

<template>
  <Panel :header="header" toggleable>
    <div class="grid grid-cols-12 mb-4 mt-2">
      <div class="col-span-12 md:col-span-9">
        <InputGroup>
          <InputText
            v-model="value"
            :placeholder="t('customGroup.search')"
            class="w-full" />
          <InputGroupAddon>
            <Button
              icon="pi pi-search"
              :loading="isDebouncing"
              severity="secondary"
              variant="text"
              @click="onSearch(value)" />
          </InputGroupAddon>
        </InputGroup>
      </div>
      <div class="col-span-12 md:col-span-3">
        <div class="flex justify-end">
          <Button
            :label="t('customGroup.addGroup')"
            icon="pi pi-plus"
            @click="onAddGroup" />
        </div>
      </div>
    </div>
    <DataTable
      ref="dt"
      :value="items"
      :loading="loading"
      lazy
      paginator
      dataKey="id"
      striped-rows
      :rows="5"
      :totalRecords="totalItems"
      :filters="filters"
      :sortField="defaultOptions.sortField"
      :sortOrder="defaultOptions.sortOrder === null ? undefined : defaultOptions.sortOrder"
      :rowsPerPageOptions="[5, 10, 25, 50]"
      currentPageReportTemplate="{first} bis {last} von {totalRecords}"
      paginatorTemplate="FirstPageLink PrevPageLink CurrentPageReport NextPageLink LastPageLink RowsPerPageDropdown"
      @page="paginate($event)"
      @sort="paginate($event)"
      @filter="paginate($event)">
      <Column
        field="name"
        :header="t('customGroup.name')"
        :sortable="true"
        header-class="w-[25%]">
      </Column>
      <Column :header="t('common.actions')" header-class="w-[8rem]">
        <template #body="{ data }">
          <Button
            icon="pi pi-pencil"
            severity="primary"
            variant="text"
            v-tooltip="t('common.buttons.edit')"
            @click="onEditGroup(data)" />
          <Button
            icon="pi pi-trash"
            severity="danger"
            variant="text"
            v-tooltip="t('common.buttons.delete')"
            @click="onDeleteGroup(data)" />
        </template>
      </Column>
      <Column field="subGroupNames" :header="t('customGroup.subGroups')" header-class="w-[25%]">
      </Column>
      <Column field="articleNames" :header="t('customGroup.articles')">
      </Column>
      <template #paginatorstart>
        <Button
          type="button"
          icon="pi pi-refresh"
          text
          @click="onSearch(value)" />
      </template>
      <template #empty> {{ t("common.noEntries") }}</template>
    </DataTable>
  </Panel>
  <DeletionDialog
    v-model:visible="dialogVisible"
    :data="dialogData"
    :content="t('customGroups')"
    @delete="handleDelete($event)" />
</template>

<style scoped></style>
