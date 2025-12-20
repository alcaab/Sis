<script setup lang="ts">
    import { ref, onMounted, computed } from "vue";
    import { useRoute, useRouter } from "vue-router";
    import { useI18n } from "vue-i18n";
    import { useUserStore } from "@/stores/userStore";
    import { useNotification } from "@/composables/useNotification";
    import type { FeaturePermission } from "@/types/permissions";
    import PermissionForm from "../components/PermissionForm.vue";

    const route = useRoute();
    const router = useRouter();
    const userStore = useUserStore();
    const notify = useNotification();
    const { t } = useI18n();

    const userId = computed(() => route.params.id as string);
    const loading = ref(false);

    onMounted(async () => {
        if (userId.value) {
            await fetchUserPermissions();
        } else {
            router.push({ name: "users-list" }).then();
            notify.showError(t("admin.permissions.notifications.missingId"));
        }
    });

    const fetchUserPermissions = async () => {
        loading.value = true;
        try {
            await userStore.fetchUserPermissions(userId.value!);
        } catch (error) {
            notify.showError(error, t("common.notifications.loadError"));
            router.push({ name: "users-list" }).then();
        } finally {
            loading.value = false;
        }
    };

    const updatePermissions = async (updatedPermissions: FeaturePermission[]) => {
        loading.value = true;
        try {
            await userStore.updateUserPermissions(userId.value!, updatedPermissions);
            notify.showSuccess(t("admin.permissions.notifications.updateSuccess"));
        } catch (error) {
            notify.showError(error, t("admin.permissions.notifications.updateError"));
        } finally {
            loading.value = false;
        }
    };

    const goBack = () => {
        router.push({ name: "users-list" });
    };
</script>

<template>
    <PermissionForm
        :initialSchema="userStore.currentUserPermissions"
        :entityName="userStore.currentUserPermissions?.entityName || 'Unknown User'"
        :loading="loading"
        :backLabel="t('admin.permissions.backToUsers')"
        :subtitle="t('admin.permissions.userSubtitle')"
        @save="updatePermissions"
        @cancel="goBack"
    />
</template>
