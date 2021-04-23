using SQLite;

namespace ProfileHandler.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string ProfilePicture { get; set; }
    }
}
