import { AcademicYearStatus } from "@/types/academic-year";
import { EducationalLevelType } from "@/types/educational-level";
import { PermissionAction } from "@/types/permissions";
import type { LookupModel } from "@/types/common";
import i18n from "@/i18n";
import { DayOfWeek } from "@/types/days-of-week.ts";

class EnumService {
    private cache: Map<string, LookupModel[]> = new Map();

    public get academicYearStatuses(): LookupModel[] {
        return this.getOrSetCache("AcademicYearStatus", AcademicYearStatus, (key) =>
            i18n.global.t(`schoolSettings.academicYear.statusLabels.${key.toLowerCase()}`),
        );
    }

    public get educationalLevelTypes(): LookupModel[] {
        return this.getOrSetCache("EducationalLevelType", EducationalLevelType, (key) =>
            i18n.global.t(`schoolSettings.educationalLevel.types.${this.toCamelCase(key)}`),
        );
    }

    public get permissionActions(): LookupModel[] {
        return this.getOrSetCache("PermissionAction", PermissionAction, (key) =>
            i18n.global.t(`permissions.actions.${key.toLowerCase()}`),
        );
    }

    public get daysOfWeek(): LookupModel[] {
        return this.getOrSetCache("DayOfWeek", DayOfWeek, (key) => {
            const name = i18n.global.t(`schoolSettings.dayOfWeek.names.${key.toLowerCase()}`);
            const shortName = i18n.global.t(`schoolSettings.dayOfWeek.shortNames.${key.toLowerCase()}`);
            return `${name} (${shortName})`;
        });
    }

    public getDescription(
        enumName: "AcademicYearStatus" | "EducationalLevelType" | "PermissionAction" | "DayOfWeek",
        id: number | string,
    ): string {
        let list: LookupModel[] = [];
        if (enumName === "AcademicYearStatus") list = this.academicYearStatuses;
        else if (enumName === "EducationalLevelType") list = this.educationalLevelTypes;
        else if (enumName === "PermissionAction") list = this.permissionActions;
        else if (enumName === "DayOfWeek") list = this.daysOfWeek;

        const item = list.find((x) => x.id === id);
        return item ? item.description : id.toString();
    }

    private getOrSetCache(
        cacheKey: string,
        enumType: Record<string, string | number>,
        translationResolver: (key: string) => string,
    ): LookupModel[] {
        if (!this.cache.has(cacheKey)) {
            const data = this.createDataSource(enumType, translationResolver);
            this.cache.set(cacheKey, data);
        }
        return this.cache.get(cacheKey)!;
    }

    private createDataSource(
        enumType: Record<string, string | number>,
        translationResolver: (key: string) => string,
    ): LookupModel[] {
        return Object.entries(enumType)
            .filter(([key]) => isNaN(Number(key)))
            .map(([key, value]) => {
                return {
                    id: value,
                    value: key,
                    description: translationResolver(key) ?? key,
                };
            });
    }

    private toCamelCase(str: string): string {
        return str.charAt(0).toLowerCase() + str.slice(1);
    }
}

export const enumService = new EnumService();
