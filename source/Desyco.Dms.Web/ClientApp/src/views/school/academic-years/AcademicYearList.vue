<script setup lang="ts">
    import { ref, onMounted } from "vue";
    import { useRouter } from "vue-router";
    import { useAcademicYearStore } from "@/stores/academicYearStore";
    import { AcademicYearStatus, type AcademicYearDto } from "@/types/academic-year";
    import { FilterMatchMode } from "@primevue/core/api";
    import type { RequestParamsPayload } from "@/utils/queryOptions/queryOptionModels";
    import { useDataTableUtils } from "@/utils/useDataTableUtils";
    import { useDebounce } from "@/utils/useDebounce";
    import TableActions from "@/components/common/TableActions.vue";
    import { useNotification } from "@/composables/useNotification";
    import { useDeleteConfirm } from "@/composables/useDeleteConfirm";
    import { useI18n } from "vue-i18n";
    import { enumService } from "@/service/EnumService";

    const router = useRouter();
    const store = useAcademicYearStore();
    const { paginate } = useDataTableUtils(handlePagination);
    const notify = useNotification();
    const { confirmDelete, deletingItemId } = useDeleteConfirm();
    const { t } = useI18n();

    const dt = ref();

    const filters = ref({
        global: { value: "", matchMode: FilterMatchMode.CONTAINS },
    });

    const lazyParams = ref({
        first: 0,
        rows: 10,
        sortField: "name",
        sortOrder: -1,
    });

    onMounted(() => {
        onSearch();
    });

    function handlePagination(event: RequestParamsPayload) {
        store.fetchAcademicYears(event).catch((error) => {
            notify.showError(error, t("common.notifications.loadError"));
        });
    }

    const onSearch = () => {
        dt.value.filter({});
    };

    const onDebouncedSearch = useDebounce(() => {
        onSearch();
    }, 600);

    const openNew = () => {
        router.push({ name: "academic-year-create" });
    };

    const editAcademicYear = (item: AcademicYearDto) => {
        router.push({ name: "academic-year-edit", params: { id: item.id } });
    };

    const onConfirmDelete = (item: AcademicYearDto) => {
        confirmDelete({
            item: { id: item.id, name: item.name },
            deleteAction: store.deleteAcademicYear,
        });
    };

    const getStatusSeverity = (status: AcademicYearStatus) => {
        switch (status) {
            case AcademicYearStatus.Upcoming:
                return "info";
            case AcademicYearStatus.Current:
                return "success";
            case AcademicYearStatus.Closed:
                return "secondary";
            default:
                return "contrast";
        }
    };
</script>

<template>
    <div class="w-full animate fade-in">
        <DataTable
            ref="dt"
            :value="store.academicYears"
            lazy
            :paginator="true"
            :rows="lazyParams.rows"
            :totalRecords="store.totalRecords"
            :loading="store.loading || !!deletingItemId"
            dataKey="id"
            :sortField="lazyParams.sortField"
            :sortOrder="lazyParams.sortOrder === null ? undefined : lazyParams.sortOrder"
            @page="paginate"
            @sort="paginate"
            @filter="paginate"
            :filters="filters"
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
            :rowsPerPageOptions="[5, 10, 25]"
            currentPageReportTemplate="Showing {first} to {last} of {totalRecords} entries"
        >
            <template #header>
                <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
                    <h4 class="m-0 text-xl font-semibold text-surface-900 dark:text-surface-0">
                        {{ t("schoolSettings.academicYear.title") }}
                    </h4>
                    <div class="flex items-center gap-2">
                        <IconField iconPosition="left">
                            <InputIcon>
                                <i class="pi pi-search" />
                            </InputIcon>
                            <InputText
                                v-model="filters['global'].value"
                                :placeholder="t('common.placeholders.search')"
                                @input="onDebouncedSearch"
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
                :header="t('schoolSettings.academicYear.name')"
                :sortable="true"
                style="min-width: 12rem"
            ></Column>
            <Column
                field="startDate"
                :header="t('schoolSettings.academicYear.startDate')"
                :sortable="true"
                style="min-width: 10rem"
            >
                <template #body="slotProps">
                    {{ slotProps.data.startDate ? new Date(slotProps.data.startDate).toLocaleDateString() : "-" }}
                </template>
            </Column>
            <Column
                field="endDate"
                :header="t('schoolSettings.academicYear.endDate')"
                :sortable="true"
                style="min-width: 10rem"
            >
                <template #body="slotProps">
                    {{ slotProps.data.endDate ? new Date(slotProps.data.endDate).toLocaleDateString() : "-" }}
                </template>
            </Column>
            <Column
                field="status"
                :header="t('schoolSettings.academicYear.status')"
                :sortable="true"
                style="min-width: 10rem"
            >
                <template #body="slotProps">
                    <Tag
                        :value="enumService.getDescription('AcademicYearStatus', slotProps.data.status)"
                        :severity="getStatusSeverity(slotProps.data.status)"
                    />
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
                        @edit="editAcademicYear(slotProps.data)"
                        @delete="onConfirmDelete(slotProps.data)"
                    />
                </template>
            </Column>
            <template #paginatorstart>
                <Button
                    type="button"
                    icon="pi pi-refresh"
                    :title="t('common.buttons.refresh')"
                    text
                    @click="onSearch"
                />
            </template>
        </DataTable>
    </div>
    <ConfirmDialog />
</template>
