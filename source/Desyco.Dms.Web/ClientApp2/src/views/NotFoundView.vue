<script lang="ts" setup>
import { NotFound } from "netto-primevue";
import Button from "primevue/button";
import { useRouter } from "vue-router";

const router = useRouter();
</script>

<template>
  <div class="flex flex-col items-start gap-2">
    <Button
      icon="pi pi-arrow-left"
      label="zurück"
      severity="secondary"
      @click="router.go(-1)" />
    <NotFound />
  </div>
</template>

<style scoped></style>
