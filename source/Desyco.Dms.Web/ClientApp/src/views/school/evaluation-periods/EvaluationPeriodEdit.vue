<script setup lang="ts">
    import { ref, onMounted } from "vue";
    import { useRouter, useRoute } from "vue-router";
    import { useEvaluationPeriodStore } from "@/stores/evaluationPeriodStore";
    import { EvaluationPeriodService } from "@/service/EvaluationPeriodService";
    import EvaluationPeriodForm from "./EvaluationPeriodForm.vue";
    import { useNotification } from "@/composables/useNotification";
    import { useI18n } from "vue-i18n";
    import type { EvaluationPeriodDto } from "@/types/evaluation-period";

    const router = useRouter();
    const route = useRoute();
    const store = useEvaluationPeriodStore();
    const notify = useNotification();
    const { t } = useI18n();

    const evaluationPeriod = ref<EvaluationPeriodDto | null>(null);
    const loading = ref(true);
    const returnUrl = (route.query.returnUrl as string) || "/school-settings/evaluation-period";

    onMounted(async () => {
        const id = Number(route.params.id);
        if (id) {
            try {
                const response = await EvaluationPeriodService.getEvaluationPeriod(id);
                evaluationPeriod.value = response.data;
            } catch (error) {
                notify.showError(error, t("common.notifications.loadError"));
                router.push(returnUrl).then();
            } finally {
                loading.value = false;
            }
        }
    });

    const handleUpdate = async (data: EvaluationPeriodDto) => {
        const id = Number(route.params.id);
        try {
            await store.updateEvaluationPeriod(id, data);
            notify.showSuccess(t("common.notifications.updateSuccess"));
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
                {{ t("schoolSettings.evaluationPeriod.editHeader") }}
            </div>
            <BlockUI
                :blocked="loading"
                fullScreen
            >
                <ProgressSpinner v-if="loading" />
            </BlockUI>
            <EvaluationPeriodForm
                v-if="evaluationPeriod"
                :initialData="evaluationPeriod"
                :isEditing="true"
                :loading="store.loading"
                @submit="handleUpdate"
                @cancel="handleCancel"
            />
        </div>
    </div>
</template>
