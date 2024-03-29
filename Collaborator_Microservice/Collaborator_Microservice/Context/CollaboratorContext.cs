using Microsoft.EntityFrameworkCore;
using Entity;

namespace Context
{
    public class CollaboratorContext : DbContext
    {
        public CollaboratorContext(DbContextOptions option) : base(option)
        {

        }
        public DbSet<CollaboratorEntity> Collaborators { get; set; }
    }
}
