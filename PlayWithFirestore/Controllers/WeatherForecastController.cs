
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using PlayWithFirestore.Models;
using System.Collections.Generic;
using System.Reflection;

namespace PlayWithFirestore.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly FirestoreDb _db;
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _db = FirestoreDb.Create("database-foodadrink");
        }

        [HttpGet("automated-adding-data")]
        public async Task<IActionResult> AddData()
        {
            
            /*Dictionary<string, object> user = new()
            {
                { "First", "Ada"},
                { "Last", "Lovelace"},
                { "Born", 1815}
            };
            await docRef.SetAsync(user);*/
            /*Dictionary<string, object> user2 = new Dictionary<string, object>
            {
                { "First", "Alan" },
                { "Middle", "Mathison" },
                { "Last", "Turing" },
                { "Born", 1912 }
            };*/
            User user2 = new()
            {
                First = "Alan",
                Middle = "Mathison",
                Last = "Turing",
                Born = 1912
            };
            /*var inforAdding = _mapper.Map<Dictionary<string, object>>(user2);*/
            DocumentReference docRef = await _db.Collection("users").AddAsync(user2);
            return Ok();
        }
        [HttpGet("get-info")]
        public async Task<ActionResult<List<User>>> GetIO()
        {
            CollectionReference usersRef = _db.Collection("users");
            QuerySnapshot snapshots = await usersRef.GetSnapshotAsync();
            List<User> usRe = new();

            foreach (var document in snapshots.Documents)
            {
                Console.WriteLine("User: {0}", document.Id);
                Dictionary<string, object> documentDictionary = document.ToDictionary();
                User user = new User
                {
                    // Populate the properties using the values from the dictionary
                    First = documentDictionary["First"].ToString(),
                    Middle = documentDictionary["Middle"].ToString(),
                    Last = documentDictionary["Last"].ToString(),
                    Born = Convert.ToInt32(documentDictionary["Born"])
                    // Add other properties as needed
                };

                // Add the user to the list
                usRe.Add(user);
            }
            return Ok(usRe);
        }
    }
}