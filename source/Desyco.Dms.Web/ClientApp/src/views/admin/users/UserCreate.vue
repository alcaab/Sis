<script setup lang="ts">
    import { useRouter } from "vue-router";
    import { useI18n } from "vue-i18n";
    import UserForm from "./UserForm.vue";
    import type { UserDto } from "@/types/user";
    import { useUserStore } from "@/stores/userStore";
    import { useNotification } from "@/composables/useNotification";
    import { ref } from "vue";

    const router = useRouter();
    const userStore = useUserStore();
    const { t } = useI18n();
    const { showError, showSuccess } = useNotification();
    const loading = ref(false);

    const handleCreate = async (user: UserDto) => {
        try {
            await userStore.createUser(user);
            showSuccess(t("common.notifications.createSuccess"));
            router.push({ name: "users-list" }).then();
        } catch (error) {
            showError(t("common.notifications.createError"));
        }
    };

    const handleCancel = () => {
        router.push({ name: "users-list" });
    };
</script>

<template>
    <div class="w-full animate fade-in">
        <div class="p-2">
            <div class="font-semibold text-xl mb-6">
                {{ t("admin.users.createHeader") }}
            </div>
            <UserForm
                :isEditing="false"
                :loading="loading"
                @submit="handleCreate"
                @cancel="handleCancel"
            />
        </div>
    </div>
</template>
