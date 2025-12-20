<script setup lang="ts">
    import { ref, onMounted, computed } from "vue";
    import { useRoute, useRouter } from "vue-router";
    import { useI18n } from "vue-i18n";
    import { useRoleStore } from "@/stores/roleStore";
    import { useNotification } from "@/composables/useNotification";
    import type { PermissionSchema, FeaturePermission, PermissionGroup } from "@/types/permissions";
    import { PermissionAction } from "@/types/permissions";

    const route = useRoute();
    const router = useRouter();
    const roleStore = useRoleStore();
    const notify = useNotification();
    const { t } = useI18n();

    const roleId = ref<string | null>(null);
    const roleName = ref<string | null>(null);
    const loading = ref(false);
    const searchQuery = ref("");
    const activeAccordionIndices = ref<number[]>([]);

    // Mutable schema for UI
    const editableSchema = ref<PermissionSchema | null>(null);

    onMounted(async () => {
        roleId.value = route.params.id as string;
        if (roleId.value) {
            await fetchRolePermissions();
        } else {
            router.push({ name: "roles-list" });
            notify.showError(t("admin.permissions.notifications.missingId"));
        }
    });

    const fetchRolePermissions = async () => {
        loading.value = true;
        try {
            await roleStore.fetchRolePermissions(roleId.value!);
            const rawData = roleStore.currentRolePermissions;

            if (rawData) {
                // Deep copy for editing
                editableSchema.value = JSON.parse(JSON.stringify(rawData));
                roleName.value = rawData.entityName || "Unknown Role";
                expandAll();
            }
        } catch (error) {
            notify.showError(error, t("admin.permissions.notifications.loadError"));
            router.push({ name: "roles-list" });
        } finally {
            loading.value = false;
        }
    };

    // Filter logic
    const filteredGroups = computed(() => {
        if (!editableSchema.value) return [];

        const query = searchQuery.value.toLowerCase().trim();
        if (!query) return editableSchema.value.groups;

        return editableSchema.value.groups
            .map((group) => {
                // Check if group name matches
                const groupMatches = group.groupName.toLowerCase().includes(query);

                // Filter features
                const matchingFeatures = group.features.filter(
                    (f) => f.description.toLowerCase().includes(query) || f.code.toLowerCase().includes(query),
                );

                if (groupMatches || matchingFeatures.length > 0) {
                    return {
                        ...group,
                        features: matchingFeatures.length > 0 ? matchingFeatures : groupMatches ? group.features : [],
                    };
                }
                return null;
            })
            .filter((g) => g !== null && g.features.length > 0) as PermissionGroup[];
    });

    // Helper to count active permissions in a group
    const getActiveCount = (group: PermissionGroup) => {
        let count = 0;
        group.features.forEach((f) => {
            if (f.read) count++;
            if (f.write) count++;
            if (f.delete) count++;
            if (f.customPermissions) count += f.customPermissions.length;
        });
        return count;
    };

    // Toggle "Full Access" for a row
    const toggleFullAccess = (item: any, value: boolean) => {
        item.read = value;
        item.write = value;
        item.delete = value;

        if (item.availableCustomPermissions) {
            if (value) {
                item.customPermissions = [...item.availableCustomPermissions];
            } else {
                item.customPermissions = [];
            }
        }
    };

    const isFullAccess = (item: any) => {
        const standard = item.read && item.write && item.delete;
        const availableCustomPermissions = item.availableCustomPermissions ?? [];
        const custom =
            availableCustomPermissions.length == 0 ||
            (availableCustomPermissions.length > 0 &&
                item.customPermissions &&
                availableCustomPermissions.every((p: string) => item.customPermissions.includes(p)));
        return standard && custom;
    };

    const isCustomGranted = (feature: any, permName: string) => {
        return feature.customPermissions && feature.customPermissions.includes(permName);
    };

    const toggleCustom = (feature: any, permName: string, value: boolean) => {
        if (!feature.customPermissions) feature.customPermissions = [];

        if (value) {
            if (!feature.customPermissions.includes(permName)) {
                feature.customPermissions.push(permName);
            }
        } else {
            feature.customPermissions = feature.customPermissions.filter((p: string) => p !== permName);
        }
    };

    const expandAll = () => {
        if (editableSchema.value) {
            activeAccordionIndices.value = editableSchema.value.groups.map((_, i) => i);
        }
    };

    const collapseAll = () => {
        activeAccordionIndices.value = [];
    };

    const updatePermissions = async () => {
        loading.value = true;
        try {
            if (!editableSchema.value) return;

            const updatedPermissions: FeaturePermission[] = [];

            // Iterate over the FULL schema (editableSchema), not the filtered view
            editableSchema.value.groups.forEach((group) => {
                group.features.forEach((feature) => {
                    if (feature.read)
                        updatedPermissions.push({
                            featureId: feature.featureId,
                            featureCode: feature.code,
                            action: PermissionAction.Read,
                            isGranted: true,
                        });
                    if (feature.write)
                        updatedPermissions.push({
                            featureId: feature.featureId,
                            featureCode: feature.code,
                            action: PermissionAction.Write,
                            isGranted: true,
                        });
                    if (feature.delete)
                        updatedPermissions.push({
                            featureId: feature.featureId,
                            featureCode: feature.code,
                            action: PermissionAction.Delete,
                            isGranted: true,
                        });

                    if (feature.customPermissions && feature.customPermissions.length > 0) {
                        feature.customPermissions.forEach((permName: string) => {
                            updatedPermissions.push({
                                featureId: feature.featureId,
                                featureCode: feature.code,
                                action: PermissionAction.None,
                                customActionName: permName,
                                isGranted: true,
                            });
                        });
                    }
                });
            });

            await roleStore.updateRolePermissions(roleId.value!, updatedPermissions);
            notify.showSuccess(t("admin.permissions.notifications.updateSuccess"));
        } catch (error) {
            notify.showError(error, t("admin.permissions.notifications.updateError"));
        } finally {
            loading.value = false;
        }
    };

    const goBack = () => {
        router.push({ name: "roles-list" });
    };
