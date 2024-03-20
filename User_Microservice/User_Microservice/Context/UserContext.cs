using Microsoft.EntityFrameworkCore;
using User_Microservice.Entity;

namespace User_Microservice.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions option) : base(option)
        {

        }

        public DbSet<UserEntity> User_Details { get; set; }
    }
}
