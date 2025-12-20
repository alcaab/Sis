<script setup lang="ts">
    import { ref, onMounted } from "vue";
    import { useRouter } from "vue-router";
    import { useUserStore } from "@/stores/userStore";
    import { useNotification } from "@/composables/useNotification";
    import { useConfirm } from "primevue/useconfirm";
    import { useI18n } from "vue-i18n";
    import { FilterMatchMode } from "@primevue/core/api";
    import TableActions from "@/components/common/TableActions.vue";
    import { UserDto } from "@/types/user.ts";

    const router = useRouter();
    const userStore = useUserStore();
    const notify = useNotification();
    const confirm = useConfirm();
    const { t } = useI18n();

    const dt = ref();
    const isDeleting = ref(false);
    const deletingItemId = ref<string | null>(null);
    const users = ref<UserDto[]>([]);
    const loading = ref(false);

    const filters = ref({
        global: { value: "", matchMode: FilterMatchMode.CONTAINS },
    });

    onMounted(async () => {
        await fetchUsers();
    });

    const fetchUsers = async () => {
        loading.value = true;
        try {
            await userStore.fetchAllUsers();
            users.value = userStore.users;
        } catch (error) {
            notify.showError(error, t("common.notifications.loadError"));
        } finally {
            loading.value = false;
        }
    };

    const editUser = (item: UserDto) => {
        router.push({ name: "user-edit", params: { id: item.id } });
    };

    const openNew = () => {
        router.push({ name: "user-create" });
    };

    const confirmDelete = (user: UserDto) => {
        confirm.require({
            message: t("common.notifications.confirmDelete", { name: user.userName }),
            header: t("common.notifications.confirmDeleteTitle"),
            icon: "pi pi-exclamation-triangle",
            accept: async () => {
                isDeleting.value = true;
                deletingItemId.value = user.id;
                try {
                    await userStore.deleteUser(user.id);
                    notify.showSuccess(t("common.notifications.deleteSuccess"));
                } catch (error) {
                    notify.showError(error, t("common.notifications.deleteError"));
                } finally {
                    isDeleting.value = false;
                    deletingItemId.value = null;
                }
            },
        });
    };

    const managePermissions = (role: UserDto) => {
        router.push({ name: "user-permissions", params: { id: role.id } });
    };

    const activeState = (lockoutEnabled: boolean) =>
        !lockoutEnabled ? t("common.activeState.false") : t("common.activeState.true");
</script>

<template>
    <div class="w-full animate fade-in">
        <DataTable
            ref="dt"
            :value="users"
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
                    <h4 class="m-0 text-xl font-semibold text-surface-900 dark:text-surface-0">
                        {{ t("admin.users.title") }}
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
                            @click="openNew"
                            icon="pi pi-plus"
                        />
                    </div>
                </div>
            </template>
            <Column
                field="userName"
                :header="t('admin.users.userName')"
                :sortable="true"
            ></Column>
            <!--            <Column-->
            <!--                field="email"-->
            <!--                :header="t('admin.users.email')"-->
            <!--                :sortable="true"-->
            <!--            ></Column>-->
            <Column
                field="firstName"
                :header="t('admin.users.firstName')"
                :sortable="true"
            ></Column>
            <Column
                field="lastName"
                :header="t('admin.users.lastName')"
                :sortable="true"
            ></Column>
            <Column
                field="lockoutEnabled"
                :header="t('admin.users.lockoutEnabled')"
                :sortable="true"
                style="width: 10px"
            >
                <template #body="{ data }">
                    <div class="flex justify-center">
                        <i
                            :title="activeState(data.lockoutEnabled)"
                            :class="[
                                'pi',
                                {
                                    'pi-check-circle text-green-500': data.lockoutEnabled,
                                    'pi-times-circle text-red-500': !data.lockoutEnabled,
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
                    <div class="flex justify-end w-full">{{ t("common.actions") }}</div>
                </template>
                <template #body="slotProps">
                    <TableActions
                        :id="slotProps.data.id"
                        :loading="deletingItemId === slotProps.data.id"
                        @delete="confirmDelete(slotProps.data)"
                        @edit="editUser(slotProps.data)"
                        :editTooltip="t('common.buttons.edit')"
                        :deleteTooltip="t('common.buttons.delete')"
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
                    @click="fetchUsers"
                    :title="t('common.buttons.refresh')"
                />
            </template>
        </DataTable>
    </div>
    <ConfirmDialog></ConfirmDialog>
</template>
