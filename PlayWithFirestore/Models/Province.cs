using Google.Cloud.Firestore;

namespace PlayWithFirestore.Models
{
    [FirestoreData]
    public class Province
    {
        [FirestoreDocumentId]
        public string Id { get; set; }
        [FirestoreProperty]
        public string Name { get; set; }
    }
}
