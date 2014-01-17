using SharpMap.Data.Providers;
using SharpMap.Forms;
using SharpMap.Layers;
using SharpMap.Styles;
using SharpMap.Data;
using Pan.Utilities;
using Npgsql;
using System.Data;
using System.Collections.Generic;

namespace Oasis_v1._1
{
    public class PostGISHelper
    {
        #region Private Variables

            private string _connectionString;

            private VectorLayer loadLayer;

            public List<string> _metaTableList;

        #endregion

        #region Constructors

        /// <summary>
        /// Create instance of this class by using a connection string
        /// </summary>
        /// <param name="DataBaseConnectionString"></param>
        public PostGISHelper(string DataBaseConnectionString)
        {
            _connectionString = DataBaseConnectionString;
        }

        /// <summary>
        /// Create instance of this class by using a database connection
        /// </summary>
        /// <param name="dbConnection"></param>
        public PostGISHelper(DbConnection dbConnection)
        {
            _connectionString = dbConnection._connectionString;
        }

        /// <summary>
        /// Create instance of this class by using a helper itself
        /// </summary>
        /// <param name="pgHelper"></param>
        public PostGISHelper(PostGISHelper pgHelper)
        {
            _connectionString = pgHelper.DBConnectString;
        }

        #endregion

        #region Method

        /// <summary>
        /// Get meta data for data layers in Postgis database
        /// </summary>
        /// <returns></returns>
        public List<string> GetExistingLayerInformation()
        {
            //SQL command: "select * from geometry_columns"
            using (var conn = new NpgsqlConnection(DBConnectString))
            {
                var sql = string.Format("SELECT * FROM \"geometry_columns\" ");
                conn.Open();
                using (var rdr = new NpgsqlCommand(sql, conn).ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        _metaTableList = new List<string>();
                        while (rdr.Read())
                        {
                            for (int i = 0; i < rdr.FieldCount; i++)
                            {
                                _metaTableList.Add(rdr[i].ToString());
                            }
                        }
                        conn.Close();
                    }
                }
            }
            return _metaTableList;
        }

        /// <summary>
        /// Obtain all geometry elements to a table by using PostGIS indexing
        /// </summary>
        /// <param name="importDataName"></param>
        /// <returns></returns>
        public FeatureDataTable GetPostGisLayerTable(string importDataName)
        {
            using (var p1 = new PostGIS(DBConnectString, importDataName, "gid"))
            {
                FeatureDataSet fds = new FeatureDataSet();
                p1.ExecuteIntersectionQuery(p1.GetExtents(), fds);
                FeatureDataTable fdt = fds.Tables[0];
                return fdt;
            }
        }

        public static List<PostGisMetaItem> FormMetaToView(List<string> strings)
        {
            List<PostGisMetaItem> pgitem = new List<PostGisMetaItem>();

            for (int order = 1; order <=strings.Count; order++)
            {
                if (order % 7 == 0)
                {
                    PostGisMetaItem pgm = new PostGisMetaItem();
                    pgm.DBName = strings[order - 7];
                    pgm.DBSchema = strings[order - 6];
                    pgm.DBTable = strings[order - 5];
                    pgm.DBGeometry = strings[order - 4];
                    pgm.DBCoordDimen = strings[order - 3];
                    pgm.DB_SRID = strings[order - 2];
                    pgm.DBGeometryType = strings[order-1];
                    pgitem.Add(pgm);
                }
            }

            return pgitem;
        }

        public void LoadPostGisSharpLayer(string importLayerName, MapBox mapbox, DataGridViewPersistent dgvp)
        {
            if (mapbox != null && dgvp != null)
            {
                this.loadLayerName = importLayerName;
                this.loadLayer = new VectorLayer(importLayerName);
            }
            using (var p1 = new PostGIS(DBConnectString, importLayerName, "gid"))
            {
                FeatureDataSet fds = new FeatureDataSet();
                p1.ExecuteIntersectionQuery(p1.GetExtents(), fds);
                dgvp.DataSource = fds.Tables[0];
                loadLayer.DataSource = new GeometryFeatureProvider(fds.Tables[0]);
                // TODO temp
                //State.InforNodeLayer = loadLayer;
            }
            mapbox.Map.Layers.Add(loadLayer);
            mapbox.Map.ZoomToExtents();
            mapbox.Refresh();
        }

        #endregion

        #region Property

        public List<string> MetaTableList
        {
            get
            {
                return _metaTableList;
            }
        }

        public string DBConnectString
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

        public string loadLayerName { get; set; }

        #endregion
    }
}
