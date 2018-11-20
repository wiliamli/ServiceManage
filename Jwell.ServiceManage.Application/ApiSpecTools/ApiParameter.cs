using Newtonsoft.Json;

namespace Jwell.ServiceManage.Application.ApiSpecTools
{
    public class ApiParameter
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ParameterType { get; set; }

        public string DataType { get; set; }

        public string Format { get; set; }

        public string ExampleValue { get; set; }

        [JsonIgnore]
        public string Ref { get; set; }
    }
}
