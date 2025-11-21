const appPermissions = {
  // General admin role
  admin: "role.admin",

  // Articles
  articlesPreviewRead: "articles.preview.read",

  // Jobs
  job: "admin.jobs.read",

  // Settings
  settingsRead: "admin.appSettings.read",
  settingsWrite: "admin.appSettings.write",

  // Custom Groups
  customGroupsRead: "admin.customGroups.read",
  customGroupsWrite: "admin.customGroups.write",

  // GTIN Article
  gtinArticleRead: "admin.gtinArticle.read",
  gtinArticleWrite: "admin.gtinArticle.write",

  // Email
  emailSettingsRead: "admin.email.settings.read",
  emailSettingsWrite: "admin.email.settings.write",
  emailTemplatesRead: "admin.email.templates.read",
  emailTemplatesWrite: "admin.email.templates.write",

  // Administration Section
  administrationSection: [
    "role.admin",
    "administration.settings.read",
    "administration.customGroups.read",
    "administration.gtinArticle.read",
    "administration.email.settings.read",
    "administration.email.templates.read",
  ],
  
};

export default appPermissions;
