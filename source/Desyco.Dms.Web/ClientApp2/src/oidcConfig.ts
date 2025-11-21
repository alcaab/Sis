import { UserManager, WebStorageStateStore } from "oidc-client-ts";

const userManager = new UserManager({
  authority: import.meta.env.VITE_AUTHORITY_URL,
  client_id: import.meta.env.VITE_CLIENT_ID,
  redirect_uri: import.meta.env.VITE_APP_URL + "/signin-oidc",
  userStore: new WebStorageStateStore({
    store: localStorage,
    prefix: "Kassentableau-oidc",
  }),
  post_logout_redirect_uri: import.meta.env.VITE_APP_URL + "/signout-oidc",
  popup_redirect_uri: import.meta.env.VITE_APP_URL + "/signin-oidc",
  response_type: "code",
  scope: "openid profile offline_access appchrome kassentableau identityserverapi_public",
  automaticSilentRenew: true,
  loadUserInfo: true,
  monitorSession: false,
  silent_redirect_uri: import.meta.env.VITE_APP_URL + "/silent-renew",
});

export default userManager;
