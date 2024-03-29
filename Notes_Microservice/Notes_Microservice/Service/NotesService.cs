using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using Notes_Microservice.Context;
using Notes_Microservice.Entity;
using Notes_Microservice.Interface;
using Notes_Microservice.Model;
using Notes_Microservice.Models;
using System.Net.Http.Headers;

namespace Notes_Microservice.Service
{
    public class NotesService : INotesInterface
    {
        private readonly NotesContext _context;
        private readonly IConfiguration _configuration;

        public NotesService(NotesContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<NotesEntity> AddNote(NotesModel model, int userId, string token)
        {
            NotesEntity notesEntity = new NotesEntity();
            notesEntity.Title = model.Title;
            notesEntity.Description = model.Description;
            notesEntity.Colour = model.Colour;
            notesEntity.CreatedBy = userId;

            notesEntity.User = await GetUserDetails(token);

            _context.Notes.Add(notesEntity);
            _context.SaveChanges();

            return notesEntity;
        }

        public async Task<UserEntity> GetUserDetails(string token)
        {
            HttpClient httpClient = new HttpClient();

            //string url = "https://localhost:7069/api/User";
            string? apiUrl = _configuration["ApiUrl"];
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage responseMessage = await httpClient.SendAsync(request);

            if (responseMessage.IsSuccessStatusCode)
            {
                string responseContent = await responseMessage.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<ResponseModel<UserEntity>>(responseContent);

                if(response != null && response.Data != null)
                {
                    return response.Data;
                }
                else
                {
                    throw new Exception("Failed to retrive user data");
                }
            }
            else
            {
                throw new Exception("Failed to retrive user data");
            }


        }

        public List<NotesEntity> GetNotes(int userId)
        {
            var notes = _context.Notes.Where(e => e.CreatedBy == userId).ToList();
            return notes;
        }

        public bool EditNote(int noteId, int userId, NotesModel model)
        {
            var note = _context.Notes.FirstOrDefault(e => e.NoteId == noteId && e.CreatedBy == userId);
            if (note != null)
            {
                note.Title = model.Title;
                note.Description = model.Description;
                note.Colour = model.Colour;

                _context.SaveChanges();

                return true;
            }
            return false;
        }

        public bool DeleteNote(int noteId, int userId)
        {
            var note = _context.Notes.FirstOrDefault(e => (e.NoteId == noteId && e.CreatedBy == userId));
            if (note != null)
            {
                _context.Notes.Remove(note);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public int IsArchivedOrUnarchived(int noteId)
        {
            var note = _context.Notes.FirstOrDefault(x => x.NoteId == noteId);
            if (note != null)
            {
                if (note.IsArchived)
                {
                    note.IsArchived = false;
                    _context.SaveChanges();
                    return 1; // note unarchived
                }
                else
                {
                    note.IsArchived = true;
                    _context.SaveChanges();
                    return 2; // note archived
                }
            }
            
            return 0; // note not found
        }

        public int TrashUntrashNote(int noteId)
        {
            var Note = _context.Notes.FirstOrDefault(o => o.NoteId == noteId);
            if (Note != null)
            {
                if (Note.IsDeleted)
                {
                    Note.IsDeleted = false;
                    _context.SaveChanges();
                    return 1; // 1 => note untrashed 
                }
                else
                {
                    Note.IsDeleted = true;
                    _context.SaveChanges();
                    return 2; // 2 => note trashed
                }

            }

            return 0; // 0 => note not found
        }
    }
}
