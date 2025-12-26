namespace Desyco.Iam.Contracts.Permissions;

public static class Permissions
{
    public const string Root = "Permissions";
    
    public static class AcademicYears
    {
        public const string Code = "AcademicYears";
        public const string Read = $"{Root}.{Code}.Read";
        public const string Write = $"{Root}.{Code}.Write";
        public const string Delete = $"{Root}.{Code}.Delete";
    }

    public static class Students
    {
        public const string Code = "Students";
        public const string Read = $"{Root}.{Code}.Read";
        public const string Write = $"{Root}.{Code}.Write";
        public const string Delete = $"{Root}.{Code}.Delete";
    }

    public static class Teachers
    {
        public const string Code = "Teachers";
        public const string Read = $"{Root}.{Code}.Read";
        public const string Write = $"{Root}.{Code}.Write";
        public const string Delete = $"{Root}.{Code}.Delete";
    }

    public static class Classrooms
    {
        public const string Code = "Classrooms";
        public const string Read = $"{Root}.{Code}.Read";
        public const string Write = $"{Root}.{Code}.Write";
        public const string Delete = $"{Root}.{Code}.Delete";
    }

    public static class Enrollments
    {
        public const string Code = "Enrollments";
        public const string Read = $"{Root}.{Code}.Read";
        public const string Write = $"{Root}.{Code}.Write";
        public const string Delete = $"{Root}.{Code}.Delete";
    }

    public static class Users
    {
        public const string Code = "Users";
        public const string Read = $"{Root}.{Code}.Read";
        public const string Write = $"{Root}.{Code}.Write";
        public const string Delete = $"{Root}.{Code}.Delete";
    }

    public static class Roles
    {
        public const string Code = "Roles";
        public const string Read = $"Permissions.{Code}.Read";
        public const string Write = $"Permissions.{Code}.Write";
        public const string Delete = $"Permissions.{Code}.Delete";
    }

    public static class EvaluationPeriods
    {
        public const string Code = "EvaluationPeriods";
        public const string Read = $"{Root}.{Code}.Read";
        public const string Write = $"{Root}.{Code}.Write";
        public const string Delete = $"{Root}.{Code}.Delete";
    }

    public static class EducationalLevels
    {
        public const string Code = "EducationalLevels";
        public const string Read = $"{Root}.{Code}.Read";
        public const string Write = $"{Root}.{Code}.Write";
        public const string Delete = $"{Root}.{Code}.Delete";
    }

    public static class DaysOfWeek
    {
        public const string Code = "DaysOfWeek";
        public const string Read = $"{Root}.{Code}.Read";
        public const string Write = $"{Root}.{Code}.Write";
        public const string Delete = $"{Root}.{Code}.Delete";
    }
    
    // Permission management feature itself
    public static class Security
    {
        public const string Code = "Permissions";
        public const string Read = $"Permissions.{Code}.Read";
        public const string Write = $"Permissions.{Code}.Write";
        public const string Delete = $"Permissions.{Code}.Delete";
    }
}
