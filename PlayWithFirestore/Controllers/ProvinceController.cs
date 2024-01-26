using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlayWithFirestore.Models;

namespace PlayWithFirestore.Controllers
{
    [Route("api/provinces")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly FirestoreDb _db;
        public ProvinceController()
        {
            _db = FirestoreDb.Create("database-foodadrink");
        }

        [HttpPost("province")]
        public async Task<IActionResult> InsertProvince([FromBody] Province province)
        {
            await _db.Collection("provinces").AddAsync(province);
            return Ok(province);
        }
        [HttpGet]
        public async Task<ActionResult<List<Province>>> GetProvince()
        {
            List<Province> result = new();
            CollectionReference provincesRef = _db.Collection("provinces");
            QuerySnapshot snapshots = await provincesRef.GetSnapshotAsync();
            if (snapshots.Count() > 0)
            {
                foreach (var snapshot in snapshots)
                {
                    result.Add(snapshot.ConvertTo<Province>());
                }
                return Ok(result);
            }
            return NotFound();
        }
        [HttpDelete("province/{id}")]
        public async Task<IActionResult> DeleteProvince(string id)
        {
            DocumentReference provincesRef = _db.Collection("provinces").Document(id);
            await provincesRef.DeleteAsync();
            return NoContent();
        }
        [HttpPut("province")]
        public async Task<IActionResult> UpdateProvince([FromBody] Province province)
        {
            DocumentReference provincesRef = _db.Collection("provinces").Document(province.Id);
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "Name", province.Name.ToString() }
            };
            await provincesRef.UpdateAsync(updates);
            return NoContent();
        }
        [HttpGet("province/{id}")]
        public async Task<ActionResult<Province>> GetProvinceById(string id)
        {
            var snapshot = await _db.Collection("provinces").Document(id).GetSnapshotAsync();
            if (snapshot.Exists)
            {
                return Ok(snapshot.ConvertTo<Province>());
            }
            return NotFound();
        }
    }
}
