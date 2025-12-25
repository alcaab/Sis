<script setup lang="ts">
    import { onMounted } from "vue";
    import { useRouter, useRoute } from "vue-router";
    import { useEvaluationPeriodStore } from "@/stores/evaluationPeriodStore";
    import { useI18n } from "vue-i18n";

    const props = defineProps<{
        academicYearId: number;
    }>();

    const router = useRouter();
    const route = useRoute();
    const store = useEvaluationPeriodStore();
    const { t } = useI18n();

    onMounted(async () => {
        if (props.academicYearId) {
            await store.fetchByAcademicYear(props.academicYearId);
        }
    });

    const openNew = () => {
        router.push({
            name: "evaluation-period-create",
            query: {
                academicYearId: props.academicYearId,
                returnUrl: route.fullPath,
            },
        });
    };
</script>

<template>
    <div class="mt-6">
        <!--        <div class="flex items-center justify-between mb-4">-->
        <!--            <h5 class="text-lg font-bold m-0">{{ t("schoolSettings.evaluationPeriod.title") }}</h5>-->
        <!--            <Button-->
        <!--                :label="t('common.buttons.new')"-->
        <!--                icon="pi pi-plus"-->
        <!--                size="small"-->
        <!--                @click="openNew"-->
        <!--            />-->
        <!--        </div>-->

        <DataTable
            :value="store.evaluationPeriods"
            :loading="store.loading"
            dataKey="id"
            responsiveLayout="scroll"
            class="p-datatable-sm"
        >
            <template #header>
                <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
                    <h5 class="m-0 text-xl font-semibold text-surface-900 dark:text-surface-0">
                        {{ t("schoolSettings.evaluationPeriod.title") }}
                    </h5>
                    <div class="flex items-center gap-2">
                        <IconField iconPosition="left">
                            <InputIcon>
                                <i class="pi pi-search" />
                            </InputIcon>
                            <InputText
                                :placeholder="t('common.placeholders.search')"
                                size="small"
                                class="w-full md:w-auto"
                            />
                        </IconField>
                        <Button
                            :title="t('common.buttons.new')"
                            icon="pi pi-plus"
                            size="small"
                            @click="openNew"
                        />
                    </div>
                </div>
            </template>
            <template #empty>
                <div class="text-center p-4 text-surface-500">No evaluation periods found for this academic year.</div>
            </template>

            <Column
                field="name"
                :header="t('schoolSettings.evaluationPeriod.name')"
            ></Column>
            <Column
                field="shortName"
                :header="t('schoolSettings.evaluationPeriod.shortName')"
            ></Column>
            <Column
                field="startDate"
                :header="t('schoolSettings.evaluationPeriod.startDate')"
            >
                <template #body="slotProps">
                    {{ slotProps.data.startDate ? new Date(slotProps.data.startDate).toLocaleDateString() : "-" }}
                </template>
            </Column>
            <Column
                field="endDate"
                :header="t('schoolSettings.evaluationPeriod.endDate')"
            >
                <template #body="slotProps">
                    {{ slotProps.data.endDate ? new Date(slotProps.data.endDate).toLocaleDateString() : "-" }}
                </template>
            </Column>
            <Column
                field="weight"
                :header="t('schoolSettings.evaluationPeriod.weight')"
            >
                <template #body="slotProps"> {{ slotProps.data.weight }}% </template>
            </Column>
        </DataTable>
    </div>
</template>
