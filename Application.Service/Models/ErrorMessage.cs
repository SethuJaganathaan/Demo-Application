namespace Application.Service.Models
{
    public class ErrorMessage
    {
        public int Status { get; set; }
        public string Types { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
    }
}
