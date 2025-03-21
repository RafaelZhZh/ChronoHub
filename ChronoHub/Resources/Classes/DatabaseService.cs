using SQLite;
using System.Collections.ObjectModel;

namespace ChronoHub;
public class ChronoDataSQL
{
    [PrimaryKey, NotNull]
    public string Name{ get; set; }
    public int Time { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateTimeLastStart { get; set; }
}


public class DatabaseService
{
    private SQLiteConnection _connection;

    public DatabaseService()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "mydb.db");
        _connection = new SQLiteConnection(dbPath);
        _connection.CreateTable<ChronoDataSQL>();
    }

    public void SaveData(ObservableCollection<Chrono> chronoListSQL)
    {
        _connection.DeleteAll<ChronoDataSQL>();
        foreach (var item in chronoListSQL)
        {
            var chronoData = new ChronoDataSQL
            {
                Name = item.Name,
                Time = item._seconds,
                IsActive = item.CanStop,
                DateTimeLastStart = item.DateTimeLastStart
            };
            _connection.InsertOrReplace(chronoData);
        }
    }

    public List<Chrono> GetData()
    {
        var chronoData = _connection.Table<ChronoDataSQL>().ToList();
        var chronoListSQL = new List<Chrono>();
        foreach (var item in chronoData)
        {
            var chrono = new Chrono(item.Name, item.Time, !item.IsActive, item.IsActive, item.DateTimeLastStart);
            chronoListSQL.Add(chrono);
        }

        return chronoListSQL;
    }
}
