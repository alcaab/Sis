<script setup lang="ts">
    import { ref } from "vue";
    import { useAcademicYearStore } from "@/stores/academicYearStore";
    import { useRouter } from "vue-router";
    import AcademicYearForm from "./AcademicYearForm.vue";
    import type { AcademicYearDto } from "@/types/academic-year";
    import { useNotification } from "@/composables/useNotification";

    const store = useAcademicYearStore();
    const router = useRouter();
    const notify = useNotification();
    const loading = ref(false);

    const handleCreate = async (data: AcademicYearDto) => {
        loading.value = true;
        try {
            // Convert dates to ISO string before sending
            const payload = { ...data };
            if (payload.startDate instanceof Date) {
                payload.startDate = payload.startDate.toISOString().split("T")[0];
            }
            if (payload.endDate instanceof Date) {
                payload.endDate = payload.endDate.toISOString().split("T")[0];
            }

            await store.createAcademicYear(payload);
            notify.showSuccess("Academic Year Created");
            router.push({ name: "academic-year-list" }).then();
        } catch (error: any) {
            notify.showError(error, "Creation failed");
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
        <AcademicYearForm
            :isEditing="false"
            :loading="loading"
            @submit="handleCreate"
            @cancel="handleCancel"
        />
    </div>
</template>
