import { fileURLToPath, URL } from 'node:url';

import { PrimeVueResolver } from '@primevue/auto-import-resolver';
import tailwindcss from '@tailwindcss/vite';
import vue from '@vitejs/plugin-vue';
import Components from 'unplugin-vue-components/vite';
import { defineConfig } from 'vite';

// https://vitejs.dev/config/
export default defineConfig({
    optimizeDeps: {
        include: ['yup', 'vee-validate', '@vee-validate/yup', 'tiny-case', 'property-expr']
    },
    plugins: [
        vue(),
        tailwindcss(),
        Components({
            resolvers: [PrimeVueResolver()]
        })
    ],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        port: 5173,
        proxy: {
            '^/api': {
                target: 'https://localhost:5001',
                changeOrigin: true,
                secure: false
            },
            '^/scalar': {
                target: 'https://localhost:5001',
                changeOrigin: true,
                secure: false
            }
        }
    }
});
