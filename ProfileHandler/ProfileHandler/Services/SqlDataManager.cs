using ProfileHandler.Helpers;
using ProfileHandler.Models;
using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileHandler.Data
{
    public class SqlDataManager
    {
        static object locker = new object();
        //SQLiteConnection database;

        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });
        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;
        public SqlDataManager()
        {
            try
            {
                InitializeAsync().SafeFireAndForget(false);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        async Task InitializeAsync()
        {
            try
            {
                if (!initialized)
                {
                    initialized = true;                    
                    if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(User).Name))
                    {
                        await Database.CreateTablesAsync(CreateFlags.None, typeof(User)).ConfigureAwait(false);
                    }                   
                   
                }
            }
            catch (Exception ex) { throw ex; }
        }

        

        #region User Details
        public Task<List<User>> GetUser()
        {
            try
            {
                lock (locker)
                {
                    return Database.Table<User>().ToListAsync();
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public Task<int> SaveOrUpdateUser(User item)
        {
            try
            {
                lock (locker)
                {
                    if (Database.ExecuteScalarAsync<bool>("SELECT EXISTS(SELECT 1 FROM User WHERE Id=?)", item.Id).Result)
                    {
                        //Update Item  
                        return Database.UpdateAsync(item);
                        //return item.Id;
                    }
                    else
                    {
                        //Insert item  
                        return Database.InsertAsync(item);
                    }


                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        #endregion        
    }
}
