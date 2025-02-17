namespace TestApplication.Application.DTO_s
{
    public class AuthResponseDTO
    {
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string JWT { get; set; } = string.Empty;

    }
}
