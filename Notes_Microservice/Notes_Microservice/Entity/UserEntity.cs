using System.ComponentModel.DataAnnotations;

namespace Notes_Microservice.Entity
{
    public class UserEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
    }
}
