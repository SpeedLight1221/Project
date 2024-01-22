using SQLite;


namespace seznam
{
    public class DB
    {

        SQLiteAsyncConnection db;

        public DB()
        {

        }

        async Task Init()
        {
            if (db is not null)
            {
                return;
            }

            db = new SQLiteAsyncConnection(Constants.DBPath);
            await db.CreateTableAsync<DBImage>();
        }


        public async Task<List<DBImage>> GetAll()
        {
            return await db.Table<DBImage>().ToListAsync();
        }

        public async Task<DBImage> GetOne(int id)
        {
            return await db.Table<DBImage>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Insert(DBImage img)
        {
            await db.InsertAsync(img);
        }

        public async Task Update(DBImage img)
        {
            await db.UpdateAsync(img);
        }

        public async Task Remove(DBImage img)
        {
            await db.DeleteAsync(img);
        }

    }


    public static class Constants
    {
        public const string FileName = "gallery.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DBPath => Path.Combine(FileSystem.AppDataDirectory, FileName);

    }

    [Table("Images")]
    public class DBImage
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("ID")]
        public int Id { get; set; }

        [Column("Image")]
        public string Img { get; set; }
        [Column("Title")]
        public string Title { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("Location")]
        public string Location { get; set; }
        [Column("Created")]
        public DateTime Created { get; set; }

        [Column("Modified")]
        public DateTime Modified { get; set; }



    }
}



