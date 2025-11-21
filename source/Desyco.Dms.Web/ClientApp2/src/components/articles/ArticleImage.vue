<script setup lang="ts">
import { ref } from "vue";

defineProps<{ imageUrl: string }>();

const loaded = ref(false);

function onLoad() {
  loaded.value = true;
}
</script>

<template>
  <div class="w-full h-[120px] mb-0 rounded-md overflow-hidden flex items-center justify-center">
    <img
      v-show="loaded"
      :src="imageUrl"
      alt="Article"
      @load="onLoad"
      class="w-full h-[120px] object-contain rounded-md"
    />
    <img
      v-show="!loaded"
      src="@/data/images/placeHolder.jpg"
      alt="Loading..."
      class="w-full h-[120px] object-contain rounded-md"
    />
  </div>
</template>
