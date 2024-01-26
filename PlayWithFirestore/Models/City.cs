using Google.Cloud.Firestore;

namespace PlayWithFirestore.Models
{
    [FirestoreData]
    public class City
    {
        [FirestoreDocumentId]
        public string? Id { get; set; }
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string ProvinceId { get; set; }
    }
}
