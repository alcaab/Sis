<script setup lang="ts">
import { EmailSettings } from "netto-primevue";
import { useI18n } from "vue-i18n";
import Card from "primevue/card";

const { t } = useI18n();
</script>

<template>
  <Card>
    <template #title>{{ t("emailSettings") }}</template>
    <template #content>
      <EmailSettings></EmailSettings>
    </template>
  </Card>
</template>

<style scoped></style>
