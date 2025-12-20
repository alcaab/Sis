<script setup lang="ts">
    import { ref, onMounted, computed } from "vue";
    import { useRoute, useRouter } from "vue-router";
    import { useI18n } from "vue-i18n";
    import { useRoleStore } from "@/stores/roleStore";
    import { useNotification } from "@/composables/useNotification";
    import type { FeaturePermission } from "@/types/permissions";
    import PermissionForm from "../components/PermissionForm.vue";

    const route = useRoute();
    const router = useRouter();
    const roleStore = useRoleStore();
    const notify = useNotification();
    const { t } = useI18n();
    const roleId = computed(() => route.params.id as string);
    const loading = ref(false);

    onMounted(async () => {
        if (roleId.value) {
            await fetchRolePermissions();
        } else {
            router.push({ name: "roles-list" }).then();
            notify.showError(t("admin.permissions.notifications.missingId"));
        }
    });

    const fetchRolePermissions = async () => {
        loading.value = true;
        try {
            await roleStore.fetchRolePermissions(roleId.value!);
        } catch (error) {
            notify.showError(error, t("common.notifications.loadError"));
            router.push({ name: "roles-list" }).then();
        } finally {
            loading.value = false;
        }
    };

    const updatePermissions = async (updatedPermissions: FeaturePermission[]) => {
        loading.value = true;
        try {
            await roleStore.updateRolePermissions(roleId.value!, updatedPermissions);
            notify.showSuccess(t("common.notifications.updateSuccess"));
        } catch (error) {
            notify.showError(error, t("common.notifications.updateError"));
        } finally {
            loading.value = false;
        }
    };

    const goBack = () => {
        router.push({ name: "roles-list" });
    };
</script>

<template>
    <PermissionForm
        :initialSchema="roleStore.currentRolePermissions"
        :entityName="roleStore.currentRolePermissions?.entityName || 'Unknown Role'"
        :loading="loading"
        :backLabel="t('admin.permissions.backToRoles')"
        :subtitle="t('admin.permissions.subtitle')"
        @save="updatePermissions"
        @cancel="goBack"
    />
</template>
