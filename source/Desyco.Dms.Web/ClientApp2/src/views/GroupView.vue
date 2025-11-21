<script setup lang="ts">
import { useI18n } from "vue-i18n";
import { ref } from "vue";
import { Card } from "primevue";
import CustomGroup from "@/components/customGroups/CustomGroup.vue";

const { t } = useI18n();
const viewTitle = ref(t("customGroups"));
</script>

<template>
  <Card>
    <template #title>
      {{ viewTitle }}
    </template>
    <template #content>
      <CustomGroup
        :mainGroupNumber="22"
        :header="t('customGroup.mainGroup.bakeOff')" />
    </template>
  </Card>
</template>

<style scoped></style>
