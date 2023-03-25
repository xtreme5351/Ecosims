using SQLite4Unity3d;

namespace SystemControllers.Data
{
    [Table("SaveTable")]
    public class SaveLinq
    {
        [PrimaryKey] 
        [Column("SessionID")]
        public int SessionID
        { get; set; }
        
        [Column("UserID")]
        public int UserID
        { get; set; }

        [Column("LastAccessed")]
        public string Last
        { get; set; }
        
        [Column("Duration")]
        public float Duration
        { get; set; }
    }
}

