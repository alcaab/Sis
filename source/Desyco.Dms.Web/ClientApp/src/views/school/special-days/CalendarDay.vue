<script setup lang="ts">
    import { computed } from "vue";

    const props = defineProps<{
        date: any;
        meta?: any;
        disabledDays: number[];
    }>();

    defineEmits(["create", "edit", "delete"]);

    const getDisabledDays = computed(() => {
        return props.disabledDays ?? [];
    });
</script>

<template>
    <template v-if="getDisabledDays.includes(new Date(date.year, date.month, date.day).getDay())">
        <div class="p-2"></div>
    </template>
    <template v-else>
        <div
            class="calendar-day"
            :class="{
                today: date.today,
                other: date.otherMonth,
            }"
        >
            <div class="day-number">
                {{ date.day }}
            </div>

            <div
                v-if="meta"
                class="event"
            >
                <span class="event-name">{{ meta.name }}</span>

                <div class="actions">
                    <Button
                        icon="pi pi-pencil"
                        size="small"
                        text
                        @click.stop="$emit('edit', meta)"
                    />
                    <Button
                        icon="pi pi-trash"
                        size="small"
                        text
                        severity="danger"
                        @click.stop="$emit('delete', meta)"
                    />
                </div>
            </div>

            <Button
                v-else
                icon="pi pi-plus"
                size="small"
                text
                class="add-btn"
                @click.stop="$emit('create', date)"
            />
        </div>
    </template>
</template>
<style
        scoped
        lang="scss"
    >
        .calendar-day {
            position: relative;
            height: 100%;
            padding: 0.25rem;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
        }

        .day-number {
            font-size: 0.75rem;
            font-weight: 600;
        }

        .calendar-day.today {
            border: 1px solid var(--primary-color);
            border-radius: 4px;
        }

        .calendar-day.other {
            opacity: 0.4;
        }

        .event {
            font-size: 0.65rem;
            background: var(--primary-100);
            border-radius: 4px;
            padding: 2px;
        }

        .actions {
            display: flex;
            justify-content: end;
            gap: 0.25rem;
        }
</style>
