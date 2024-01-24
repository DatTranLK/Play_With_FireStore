using Google.Cloud.Firestore;

namespace PlayWithFirestore.Models
{
    [FirestoreData]
    public class Store
    {
        [FirestoreDocumentId]
        public string Id { get; set; }
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string DistrictId { get; set; }
        [FirestoreProperty]
        public string CategoryId { get; set; }
        [FirestoreProperty]
        public string Address { get; set; }
        [FirestoreProperty]
        public TimeSpan StartTime { get; set; }
        [FirestoreProperty]
        public TimeSpan EndTime { get; set; }
    }
}
