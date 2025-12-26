<script setup lang="ts">
    import { ref, onMounted } from "vue";
    import { useEducationalLevelStore } from "@/stores/educationalLevelStore";
    import { useNotification } from "@/composables/useNotification";
    import { useDeleteConfirm } from "@/composables/useDeleteConfirm";
    import { useI18n } from "vue-i18n";
    import { FilterMatchMode } from "@primevue/core/api";
    import { EducationalLevelType, type EducationalLevelDto } from "@/types/educational-level";
    import EducationalLevelForm from "./EducationalLevelForm.vue";
    import TableActions from "@/components/common/TableActions.vue";
    import type { RequestParamsPayload } from "@/utils/queryOptions/queryOptionModels.ts";
    import { useDataTableUtils } from "@/utils/useDataTableUtils.ts";
    import { useDebounce } from "@/utils/useDebounce.ts";

    const store = useEducationalLevelStore();
    const { paginate } = useDataTableUtils(handlePagination);
    const notify = useNotification();
    const { confirmDelete, deletingItemId } = useDeleteConfirm();
    const { t } = useI18n();

    const dt = ref();
    const itemDialog = ref(false);
    const selectedItem = ref<EducationalLevelDto | null>(null);
    const submitted = ref(false);

    const filters = ref({
        global: { value: "", matchMode: FilterMatchMode.CONTAINS },
    });

    const lazyParams = ref({
        first: 0,
        rows: 10,
        sortField: "name",
        sortOrder: 1,
    });

    onMounted(() => {
        onSearch();
    });

    function handlePagination(event: RequestParamsPayload) {
        store.fetchEducationalLevels(event).catch((error) => {
            notify.showError(error, t("common.notifications.loadError"));
        });
    }

    const onSearch = async () => {
        dt.value.filter({});
    };

    const onDebouncedSearch = useDebounce(() => {
        onSearch();
    }, 600);

    // onMounted(async () => {
    //     await fetchItems();
    // });
    //
    // const fetchItems = async () => {
    //     try {
    //         await store.fetchEducationalLevels({ rows: 100, sortField: "name", sortOrder: 1 });
    //     } catch (error) {
    //         notify.showError(error, t("common.notifications.loadError"));
    //     }
    // };

    const openNew = () => {
        selectedItem.value = null;
        submitted.value = false;
        itemDialog.value = true;
    };

    const hideDialog = () => {
        itemDialog.value = false;
        submitted.value = false;
    };

    const saveItem = async (data: EducationalLevelDto) => {
        submitted.value = true;
        try {
            if (selectedItem.value && selectedItem.value.id > 0) {
                await store.updateEducationalLevel(selectedItem.value.id, data);
                notify.showSuccess(t("common.notifications.updateSuccess"));
            } else {
                await store.createEducationalLevel(data);
                notify.showSuccess(t("common.notifications.createSuccess"));
            }
            hideDialog();
            await onSearch();
        } catch (error) {
            notify.showError(error, t("common.notifications.operationError"));
        }
    };

    const editItem = (item: EducationalLevelDto) => {
        selectedItem.value = { ...item };
        itemDialog.value = true;
    };

    const onConfirmDelete = (item: EducationalLevelDto) => {
        confirmDelete({
            item: item,
            deleteAction: store.deleteEducationalLevel,
            onSuccess: onSearch,
        });
    };

    const getLevelTypeLabel = (type: EducationalLevelType) => {
        switch (type) {
            case EducationalLevelType.EarlyChildhood:
                return t("schoolSettings.educationalLevel.types.earlyChildhood");
            case EducationalLevelType.Primary:
                return t("schoolSettings.educationalLevel.types.primary");
            case EducationalLevelType.Secondary:
                return t("schoolSettings.educationalLevel.types.secondary");
            default:
                return "";
        }
    };
</script>

<template>
    <div class="w-full animate fade-in">
        <DataTable
            ref="dt"
            :value="store.educationalLevels"
            lazy
            :paginator="true"
            :rows="lazyParams.rows"
            dataKey="id"
            :sortField="lazyParams.sortField"
            :sortOrder="lazyParams.sortOrder === null ? undefined : lazyParams.sortOrder"
            :loading="store.loading || !!deletingItemId"
            :totalRecords="store.totalRecords"
            @page="paginate"
            @sort="paginate"
            @filter="paginate"
            :filters="filters"
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
            :rowsPerPageOptions="[5, 10, 25]"
            :currentPageReportTemplate="t('common.pageReportTemplate')"
        >
            <template #header>
                <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
                    <h4 class="m-0 text-xl font-semibold text-surface-900 dark:text-surface-0">
                        {{ t("schoolSettings.educationalLevel.title") }}
                    </h4>
                    <div class="flex items-center gap-2">
                        <IconField iconPosition="left">
                            <InputIcon>
                                <i class="pi pi-search" />
                            </InputIcon>
                            <InputText
                                v-model="filters['global'].value"
                                @input="onDebouncedSearch"
                                :placeholder="t('common.placeholders.search')"
                                class="w-full md:w-auto"
                            />
                        </IconField>
                        <Button
                            :label="t('common.buttons.new')"
                            icon="pi pi-plus"
                            @click="openNew"
                        />
                    </div>
                </div>
            </template>

            <Column
                field="name"
                :header="t('schoolSettings.educationalLevel.name')"
                :sortable="true"
                style="min-width: 12rem"
            ></Column>
            <Column
                field="levelTypeId"
                :header="t('schoolSettings.educationalLevel.levelType')"
                :sortable="true"
                style="min-width: 10rem"
            >
                <template #body="slotProps">
                    {{ getLevelTypeLabel(slotProps.data.levelTypeId) }}
                </template>
            </Column>
            <Column
                field="isActive"
                :header="t('schoolSettings.educationalLevel.isActive')"
                :sortable="true"
                style="width: 10px"
            >
                <template #body="{ data }">
                    <div class="flex justify-center">
                        <i
                            :class="[
                                'pi',
                                {
                                    'pi-check-circle text-green-500': data.isActive,
                                    'pi-times-circle text-red-500': !data.isActive,
                                },
                            ]"
                        ></i>
                    </div>
                </template>
            </Column>
            <Column
                :exportable="false"
                style="width: 4rem"
            >
                <template #header>
                    <div class="flex justify-center w-full">{{ t("common.actions") }}</div>
                </template>
                <template #body="slotProps">
                    <TableActions
                        :id="slotProps.data.id"
                        :loading="deletingItemId === slotProps.data.id"
                        @edit="editItem(slotProps.data)"
                        @delete="onConfirmDelete(slotProps.data)"
                    />
                </template>
            </Column>
            <template #paginatorstart>
                <Button
                    type="button"
                    icon="pi pi-refresh"
                    text
                    @click="onSearch"
                    :title="t('common.buttons.refresh')"
                />
            </template>
        </DataTable>
    </div>

    <Dialog
        v-model:visible="itemDialog"
        :style="{ width: '50vw' }"
        :breakpoints="{ '960px': '75vw', '640px': '90vw' }"
        :header="t('schoolSettings.educationalLevel.dialogHeader')"
        :modal="true"
        class="p-fluid"
    >
        <EducationalLevelForm
            :initialData="selectedItem"
            :submitted="submitted"
            :loading="store.loading"
            @submit="saveItem"
            @cancel="hideDialog"
        />
    </Dialog>
    <ConfirmDialog />
</template>
