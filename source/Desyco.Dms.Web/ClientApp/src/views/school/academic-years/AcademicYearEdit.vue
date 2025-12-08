<script setup lang="ts">
    import { ref, onMounted } from "vue";
    import { useRoute, useRouter } from "vue-router";
    import { useAcademicYearStore } from "@/stores/academicYearStore";
    import { AcademicYearService } from "@/service/AcademicYearService";
    import { useToast } from "primevue/usetoast";
    import AcademicYearForm from "./AcademicYearForm.vue";
    import type { AcademicYearDto } from "@/types/academic-year";

    const route = useRoute();
    const router = useRouter();
    const store = useAcademicYearStore();
    const toast = useToast();

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
            toast.add({ severity: "error", summary: "Error", detail: "Failed to load data", life: 3000 });
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
            toast.add({ severity: "success", summary: "Successful", detail: "Academic Year Updated", life: 3000 });
            router.push({ name: "academic-year-list" }).then();
        } catch (error: any) {
            const errorMessage = error.response?.data?.message || error.message || "Update failed";
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
    <div class="w-full animate fade-in">
        <AcademicYearForm
            :isEditing="true"
            :loading="loading"
            :initialData="initialData"
            @submit="handleUpdate"
            @cancel="handleCancel"
        />
    </div>
</template>
