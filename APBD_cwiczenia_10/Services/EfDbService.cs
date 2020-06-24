using APBD_cwiczenia_10.Models;
using APBD_cwiczenia_10.Requests;
using APBD_cwiczenia_10.Responses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace APBD_cwiczenia_10.Services
{
    public class EfDbService :IDbService
    {
        public s17180Context _context { get; set; }

        public EfDbService(s17180Context context)
        {
            _context = context;
        }

        public List<Student> GetStudents()
        {
            return _context.Student.ToList();
        }

        public Student UpdateStudent(string index, UpdateStudentRequest request)
        {
           Student s = (from student in _context.Student 
                        where student.IndexNumber == index 
                        select student).FirstOrDefault();

            if (s == null)
                throw new Exception("Nie ma takiego studenta");


                if(request.FirstName != null)
                s.FirstName = request.FirstName;

                if (request.LastName != null)
                    s.LastName = request.LastName;

                if (request.IdEnrollment != null && request.IdEnrollment >0)
                    s.IdEnrollment = request.IdEnrollment;

                if (request.Password != null)
                    s.Password = request.Password;

                if (request.BirthDate != null)
                    s.BirthDate = request.BirthDate;

            

            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
               
            }


            return s;
        }

        public void DeleteStudent(string index)
        {

            var student = _context.Student.Find(index);
            if (student == null)
                throw new Exception("nie można usunąć, nie znaleziono indeksu");


            _context.Student.Remove(student);
            _context.SaveChanges();
        }

        public EnrollmentStudentResponse EnrollStudent(EnrollStudentRequest request)
        {
            Studies studies = _context.Studies.Where(s => s.Name == request.Studies).First();

            if (studies == null)
                throw new Exception("nie ma takich studiów");

            Student st = _context.Student.Where(s=> s.IndexNumber == request.IndexNumber).First();

            if (st != null)
                throw new Exception("student o podanym indeksie jest juz w bazie");

            var enrollment = _context.Enrollment
                             .OrderBy(e=> e.StartDate)
                             .Where(e => e.IdStudy == studies.IdStudy && e.Semester ==1).First();

            if(enrollment == null)
            {
                int maxId = _context.Enrollment.Max(e => e.IdEnrollment);

                enrollment = new Enrollment
                {
                    IdEnrollment = maxId,
                    Semester = 1,
                    IdStudy = studies.IdStudy,
                    StartDate = DateTime.Now
                };

                _context.Enrollment.Add(enrollment);
                _context.SaveChanges();

            }
            string pattern = "MM-dd-yyyy";
            DateTime parsedDate;
            if (!DateTime.TryParseExact(request.BirthDate, pattern, null, DateTimeStyles.None, out parsedDate))
                throw new Exception("niepoprawna data");

            Student student = new Student
            {
                IndexNumber = request.IndexNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = parsedDate,
                IdEnrollment = enrollment.IdEnrollment                
            };

            _context.Student.Add(student);

            _context.SaveChanges();

            EnrollmentStudentResponse response = new EnrollmentStudentResponse
            {
                IdEnrollment = enrollment.IdEnrollment,
                IdStudy = enrollment.IdStudy,
                Semester = enrollment.Semester,
                StartDate = enrollment.StartDate
            };
            return response;
        }

        public EnrollmentStudentResponse PromoteStudents(int semester, string studies)
        {

            int idStud;
            try
            {
                 idStud = _context.Studies.Where(s => s.Name == studies).First().IdStudy;

            }
            catch (Exception ex)
            {
                throw new Exception("nie znaleziono studiów");
            }


         
            Enrollment enrollment;
            try
            {
                enrollment = _context.Enrollment.Where(e => e.IdStudy == idStud && e.Semester == semester).OrderBy(e=>e.StartDate).First();

            }catch(Exception ex)
            {
                throw new Exception("nie znaleziono wpisów na dany semestr");
            }

                

            enrollment.Semester += 1;

            _context.SaveChanges();

            return new EnrollmentStudentResponse
                    {
                        IdEnrollment = enrollment.IdEnrollment,
                        IdStudy = enrollment.IdStudy,
                        StartDate = enrollment.StartDate,
                        Semester = enrollment.Semester
                    };
        }
    }
}
