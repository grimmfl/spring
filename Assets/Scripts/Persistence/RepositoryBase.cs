using System;
using System.IO;
using SQLite4Unity3d;
using UnityEngine;

namespace Persistence
{
	public class RepositoryBase
	{
		private const string Db = "db.db";
		private readonly string _dbPath;

		protected RepositoryBase()
		{
#if UNITY_EDITOR
            _dbPath = string.Format(@"Assets/StreamingAssets/{0}", Db);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, Db);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID 
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + Db);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + Db;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + Db;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + Db;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + Db;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + Db;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        _dbPath = filepath;
#endif
			
			
		}

		protected SQLiteConnection OpenConnection()
		{
			return new SQLiteConnection(_dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
		}

		protected TResult Query<TResult>(Func<SQLiteConnection, TResult> query)
		{
			var connection = OpenConnection();

			var result = query.Invoke(connection);
			
			connection.Close();

			return result;
		}
	}
}