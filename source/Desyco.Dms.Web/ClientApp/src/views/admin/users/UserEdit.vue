<script setup lang="ts">
    import { ref, onMounted } from "vue";
    import { useRoute, useRouter } from "vue-router";
    import { useI18n } from "vue-i18n";
    import UserForm from "./UserForm.vue";
    import type { UserDto } from "@/types/user";
    import { useUserStore } from "@/stores/userStore";
    import { useNotification } from "@/composables/useNotification";
    import { userService } from "@/service/userService.ts";

    const route = useRoute();
    const router = useRouter();
    const userStore = useUserStore();
    const { t } = useI18n();
    const { showError, showSuccess } = useNotification();
    const initialData = ref<UserDto | undefined>(undefined);
    const loading = ref(true);
    const id = route.params.id as string;

    const loadUserData = async () => {
        loading.value = true;
        try {
            const user = await userService.getById(id);
            if (!user) {
                showError(t("users.notifications.notFound"));
                router.push({ name: "users-list" }).then();
                return;
            }
            initialData.value = user;
        } catch (error) {
            showError(t("users.notifications.fetchError"));
            router.push({ name: "users-list" }).then();
        } finally {
            loading.value = false;
        }
    };

    onMounted(async () => {
        if (!id) {
            router.push({ name: "users-list" }).then();
            return;
        }

        await loadUserData();
    });

    const handleUpdate = async (user: UserDto) => {
        if (!id) return;
        try {
            await userStore.updateUser(id, user);
            showSuccess(t("common.notifications.updateSuccess"));
            router.push({ name: "users-list" }).then();
        } catch (error) {
            showError(t("common.notifications.updateError"));
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
                {{ t("admin.users.editHeader", { userName: initialData?.userName }) }}
            </div>
            <UserForm
                :isEditing="true"
                :loading="loading"
                :initialData="initialData"
                @submit="handleUpdate"
                @cancel="handleCancel"
            />
        </div>
    </div>
</template>
