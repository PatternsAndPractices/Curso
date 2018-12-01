using MVC.Controller;
using MVC.Model;
using MVC.View;
using MVC.ViewInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Student student = RetrieveStudentFromDataBase();

            IStudentView studentView = new StudentView();

            StudentController studentController = new StudentController(student, studentView);

            studentController.UpdateView();

            Console.WriteLine("\nPresiona una tecla para continuar\n");
            Console.ReadKey();

            studentController.SetStudentView(new StudentGridView());
            studentController.UpdateView();

            Console.WriteLine("\nPresiona una tecla para continuar\n");
            Console.ReadKey();

            Student studentFile = RetrieveStudentFromFile();

            studentController.SetStudentModel(studentFile);
            studentController.UpdateView();

            Console.WriteLine("\nPresiona una tecla para continuar\n");
            Console.ReadKey();


        }

        private static Student RetrieveStudentFromDataBase()
        {
            Student student = new Student();
            student.Id = 1;
            student.FirstName = "Julio";
            student.LastName = "Robles";
            student.Age = 44;

            return student;
        }

        private static Student RetrieveStudentFromFile()
        {
            Student student = new Student();
            student.Id = 2;
            student.FirstName = "Pilar";
            student.LastName = "Lopez";
            student.Age = 38;

            return student;
        }

    }
}
