<script setup lang="ts">
    import { ref } from "vue";
    import { useAcademicYearStore } from "@/stores/academicYearStore";
    import { useRouter } from "vue-router";
    import { useToast } from "primevue/usetoast";
    import AcademicYearForm from "./AcademicYearForm.vue";
    import type { AcademicYearDto } from "@/types/academic-year";

    const store = useAcademicYearStore();
    const router = useRouter();
    const toast = useToast();
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
            toast.add({ severity: "success", summary: "Successful", detail: "Academic Year Created", life: 3000 });
            router.push({ name: "academic-year-list" }).then();
        } catch (error: any) {
            const errorMessage = error.response?.data?.message || error.message || "Creation failed";
            toast.add({ severity: "error", summary: "Error", detail: errorMessage, life: 3000 });
        } finally {
            loading.value = false;
        }
    };

    const handleCancel = () => {
        router.push({ name: "academic-year-list" });
    };
</script>

<template>
    <div class="w-full">
        <AcademicYearForm
            :isEditing="false"
            :loading="loading"
            @submit="handleCreate"
            @cancel="handleCancel"
        />
    </div>
</template>
