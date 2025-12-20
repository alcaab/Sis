<script setup lang="ts">
    import { ref, onMounted } from "vue";
    import { useRoute, useRouter } from "vue-router";
    import { useAcademicYearStore } from "@/stores/academicYearStore";
    import { AcademicYearService } from "@/service/AcademicYearService";
    import AcademicYearForm from "./AcademicYearForm.vue";
    import type { AcademicYearDto } from "@/types/academic-year";
    import { useNotification } from "@/composables/useNotification";
    import { useI18n } from "vue-i18n";

    const route = useRoute();
    const router = useRouter();
    const store = useAcademicYearStore();
    const notify = useNotification();
    const { t } = useI18n();

    const loading = ref(false);
    const initialData = ref<Partial<AcademicYearDto>>({});
    const id = Number(route.params.id);

    onMounted(async () => {
        if (!id) {
            router.push({ name: "academic-year-list" }).then();
            return;
        }
        await loadData();
    });

    const loadData = async () => {
        try {
            const response = await AcademicYearService.getAcademicYear(id);
            initialData.value = response.data;
        } catch (error) {
            console.error(error);
            notify.showError(error, t("common.notifications.loadError"));
            router.push({ name: "academic-year-list" }).then();
        }
    };

    const handleUpdate = async (data: AcademicYearDto) => {
        loading.value = true;
        try {
            const payload = { ...data };
            if (payload.startDate instanceof Date) {
                payload.startDate = payload.startDate.toISOString().split("T")[0];
            }
            if (payload.endDate instanceof Date) {
                payload.endDate = payload.endDate.toISOString().split("T")[0];
            }

            await store.updateAcademicYear(id, payload);
            notify.showSuccess(t("common.notifications.updateSuccess"));
            router.push({ name: "academic-year-list" }).then();
        } catch (error: any) {
            notify.showError(error, t("common.notifications.updateError"));
        } finally {
            loading.value = false;
        }
    };

    const handleCancel = () => {
        router.push({ name: "academic-year-list" });
    };
</script>

<template>
    <div class="w-full animate fade-in">
        <div class="p-2">
            <div class="font-semibold text-xl mb-6">
                {{ t("schoolSettings.academicYear.editHeader") }}
            </div>
            <AcademicYearForm
                :isEditing="true"
                :loading="loading"
                :initialData="initialData"
                @submit="handleUpdate"
                @cancel="handleCancel"
            />
        </div>
    </div>
</template>
