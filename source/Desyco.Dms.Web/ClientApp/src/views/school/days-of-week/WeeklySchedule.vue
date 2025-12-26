<script setup lang="ts">
import { ref } from 'vue';
import type { DayOfWeekDto, UpdateDayOfWeekCommand } from '@/types/days-of-week';
import SelectButton from 'primevue/selectbutton';
import DatePicker from 'primevue/datepicker';
import Button from 'primevue/button';
import Accordion from 'primevue/accordion';
import AccordionPanel from 'primevue/accordionpanel';
import AccordionHeader from 'primevue/accordionheader';
import AccordionContent from 'primevue/accordioncontent';

const props = defineProps<{
    days: DayOfWeekDto[];
    saving: boolean;
}>();

const emit = defineEmits<{
    (e: 'save', id: number, command: UpdateDayOfWeekCommand): void;
    (e: 'error', message: string): void;
}>();

const schoolDayOptions = [
    { label: 'Yes', value: true },
    { label: 'No', value: false }
];

// Helper to convert TimeOnly string "HH:mm:ss" to Date
const toDate = (timeStr: string | null): Date | null => {
    if (!timeStr) return null;
    const [hours, minutes, seconds] = timeStr.split(':').map(Number);
    const date = new Date();
    date.setHours(hours, minutes, seconds || 0, 0);
    return date;
};

// Helper to convert Date to TimeOnly string "HH:mm:ss"
const toTimeStr = (date: Date | null): string | null => {
    if (!date) return null;
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    const seconds = date.getSeconds().toString().padStart(2, '0'); // Optional, depending on backend requirement
    return `${hours}:${minutes}:${seconds}`;
};

// Local state to hold form values for each day, to avoid mutating props directly
// We initialize it when expanding or just compute it?
// Better to have a local copy of the day being edited. 
// Or just bind directly if we don't care about cancel? 
// Ideally we create a copy. 
// Since AccordionPanel allows multiple open, we need state for all.
// Let's create a reactive map of "drafts" based on props.days.

// Actually, since it's a list, let's just make a function that handles the submit.
// But v-model needs a target. 
// I'll create a local ref initialized from props.days. 
// But props can change (e.g. after save).
// A simple way is to use a map of form data.

const forms = ref<Record<number, any>>({});

// Initialize forms based on days
// We need to watch props.days to update forms if data changes from outside (e.g. initial load)
import { watchEffect } from 'vue';

watchEffect(() => {
    props.days.forEach(day => {
        if (!forms.value[day.id]) {
            forms.value[day.id] = {
                id: day.id,
                isSchoolDay: day.isSchoolDay,
                openTime: toDate(day.openTime),
                startTime: toDate(day.startTime),
                endTime: toDate(day.endTime),
                closeTime: toDate(day.closeTime),
            };
        }
    });
});

const onSave = (dayId: number) => {
    const form = forms.value[dayId];
    if (!form) return;

    // Validation
    if (form.isSchoolDay) {
        const start = form.startTime;
        const end = form.endTime;
        if (start && end && start >= end) {
            // We should show an error. Since this is a dumb component, we can emit an error or handle it here?
            // Ideally use a toast or show validation message.
            // But we don't have access to Toast service here easily unless injected.
            // For simplicity, let's just use alert or emit an error event?
            // Or better, bind validation state.
            // Given "WeeklySchedule.vue" is "dumb" but also "complex", maybe it should inject toast?
            // Or emit an event 'error'.
            // But standard practice in this project seems to be handling toast in container.
            // Let's emit 'error' or throw.
            // Actually, I'll return early and maybe set a local error state if I had one.
            // But I'll just rely on backend validation for now if I can't easily show error, 
            // OR I can add a small error message in the UI.
            
            // Let's emit an error event which the container can toast.
            emit('error', 'Start time must be before End time.');
            return;
        }
        
        // Also validate required fields
        if (!form.openTime || !form.startTime || !form.endTime || !form.closeTime) {
             emit('error', 'All time fields are required for a school day.');
             return;
        }
    }

    const command: UpdateDayOfWeekCommand = {
        id: form.id,
        isSchoolDay: form.isSchoolDay,
        openTime: toTimeStr(form.openTime),
        startTime: toTimeStr(form.startTime),
        endTime: toTimeStr(form.endTime),
        closeTime: toTimeStr(form.closeTime),
    };
    emit('save', dayId, command);
};
</script>

<template>
    <div class="card">
        <Accordion value="0">
            <AccordionPanel v-for="day in days" :key="day.id" :value="String(day.id)">
                <AccordionHeader>{{ day.name }}</AccordionHeader>
                <AccordionContent>
                    <div v-if="forms[day.id]" class="flex flex-col gap-4">
                        <div class="flex items-center gap-4">
                            <label class="font-bold w-32">School Day</label>
                            <SelectButton v-model="forms[day.id].isSchoolDay" :options="schoolDayOptions" optionLabel="label" optionValue="value" />
                        </div>

                        <div v-if="forms[day.id].isSchoolDay" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
                            <div class="flex flex-col gap-2">
                                <label>Opens</label>
                                <DatePicker v-model="forms[day.id].openTime" timeOnly showIcon hourFormat="24" />
                            </div>
                            <div class="flex flex-col gap-2">
                                <label>Starts</label>
                                <DatePicker v-model="forms[day.id].startTime" timeOnly showIcon hourFormat="24" />
                            </div>
                            <div class="flex flex-col gap-2">
                                <label>Ends</label>
                                <DatePicker v-model="forms[day.id].endTime" timeOnly showIcon hourFormat="24" />
                            </div>
                            <div class="flex flex-col gap-2">
                                <label>Closes</label>
                                <DatePicker v-model="forms[day.id].closeTime" timeOnly showIcon hourFormat="24" />
                            </div>
                        </div>

                        <div class="flex justify-end mt-4">
                            <Button label="Save" icon="pi pi-save" @click="onSave(day.id)" :loading="saving" />
                        </div>
                    </div>
                </AccordionContent>
            </AccordionPanel>
        </Accordion>
    </div>
</template>
