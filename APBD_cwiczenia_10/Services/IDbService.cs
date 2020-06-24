using APBD_cwiczenia_10.Models;
using APBD_cwiczenia_10.Requests;
using APBD_cwiczenia_10.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBD_cwiczenia_10.Services
{
    public interface IDbService
    {
        public List<Student> GetStudents();
        public Student UpdateStudent(string index, Requests.UpdateStudentRequest request);
        void DeleteStudent(string index);
        EnrollmentStudentResponse EnrollStudent(EnrollStudentRequest request);
        EnrollmentStudentResponse PromoteStudents(int semester, string studies);
    }
}
