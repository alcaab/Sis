<script setup lang="ts">
    import { ref, onMounted, computed } from "vue";
    import { useRoute, useRouter } from "vue-router";
    import { useI18n } from "vue-i18n";
    import { useUserStore } from "@/stores/userStore";
    import { useNotification } from "@/composables/useNotification";
    import type { PermissionSchema } from "@/types/permissions.ts";

    const route = useRoute();
    const router = useRouter();
    const userStore = useUserStore();
    const { t } = useI18n();
    const { showError } = useNotification();

    const userId = computed(() => route.params.id as string);
    const userName = ref<string | null>(null);
    const loading = computed(() => userStore.loading);
    const permissionSchema = ref<PermissionSchema | null>(null);

    onMounted(async () => {
        if (userId.value) {
            await loadUserPermissions();
        } else {
            showError(t("admin.permissions.notifications.missingId"));
            router.push({ name: "uses-list" }).then();
        }
    });

    const loadUserPermissions = async () => {
        try {
            await userStore.fetchUserPermissions(userId.value);
            const rawData = userStore.currentUserPermissions;

            if (rawData) {
                permissionSchema.value = JSON.parse(JSON.stringify(rawData));
                userName.value = rawData.entityName || "Unknown User";
                //expandAll();
            }
        } catch (error) {
            showError(t("common.notifications.loadError"));
            router.push({ name: "uses-list" }).then();
        }
    };
</script>

<template>
    {{ userName }}
</template>
