namespace Application.Repository.DTO.Exception
{
    public class NotFound : System.Exception
    {
        public NotFound() : base() { }

        public NotFound(string message) : base(message)
        {
            
        }
    }
}
