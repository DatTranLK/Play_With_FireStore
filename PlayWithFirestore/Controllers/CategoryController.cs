using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlayWithFirestore.Models;

namespace PlayWithFirestore.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly FirestoreDb _db;
        public CategoryController()
        {
            _db = FirestoreDb.Create("database-foodadrink");
        }
        [HttpPost("category")]
        public async Task<IActionResult> InsertCategory([FromBody]Category category)
        {
            await _db.Collection("categories").AddAsync(category);
            return Ok(category);
        }
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategory()
        {
            List<Category> result = new();
            CollectionReference categoriesRef = _db.Collection("categories");
            QuerySnapshot snapshots = await categoriesRef.GetSnapshotAsync();
            if(snapshots.Count() > 0)
            {
                foreach (var snapshot in snapshots)
                {
                    result.Add(snapshot.ConvertTo<Category>());
                }
                return Ok(result);
            }
            return NotFound();
        }
        [HttpDelete("category/{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            DocumentReference categoryRef = _db.Collection("categories").Document(id);
            await categoryRef.DeleteAsync();
            return NoContent();
        }
        [HttpPut("category")]
        public async Task<IActionResult> UpdateCategory([FromBody] Category category)
        {
            DocumentReference categoryRef = _db.Collection("categories").Document(category.Id);
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "Name", category.Name.ToString() }
            };
            await categoryRef.UpdateAsync(updates);
            return NoContent();
        }
    }
}
