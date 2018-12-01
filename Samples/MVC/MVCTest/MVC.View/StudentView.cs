using MVC.Model;
using MVC.ViewInterface;
using System;

namespace MVC.View
{
    public class StudentView : IStudentView
    {
        public void Show(Student student)
        {
            Console.WriteLine("Student Data");
            Console.WriteLine("Id           : {0}", student.Id);
            Console.WriteLine("First Name   : {0}", student.FirstName);
            Console.WriteLine("Last Name    : {0}", student.LastName);
            Console.WriteLine("Age          : {0}", student.Age);
        }
    }
}