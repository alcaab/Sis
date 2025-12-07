<script setup lang="ts">
import { ref, onMounted, watch } from 'vue';
import { useAcademicYearStore } from '@/stores/academicYearStore';
import { AcademicYearStatus, type AcademicYearDto, type CreateAcademicYearDto, type UpdateAcademicYearDto } from '@/types/academic-year';
import { FilterMatchMode } from '@primevue/core/api';
import type { RequestParamsPayload } from '@/utils/queryOptions/queryOptionModels.ts';
import { useToast } from 'primevue/usetoast';
import { useConfirm } from 'primevue/useconfirm'; // Uncommented
import { useDataTableUtils } from '@/utils/useDataTableUtils.ts';
import { useDebounce } from '@/utils/useDebounce.ts';

const store = useAcademicYearStore();
const { paginate } = useDataTableUtils(handlePagination);
const { inputValue, debouncedValue } = useDebounce('', 750);
const toast = useToast();
const confirm = useConfirm(); // Initialized

const dt = ref();
const academicYearDialog = ref(false);
const academicYear = ref<Partial<AcademicYearDto>>({});
const submitted = ref(false);
const isSaving = ref(false); // New: Loading state for save/delete operations

const filters = ref({
    global: { value: '', matchMode: FilterMatchMode.CONTAINS }
});

const lazyParams = ref({
    first: 0,
    rows: 10,
    sortField: 'name',
    sortOrder: -1
});

const statuses = ref([
    { label: 'Upcoming', value: AcademicYearStatus.Upcoming },
    { label: 'Current', value: AcademicYearStatus.Current },
    { label: 'Closed', value: AcademicYearStatus.Closed }
]);

onMounted(() => onSearch());

function handlePagination(event: RequestParamsPayload) {
    store.fetchAcademicYears(event);
}

const onSearch = () => {
    dt.value.filter({});
};

watch(debouncedValue, (newValue) => {
    filters.value['global'].value = newValue;
    onSearch();
});

const openNew = () => {
    academicYear.value = {
        status: AcademicYearStatus.Upcoming
    };
    submitted.value = false;
    academicYearDialog.value = true;
};

const hideDialog = () => {
    academicYearDialog.value = false;
    submitted.value = false;
};

const saveAcademicYear = async () => {
    submitted.value = true;
    isSaving.value = true; // Set loading true

    if (academicYear.value.name?.trim()) {
        try {
            // Date handling: Convert Date objects to YYYY-MM-DD string for API
            const payload = { ...academicYear.value };
            if (payload.startDate instanceof Date) {
                payload.startDate = payload.startDate.toISOString().split('T')[0];
            }
            if (payload.endDate instanceof Date) {
                payload.endDate = payload.endDate.toISOString().split('T')[0];
            }

            if (payload.id) {
                await store.updateAcademicYear(payload.id, payload as UpdateAcademicYearDto);
                toast.add({ severity: 'success', summary: 'Successful', detail: 'Academic Year Updated', life: 3000 });
            } else {
                await store.createAcademicYear(payload as CreateAcademicYearDto);
                toast.add({ severity: 'success', summary: 'Successful', detail: 'Academic Year Created', life: 3000 });
            }
            academicYearDialog.value = false;
            academicYear.value = {};
        } catch (error: any) {
            // Type 'any' for error for now, can be more specific with axios errors
            const errorMessage = error.response?.data?.message || error.message || 'Operation failed';
            toast.add({ severity: 'error', summary: 'Error', detail: errorMessage, life: 3000 });
        } finally {
            isSaving.value = false; // Set loading false
        }
    } else {
        isSaving.value = false; // If validation fails immediately
    }
};

const editAcademicYear = (item: AcademicYearDto) => {
    academicYear.value = { ...item };

    if (academicYear.value.startDate) {
        academicYear.value.startDate = new Date(academicYear.value.startDate);
    }
    if (academicYear.value.endDate) {
        academicYear.value.endDate = new Date(academicYear.value.endDate);
    }

    academicYearDialog.value = true;
};

// Replaced confirmDeleteAcademicYear and deleteAcademicYear with ConfirmationService
const confirmDeleteAcademicYear = (item: AcademicYearDto) => {
    confirm.require({
        message: `Are you sure you want to delete ${item.name}?`,
        header: 'Confirm Deletion',
        icon: 'pi pi-exclamation-triangle',
        accept: async () => {
            isSaving.value = true; // Set loading true during delete
            try {
                if (item.id) {
                    await store.deleteAcademicYear(item.id);
                    academicYear.value = {}; // Clear selected item
                    toast.add({ severity: 'success', summary: 'Successful', detail: 'Academic Year Deleted', life: 3000 });
                }
            } catch (error: any) {
                const errorMessage = error.response?.data?.message || error.message || 'Delete failed';
                toast.add({ severity: 'error', summary: 'Error', detail: errorMessage, life: 3000 });
            } finally {
                isSaving.value = false; // Set loading false
            }
        }
    });
};

