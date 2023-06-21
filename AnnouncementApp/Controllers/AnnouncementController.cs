using System.IO;
using System.Text;
using AnnouncementApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AnnouncementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private static List<Announcement> _announcements = SetList();

        private static List<Announcement> SetList()
            => LoadList() != "" ? JsonConvert.DeserializeObject<List<Announcement>>(LoadList()) 
                : new List<Announcement>();

        private static void SaveList()
        {
            string json = JsonConvert.SerializeObject(_announcements);
            using (Stream stream = new FileStream("announcements.json", FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.Write(json);
                }
            }
        }

        private static string LoadList()
        {
            if (System.IO.File.Exists("announcements.json"))
            {
                string txt = "";
                using (Stream stream = new FileStream("announcements.json", FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        while (!reader.EndOfStream)
                        {
                            txt += reader.ReadLine() + "\n";
                        }
                    }
                }
                return txt;
            }
            return "";
        }

        [HttpPost("announcement/add")]
        public IActionResult AddAnnouncement(Announcement announcement)
        {
            Announcement anotherAnnouncement = new Announcement(announcement.Title, announcement.Description);

            _announcements.Add(anotherAnnouncement);
            SaveList();
            return Ok("Announcement was added!");
        }

        [HttpGet("announcement/get/all")]
        public IActionResult GetAllAnnouncement()
        {
            if (_announcements.Count > 0)
                return Ok(_announcements);
            return NoContent();
        }

        [HttpDelete("announcement/delete")]
        public IActionResult DeleteAnnouncement(Announcement announcement)
        {
            if (_announcements.Count <= 0) return NoContent();
            foreach (var value in _announcements)
            {
                if (value.Id == announcement.Id)
                {
                    _announcements.Remove(value);
                    SaveList();
                    return Ok();
                }
            }
            return NotFound();
        }

        [HttpPut("announcement/edit")]
        public IActionResult EditAnnouncement(string id, Announcement announcement)
        {
            if (_announcements.Count <= 0) return NoContent();
            for (int i = 0; i < _announcements.Count; i++)
            {
                if (_announcements[i].Id == id)
                {
                    _announcements[i] = announcement;
                    _announcements[i].Id = id;
                    SaveList();
                    return Ok();
                }
            }

            return NotFound();

        }

        [HttpGet("announcement/get/similar")]
        public IActionResult GetSimilarAnnouncement(string title, string description)
        {
            List<Announcement> result = new List<Announcement>();
            if (_announcements.Count <= 0) return NoContent();
            foreach (var value in _announcements)
            {
                if (result.Count == 3)
                    return Ok(result);

                string[] titleWords = title.Split(" ");
                string[] descriptionWords = description.Split(" ");

                bool contains = titleWords.Any(t => value.Title.Contains(t));

                if (contains)
                    result.AddRange(from t in descriptionWords where value.Description.Contains(t) select value);
                    
            }

            if (result.Count > 0)
                return Ok(result);

            return NotFound();
        }

        [HttpGet("announcement/get/info")]
        public IActionResult GetMoreInfo(string id)
        {
            if (_announcements.Count <= 0) return NoContent();
            foreach (var value in _announcements)
            {
                if (value.Id == id)
                    return Ok(value);
            }
            return NotFound();
        }
    }
}
