<script setup lang="ts">
import Card from "primevue/card";

interface Props {
  number: number;
  title: string;
  image: string | null;
}

const props = defineProps<Props>();
</script>

<template>
  <Card
    class="article-card w-full h-[200px] transition-transform duration-200
           hover:-translate-y-[2px] hover:shadow-lg
           dark:border-surface-700 dark:hover:shadow-[0_4px_12px_rgb(75,85,99)]"
  >
    <template #content>
      <div class="flex flex-col h-full p-[0.2rem]">
        <img
          :src="`/api/v1/articles/${props.number}/images`"
          alt="Article"
          class="w-full h-[120px] object-contain rounded mb-2 hover:opacity-90 transition-opacity bg-white"
        />
        <div
          class="text-black dark:text-gray-100 font-bold mx-2 text-center leading-[1.2rem]
                 overflow-hidden text-ellipsis line-clamp-3"
        >
          <div v-for="(text, index) in title" :key="index">
            {{ text }}
          </div>
        </div>
      </div>
    </template>
  </Card>
</template>

<style scoped>

.line-clamp-3 {
  display: -webkit-box !important;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
}
</style>
