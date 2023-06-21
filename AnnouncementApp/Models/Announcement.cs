using System.Text.Json.Serialization;

namespace AnnouncementApp.Models
{
    public class Announcement
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime AddingDate { get; set; }
       
        public Announcement(string title, string description)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Description = description;
            AddingDate = DateTime.Now;
        }
    }
}
