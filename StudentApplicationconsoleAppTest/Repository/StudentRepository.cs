using StudentApplicationconsoleAppTest.Models;
using StudentApplicationconsoleAppTest.Repository.IRepository;
using System;


namespace StudentApplicationconsoleAppTest.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private List<Student> students = new List<Student>();
        private int lastAssignedId = 0;

        public IEnumerable<Student> GetStudents()
        {
            return students;
        }

        public Student GetStudentById(int id)
        {
            return students.FirstOrDefault(s => s.ID == id);
        }





        public void AddStudent(Student student)
        {
            student.ID = ++lastAssignedId;
            students.Add(student);
        }

        public void DeleteStudent(int id)
        {
            var student = students.FirstOrDefault(s => s.ID == id);
            if (student != null)
            {
                students.Remove(student);
            }
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return students;
        }

       

        public void UpdateStudent(Student student)
        {
            var existingStudent = students.FirstOrDefault(s => s.ID == student.ID);
            if (existingStudent != null)
            {
                existingStudent.Name = student.Name;
                existingStudent.Age = student.Age;
                existingStudent.Grade = student.Grade;
            }
        }
        public IEnumerable<Student> SortStudentsByName()
        {
            return students.OrderBy(s => s.Name);
        }

        public IEnumerable<Student> SortStudentsByGrade()
        {
            return students.OrderBy(s => s.Grade);
        }
    }
}