</script>

<template>
    <div class="w-full animate fade-in">
        <!-- Header & Toolbar -->
        <div class="flex flex-col md:flex-row md:justify-between md:items-center mb-6 gap-4">
            <div>
                <div
                    class="flex items-center gap-2 text-surface-500 dark:text-surface-400 mb-1 cursor-pointer hover:text-primary transition-colors"
                    @click="goBack"
                >
                    <i class="pi pi-arrow-left text-sm"></i>
                    <span class="text-sm font-medium">{{ t("admin.permissions.backToRoles") }}</span>
                </div>
                <div class="flex items-center gap-3">
                    <div class="w-10 h-10 rounded-full bg-primary/10 flex items-center justify-center text-primary">
                        <i class="pi pi-shield text-xl"></i>
                    </div>
                    <div>
                        <h2 class="text-xl font-bold m-0">{{ roleName }}</h2>
                        <span class="text-sm text-surface-500">{{ t("admin.permissions.subtitle") }}</span>
                    </div>
                </div>
            </div>

            <div class="flex items-center gap-2">
                <Button
                    :label="t('common.buttons.saveAll')"
                    icon="pi pi-check"
                    :loading="loading"
                    @click="updatePermissions"
                />
            </div>
        </div>

        <!-- Search & Global Controls -->
        <div
            class="flex flex-col sm:flex-row justify-between items-center mb-4 gap-3 bg-surface-50 dark:bg-surface-900 p-3 rounded-border border border-surface-200 dark:border-surface-700"
        >
            <IconField
                iconPosition="left"
                class="w-full sm:w-auto"
            >
                <InputIcon>
                    <i class="pi pi-search" />
                </InputIcon>
                <InputText
                    v-model="searchQuery"
                    :placeholder="t('common.placeholders.search')"
                    class="w-full sm:w-80"
                />
            </IconField>

            <div class="flex gap-2">
                <Button
                    :label="t('admin.permissions.expandAll')"
                    icon="pi pi-angle-double-down"
                    size="small"
                    severity="secondary"
                    text
                    @click="expandAll"
                />
                <Button
                    :label="t('admin.permissions.collapseAll')"
                    icon="pi pi-angle-double-up"
                    size="small"
                    severity="secondary"
                    text
                    @click="collapseAll"
                />
            </div>
        </div>

        <ProgressSpinner
            v-if="loading && !editableSchema"
            class="flex justify-center"
        />

        <div v-else-if="editableSchema && filteredGroups.length > 0">
            <Accordion
                :multiple="true"
                :activeIndex="activeAccordionIndices"
            >
                <AccordionTab
                    v-for="group in filteredGroups"
                    :key="group.groupName"
                >
                    <template #header>
                        <div class="flex align-items-center justify-content-between w-full pr-4">
                            <span class="font-bold">{{ group.groupName }}</span>
                            <Badge
                                :value="getActiveCount(group)"
                                :severity="getActiveCount(group) > 0 ? undefined : 'secondary'"
                                class="ml-2"
                            ></Badge>
                        </div>
                    </template>
                    <!--stripedRows-->
                    <DataTable
                        :value="group.features"
                        size="small"
                        class="p-datatable-sm"
                    >
                        <Column
                            field="description"
                            :header="t('admin.permissions.featureHeader')"
                            style="min-width: 250px"
                        >
                            <template #body="slotProps">
                                <div class="flex flex-col">
                                    <span class="font-medium text-surface-900 dark:text-surface-0">{{
                                        slotProps.data.description
                                    }}</span>
                                    <span class="text-xs text-surface-500">{{ slotProps.data.code }}</span>
                                </div>
                            </template>
                        </Column>

                        <!-- Full Access Toggle -->
                        <Column
                            :header="t('admin.permissions.allHeader')"
                            headerClass="text-center"
                            bodyClass="text-center"
                            style="width: 80px"
                        >
                            <template #body="slotProps">
                                <Checkbox
                                    :modelValue="isFullAccess(slotProps.data)"
                                    @update:modelValue="(val) => toggleFullAccess(slotProps.data, val)"
                                    binary
                                    v-tooltip.top="t('admin.permissions.toggleAllTooltip')"
                                />
                            </template>
                        </Column>

                        <Column
                            :header="t('admin.permissions.readHeader')"
                            headerClass="text-center"
                            bodyClass="text-center"
                            style="width: 80px"
                        >
                            <template #body="slotProps">
                                <Checkbox
                                    v-model="slotProps.data.read"
                                    binary
                                />
                            </template>
                        </Column>
                        <Column
                            :header="t('admin.permissions.writeHeader')"
                            headerClass="text-center"
                            bodyClass="text-center"
                            style="width: 80px"
                        >
                            <template #body="slotProps">
                                <Checkbox
                                    v-model="slotProps.data.write"
                                    binary
                                />
                            </template>
                        </Column>
                        <Column
                            :header="t('admin.permissions.deleteHeader')"
                            headerClass="text-center"
                            bodyClass="text-center"
                            style="width: 80px"
                        >
                            <template #body="slotProps">
                                <Checkbox
                                    v-model="slotProps.data.delete"
                                    binary
                                />
                            </template>
                        </Column>
                        <Column
                            :header="t('admin.permissions.customHeader')"
                            style="min-width: 200px"
                        >
                            <template #body="slotProps">
                                <div
                                    v-if="
                                        slotProps.data.availableCustomPermissions &&
                                        slotProps.data.availableCustomPermissions.length
                                    "
                                    class="flex gap-3 flex-wrap items-center"
                                >
                                    <div
                                        v-for="perm in slotProps.data.availableCustomPermissions"
                                        :key="perm"
                                        class="flex items-center gap-1"
                                    >
                                        <Checkbox
                                            :inputId="`${slotProps.data.code}-${perm}`"
                                            :modelValue="isCustomGranted(slotProps.data, perm)"
                                            @update:modelValue="(val) => toggleCustom(slotProps.data, perm, val)"
                                            binary
                                        />
                                        <label
                                            :for="`${slotProps.data.code}-${perm}`"
                                            class="text-sm cursor-pointer select-none"
                                            >{{ perm }}</label
                                        >
                                    </div>
                                </div>
                                <span
                                    v-else
                                    class="text-surface-400 text-xs italic"
                                ></span>
                            </template>
                        </Column>
                    </DataTable>
                </AccordionTab>
            </Accordion>
        </div>
        <div
            v-else-if="editableSchema && filteredGroups.length === 0"
            class="text-center py-8"
        >
            <i class="pi pi-filter-slash text-4xl text-surface-400 mb-3"></i>
            <p class="text-lg text-surface-600">{{ t("admin.permissions.noResults", { query: searchQuery }) }}</p>
            <Button
                :label="t('admin.permissions.clearSearch')"
                size="small"
                outlined
                class="mt-2"
                @click="searchQuery = ''"
            />
        </div>

        <Message
            v-else
            severity="info"
            >{{ t("admin.permissions.noSchema") }}</Message
        >
    </div>
</template>

<style scoped>
    :deep(.p-accordion-header-link) {
        background-color: var(--surface-50);
        border-radius: var(--border-radius);
        margin-bottom: 0.5rem;
    }
    :deep(.p-accordion-content) {
        border: none;
        padding: 0 0 1rem 0;
    }
    :deep(.p-accordion-tab-active .p-accordion-header-link) {
        background-color: var(--surface-100);
        color: var(--primary-color);
    }
</style>
