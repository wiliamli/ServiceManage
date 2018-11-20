using System.Collections.Generic;
using Jwell.Modules.DSFClient;
using Jwell.Modules.DSFClient.Proxy;

namespace Jwell.ServiceManage.Test
{
    public class Student
    {
        public int StudentId { get; set; }

        public string Name { get; set; }
    }

    public class Teacher
    {
        public int TeacherId { get; set; }

        public string Name { get; set; }
    }

    public class Grade
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Teacher Teacher { get; set; }

        public List<Student> Students { get; set; }
    }

    public class PutTest
    {
        public string Id { get; set; }

        public Grade Grades { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Put-Test
            var putTest = new PutTest
            {
                Id = "123",
                Grades = new Grade
                {
                    Id = 1,
                    Name = "tzj",
                    Students = new List<Student> { new Student { StudentId = 12,Name = "tzj"} },
                    Teacher = new Teacher { Name = "tzj",TeacherId = 123}
                }
            };

            var act = new Act("Jwell.Test2", "6f88b9550a56481bbfd3fe60276fc9ca");
            var dsfParam = new DSFParam<PutTest> {Data = putTest, Act = act};

            var serviceProxyFactory = new ServiceProxyFactory();
            var response = serviceProxyFactory.Create<object, PutTest>("Jwell.Test1", "0.0.0.1", "Values", "Put", dsfParam);
        }          
    }
}
