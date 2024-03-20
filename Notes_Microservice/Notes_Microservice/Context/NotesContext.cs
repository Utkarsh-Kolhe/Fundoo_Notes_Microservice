using Microsoft.EntityFrameworkCore;
using Notes_Microservice.Entity;

namespace Notes_Microservice.Context
{
    public class NotesContext : DbContext
    {
        public NotesContext(DbContextOptions option) : base(option)
        {

        }

        public DbSet<NotesEntity> Notes { get; set; }
    }
}
