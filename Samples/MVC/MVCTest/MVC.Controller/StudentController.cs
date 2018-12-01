using MVC.Model;
using MVC.ViewInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Controller
{
    public class StudentController
    {
        private Student student;
        private IStudentView studentView;

        public StudentController(Student student, IStudentView studentView)
        {
            SetStudentModel(student);
            SetStudentView(studentView);
        }

        public void SetStudentModel(Student student)
        {
            this.student = student;
        }

        public void SetStudentView(IStudentView studentView)
        {
            this.studentView = studentView;
        }

        public void UpdateView()
        {
            this.studentView.Show(this.student);
        }
    }
}
