import { ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';

export function useBreadcrumbs() {
    const route = useRoute();
    const router = useRouter();

    const home = ref({
        icon: 'pi pi-home',
        to: '/',
    });
    const items = ref();

    const generateBreadcrumbs = () => {
        const breadcrumbItems = [];
        // Add "Home" if needed, but it's set in `home` ref
        // breadcrumbItems.push({ label: 'Home', to: '/' }); 

        // Filter out routes that don't have a breadcrumb meta
        const matchedRoutes = route.matched.filter(match => match.meta && match.meta.breadcrumb);

        for (const match of matchedRoutes) {
            const breadcrumbLabel = match.meta.breadcrumb;
            let path = match.path;

            // If the route has dynamic segments, replace them with actual values
            // This part might need more sophisticated handling for complex dynamic routes
            for (const key in route.params) {
                path = path.replace(`:${key}`, route.params[key] as string);
            }

            breadcrumbItems.push({
                label: breadcrumbLabel as string,
                to: path
            });
        }
        items.value = breadcrumbItems;
    };

    // Watch for route changes to update breadcrumbs
    watch(
        () => route.path,
        () => {
            generateBreadcrumbs();
        },
        { immediate: true } // Generate immediately on component mount
    );

    return { home, items };
}
