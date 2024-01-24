using Google.Cloud.Firestore;

namespace PlayWithFirestore.Models
{
    [FirestoreData]
    public class District
    {
        [FirestoreDocumentId]
        public string Id { get; set; }
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string CityId { get; set; }
    }
}
