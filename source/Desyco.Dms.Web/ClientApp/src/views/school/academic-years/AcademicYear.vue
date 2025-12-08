<script setup lang="ts">
    import { ref, onMounted } from "vue";
    import { useRouter } from "vue-router";
    import { useAcademicYearStore } from "@/stores/academicYearStore";
    import { AcademicYearStatus, type AcademicYearDto } from "@/types/academic-year";
    import { FilterMatchMode } from "@primevue/core/api";
    import type { RequestParamsPayload } from "@/utils/queryOptions/queryOptionModels";
    import { useToast } from "primevue/usetoast";
    import { useConfirm } from "primevue/useconfirm";
    import { useDataTableUtils } from "@/utils/useDataTableUtils";
    import { useDebounce } from "@/utils/useDebounce";
    import TableActions from "@/components/common/TableActions.vue";

    const router = useRouter();
    const store = useAcademicYearStore();
    const { paginate } = useDataTableUtils(handlePagination);
    const toast = useToast();
    const confirm = useConfirm();

    const dt = ref();
    const isDeleting = ref(false);
    const deletingItemId = ref<number | null>(null);

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
        store.fetchAcademicYears(event);
    }

    // Manual refresh
    const onSearch = () => {
        dt.value.filter({});
    };

    // Debounced search for input
    const onDebouncedSearch = useDebounce(() => {
        onSearch();
    }, 600);

    const openNew = () => {
        router.push({ name: "academic-year-create" });
    };

    const editAcademicYear = (item: AcademicYearDto) => {
        router.push({ name: "academic-year-edit", params: { id: item.id } });
    };

    const confirmDeleteAcademicYear = (item: AcademicYearDto) => {
        confirm.require({
            message: `Are you sure you want to delete ${item.name}?`,
            header: "Confirm Deletion",
            icon: "pi pi-exclamation-triangle",
            accept: async () => {
                isDeleting.value = true;
                deletingItemId.value = item.id as number; // Set the ID of the item being deleted
                try {
                    if (item.id) {
                        await store.deleteAcademicYear(item.id);
                        toast.add({
                            severity: "success",
                            summary: "Successful",
                            detail: "Academic Year Deleted",
                            life: 3000,
                        });
                    }
                } catch (error: any) {
                    const errorMessage = error.response?.data?.message || error.message || "Delete failed";
                    toast.add({ severity: "error", summary: "Error", detail: errorMessage, life: 3000 });
                } finally {
                    isDeleting.value = false;
                    deletingItemId.value = null; // Reset after deletion
                }
            },
        });
    };

    const getStatusLabel = (status: AcademicYearStatus) => {
        switch (status) {
            case AcademicYearStatus.Upcoming:
                return "Upcoming";
            case AcademicYearStatus.Current:
                return "Current";
            case AcademicYearStatus.Closed:
                return "Closed";
            default:
                return "Unknown";
        }
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
    <DataTable
        ref="dt"
        :value="store.academicYears"
        lazy
        :paginator="true"
        :rows="lazyParams.rows"
        :totalRecords="store.totalRecords"
        :loading="store.loading || isDeleting"
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
                <h4 class="m-0 text-xl font-semibold text-surface-900 dark:text-surface-0">Academic Years</h4>
                <div class="flex items-center gap-2">
                    <IconField iconPosition="left">
                        <InputIcon>
                            <i class="pi pi-search" />
                        </InputIcon>
                        <InputText
                            v-model="filters['global'].value"
                            placeholder="Search..."
                            @input="onDebouncedSearch"
                            class="w-full md:w-auto"
                        />
                    </IconField>
                    <Button
                        label="New"
                        icon="pi pi-plus"
                        @click="openNew"
                    />
                </div>
            </div>
        </template>

        <Column
            field="name"
            header="Name"
            sortable
            style="min-width: 12rem"
        ></Column>
        <Column
            field="startDate"
            header="Start Date"
            sortable
            style="min-width: 10rem"
        >
            <template #body="slotProps">
                {{ slotProps.data.startDate ? new Date(slotProps.data.startDate).toLocaleDateString() : "-" }}
            </template>
        </Column>
        <Column
            field="endDate"
            header="End Date"
            sortable
            style="min-width: 10rem"
        >
            <template #body="slotProps">
                {{ slotProps.data.endDate ? new Date(slotProps.data.endDate).toLocaleDateString() : "-" }}
            </template>
        </Column>
        <Column
            field="status"
            header="Status"
            sortable
            style="min-width: 10rem"
        >
            <template #body="slotProps">
                <Tag
                    :value="getStatusLabel(slotProps.data.status)"
                    :severity="getStatusSeverity(slotProps.data.status)"
                />
            </template>
        </Column>
        <Column
            :exportable="false"
            style="min-width: 8rem"
        >
            <template #header>
                <div class="flex justify-end w-full">Actions</div>
            </template>
            <template #body="slotProps">
                <TableActions
                    :id="slotProps.data.id"
                    :loading="deletingItemId === slotProps.data.id"
                    @edit="editAcademicYear(slotProps.data)"
                    @delete="confirmDeleteAcademicYear(slotProps.data)"
                />
            </template>
        </Column>
        <template #paginatorstart>
            <Button
                type="button"
                icon="pi pi-refresh"
                text
                @click="onSearch"
            />
        </template>
    </DataTable>
    <ConfirmDialog />
</template>
