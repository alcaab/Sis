<script setup lang="ts">
    import { ref, onMounted } from "vue";
    import { useRouter } from "vue-router";
    import { useRoleStore } from "@/stores/roleStore";
    import { useNotification } from "@/composables/useNotification";
    import { useDeleteConfirm } from "@/composables/useDeleteConfirm";
    import { useI18n } from "vue-i18n";
    import { FilterMatchMode } from "@primevue/core/api";
    import type { RoleDto } from "@/types/role";
    import RoleForm from "./RoleForm.vue";
    import TableActions from "@/components/common/TableActions.vue";
    import Button from "primevue/button";

    const router = useRouter();
    const roleStore = useRoleStore();
    const notify = useNotification();
    const { confirmDelete, deletingItemId } = useDeleteConfirm();
    const { t } = useI18n();

    const dt = ref();

    const roles = ref<RoleDto[]>([]);
    const selectedRole = ref<RoleDto | null>(null);
    const roleDialog = ref(false);
    const submitted = ref(false);
    const loading = ref(false);

    const filters = ref({
        global: { value: "", matchMode: FilterMatchMode.CONTAINS },
    });

    onMounted(async () => {
        await fetchRoles();
    });

    const fetchRoles = async () => {
        loading.value = true;
        try {
            await roleStore.fetchAllRoles();
            roles.value = roleStore.roles;
        } catch (error) {
            notify.showError(error, t("common.notifications.loadError"));
        } finally {
            loading.value = false;
        }
    };

    const openNew = () => {
        selectedRole.value = null;
        submitted.value = false;
        roleDialog.value = true;
    };

    const hideDialog = () => {
        roleDialog.value = false;
        submitted.value = false;
    };

    const saveRole = async (roleData: RoleDto) => {
        submitted.value = true;
        loading.value = true;

        try {
            if (selectedRole.value && "id" in selectedRole.value) {
                await roleStore.updateRole(selectedRole.value.id, roleData as RoleDto);
                notify.showSuccess(t("common.notifications.updateSuccess"));
            } else {
                await roleStore.createRole(roleData);
                notify.showSuccess(t("common.notifications.createSuccess"));
            }
            hideDialog();
            await fetchRoles(); // Refresh local list
        } catch (error) {
            notify.showError(error, t("common.notifications.operationError"));
        } finally {
            loading.value = false;
        }
    };

    const editRole = (role: RoleDto) => {
        selectedRole.value = { ...role };
        roleDialog.value = true;
    };

    const onConfirmDelete = (role: RoleDto) => {
        confirmDelete({
            item: { id: role.id, name: role.name },
            deleteAction: roleStore.deleteRole,
            onSuccess: fetchRoles,
        });
    };

    const managePermissions = (role: RoleDto) => {
        router.push({ name: "role-permissions", params: { id: role.id } });
    };
</script>

<template>
    <div class="w-full animate fade-in">
        <DataTable
            ref="dt"
            :value="roles"
            dataKey="id"
            :loading="loading || !!deletingItemId"
            :paginator="true"
            :rows="10"
            :filters="filters"
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
            :rowsPerPageOptions="[5, 10, 25]"
            currentPageReportTemplate="Showing {first} to {last} of {totalRecords} entries"
        >
            <template #header>
                <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
                    <h4 class="m-0 text-xl font-semibold text-surface-900 dark:text-surface-0">
                        {{ t("admin.roles.title") }}
                    </h4>
                    <div class="flex items-center gap-2">
                        <IconField iconPosition="left">
                            <InputIcon>
                                <i class="pi pi-search" />
                            </InputIcon>
                            <InputText
                                v-model="filters['global'].value"
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
                :header="t('admin.roles.name')"
                :sortable="true"
                style="min-width: 12rem"
            ></Column>
            <Column
                field="description"
                :header="t('admin.roles.description')"
                :sortable="true"
                style="min-width: 16rem"
            ></Column>
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
                        @edit="editRole(slotProps.data)"
                        @delete="onConfirmDelete(slotProps.data)"
                    >
                        <template #left>
                            <Button
                                icon="pi pi-shield"
                                severity="info"
                                outlined
                                :title="t('common.buttons.permissions')"
                                @click="managePermissions(slotProps.data)"
                            />
                        </template>
                    </TableActions>
                </template>
            </Column>
            <template #paginatorstart>
                <Button
                    type="button"
                    icon="pi pi-refresh"
                    text
                    @click="fetchRoles"
                    :title="t('common.buttons.refresh')"
                />
            </template>
        </DataTable>
    </div>
    <Dialog
        v-model:visible="roleDialog"
        :style="{ width: '50vw' }"
        :breakpoints="{ '960px': '75vw', '640px': '90vw' }"
        :header="t('admin.roles.dialogHeader')"
        :modal="true"
        class="p-fluid"
    >
        <RoleForm
            :initialData="selectedRole"
            :submitted="submitted"
            :loading="loading"
            @submit="saveRole"
            @cancel="hideDialog"
        />
    </Dialog>
    <ConfirmDialog></ConfirmDialog>
</template>
