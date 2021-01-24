using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace WpfApplication1
{
    public static class DataBase
    {
        public static void CreateDBFile()
        {
            if (!File.Exists(Config.DB_PATH))
            {
                SQLiteConnection.CreateFile(Config.DB_PATH);
            }
        }

        public static void CreateUserTable()
        {
            using (SQLiteConnection Connect = new SQLiteConnection($@"Data Source = {Config.DB_PATH}; Version=3;"))
            {
                string commandText =
                    "CREATE TABLE IF NOT EXISTS [Users] ( [id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, [u_log] VARCHAR(10) NOT NULL, [u_pass] VARCHAR(10) NOT NULL, [is_admin] INTEGER DEFAULT 0)";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Connect.Open();
                Command.ExecuteNonQuery(); 
                Connect.Close(); 
            }
        }

        public static void CreateRecordMetaTable()
        {
            using (SQLiteConnection Connect = new SQLiteConnection($@"Data Source = {Config.DB_PATH}; Version=3;"))
            {
                string commandText =
                    "CREATE TABLE IF NOT EXISTS [RecordMeta] ( [id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, [ReportName] VARCHAR(24) NOT NULL UNIQUE)";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Connect.Open();
                Command.ExecuteNonQuery(); 
                Connect.Close(); 
            }
        }

        public static void CreateRecordTable()
        {
            using (SQLiteConnection Connect = new SQLiteConnection($@"Data Source = {Config.DB_PATH}; Version=3;"))
            {
                string commandText =
                    "CREATE TABLE IF NOT EXISTS [Record] ( [id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, [ReportId] INTEGER, [ObjectNameDez] VARCHAR(200) NOT NULL, [ObjectCountDez] INTEGER, [ObjectArea] VARCHAR (100) NOT NULL, [ProcessType] VARCHAR (5) NOT NULL, [DezType] VARCHAR (5) NOT NULL, [ProcessesPerMonth] INTEGER NOT NULL, [DezinfectionThing] VARCHAR (200) NOT NULL, [WorkConcentration] REAL NOT NULL, [RashodRastvora] VARCHAR (100), [KolichestvoRashodaOdnokrat] REAL)";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Connect.Open();
                Command.ExecuteNonQuery(); 
                Connect.Close(); 
            }
        }
        
        public static void DropRecordTable()
        {
            using (SQLiteConnection Connect = new SQLiteConnection($@"Data Source = {Config.DB_PATH}; Version=3;"))
            {
                string commandText =
                    "DROP TABLE IF EXISTS [Record]";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Connect.Open();
                Command.ExecuteNonQuery(); 
                Connect.Close(); 
            }
        }
        
        public static void DropRecordMetaTable()
        {
            using (SQLiteConnection Connect = new SQLiteConnection($@"Data Source = {Config.DB_PATH}; Version=3;"))
            {
                string commandText =
                    "DROP TABLE IF EXISTS [RecordMeta]";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Connect.Open();
                Command.ExecuteNonQuery(); 
                Connect.Close(); 
            }
        }
        
        public static void InsertUser(string login, string password, int is_admin)
        {
            using (SQLiteConnection Connect = new SQLiteConnection($@"Data Source={Config.DB_PATH}; Version=3;"))
            {

                string commandText =
                    "INSERT INTO [Users] ([u_log], [u_pass], [is_admin]) VALUES (@login, @password, @is_admin)";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Command.Parameters.AddWithValue("@login", login);
                Command.Parameters.AddWithValue("@password", password);
                Command.Parameters.AddWithValue("@is_admin", is_admin);
                Connect.Open(); 
                Command.ExecuteNonQuery();
                Connect.Close();
            }
        }

        public static User GetUser(string login, string password)
        {
            using (SQLiteConnection Connect = new SQLiteConnection($@"Data Source={Config.DB_PATH}; Version=3;"))
            {

                string commandText =
                    "SELECT * FROM [Users] WHERE [u_log] = @login AND [u_pass] = @password";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Command.Parameters.AddWithValue("@login", login);
                Command.Parameters.AddWithValue("@password", password);
                Connect.Open();
                User fetchedUser = null;
                using (SQLiteDataReader rdr = Command.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        fetchedUser = new User
                        {
                            Login = rdr.GetString(1), Password = rdr.GetString(2), IsAdmin = rdr.GetInt32(3)
                        };

                    }
                }
                Connect.Close();
                return fetchedUser;
            }
        }

        public static int SaveReport(RecordMeta report)
        {
            using (SQLiteConnection Connect = new SQLiteConnection($@"Data Source={Config.DB_PATH}; Version=3;"))
            {
                string commandText =
                    @"INSERT INTO [RecordMeta] ([ReportName]) VALUES (@reportMeta);
                      select last_insert_rowid();";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Command.Parameters.AddWithValue("@reportMeta", report.Name);
                Connect.Open(); 
                Int64 id = (Int64)Command.ExecuteScalar();
                Connect.Close();
                return (int)id;
            }
        }

        public static void SaveRecords(ICollection<Record> list, RecordMeta recordMeta)
        {
            using (SQLiteConnection Connect = new SQLiteConnection($@"Data Source={Config.DB_PATH}; Version=3;"))
            {

                string commandTextDeleteAll =
                    "DELETE FROM [Record] WHERE [ReportId] = @reportId";
                SQLiteCommand Command = new SQLiteCommand(commandTextDeleteAll, Connect);
                Command.Parameters.AddWithValue("@reportId", recordMeta.Id);
                Connect.Open(); 
                Command.ExecuteNonQuery();
                
                string commandText =
                    "INSERT INTO [Record] ([ReportId], [ObjectNameDez], [ObjectCountDez], [ObjectArea], [ProcessType], [DezType], [ProcessesPerMonth], [DezinfectionThing], [WorkConcentration], [RashodRastvora], [KolichestvoRashodaOdnokrat]) VALUES (@reportId, @objectNameDez, @objectCountDez, @objectArea, @processType, @dezType, @processesPerMonth, @dezinfectionThing, @workConcentration, @rashodRastvora, @kolichestvoRashodaOdnokrat)";
                Command = new SQLiteCommand(commandText, Connect);
                foreach (var o in list)
                {
                    Command.Parameters.Clear();
                    Command.Parameters.AddWithValue("@reportId", o.Meta.Id);
                    Command.Parameters.AddWithValue("@objectNameDez", o.ObjectNameDez);
                    Command.Parameters.AddWithValue("@objectCountDez", Int32.Parse(o.ObjectCountDez));
                    Command.Parameters.AddWithValue("@objectArea", Single.Parse(o.ObjectArea));
                    Command.Parameters.AddWithValue("@processType", o.ProcessType);
                    Command.Parameters.AddWithValue("@dezType", o.DezType);
                    Command.Parameters.AddWithValue("@processesPerMonth", o.ProcessesPerMonth);
                    Command.Parameters.AddWithValue("@dezinfectionThing", o.DezinfectionThing);
                    Command.Parameters.AddWithValue("@workConcentration", o.WorkConcentration);
                    Command.Parameters.AddWithValue("@rashodRastvora", o.RashodRastvora);
                    Command.Parameters.AddWithValue("@kolichestvoRashodaOdnokrat", o.KolichestvoRashodaOdnokrat);
                    Command.ExecuteNonQuery();
                }
                Connect.Close();
            }
        }

        public static void DeleteReport(RecordMeta report)
        {
            using (SQLiteConnection Connect = new SQLiteConnection($@"Data Source={Config.DB_PATH}; Version=3;"))
            {
                string commandText =
                    "DELETE FROM [Record] WHERE [ReportId] = @reportId";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Command.Parameters.AddWithValue("@reportId", report.Name);
                Connect.Open(); 
                Command.ExecuteNonQuery();
                
                string commandTextDeleteReport =
                    "DELETE FROM [RecordMeta] WHERE [ReportName] = @reportName";
                Command = new SQLiteCommand(commandTextDeleteReport, Connect);
                Command.Parameters.AddWithValue("@reportName", report.Name);
                Command.ExecuteNonQuery();
                
                Connect.Close();
            }
        }

        public static List<RecordMeta> GetReports()
        {
            List<RecordMeta> reports = new List<RecordMeta>();
            using (SQLiteConnection Connect = new SQLiteConnection($@"Data Source={Config.DB_PATH}; Version=3;"))
            {

                string commandText =
                    "SELECT * FROM [RecordMeta]";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Connect.Open();
                using (SQLiteDataReader rdr = Command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        reports.Add(new RecordMeta
                        {
                            Id = rdr.GetInt32(0), Name = rdr.GetString(1)
                        });
                    }
                }
                Connect.Close();
                return reports;
            }
        }
        
        public static List<Record> GetRecords(RecordMeta report)
        {
            List<Record> reports = new List<Record>();
            using (SQLiteConnection Connect = new SQLiteConnection($@"Data Source={Config.DB_PATH}; Version=3;"))
            {

                string commandText =
                    "SELECT * FROM [Record] WHERE [ReportId] = @reportId";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Command.Parameters.AddWithValue("@reportId", report.Id);
                Connect.Open();
                using (SQLiteDataReader rdr = Command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        reports.Add(new Record
                        {
                            Id = rdr.GetInt32(0),
                            Meta = report,
                            ObjectNameDez = rdr.GetString(2),
                            ObjectCountDez = rdr.GetInt32(3).ToString(),
                            ObjectArea = rdr.GetString(4).Replace(".", ","),
                            ProcessType = rdr.GetString(5),
                            DezType = rdr.GetString(6),
                            ProcessesPerMonth = rdr.GetInt32(7),
                            DezinfectionThing = rdr.GetString(8),
                            WorkConcentration = rdr.GetFloat(9),
                            RashodRastvora = rdr.GetString(10).Replace(".", ","),
                            KolichestvoRashodaOdnokrat = rdr.GetFloat(11)
                        });
                    }
                }
                Connect.Close();
                return reports;
            }
        }
    }
}