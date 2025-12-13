<script setup lang="ts">
    import { ref, onMounted, computed, watch } from "vue";
    import { useRoute, useRouter } from "vue-router";
    import { useRoleStore } from "@/stores/roleStore";
    import { useNotification } from "@/composables/useNotification";
    import type { PermissionSchema, PermissionGroup, FeaturePermission, PermissionItem } from "@/types/permissions";
    import { PermissionAction } from "@/types/permissions";

    const route = useRoute();
    const router = useRouter();
    const roleStore = useRoleStore();
    const notify = useNotification();

    const roleId = ref<string | null>(null);
    const roleName = ref<string | null>(null);
    const loading = ref(false);
    const permissionSchema = ref<PermissionSchema | null>(null);

    // Computed property to create the mutable structure for checkboxes
    const editablePermissionSchema = computed<PermissionSchema | null>(() => {
        if (!permissionSchema.value) return null;

        const schemaCopy: PermissionSchema = JSON.parse(JSON.stringify(permissionSchema.value));
        return schemaCopy;
    });

    onMounted(async () => {
        roleId.value = route.params.id as string;
        if (roleId.value) {
            await fetchRolePermissions();
        } else {
            router.push({ name: "roles-list" });
            notify.showError("Role ID not provided.");
        }
    });

    const fetchRolePermissions = async () => {
        loading.value = true;
        try {
            await roleStore.fetchRolePermissions(roleId.value!);
            permissionSchema.value = roleStore.currentRolePermissions;
            roleName.value = permissionSchema.value?.entityName || "Unknown Role";
        } catch (error) {
            notify.showError(error, "Failed to load role permissions");
            router.push({ name: "roles-list" });
        } finally {
            loading.value = false;
        }
    };

    const updatePermissions = async () => {
        loading.value = true;
        try {
            if (!editablePermissionSchema.value) return;

            const updatedPermissions: FeaturePermission[] = [];

            editablePermissionSchema.value.groups.forEach((group) => {
                group.features.forEach((feature) => {
                    // Add standard R/W/D permissions
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

                    // TODO: Handle custom permissions (e.g., Print)
                    // This would require a dedicated dialog or more complex UI
                    feature.customPermissions.forEach((customAction) => {
                        // For now, assume if it exists in the list, it's granted
                        // In a real scenario, custom actions might need their own checkboxes/toggles
                        // For custom permissions, action will be PermissionAction.None or a specific custom enum value if we extend it.
                        // For now, assume custom actions are always "granted" if they are present in the list,
                        // and we just send them as PermissionAction.None
                        updatedPermissions.push({
                            featureId: feature.featureId,
                            featureCode: feature.code,
                            action: PermissionAction.None,
                            isGranted: true,
                        });
                    });
                });
            });

            await roleStore.updateRolePermissions(roleId.value!, updatedPermissions);
            notify.showSuccess("Permissions updated successfully!");
        } catch (error) {
            notify.showError(error, "Failed to update permissions");
        } finally {
            loading.value = false;
        }
    };

    const goBack = () => {
        router.push({ name: "roles-list" });
    };
</script>

<template>
    <div class="card">
        <div class="flex justify-content-between align-items-center mb-4">
            <div>
                <Button
                    icon="pi pi-arrow-left"
                    label="Back to Roles"
                    text
                    @click="goBack"
                />
                <h2 class="mt-2 mb-0">Permissions for {{ roleName }}</h2>
            </div>
            <Button
                label="Save Changes"
                icon="pi pi-save"
                :loading="loading"
                @click="updatePermissions"
            />
        </div>

        <ProgressSpinner v-if="loading" />

        <div v-else-if="editablePermissionSchema">
            <Accordion :activeIndex="0">
                <AccordionTab
                    v-for="group in editablePermissionSchema.groups"
                    :key="group.groupName"
                    :header="group.groupName"
                >
                    <DataTable :value="group.features">
                        <Column
                            field="description"
                            header="Feature"
                            style="min-width: 150px"
                        >
                            <template #body="slotProps">
                                {{ slotProps.data.description }} ({{ slotProps.data.code }})
                            </template>
                        </Column>
                        <Column header="Read">
                            <template #body="slotProps">
                                <Checkbox
                                    v-model="slotProps.data.read"
                                    binary
                                />
                            </template>
                        </Column>
                        <Column header="Write">
                            <template #body="slotProps">
                                <Checkbox
                                    v-model="slotProps.data.write"
                                    binary
                                />
                            </template>
                        </Column>
                        <Column header="Delete">
                            <template #body="slotProps">
                                <Checkbox
                                    v-model="slotProps.data.delete"
                                    binary
                                />
                            </template>
                        </Column>
                        <Column header="Custom">
                            <template #body="slotProps">
                                <div v-if="slotProps.data.customPermissions && slotProps.data.customPermissions.length">
                                    <Tag
                                        v-for="perm in slotProps.data.customPermissions"
                                        :key="perm"
                                        :value="perm"
                                        severity="secondary"
                                        class="mr-1"
                                    />
                                </div>
                                <span v-else>-</span>
                            </template>
                        </Column>
                    </DataTable>
                </AccordionTab>
            </Accordion>
        </div>
        <Message
            v-else
            severity="info"
            >No permissions schema available for this role.</Message
        >
    </div>
</template>