const getStatusLabel = (status: AcademicYearStatus) => {
    switch (status) {
        case AcademicYearStatus.Upcoming:
            return 'Upcoming';
        case AcademicYearStatus.Current:
            return 'Current';
        case AcademicYearStatus.Closed:
            return 'Closed';
        default:
            return 'Unknown';
    }
};

const getStatusSeverity = (status: AcademicYearStatus) => {
    switch (status) {
        case AcademicYearStatus.Upcoming:
            return 'info';
        case AcademicYearStatus.Current:
            return 'success';
        case AcademicYearStatus.Closed:
            return 'secondary';
        default:
            return 'contrast';
    }
};
</script>

<template>
    <div class="card">
        <DataTable
            ref="dt"
            :value="store.academicYears"
            lazy
            :paginator="true"
            :rows="lazyParams.rows"
            :totalRecords="store.totalRecords"
            :loading="store.loading"
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
                            <InputText v-model="inputValue" placeholder="Search..." @keydown.enter="onSearch" class="w-full md:w-auto" />
                        </IconField>
                        <Button label="New" icon="pi pi-plus" @click="openNew" />
                    </div>
                </div>
            </template>

            <Column field="name" header="Name" sortable style="min-width: 12rem"></Column>
            <Column field="startDate" header="Start Date" sortable style="min-width: 10rem">
                <template #body="slotProps">
                    {{ slotProps.data.startDate ? new Date(slotProps.data.startDate).toLocaleDateString() : '-' }}
                </template>
            </Column>
            <Column field="endDate" header="End Date" sortable style="min-width: 10rem">
                <template #body="slotProps">
                    {{ slotProps.data.endDate ? new Date(slotProps.data.endDate).toLocaleDateString() : '-' }}
                </template>
            </Column>
            <Column field="status" header="Status" sortable style="min-width: 10rem">
                <template #body="slotProps">
                    <Tag :value="getStatusLabel(slotProps.data.status)" :severity="getStatusSeverity(slotProps.data.status)" />
                </template>
            </Column>
            <Column :exportable="false" style="min-width: 8rem" header="Actions">
                <template #body="slotProps">
                    <Button icon="pi pi-pencil" outlined rounded class="mr-2" @click="editAcademicYear(slotProps.data)" />
                    <Button icon="pi pi-trash" outlined rounded severity="danger" @click="confirmDeleteAcademicYear(slotProps.data)" />
                </template>
            </Column>
        </DataTable>

        <Dialog v-model:visible="academicYearDialog" :style="{ width: '450px' }" header="Academic Year Details" :modal="true">
            <div class="flex flex-col gap-4">
                <div class="flex flex-col gap-2">
                    <label for="name" class="font-bold">Name</label>
                    <InputText id="name" v-model.trim="academicYear.name" required="true" autofocus :invalid="submitted && !academicYear.name" fluid />
                    <small class="text-red-500" v-if="submitted && !academicYear.name">Name is required.</small>
                </div>
                <div class="flex flex-col gap-2">
                    <label for="startDate" class="font-bold">Start Date</label>
                    <Calendar id="startDate" :v-model="academicYear.startDate" showIcon dateFormat="yy-mm-dd" fluid />
                </div>
                <div class="flex flex-col gap-2">
                    <label for="endDate" class="font-bold">End Date</label>
                    <Calendar id="endDate" :v-model="academicYear.endDate" showIcon dateFormat="yy-mm-dd" fluid />
                </div>
                <div class="flex flex-col gap-2">
                    <label for="status" class="font-bold">Status</label>
                    <SelectButton v-model="academicYear.status" :options="statuses" optionLabel="label" optionValue="value" class="w-full" :pt="{ button: { class: 'w-full' } }" />
                </div>
            </div>
            <template #footer>
                <Button label="Cancel" icon="pi pi-times" text @click="hideDialog" />
                <Button label="Save" icon="pi pi-check" :loading="isSaving" @click="saveAcademicYear" />
            </template>
        </Dialog>

        <ConfirmDialog />
        <!-- Added ConfirmDialog here -->
    </div>
</template>
