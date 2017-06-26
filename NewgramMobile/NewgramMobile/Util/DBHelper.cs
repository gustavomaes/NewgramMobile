using NewgramMobile.Custom;
using NewgramMobile.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NewgramMobile.Util
{
    public class DBHelper : IDisposable
    {
        SQLiteAsyncConnection database;

        public DBHelper()
        {
            database = new SQLiteAsyncConnection(DependencyService.Get<IArquivo>().LocalBanco("Newgram.db3"));
            database.CreateTableAsync<Post>().Wait();
        }

        //Salva cache
        public void SalvaTudo(List<Post> Posts)
        {
            database.ExecuteAsync("delete from post").Wait();

            foreach (var p in Posts)
                p.SerializaUsuarioDados();
            database.InsertAllAsync(Posts);
        }

        //GET Cache
        public async Task<List<Post>> GetCahe()
        {
            List<Post> Posts = await database.Table<Post>().ToListAsync();

            foreach (var p in Posts)
                p.DesSerializaUsuarioDados();

            return Posts;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
