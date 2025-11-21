<script setup lang="ts">
import {useI18n} from "vue-i18n";
import {ref} from "vue";
import AppSettings from "@/components/appSettings/AppSettings.vue";
import {Card} from "primevue";

const { t } = useI18n();
const viewTitle = ref(t("appSettings.title"));
</script>

<template>
  <Card>
    <template #title>
      {{ viewTitle }}
    </template>
    <template #content>
      <app-settings></app-settings>
    </template>
  </Card>
</template>

<style scoped>

</style>
