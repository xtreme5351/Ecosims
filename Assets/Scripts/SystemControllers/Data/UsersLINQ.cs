using SQLite4Unity3d;

namespace SystemControllers.Data
{
    [Table("Users")]
    public class UserLinq
    {
        [PrimaryKey] 
        [Column("userID")]
        public string UserID
        { get; set; }

        [Column("username")]
        public string Username
        { get; set; }

        [Column("password")]
        public string Password
        { get; set; }
        
        [Column("redID")]
        public int RedID
        { get; set; }
    }
}


