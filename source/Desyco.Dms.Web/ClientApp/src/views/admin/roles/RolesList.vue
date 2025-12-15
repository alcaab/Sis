<script setup lang="ts">
    import { ref, onMounted } from "vue";
    import { useRouter } from "vue-router";
    import { useRoleStore } from "@/stores/roleStore";
    import { useNotification } from "@/composables/useNotification";
    import { useConfirm } from "primevue/useconfirm";
    import { useI18n } from "vue-i18n";
    import { FilterMatchMode } from "@primevue/core/api";
    import type { RoleDto, CreateRoleDto, UpdateRoleDto } from "@/types/role";
    import RoleForm from "./RoleForm.vue";
    import TableActions from "@/components/common/TableActions.vue";

    const router = useRouter();
    const roleStore = useRoleStore();
    const notify = useNotification();
    const confirm = useConfirm();
    const { t } = useI18n();

    const dt = ref();
    const isDeleting = ref(false);
    const deletingItemId = ref<string | null>(null);

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
            notify.showError(error, t("admin.roles.list.notifications.loadError"));
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

    const saveRole = async (roleData: CreateRoleDto | UpdateRoleDto) => {
        submitted.value = true;
        loading.value = true;

        try {
            if (selectedRole.value && "id" in selectedRole.value) {
                await roleStore.updateRole(selectedRole.value.id, roleData as UpdateRoleDto);
                notify.showSuccess(t("admin.roles.list.notifications.updateSuccess"));
            } else {
                await roleStore.createRole(roleData as CreateRoleDto);
                notify.showSuccess(t("admin.roles.list.notifications.createSuccess"));
            }
            hideDialog();
            await fetchRoles(); // Refresh local list
        } catch (error) {
            notify.showError(error, t("admin.roles.list.notifications.operationError"));
        } finally {
            loading.value = false;
        }
    };

    const editRole = (role: RoleDto) => {
        selectedRole.value = { ...role };
        roleDialog.value = true;
    };

    const confirmDeleteRole = (role: RoleDto) => {
        confirm.require({
            message: t("admin.roles.list.confirmDeleteMessage", { name: role.name }),
            header: t("admin.roles.list.confirmDeleteTitle"),
            icon: "pi pi-exclamation-triangle",
            accept: async () => {
                isDeleting.value = true;
                deletingItemId.value = role.id;
                try {
                    await roleStore.deleteRole(role.id);
                    notify.showSuccess(t("admin.roles.list.notifications.deleteSuccess"));
                    await fetchRoles();
                } catch (error) {
                    notify.showError(error, t("admin.roles.list.notifications.deleteError"));
                } finally {
                    isDeleting.value = false;
                    deletingItemId.value = null;
                }
            },
        });
    };

    const managePermissions = (role: RoleDto) => {
        router.push({ name: "role-permissions", params: { id: role.id } });
    };
</script>

<template>
    <DataTable
        ref="dt"
        :value="roles"
        dataKey="id"
        :loading="loading || isDeleting"
        :paginator="true"
        :rows="10"
        :filters="filters"
        paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
        :rowsPerPageOptions="[5, 10, 25]"
        currentPageReportTemplate="Showing {first} to {last} of {totalRecords} entries"
    >
        <template #header>
            <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
                <h4 class="m-0 text-xl font-semibold text-surface-900 dark:text-surface-0">{{ t("admin.roles.list.header") }}</h4>
                <div class="flex items-center gap-2">
                    <IconField iconPosition="left">
                        <InputIcon>
                            <i class="pi pi-search" />
                        </InputIcon>
                        <InputText
                            v-model="filters['global'].value"
                            :placeholder="t('admin.roles.list.searchPlaceholder')"
                            class="w-full md:w-auto"
                        />
                    </IconField>
                    <Button
                        :label="t('admin.roles.list.newButton')"
                        icon="pi pi-plus"
                        @click="openNew"
                    />
                </div>
            </div>
        </template>

        <Column
            field="name"
            :header="t('admin.roles.list.nameHeader')"
            :sortable="true"
            style="min-width: 12rem"
        ></Column>
        <Column
            field="description"
            :header="t('admin.roles.list.descriptionHeader')"
            :sortable="true"
            style="min-width: 16rem"
        ></Column>
        <Column
            :exportable="false"
            style="min-width: 12rem"
        >
            <template #header>
                <div class="flex justify-end w-full">{{ t("admin.roles.list.actionsHeader") }}</div>
            </template>
            <template #body="slotProps">
                <div class="flex justify-end gap-2">
                    <Button
                        icon="pi pi-shield"
                        severity="info"
                        outlined
                        class="mr-2"
                        v-tooltip.top="t('admin.roles.list.managePermissionsTooltip')"
                        @click="managePermissions(slotProps.data)"
                    />
                    <TableActions
                        :id="slotProps.data.id"
                        :loading="deletingItemId === slotProps.data.id"
                        @edit="editRole(slotProps.data)"
                        @delete="confirmDeleteRole(slotProps.data)"
                    />
                </div>
            </template>
        </Column>
    </DataTable>

    <Dialog
        v-model:visible="roleDialog"
        :style="{ width: '50vw' }"
        :breakpoints="{ '960px': '75vw', '640px': '90vw' }"
        :header="t('admin.roles.form.header')"
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
