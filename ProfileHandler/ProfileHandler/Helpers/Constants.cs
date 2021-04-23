using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProfileHandler.Helpers
{
    public class Constants
    {
        #region DB Variables

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        private const string DatabaseFilename = "profile_handler.db3";
        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return System.IO.Path.Combine(basePath, DatabaseFilename);
            }
        }

        #endregion
    }
}
