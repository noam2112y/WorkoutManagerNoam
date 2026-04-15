namespace WorkoutManagerNoam.Models
{
    public class User
    {
        public string? UserEmail { get; set; }
        public string? UserPassword { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? UMobile { get; set; }

        public bool IsParent { get; set; }
        public double Balance { get; set; }

        public int Id { get; set; }
        public DateTime UBDate { get; set; }
        public DateTime RegDate { get; set; }

        public string? UEmail
        {
            get { return UserEmail; }
            set { UserEmail = value; }
        }
        public int ParentId { get; set; }
    }
}