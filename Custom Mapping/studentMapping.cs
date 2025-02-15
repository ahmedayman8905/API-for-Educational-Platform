namespace Api_1.Contract;

public static class studentMapping
{
    public static StudentResponse mapp_to_student_Response(this Student student)
    {
        return new StudentResponse
        {
           FullName = student.FullName,
            //BirthDay = student.BirthDay,
            Email = student.Email,
            Gender = student.Gender,
            Password = student.Password,
        };
    }
    public static IEnumerable<StudentResponse> mapp_to_listStudenRespons(this IEnumerable<Student> students)
    {
        return students.Select(mapp_to_student_Response);
    }

}
