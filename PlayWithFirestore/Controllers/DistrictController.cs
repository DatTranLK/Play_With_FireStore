using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlayWithFirestore.Models;

namespace PlayWithFirestore.Controllers
{
    [Route("api/districts")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly FirestoreDb _db;
        public DistrictController()
        {
            _db = FirestoreDb.Create("database-foodadrink");
        }
        [HttpPost("district")]
        public async Task<IActionResult> InsertDistrict([FromBody] District district)
        {
            var citySnapshot = await _db.Collection("cities").Document(district.CityId).GetSnapshotAsync();
            if (!citySnapshot.Exists)
            {
                return NotFound("Không tìm thấy thành phố hợp lệ");
            }

            await _db.Collection("districts").AddAsync(district);
            return Ok(district);
        }
        [HttpGet]
        public async Task<ActionResult<List<District>>> GetDistrict()
        {
            List<District> result = new();
            CollectionReference districtsRef = _db.Collection("districts");
            QuerySnapshot snapshots = await districtsRef.GetSnapshotAsync();
            if (snapshots.Count() > 0)
            {
                foreach (var snapshot in snapshots)
                {
                    result.Add(snapshot.ConvertTo<District>());
                }
                return Ok(result);
            }
            return NotFound();
        }
        [HttpDelete("district/{id}")]
        public async Task<IActionResult> DeleteDistrict(string id)
        {
            DocumentReference districtsRef = _db.Collection("districts").Document(id);
            await districtsRef.DeleteAsync();
            return NoContent();
        }
        [HttpPut("district")]
        public async Task<IActionResult> UpdateDistrict([FromBody] District district)
        {
            DocumentReference districtsRef = _db.Collection("districts").Document(district.Id);
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "Name", district.Name.ToString() }
            };
            await districtsRef.UpdateAsync(updates);
            return NoContent();
        }
        [HttpGet("district/{id}")]
        public async Task<ActionResult<District>> GetDistrictById(string id)
        {
            var snapshot = await _db.Collection("districts").Document(id).GetSnapshotAsync();
            if (snapshot.Exists)
            {
                return Ok(snapshot.ConvertTo<District>());
            }
            return NotFound();

        }
        [HttpGet("/cities/city/{cityId}/districts")]
        public async Task<ActionResult<List<District>>> GetDistrictByCityId(string cityId)
        {
            List<District> result = new();
            CollectionReference districtsRef = _db.Collection("districts");
            QuerySnapshot snapshots = await districtsRef.GetSnapshotAsync();
            if (snapshots.Count() > 0)
            {
                foreach (var snapshot in snapshots)
                {
                    var x = snapshot.ConvertTo<District>();
                    if(x.CityId == cityId)
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
