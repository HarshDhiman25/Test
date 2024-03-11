using StudentApplicationconsoleAppTest.Models;
using StudentApplicationconsoleAppTest.Repository.IRepository;
using StudentApplicationconsoleAppTest.Repository;
using System.Security.Cryptography;


namespace StudentManagement
{
    class Program
    {

        static void Main(string[] args)
        {

            IStudentRepository studentRepository = new StudentRepository();


            bool exit = false;
            while (!exit)
            {

                Console.WriteLine ("Student Lists");
                Console.WriteLine("1. View all students");
                Console.WriteLine("2. Add a new student");
                Console.WriteLine("3. Update a student");
                Console.WriteLine("4. Delete a student");
                Console.WriteLine("5. Search for students");
                Console.WriteLine("6. Sort Students By Name");
                Console.WriteLine("7.Sort Students By Grade");
                Console.WriteLine("8. Exit");


                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAllStudents(studentRepository);
                        break;
                    case "2":
                        AddStudent(studentRepository);
                        break;
                    case "3":
                        UpdateStudent(studentRepository);
                        break;
                    case "4":
                        DeleteStudent(studentRepository);
                        break;
                    case "5":
                        SearchStudents(studentRepository);
                        break;
                    case "6":
                        SortStudentsByName(studentRepository);
                        break;
                    case "7":
                        SortStudentsByGrade(studentRepository);
                        break;
                    case "8":
                        exit = true;
                        break;


                    default:
                        Console.WriteLine("Invalid choice! Please try again.");
                        break;
                }
            }
        }

        private static void SortStudentsByGrade(IStudentRepository studentRepository)
        {
            var students = studentRepository.GetAllStudents();

            if (!students.Any())
            {
                Console.WriteLine("There is no Grade exist to sort.");
                return;
            }

            var sortedStudents = students.OrderBy(s => s.Grade);
            foreach (var student in sortedStudents)
            {
                Console.WriteLine($"ID: {student.ID}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
            }
        }


        private static void SortStudentsByName(IStudentRepository studentRepository)
        {
            var students = studentRepository.GetAllStudents();

            if (!students.Any())
            {
                Console.WriteLine("There is no Name to sort.");
                return;
            }

            var sortedStudents = students.OrderBy(s => s.Name);
            foreach (var student in sortedStudents)
            {
                Console.WriteLine($"ID: {student.ID}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
            }
        }


        static void ViewAllStudents(IStudentRepository studentRepository)
        {
            var students = studentRepository.GetAllStudents();

            if (!students.Any())
            {
                Console.WriteLine("No data found.");
                return;
            }

            foreach (var student in students)
            {
                Console.WriteLine($"ID: {student.ID}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
            }
        }


        static void AddStudent(IStudentRepository studentRepository)
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();


            if (!name.All(char.IsLetter))
            {
                Console.WriteLine("Invalid input! Name must contain only alphabetic characters.");
                return;
            }

            if (name.Length > 16)
            {
                Console.WriteLine("Invalid input! Name must be up to 16 characters.");
                return;
            }


            int age;
            bool isValidAge = false;
            do
            {
                Console.Write("Enter Age: ");
                string ageInput = Console.ReadLine();

                if (int.TryParse(ageInput, out age))
                {
                    if (age > 0 && age <= 100)
                    {
                        isValidAge = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input! Age must be a number between 1 and 100.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Age must be a number.");
                }
            } while (!isValidAge);



            string grade;
            bool isValidGrade = false;
            do
            {
                Console.Write("Enter Grade (A, B, C, or D): ");
                grade = Console.ReadLine().ToUpper();

                if (grade.Length == 1 && grade[0] >= 'A' && grade[0] <= 'D')
                {
                    isValidGrade = true;
                }
                else
                {
                    Console.WriteLine("Invalid input! Grade must be a single alphabet (A, B, C, or D).");
                }
            } while (!isValidGrade);

            Student newStudent = new Student { Name = name, Age = age, Grade = grade };
            studentRepository.AddStudent(newStudent);
            Console.WriteLine("Student added successfully!");
        }



        static void UpdateStudent(IStudentRepository studentRepository)
        {
            Console.WriteLine("Enter Student ID  ");
            string input = Console.ReadLine().Trim();

            Student existingStudent = null;


            if (int.TryParse(input, out int id))
            {
                // Search by ID
                existingStudent = studentRepository.GetStudentById(id);
            }


            if (existingStudent != null)
            {
                Console.Write("Enter new Name (leave blank if not changed): ");
                string name = Console.ReadLine().Trim();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    if (name.Length > 16)
                    {
                        Console.WriteLine("Invalid input! Name must be up to 16 characters.");
                        return;
                    }
                    existingStudent.Name = name;
                }

                Console.Write("Enter new Age (leave blank if not changed): ");
                string ageStr = Console.ReadLine().Trim();
                if (!string.IsNullOrWhiteSpace(ageStr))
                {
                    int age;
                    if (!int.TryParse(ageStr, out age) || age < 1 || age > 99)
                    {
                        Console.WriteLine("Invalid input! Age must be a number between 1 and 99.");
                        return;
                    }
                    existingStudent.Age = age;
                }

                Console.Write("Enter new Grade (leave blank if not changed): ");
                string grade = Console.ReadLine().Trim().ToUpper();
                if (!string.IsNullOrWhiteSpace(grade))
                {
                    if (grade.Length != 1 || grade[0] < 'A' || grade[0] > 'D')
                    {
                        Console.WriteLine("Invalid input! Grade must be a single alphabet (A, B, C, or D).");
                        return;
                    }
                    existingStudent.Grade = grade;
                }

                studentRepository.UpdateStudent(existingStudent);
                Console.WriteLine("Student updated successfully!");
            }
            else
            {
                Console.WriteLine("Student not found!");
            }
        }

        static void DeleteStudent(IStudentRepository studentRepository)
        {
            var students = studentRepository.GetAllStudents();

            if (!students.Any())
            {
                Console.WriteLine("There is no data to delete.");
                return;
            }

            Console.Write("Enter Student ID to delete: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid input! Please enter a valid Student ID.");
                return;
            }

            var studentToDelete = studentRepository.GetStudentById(id);
            if (studentToDelete == null)
            {
                Console.WriteLine("Student not found!");
                return;
            }

            studentRepository.DeleteStudent(id);
            Console.WriteLine("Student deleted successfully!");
        }


        static void SearchStudents(IStudentRepository studentRepository)
        {
            Console.Write("Enter search keyword: ");
            string keyword = Console.ReadLine();

            var students = studentRepository.GetAllStudents()
                .Where(s => s.Name.ToLower().Contains(keyword.ToLower()) || s.Grade.ToLower() == keyword.ToLower());
            if (students.Any())
            {
                Console.WriteLine("Search results:");
                foreach (var student in students)
                {
                    Console.WriteLine($"ID: {student.ID}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
                }
            }
            else
            {
                Console.WriteLine("No matching students found!");
            }

        }
    }
}
