namespace Jwell.ServiceManage.Application.Services.Dtos
{
    public class AuthStateParaDto
    {
        public string ServiceNumber { get; set; }

        public string VersionNumber { get; set; }

        public string InvokeNumber { get; set; }

        public byte IsEnabled { get; set; }
    }
}