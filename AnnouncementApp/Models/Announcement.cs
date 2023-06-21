using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AnnouncementApp.Models
{
    public class Announcement
    {
        [JsonProperty("Id:")]
        public string Id { get; set; }

        [JsonProperty("Title:")]
        public string Title { get; set; }

        [JsonProperty("Description:")]
        public string Description { get; set; }

        [JsonProperty("Adding date:")]
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
