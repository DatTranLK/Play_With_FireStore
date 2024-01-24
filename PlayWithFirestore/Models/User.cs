using Google.Cloud.Firestore;

namespace PlayWithFirestore.Models
{
    [FirestoreData]
    public class User
    {
        [FirestoreProperty]
        public string First { get; set; }
        [FirestoreProperty]
        public string Middle { get; set; }
        [FirestoreProperty]
        public string Last { get; set; }
        [FirestoreProperty]
        public int Born { get; set; }
    }
}
