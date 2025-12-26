<script setup lang="ts">
    import { useRouter, useRoute } from "vue-router";
    import { useEvaluationPeriodStore } from "@/stores/evaluationPeriodStore";
    import EvaluationPeriodForm from "./EvaluationPeriodForm.vue";
    import { useNotification } from "@/composables/useNotification";
    import { useI18n } from "vue-i18n";
    import type { EvaluationPeriodDto } from "@/types/evaluation-period";
    import AcademicYearForm from "@/views/school/academic-years/AcademicYearForm.vue";

    const router = useRouter();
    const route = useRoute();
    const store = useEvaluationPeriodStore();
    const notify = useNotification();
    const { t } = useI18n();

    const academicYearId = Number(route.query.academicYearId);
    const returnUrl = (route.query.returnUrl as string) || "/school-settings/evaluation-period";

    const handleCreate = async (data: EvaluationPeriodDto) => {
        try {
            await store.createEvaluationPeriod(data);
            notify.showSuccess(t("common.notifications.createSuccess"));
            router.push(returnUrl).then();
        } catch (error) {
            notify.showError(error, t("common.notifications.operationError"));
        }
    };

    const handleCancel = () => {
        router.push(returnUrl);
    };
</script>

<template>
    <div class="w-full animate fade-in">
        <div class="p-2">
            <div class="font-semibold text-xl mb-6">
                {{ t("schoolSettings.evaluationPeriod.createHeader") }}
            </div>
            <EvaluationPeriodForm
                :isEditing="false"
                :loading="store.loading"
                :initialData="{ academicYearId: academicYearId }"
                @submit="handleCreate"
                @cancel="handleCancel"
            />
        </div>
    </div>
</template>
