<script setup lang="ts">
    import { useRouter } from "vue-router";
    import { useEvaluationPeriodStore } from "@/stores/evaluationPeriodStore";
    import EvaluationPeriodForm from "./EvaluationPeriodForm.vue";
    import { useNotification } from "@/composables/useNotification";
    import { useI18n } from "vue-i18n";
    import type { EvaluationPeriodDto } from "@/types/evaluation-period";

    const router = useRouter();
    const store = useEvaluationPeriodStore();
    const notify = useNotification();
    const { t } = useI18n();

    const handleCreate = async (data: EvaluationPeriodDto) => {
        try {
            await store.createEvaluationPeriod(data);
            notify.showSuccess(t("common.notifications.createSuccess"));
            router.push({ name: "evaluation-period-list" }).then();
        } catch (error) {
            notify.showError(error, t("common.notifications.operationError"));
        }
    };

    const handleCancel = () => {
        router.push({ name: "evaluation-period-list" });
    };
</script>

<template>
    <div class="card">
        <div class="font-semibold text-xl mb-4">{{ t("schoolSettings.evaluationPeriod.createHeader") }}</div>
        <EvaluationPeriodForm
            :isEditing="false"
            :loading="store.loading"
            @submit="handleCreate"
            @cancel="handleCancel"
        />
    </div>
</template>
