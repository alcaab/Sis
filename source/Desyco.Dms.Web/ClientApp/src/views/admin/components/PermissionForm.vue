<script setup lang="ts">
    import { ref, computed, watch } from "vue";
    import { useI18n } from "vue-i18n";
    import type { PermissionSchema, FeaturePermission, PermissionGroup } from "@/types/permissions";
    import { PermissionAction } from "@/types/permissions";
    import Accordion from "primevue/accordion";
    import AccordionPanel from "primevue/accordionpanel";
    import AccordionHeader from "primevue/accordionheader";
    import AccordionContent from "primevue/accordioncontent";

    const props = defineProps<{
        initialSchema: PermissionSchema | null;
        entityName: string | null;
        loading: boolean;
        backLabel?: string;
        subtitle?: string;
    }>();

    const emit = defineEmits<{
        (e: "save", permissions: FeaturePermission[]): void;
        (e: "cancel"): void;
    }>();

    const { t } = useI18n();

    const searchQuery = ref("");
    const activeAccordionIndices = ref<number[]>([]);
    const permissionSchema = ref<PermissionSchema | null>(null);

    watch(
        () => props.initialSchema,
        (newSchema) => {
            if (newSchema) {
                permissionSchema.value = JSON.parse(JSON.stringify(newSchema));
                expandAll();
            }
        },
        { immediate: true, deep: true },
    );

    const filteredGroups = computed(() => {
        if (!permissionSchema.value) return [];

        const query = searchQuery.value.toLowerCase().trim();
        if (!query) return permissionSchema.value.groups;

        return permissionSchema.value.groups
            .map((group) => {
                const groupMatches = group.groupName.toLowerCase().includes(query);
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

    function expandAll() {
        if (permissionSchema.value) {
            activeAccordionIndices.value = permissionSchema.value.groups.map((_, i) => i);
        }
    }

    const collapseAll = () => {
        activeAccordionIndices.value = [];
    };

    const onSave = () => {
        if (!permissionSchema.value) return;

        const updatedPermissions: FeaturePermission[] = [];

        permissionSchema.value.groups.forEach((group) => {
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

        emit("save", updatedPermissions);
    };
</script>

<template>
    <div class="w-full animate fade-in">
        <!-- Header & Toolbar -->
        <div class="flex flex-col md:flex-row md:justify-between md:items-center mb-6 gap-4">
            <div>
                <div
                    class="flex items-center gap-2 text-surface-500 dark:text-surface-400 mb-1 cursor-pointer hover:text-primary transition-colors"
                    @click="$emit('cancel')"
                >
                    <i class="pi pi-arrow-left text-sm"></i>
                    <span class="text-sm font-medium">{{ backLabel || t("common.buttons.back") }}</span>
                </div>
                <div class="flex items-center gap-3">
                    <div class="w-10 h-10 rounded-full bg-primary/10 flex items-center justify-center text-primary">
                        <i class="pi pi-shield text-xl"></i>
                    </div>
                    <div>
                        <h2 class="text-xl font-bold m-0">{{ entityName || "Unknown" }}</h2>
                        <span class="text-sm text-surface-500">{{ subtitle || t("admin.permissions.subtitle") }}</span>
                    </div>
                </div>
            </div>

            <div class="flex items-center gap-2">
                <Button
                    :label="t('common.buttons.saveAll')"
                    icon="pi pi-check"
                    :loading="loading"
                    @click="onSave"
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
            v-if="loading && !permissionSchema"
            class="flex justify-center"
        />

        <div v-else-if="permissionSchema && filteredGroups.length > 0">
            <Accordion
                :multiple="true"
                :value="activeAccordionIndices"
            >
                <AccordionPanel
                    v-for="(group, i) in filteredGroups"
                    :key="group.groupName"
                    :value="i"
                >
                    <AccordionHeader>
                        <div class="flex align-items-center justify-content-between w-full pr-4">
                            <span class="font-bold">{{ group.groupName }}</span>
                            <Badge
                                :value="getActiveCount(group)"
                                :severity="getActiveCount(group) > 0 ? undefined : 'secondary'"
                                class="ml-2"
                            ></Badge>
                        </div>
                    </AccordionHeader>
                    <AccordionContent>
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
                                        :title="t('admin.permissions.toggleAllTooltip')"
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
                    </AccordionContent>
                </AccordionPanel>
            </Accordion>
        </div>
        <div
            v-else-if="permissionSchema && filteredGroups.length === 0"
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
