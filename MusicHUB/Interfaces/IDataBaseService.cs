using SQLite;
namespace MusicHUB.Interfaces
{
    public interface IDataBaseService
    {
        SQLiteAsyncConnection DataBase { get; }
    }
}
