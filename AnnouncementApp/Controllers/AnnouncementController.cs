using AnnouncementApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnnouncementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private static List<Announcement> _announcements = new List<Announcement>();

        [HttpPost("announcement/add")]
        public IActionResult AddAnnouncement(Announcement announcement)
        {
            Announcement anotherAnnouncement = new Announcement(announcement.Title, announcement.Description);

            _announcements.Add(anotherAnnouncement);
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
        public IActionResult GetSimilarAnnouncement(string id)
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
