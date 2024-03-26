using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notes_Microservice.Entity
{
    public class NotesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Colour { get; set; }

        public bool IsArchived { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public int CreatedBy { get; set; }

        [NotMapped]
        public UserEntity? User { get; set; }
    }
}
