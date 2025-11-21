import * as yup from "yup";

export function useCustomGroupSchema(t: (s: string) => string){
  return {
    schema: yup.object({
      id: yup.string().optional(),
      customGroupNumber: yup.number().optional(),
      mainGroupNumber: yup.number().optional(),
      name: yup
        .string()
        .required(t('customGroup.validations.name'))
        .max(25, t('customGroup.validations.nameMaxLength'))
        .test(
          'no-leading-trailing-spaces',
          t('customGroup.validations.nameNoSpaces'),
          (value) => (value ? value === value.trim() : false)
        )
        .test(
          'no-empty-spaces',
          t('customGroup.validations.name'),
          (value) => (value ? value.trim().length > 0 : false)
        ),
      subGroups: yup
        .array()
        .of(yup.object({}))
        .min(1, t('customGroup.validations.name')),
      subGroupNames: yup.array().of(yup.string()).optional(),
    }),
  };
}
