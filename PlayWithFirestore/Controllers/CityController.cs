using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlayWithFirestore.Models;

namespace PlayWithFirestore.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly FirestoreDb _db;
        public CityController()
        {
            _db = FirestoreDb.Create("database-foodadrink");
        }
        [HttpPost("city")]
        public async Task<IActionResult> InsertCity([FromBody] City city)
        {
            var provinceSnapshot = await _db.Collection("provinces").Document(city.ProvinceId).GetSnapshotAsync();
            if(!provinceSnapshot.Exists)
            {
                return NotFound("Không tìm thấy tỉnh, miền hợp lệ");
            }

            await _db.Collection("cities").AddAsync(city);
            return Ok(city);
        }
        [HttpGet]
        public async Task<ActionResult<List<City>>> GetCity()
        {
            List<City> result = new();
            CollectionReference citiesRef = _db.Collection("cities");
            QuerySnapshot snapshots = await citiesRef.GetSnapshotAsync();
            if (snapshots.Count() > 0)
            {
                foreach (var snapshot in snapshots)
                {
                    result.Add(snapshot.ConvertTo<City>());
                }
                return Ok(result);
            }
            return NotFound();
        }
        [HttpDelete("city/{id}")]
        public async Task<IActionResult> DeleteCity(string id)
        {
            DocumentReference citiesRef = _db.Collection("cities").Document(id);
            await citiesRef.DeleteAsync();
            return NoContent();
        }
        [HttpPut("city")]
        public async Task<IActionResult> UpdateCity([FromBody] City city)
        {
            DocumentReference citiesRef = _db.Collection("cities").Document(city.Id);
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "Name", city.Name.ToString() }
            };
            await citiesRef.UpdateAsync(updates);
            return NoContent();
        }
        [HttpGet("city/{id}")]
        public async Task<ActionResult<City>> GetCityById(string id)
        {
            var snapshot = await _db.Collection("cities").Document(id).GetSnapshotAsync();
            if (snapshot.Exists)
            {
                return Ok(snapshot.ConvertTo<City>());
            }
            return NotFound();

        }
        [HttpGet("/provinces/province/{provinceId}/cities")]
        public async Task<ActionResult<List<City>>> GetCityByProvinceId(string provinceId)
        {
            List<City> result = new();
            CollectionReference citiesRef = _db.Collection("cities");
            QuerySnapshot snapshots = await citiesRef.GetSnapshotAsync();
            if (snapshots.Count() > 0)
            {
                foreach (var snapshot in snapshots)
                {
                    var x = snapshot.ConvertTo<City>();
                    if (x.ProvinceId == provinceId)
                    {
                        result.Add(x);
                    }
                }
                return Ok(result);
            }
            return NotFound();
        }
    }
}
