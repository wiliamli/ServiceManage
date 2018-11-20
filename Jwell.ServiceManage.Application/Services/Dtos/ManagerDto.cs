using Newtonsoft.Json;

namespace Jwell.ServiceManage.Application.Services.Dtos
{
    public class ManagerDto
    {
        public string EmployeeID { get; set; }

        public string UserName { get; set; }

        public long UserID { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
    }
}