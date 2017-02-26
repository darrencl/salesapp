using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SalesApp.Core.Interfaces;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;
using System.IO;

namespace SalesApp.Droid.Database
{
    public class SqliteDroid:ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "SalesSQLite.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);
            // Create the connection
            var conn = new SQLiteConnection(new SQLitePlatformAndroid(), path);
            // Return the database connection
            return conn;
        }
    }
}