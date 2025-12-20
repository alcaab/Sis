<script setup lang="ts">
    import { ref, onMounted, computed } from "vue";
    import { useRoute } from "vue-router";
    import { useI18n } from "vue-i18n";
    import { useUserStore } from "@/stores/userStore";
    import { useNotification } from "@/composables/useNotification";
    import FeaturePermissionAccordion from "@/components/admin/FeaturePermissionAccordion.vue";

    const route = useRoute();
    const userStore = useUserStore();
    const { t } = useI18n();
    const { showError } = useNotification();

    const userId = computed(() => route.params.id as string);
    const userName = ref<string>("");
    const loadingPermissions = computed(() => userStore.loading);
    const permissionSchema = computed(() => userStore.currentUserPermissions);

    const breadcrumbHome = ref({
        icon: "pi pi-home",
        to: "/",
    });
    const breadcrumbItems = ref([
        { label: t("admin.admin"), to: "/admin" },
        { label: t("admin.users"), to: "/admin/users" },
        { label: t("users.permissions"), to: `/admin/users/${userId.value}/permissions` },
    ]);

    const loadUserPermissions = async () => {
        try {
            // const user = await userService.getById(userId.value);
            // if (user) {
            //     userName.value = user.userName || t("common.unknownUser");
            // }

            await userStore.fetchUserPermissions(userId.value);
        } catch (error) {
            showError(t("users.notifications.fetchPermissionsError"));
        }
    };

    onMounted(loadUserPermissions);
</script>

<template>
    <div class="card">
        <div class="flex justify-content-between align-items-center mb-4">
            <h1 class="text-2xl font-bold">{{ t("users.permissionsTitle", { userName: userName }) }}</h1>
            <Breadcrumb
                :home="breadcrumbHome"
                :model="breadcrumbItems"
            />
        </div>

        <BlockUI :blocked="loadingPermissions">
            <div
                v-if="loadingPermissions"
                class="flex justify-content-center align-items-center min-h-20rem"
            >
                <ProgressSpinner />
            </div>
            <div v-else-if="permissionSchema && permissionSchema.groups.length > 0">
                <FeaturePermissionAccordion
                    :permission-schema="permissionSchema"
                    :read-only="true"
                />
            </div>
            <div
                v-else
                class="flex flex-column align-items-center"
            >
                <i class="pi pi-lock-open text-4xl text-400 mb-2"></i>
                <p class="text-xl text-400">{{ t("users.noPermissionsFound") }}</p>
            </div>
        </BlockUI>
    </div>
</template>
