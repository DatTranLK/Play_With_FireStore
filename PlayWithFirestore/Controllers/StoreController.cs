using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlayWithFirestore.Models;

namespace PlayWithFirestore.Controllers
{
    [Route("api/stores")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly FirestoreDb _db;
        public StoreController()
        {
            _db = FirestoreDb.Create("database-foodadrink");
        }
        [HttpPost("store")]
        public async Task<IActionResult> InsertStore([FromBody] Store store)
        {
            var districtSnapshot = await _db.Collection("districts").Document(store.DistrictId).GetSnapshotAsync();
            if (!districtSnapshot.Exists)
            {
                return NotFound("Không tìm thấy quận hợp lệ");
            }

            var categorySnapshot = await _db.Collection("categories").Document(store.CategoryId).GetSnapshotAsync();
            if (!categorySnapshot.Exists)
            {
                return NotFound("Không tìm thấy category hợp lệ");
            }

            await _db.Collection("stores").AddAsync(store);
            return Ok(store);
        }
        [HttpGet]
        public async Task<ActionResult<List<Store>>> GetStore()
        {
            List<Store> result = new();
            CollectionReference storesRef = _db.Collection("stores");
            QuerySnapshot snapshots = await storesRef.GetSnapshotAsync();
            if (snapshots.Count() > 0)
            {
                foreach (var snapshot in snapshots)
                {
                    result.Add(snapshot.ConvertTo<Store>());
                }
                return Ok(result);
            }
            return NotFound();
        }
        [HttpDelete("store/{id}")]
        public async Task<IActionResult> DeleteStore(string id)
        {
            DocumentReference storesRef = _db.Collection("stores").Document(id);
            await storesRef.DeleteAsync();
            return NoContent();
        }
        [HttpPut("store")]
        public async Task<IActionResult> UpdateCity([FromBody] Store store)
        {
            DocumentReference storesRef = _db.Collection("stores").Document(store.Id);
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "Name", store.Name.ToString() },
                { "Address", store.Address.ToString() },
                { "MapLink", store.MapLink.ToString()},
                { "StartTime", store.StartTime.ToString()},
                { "EndTime", store.EndTime.ToString()}
            };
            await storesRef.UpdateAsync(updates);
            return NoContent();
        }
        [HttpGet("store/{id}")]
        public async Task<ActionResult<Store>> GetStoreById(string id)
        {
            var snapshot = await _db.Collection("stores").Document(id).GetSnapshotAsync();
            if (snapshot.Exists)
            {
                return Ok(snapshot.ConvertTo<Store>());
            }
            return NotFound();
        }
        [HttpGet("/districts/district/{districtId}/stores")]
        public async Task<ActionResult<List<Store>>> GetStoreByDistrictId(string districtId)
        {
            List<Store> result = new();
            CollectionReference storeRef = _db.Collection("stores");
            QuerySnapshot snapshots = await storeRef.GetSnapshotAsync();
            if (snapshots.Count() > 0)
            {
                foreach (var snapshot in snapshots)
                {
                    var x = snapshot.ConvertTo<Store>();
                    if (x.DistrictId == districtId)
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
