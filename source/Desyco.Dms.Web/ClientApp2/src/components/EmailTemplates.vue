<script setup lang="ts">
import { ref } from "vue";
import { useI18n } from "vue-i18n";
import { type EmailTemplate, EmailTemplates } from "netto-primevue";
import Card from "primevue/card";

const { t } = useI18n();

const emailTemplateDefinitions = ref<EmailTemplate[]>([
  {
    name: "Standard-E-Mail",
    title: "Standard-E-Mail",
    description: "This is a simple test email template without translations.",
    tokens: [
      {
        title: "Overview",
        description: "Overview section of the email.",
        placeholder: "{{overview}}"
      },
      {
        title: "Verification Task List",
        description: "Tasks that need verification.",
        placeholder: "{{verificationTaskList}}"
      },
      {
        title: "VL Name",
        description: "The name of the VL item.",
        placeholder: "{{vlName}}"
      }
    ]
  },
  {
    name: "MissingArticlesImages",
    title: "Benachrichtigung über fehlende Artikelbilder",
    description: "Diese E-Mail-Vorlage informiert über Artikel, denen Bilder fehlen.",
    tokens: [
      {
        title: "missingArticleTables",
        description: "Tabelle, die Artikel ohne Bilder auflistet.",
        placeholder: "missingArticlesImagesTable"
      }
    ]
  }
]);
</script>

<template>
  <Card>
    <template #title>{{ t("emailTemplates") }}</template>
    <template #content>
      <EmailTemplates :email-templates="emailTemplateDefinitions" />
    </template>
  </Card>
</template>
