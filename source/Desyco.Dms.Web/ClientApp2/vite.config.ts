import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import { readFileSync } from "fs";
import { fileURLToPath, URL } from "node:url";
import tailwindcss from "@tailwindcss/vite";
import { certFilePath, keyFilePath } from "netto-primevue/aspnetcore-https.js";

// https://vitejs.dev/config/
export default defineConfig(({ command }) => {
  const plugins = [vue(), tailwindcss()];
  if (command == "build") {
    return {
      // build specific config
      plugins: plugins,
      resolve: {
        alias: {
          "@": fileURLToPath(new URL("./src", import.meta.url)),
          "@models": fileURLToPath(new URL("./src/models", import.meta.url)),
          "@utils": fileURLToPath(new URL("./src/utils", import.meta.url)),
        },
      },
    };
  } else {
    return {
      plugins: plugins,
      resolve: {
        alias: {
          "@": fileURLToPath(new URL("./src", import.meta.url)),
          "@models": fileURLToPath(new URL("./src/models", import.meta.url)),
          "@utils": fileURLToPath(new URL("./src/utils", import.meta.url)),
        },
      },
      server: {
        https: {
          key: readFileSync(keyFilePath),
          cert: readFileSync(certFilePath),
        },
        port: 5002,
        proxy: {
          "/api": {
            target: "https://localhost:5001/",
            changeOrigin: true,
            secure: false,
          },
        },
      },
    };
  }
});
