namespace Api_1.Entity.Consts;

public static class Permissions
{
    public static string Type { get; } = "permissions";

    public const string updateStudent = "updateStudent";
    public const string GetStudents = "getstudents";
    public const string DeleteStudent = "DeleteStudent";
    public const string AddStudent = "addstudent";


    public static IList<string?> GetAllPermissions() =>
       typeof(Permissions).GetFields().Select(x => x.GetValue(x) as string).ToList();
}
