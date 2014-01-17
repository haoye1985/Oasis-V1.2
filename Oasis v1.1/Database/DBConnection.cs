// ========================================================================
// =
// =    Author:    Hao Ye
// =    Email :     hao.ye@imperial.ac.uk
// =
// =    Project:    Oasis
// =
// =    Copyright © 2013, CTS
// =
// =    No access, modification or use of this code is allowed, except by  
// =    persons who possess the express written permision of the author.
// ========================================================================

using System;
using System.Collections.Generic;
using Npgsql;
using System.Globalization;

namespace Oasis_v1._1
{
    public class DbConnection
    {
        #region Fields

        public string _connectionString;

        public string _PostGISVersion;

        public List<string> tableList;

        public bool _isConnected = false;

        #endregion

        #region Constructors

        public DbConnection(string host, string port, string database, string uid, string password)
        {
            this._connectionString = GetConnectString(host, port, database, uid, password);
        }
                
        public DbConnection(string ConnectString)
        {
            this._connectionString = ConnectString;
        }

        #endregion

        #region Methods

        public string GetConnectString(string host, string port, string database, string uid, string password)
        {
            // Construct a connection string by using a fixed order
            _connectionString = "Host=" + host + ";" + "Port=" + port + ";" + "Database=" + database + ";" + "uid=" + uid + ";" + "password=" + password + ";";
            return _connectionString;
        }

        public void GetConnect()
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    _isConnected = true;
                }
                catch (Exception)
                {
                    _isConnected = false;
                }
            }
        }

        public double GetPostGisVersion()
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("SELECT postgis_version();", conn))
                {
                    var version = command.ExecuteScalar();
                    if (version == DBNull.Value)
                        return 0;
                    var vPart = version.ToString();
                    if (vPart.Contains(" "))
                        vPart = vPart.Split(' ')[0];
                    _PostGISVersion = vPart;
                    return Convert.ToDouble(vPart, CultureInfo.InvariantCulture);
                }
            }
        }

        public List<string> GetExistingLayerInformation()
        {

            //SQL command: select * from geometry_columns

            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                var sql = string.Format("SELECT * FROM \"geometry_columns\" ");
                conn.Open();

                using (var rdr = new NpgsqlCommand(sql, conn).ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        tableList = new List<string>();
                        while (rdr.Read())
                        {
                            for (int i = 0; i < rdr.FieldCount; i++)
                            {
                                tableList.Add(rdr[i].ToString());
                            }
                        }
                        conn.Close();
                    }
                }  
            }

            return tableList;
        }

        #endregion

        #region Property

                public Boolean IsConnection
                    {
                        get 
                        {
                            return _isConnected;
                        }
                    }

                public string PostGISVersion
                    {
                        get 
                        {
                            return "PostGIS version is" + _PostGISVersion;
                        }
                    }

                public string ConnectionString
                    {
                        get
                        {
                            return _connectionString;
                        }
                        set
                        {
                            _connectionString = value;
                        }
                    }
        
        #endregion
    }
}
