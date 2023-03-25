using SQLite4Unity3d;

namespace SystemControllers.Data
{
    [Table("SessionData")]
    public class SessionLinq
    {
        [PrimaryKey] 
        [Column("DataIndex")]
        public int DataIndex
        { get; set; }
        
        [Column("SessionID")]
        public int SessionID
        { get; set; }
        
        [Column("InternalTime")]
        public int InternalTime
        { get; set; }
        
        [Column("NumUsers")]
        public int NumUsers
        { get; set; }
        
        [Column("SessionPwd")]
        public string SessionPwd
        { get; set; }
        
        [Column("SavePath")]
        public string SavePath
        { get; set; }
    }
}