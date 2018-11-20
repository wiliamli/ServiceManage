using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwell.ServiceManage.Test.SwaggerClass
{
    public class Client
    {
        public string Swagger { get; set; }

        public Info Info { get; set; }

        public string Host { get; set; }

        public string[] Schemes { get; set; }

        public Path[] Paths { get; set; }

    }

    public class Info
    {
        public string Version { get; set; }

        public string Title { get; set; }
    }

    public class Path
    {
        public Api Api { get; set; }
    }

    public class Api
    {
        public Get Get { get; set; }
    }

    public class Get
    {
        public string[] Tags { get; set; }

        public string Summary { get; set; }

        public string OperationId { get; set; }

        public string[] Consumes { get; set; }

        public string[] Produces { get; set; }

        public Parameter[] Parameters { get; set; }
    }

    public class Post
    {
        public string[] Tags { get; set; }

        public string Summary { get; set; }

        public string OperationId { get; set; }

        public string[] Consumes { get; set; }

        public string[] Produces { get; set; }

        public Parameter[] Parameters { get; set; }

    }

    public class Parameter
    {
        public string Name { get; set; }

        public string In { get; set; }

        public string Description { get; set; }

        public bool Required { get; set; }
    }
}
