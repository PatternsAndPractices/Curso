using MVC.Model;
using MVC.ViewInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.View
{
    public class StudentGridView : IStudentView
    {
        public void Show(Student student)
        {
            Console.WriteLine("+---------------{0}+", "".PadLeft(40, '-'));
            Console.WriteLine("| Student Data  {0}|", "".PadLeft(40, ' '));
            Console.WriteLine("+---------------{0}+", "".PadLeft(40,'-'));
            Console.WriteLine("| Id          : {0,-40}|", student.Id);
            Console.WriteLine("| First Name  : {0,-40}|", student.FirstName);
            Console.WriteLine("| Last Name   : {0,-40}|", student.LastName);
            Console.WriteLine("| Age         : {0,-40}|", student.Age);
            Console.WriteLine("+---------------{0}+", "".PadRight(40, '-'));
        }
    }
}
