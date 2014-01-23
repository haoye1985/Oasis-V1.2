using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;
using Common.Logging;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System.Globalization;
using Npgsql;
using SharpMap.Data;
using SharpMap.Data.Providers;

namespace Oasis_v1._1
{
    public enum SpatialObjectType
    {
        /// <summary>
        /// Spatial object stored in cartesian coordinate system, that is defined by a spatial reference system id.
        /// <para>This is the standard way</para>
        /// </summary>
        Geometry,


        /// <summary>
        /// Spatial object stored in WGS84, calculations performed on the sphere.
        /// </summary>
        /// <see href="http://workshops.opengeo.org/postgis-intro/geography.html"/>
        Geography
    }

    public class PostGIS : BaseProvider
    {
        #region Fields

        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        private string _definitionQuery;
        private string _geometryColumn;
        private string _objectIdColumn;
        private string _schema = "public";
        private string _table;

        private readonly double _postGisVersion;
        private readonly bool _supportSTIntersects;
        private readonly bool _supportSTMakeBox2d;
        private readonly bool _supportSTMakeEnvelope;
        private readonly string _prefixFunction = "";
        private readonly SpatialObjectType _postGisSpatialObject;
        private string _columns;
        private Envelope _cachedExtents;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new connection to PostGIS
        /// </summary>
        /// <param name="connectionString">Connectionstring</param>
        /// <param name="tablename">Name of data table</param>
        /// <param name="geometryColumnName">Name of geometry column</param>
        /// <param name="objectIdColumnName">Name of column with unique identifier</param>
        public PostGIS(string connectionString, string tablename, string geometryColumnName, string objectIdColumnName)
        {
            ConnectionString = connectionString;
            Table = tablename;
            GeometryColumn = geometryColumnName;
            if (!string.IsNullOrEmpty(geometryColumnName))
            {
                _postGisSpatialObject = GetSpatialObjectType();
                SRID = GetGeometrySrid();
            }
            ObjectIdColumn = objectIdColumnName;
            _postGisVersion = GetPostGISVersion();
            _supportSTIntersects = _postGisVersion >= 1.3;
            _supportSTMakeBox2d = _postGisVersion >= 1.4;
            _supportSTMakeEnvelope = _postGisVersion >= 2.0;
            if (_postGisVersion >= 1.5)
                _prefixFunction = "ST_";
        }

        /// <summary>
        /// Initializes a new connection to PostGIS
        /// </summary>
        /// <param name="connectionString">Connectionstring</param>
        /// <param name="tablename">Name of data table</param>
        /// <param name="objectIdColumnName">Name of column with unique identifier</param>
        public PostGIS(string connectionString, string tablename, string objectIdColumnName)
            : this(connectionString, tablename, "", objectIdColumnName)
        {
            int srid;
            GeometryColumn = GetGeometryColumn(out srid);
            SRID = srid;

            if (!string.IsNullOrEmpty(GeometryColumn))
            {
                _postGisSpatialObject = GetSpatialObjectType();
                SRID = GetGeometrySrid();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the extent for this data source
        /// </summary>
        public Envelope CachedExtent
        {
            get
            {
                return _cachedExtents ?? (_cachedExtents = GetExtents());
            }
            set
            {
                _cachedExtents = value;
            }
        }

        /// <summary>
        /// Connectionstring
        /// </summary>
        public string ConnectionString
        {
            get { return ConnectionID; }
            set { ConnectionID = value; }
        }

        /// <summary>
        /// Data table name
        /// </summary>
        public string Table
        {
            get { return _table; }
            set
            {
                _table = value;
                QualifyTable();
            }
        }

        /// <summary>
        /// Schema Name
        /// </summary>
        public string Schema
        {
            get { return _schema; }
            set { _schema = value; }
        }

        /// <summary>
        /// Qualified Table Name
        /// </summary>
        public string QualifiedTable
        {
            get { return string.Format("\"{0}\".\"{1}\"", _schema, _table); }
            set { _table = value; }
        }

        /// <summary>
        /// Name of geometry column
        /// </summary>
        public string GeometryColumn
        {
            get { return _geometryColumn; }
            set { _geometryColumn = value; }
        }

        /// <summary>
        /// Name of column that contains the Object ID
        /// </summary>
        public string ObjectIdColumn
        {
            get { return _objectIdColumn; }
            set { _objectIdColumn = value; }
        }

        /// <summary>
        /// Definition query used for limiting dataset
        /// </summary>
        public string DefinitionQuery
        {
            get { return _definitionQuery; }
            set { _definitionQuery = value; }
        }

        #endregion

        #region IProvider Members

        /// <summary>
        /// Returns geometries within the specified bounding box
        /// </summary>
        /// <param name="bbox"></param>
        /// <returns></returns>
        public override Collection<IGeometry> GetGeometriesInView(Envelope bbox)
        {
            var features = new Collection<IGeometry>();
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                var strSQL = "SELECT \"" + GeometryColumn + "\"" + _geometryCast + "::bytea AS \"_smGeom_\" ";
                strSQL += "FROM " + QualifiedTable + " WHERE ";

                if (!String.IsNullOrEmpty(_definitionQuery))
                    strSQL += _definitionQuery + " AND ";

                if (_supportSTIntersects)
                {
                    strSQL += "st_intersects(\"" + GeometryColumn + "\"," + GetBoxClause(bbox) + ")";
                }
                else
                {
                    strSQL += "\"" + GeometryColumn + "\" && " + GetBoxClause(bbox);
                }

                if (Logger.IsDebugEnabled)
                    Logger.Debug(string.Format("{0}\n{1}", "GetGeometriesInView: executing sql:", strSQL));

                using (var command = new NpgsqlCommand(strSQL, conn))
                {
                    using (var dr = command.ExecuteReader())
                    {

                        var reader = new PostGisReader(Factory);
                        while (dr.Read())
                        {
                            if (dr.IsDBNull(0))
                                continue;

                            var geom = reader.Read((byte[])dr.GetValue(0));
                            if (geom != null)
                                features.Add(geom);
                        }
                    }
                }
            }
            return features;
        }

        /// <summary>
        /// Returns the geometry corresponding to the Object ID
        /// </summary>
        /// <param name="oid">Object ID</param>
        /// <returns>The geometry</returns>
        public override IGeometry GetGeometryByID(uint oid)
        {
            IGeometry geom = null;
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                var strSQL = "SELECT \"" + GeometryColumn + "\"" + _geometryCast + "::bytea AS \"_smGeom_\" FROM " +
                             QualifiedTable +
                             " WHERE \"" + ObjectIdColumn + "\"=" + oid + ";";

                if (Logger.IsDebugEnabled)
                    Logger.Debug(string.Format("{0}\n{1}", "GetGeometryByID: executing sql:", strSQL));

                using (var command = new NpgsqlCommand(strSQL, conn))
                {
                    using (var dr = command.ExecuteReader())
                    {
                        var reader = new PostGisReader(Factory);
                        while (dr.Read())
                        {
                            if (!dr.IsDBNull(0))
                                geom = reader.Read((byte[])dr[0]);
                        }
                    }
                }
            }
            return geom;
        }

        /// <summary>
        /// Returns geometry Object IDs whose bounding box intersects 'bbox'
        /// </summary>
        /// <param name="bbox">The view envelope</param>
        /// <returns>A collection of ids</returns>
        public override Collection<uint> GetObjectIDsInView(Envelope bbox)
        {
            var objectlist = new Collection<uint>();
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                var strSQL = "SELECT \"" + ObjectIdColumn + "\" ";
                strSQL += "FROM " + QualifiedTable + " WHERE ";

                if (!String.IsNullOrEmpty(_definitionQuery))
                    strSQL += DefinitionQuery + " AND ";

                if (_supportSTIntersects)
                    strSQL += "ST_Intersects(\"" + GeometryColumn + "\"," + GetBoxClause(bbox) + ")";
                else
                    strSQL += "\"" + GeometryColumn + "\" && " + GetBoxClause(bbox);

                if (Logger.IsDebugEnabled)
                    Logger.Debug(string.Format("{0}\n{1}", "GetObjectIDsInView: executing sql:", strSQL));

                var cmd = new NpgsqlCommand(strSQL, conn);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr[0] == DBNull.Value)
                            continue;
                        var id = (uint)(int)dr[0];
                        objectlist.Add(id);
                    }
                }
            }
            return objectlist;
        }

        private FeatureDataTable CreateTableFromReader(DbDataReader reader, int geomIndex)
        {
            var res = new FeatureDataTable { TableName = Table };
            for (var c = 0; c < geomIndex; c++)
            {
                var fieldType = reader.GetFieldType(c);
                if (fieldType == null)
                    throw new Exception("Unable to retrieve field type for column " + c);
                res.Columns.Add(reader.GetName(c), fieldType);
            }
            return res;
        }

        /// <summary>
        /// Returns the features that intersects with 'geom'
        /// </summary>
        /// <param name="geom"></param>
        /// <param name="ds">FeatureDataSet to fill data into</param>
        protected override void OnExecuteIntersectionQuery(IGeometry geom, FeatureDataSet ds)
        {
            GetNonSpatialColumns();

            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                var strSql = "SELECT " + _columns + ", \"" + GeometryColumn + "\" AS \"_smtmp_\" ";
                strSql += "FROM " + Table + " WHERE ";

                // Attribute constraint
                if (!String.IsNullOrEmpty(_definitionQuery))
                    strSql += DefinitionQuery + " AND ";

                // Spatial constraint
                if (_supportSTIntersects)
                {
                    strSql += "ST_Intersects(@PGeo ,\"" + GeometryColumn + "\")";
                }
                else
                {
                    strSql += "\"" + GeometryColumn + "\" && @PGeo AND " +
                              _prefixFunction + "distance(\"" + GeometryColumn + "\", @PGeo)<0";
                }

                /*
                string strGeom = _prefixFunction + "GeomFromText('" + geom.AsText() + "')";
                if (SRID > 0)
                    strGeom = _prefixFunction + "setSRID(" + strGeom + "," + SRID + ")";

                string strSQL = "SELECT * , " + _prefixFunction + "AsBinary(\"" + GeometryColumn + "\") As sharpmap_tempgeometry FROM " +
                                QualifiedTable + " WHERE ";

                if (!String.IsNullOrEmpty(_definitionQuery))
                    strSQL += DefinitionQuery + " AND ";

                strSQL += "\"" + GeometryColumn + "\" && " + strGeom + " AND " + _prefixFunction + "distance(\"" + GeometryColumn + "\", " +
                          strGeom + ")<0";

                 */
                if (Logger.IsDebugEnabled)
                    Logger.Debug(string.Format("{0}\n{1}", "OnExecuteIntersectionQuery: executing sql:", strSql));

                using (var cmd = new NpgsqlCommand(strSql, conn))
                {
                    geom.SRID = SRID;
                    cmd.Parameters.AddWithValue("PGeo", new PostGisWriter().Write(geom));
                    using (var reader = cmd.ExecuteReader())
                    {
                        var geomIndex = reader.FieldCount - 1;
                        var fdt = CreateTableFromReader(reader, geomIndex);

                        var dataTransfer = new object[geomIndex];
                        var geoReader = new PostGisReader(Factory.CoordinateSequenceFactory, Factory.PrecisionModel);

                        fdt.BeginLoadData();
                        while (reader.Read())
                        {
                            IGeometry g = null;
                            if (!reader.IsDBNull(geomIndex))
                                g = geoReader.Read((byte[])reader.GetValue(geomIndex));

                            //No geometry, no feature!
                            if (g == null)
                                continue;

                            //Get all the attribute data
                            var count = reader.GetValues(dataTransfer);
                            System.Diagnostics.Debug.Assert(count == dataTransfer.Length);

                            var fdr = (FeatureDataRow)fdt.LoadDataRow(dataTransfer, true);
                            fdr.Geometry = g;
                        }
                        fdt.EndLoadData();
                        ds.Tables.Add(fdt);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the number of features in the dataset
        /// </summary>
        /// <returns>number of features</returns>
        public override int GetFeatureCount()
        {
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                var strSQL = "SELECT COUNT(*) FROM " + QualifiedTable;
                if (!String.IsNullOrEmpty(_definitionQuery))
                    strSQL += " WHERE " + DefinitionQuery;
                using (var command = new NpgsqlCommand(strSQL, conn))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Returns a datarow based on a RowID
        /// </summary>
        /// <param name="rowId"></param>
        /// <returns>datarow</returns>
        public override FeatureDataRow GetFeature(uint rowId)
        {
            GetNonSpatialColumns();
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                var strSql = "SELECT " + _columns + ", \"" + GeometryColumn + "\"" + _geometryCast +
                             "::bytea AS \"_smtmp_\" ";
                strSql += "FROM " + QualifiedTable + " WHERE \"" + ObjectIdColumn + "\" = " + rowId;

                using (var cmd = new NpgsqlCommand(strSql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            var geomIndex = reader.FieldCount - 1;
                            var fdt = CreateTableFromReader(reader, geomIndex);

                            var dataTransfer = new object[geomIndex];
                            var geoReader = new PostGisReader(Factory.CoordinateSequenceFactory, Factory.PrecisionModel);
                            fdt.BeginLoadData();
                            while (reader.Read())
                            {
                                IGeometry g = null;
                                if (!reader.IsDBNull(geomIndex))
                                    g = geoReader.Read((byte[])reader.GetValue(geomIndex));

                                //No geometry, no feature!
                                if (g == null)
                                    continue;

                                //Get all the attribute data
                                var count = reader.GetValues(dataTransfer);
                                System.Diagnostics.Debug.Assert(count == dataTransfer.Length);

                                var fdr = (FeatureDataRow)fdt.LoadDataRow(dataTransfer, true);
                                fdr.Geometry = g;
                            }
                            fdt.EndLoadData();
                            return (FeatureDataRow)fdt.Rows[1];

                        }
                        return null;
                    }
                }
            }
        }


        /// <summary>
        /// Boundingbox of dataset
        /// </summary>
        /// <returns>boundingbox</returns>
        public override Envelope GetExtents()
        {
            if (_cachedExtents != null)
                return new Envelope(_cachedExtents);

            // PostGIS geography are *always* stored in WGS84
            if (_postGisSpatialObject == SpatialObjectType.Geography)
            {
                _cachedExtents = new Envelope(-180, 180, -90, 90);
                return new Envelope(_cachedExtents);
            }

            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                var strSQL = "SELECT " + _prefixFunction + "extent(\"" + GeometryColumn + "\") FROM " + QualifiedTable;
                if (!String.IsNullOrEmpty(_definitionQuery))
                    strSQL += " WHERE " + _definitionQuery;

                using (var command = new NpgsqlCommand(strSQL, conn))
                {
                    var result = command.ExecuteScalar();
                    if (result == DBNull.Value)
                        return new Envelope();

                    var strBox = (string)result;
                    if (strBox.StartsWith("BOX("))
                    {
                        var vals =
                            strBox.Substring(4, strBox.IndexOf(")", StringComparison.InvariantCultureIgnoreCase) - 4)
                                .Split(new[] { ',', ' ' });
                        return new Envelope(
                            double.Parse(vals[0], NumberFormatInfo.InvariantInfo),
                            double.Parse(vals[2], NumberFormatInfo.InvariantInfo),
                            double.Parse(vals[1], NumberFormatInfo.InvariantInfo),
                            double.Parse(vals[3], NumberFormatInfo.InvariantInfo));
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns all features with the view box
        /// </summary>
        /// <param name="bbox">view box</param>
        /// <param name="ds">FeatureDataSet to fill data into</param>
        public override void ExecuteIntersectionQuery(Envelope bbox, FeatureDataSet ds)
        {
            GetNonSpatialColumns();
            //List<Geometry> features = new List<Geometry>();
            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                var strSql = "SELECT " + _columns + ", \"" + GeometryColumn + "\"" + _geometryCast +
                             "::bytea AS \"_smtmp_\" ";
                strSql += "FROM " + QualifiedTable + " WHERE ";

                // Attribute constraint
                if (!String.IsNullOrEmpty(_definitionQuery))
                    strSql += DefinitionQuery + " AND ";

                // Spatial constraint
                if (_supportSTIntersects)
                {
                    strSql += "ST_Intersects(\"" + GeometryColumn + "\"," + GetBoxClause(bbox) + ")";
                }
                else
                {
                    strSql += "\"" + GeometryColumn + "\" && " + GetBoxClause(bbox);
                }

                if (Logger.IsDebugEnabled)
                    Logger.Debug(string.Format("{0}\n{1}\n", "ExecuteIntersectionQuery: executing sql:", strSql));

                using (var cmd = new NpgsqlCommand(strSql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        var geomIndex = reader.FieldCount - 1;
                        var fdt = CreateTableFromReader(reader, geomIndex);

                        var dataTransfer = new object[geomIndex];
                        var geoReader = new PostGisReader(Factory.CoordinateSequenceFactory, Factory.PrecisionModel);

                        fdt.BeginLoadData();
                        while (reader.Read())
                        {
                            IGeometry g = null;
                            if (!reader.IsDBNull(geomIndex))
                                g = geoReader.Read((byte[])reader.GetValue(geomIndex));

                            //No geometry, no feature!
                            if (g == null)
                                continue;

                            //Get all the attribute data
                            var count = reader.GetValues(dataTransfer);
                            System.Diagnostics.Debug.Assert(count == dataTransfer.Length);

                            var fdr = (FeatureDataRow)fdt.LoadDataRow(dataTransfer, true);
                            fdr.Geometry = g;
                        }
                        fdt.EndLoadData();
                        ds.Tables.Add(fdt);
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the non-spatial columns
        /// </summary>
        private void GetNonSpatialColumns()
        {
            if (!string.IsNullOrEmpty(_columns))
                return;

            if (string.IsNullOrEmpty(ConnectionID))
                return;

            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var dr = new NpgsqlCommand(string.Format(
                    "SELECT \"column_name\" FROM \"information_schema\".\"columns\" " +
                    "WHERE \"table_schema\"='{0}' AND \"table_name\"='{1}';", Schema, Table), conn).ExecuteReader())
                {
                    if (!dr.HasRows)
                        throw new InvalidOperationException("Provider configuration incomplete or wrong!");

                    var columns = new List<string> { "\"" + ObjectIdColumn + "\"" };
                    while (dr.Read())
                    {
                        var column = dr.GetString(0);
                        if (string.Equals(column, ObjectIdColumn)) continue;
                        if (string.Equals(column, GeometryColumn)) continue;
                        columns.Add(string.Format("\"{0}\"", column));
                    }

                    _columns = string.Join(", ", columns);
                }
            }
        }

        /// <summary>
        /// Create an intersection area for geospatial selection
        /// </summary>
        /// <param name="bbox"></param>
        /// <returns></returns>
        private string GetBoxClause(Envelope bbox)
        {
            if (Logger.IsDebugEnabled)
            {
                var geom = Factory.ToGeometry(bbox);
                Logger.Debug(string.Format("PEnv: SRID={1};{0}", geom.AsText(), Factory.SRID));
            }

            if (_supportSTMakeEnvelope)
            {
                return string.Format(NumberFormatInfo.InvariantInfo, "ST_MakeEnvelope({0},{1},{2},{3},{4})",
                    bbox.MinX, bbox.MinY, bbox.MaxX, bbox.MaxY, Factory.SRID);
            }
            else if (_supportSTMakeBox2d)
            {
                return string.Format(NumberFormatInfo.InvariantInfo,
                    "ST_SetSRID(ST_MakeBox2D(ST_Point({0},{1}),ST_Point({2},{3})),{4})",
                    bbox.MinX, bbox.MinY, bbox.MaxX, bbox.MaxY, Factory.SRID);
            }
            else
            {


                var res = string.Format(NumberFormatInfo.InvariantInfo,
                    "box3d('BOX3D({0} {1} 0, {2} {3} 0)')",
                    bbox.MinX, bbox.MinY, bbox.MaxX, bbox.MaxY);

                if (SRID > 0)
                {
                    res = string.Format(NumberFormatInfo.InvariantInfo, "{0}setsrid({1}, {2})", _prefixFunction, res,
                        SRID);
                }
                return res;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void QualifyTable()
        {
            var dotPos = _table.IndexOf(".", StringComparison.InvariantCultureIgnoreCase);
            if (dotPos == -1)
            {
                _schema = "public";
            }
            else
            {
                _schema = _table.Substring(0, dotPos);
                _schema = _schema.Replace('"', ' ').Trim();
            }
            _table = _table.Substring(dotPos + 1);
            _table = _table.Replace('"', ' ').Trim();
        }

        /// <summary>
        /// Queries the PostGIS database to get the name of the Geometry Column. This is used if the columnname isn't specified in the constructor
        /// </summary>
        /// <remarks></remarks>
        /// <returns>Name of column containing geometry</returns>
        private string GetGeometryColumn(out int srid)
        {
            string strSQL =
                "SELECT \"f_geometry_column\", \"srid\" from public.\"geometry_columns\" WHERE \"f_table_schema\"='" +
                _schema +
                "' and \"f_table_name\"='" + _table + "'";

            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand(strSQL, conn))
                {
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr != null && dr.Read())
                        {
                            srid = dr.GetInt32(1);
                            return dr.GetString(0);
                        }
                    }
                }

                if (_postGisVersion >= 1.5)
                {
                    try
                    {
                        //Check if this is a geography column
                        strSQL =
                            "SELECT \"f_geography_column\", \"srid\" from public.\"geography_columns\" WHERE \"f_table_schema\"='" +
                            _schema +
                            "' and \"f_table_name\"='" + _table + "'";

                        using (var command = new NpgsqlCommand(strSQL, conn))
                        {
                            using (var dr = command.ExecuteReader())
                            {
                                if (dr != null && dr.Read())
                                {
                                    srid = dr.GetInt32(1);
                                    return dr.GetString(0);
                                }
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.ToString());
                    }
                }
                throw new ApplicationException("Table '" + Table + "' does not contain a geometry column");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _geometryCast = string.Empty;

        /// <summary>
        /// Queries the data t
        /// </summary>
        /// <returns></returns>
        private SpatialObjectType GetSpatialObjectType()
        {
            var sql = string.Format("SELECT \"udt_name\" from \"information_schema\".\"columns\" " +
                                    "WHERE \"table_schema\"='{0}' AND \"table_name\"='{1}' AND \"column_name\"='{2}';",
                _schema, _table, _geometryColumn);

            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var rdr = new NpgsqlCommand(sql, conn).ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        switch (rdr.GetString(0))
                        {
                            case "geometry":
                                return SpatialObjectType.Geometry;
                            case "geography":
                                _geometryCast = "::geometry";
                                return SpatialObjectType.Geography;
                            default:
                                throw new ArgumentException(
                                    "Provided geometry/geography column name does not yield geometry/geography data");
                        }
                    }
                }
                throw new NotSupportedException(
                    "Could not find geometry column within tables, need to check view definition");
            }
        }

        /// <summary>
        /// Queries the PostGIS database to get the srid of the Geometry Column. This is used if the columnname isn't specified in the constructor
        /// </summary>
        /// <remarks></remarks>
        /// <returns>Name of column containing geometry</returns>
        private int GetGeometrySrid()
        {
            if (_postGisSpatialObject == SpatialObjectType.Geography)
                return 4326;

            var strSQL = "SELECT \"srid\" FROM public.\"geometry_columns\" WHERE \"f_table_schema\"='" + _schema +
                            "' and \"f_table_name\"='" + _table + "' AND \"f_geometry_column\"='" + GeometryColumn + "';";

            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand(strSQL, conn))
                {
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr != null && dr.Read())
                            return dr.GetInt32(0);
                    }
                }
            }
            throw new ApplicationException("Table '" + Table + "' does not contain a geometry column named '" + GeometryColumn + "')");
        }

        /// <summary>
        /// Reads the postgis version installed on the server
        /// </summary>
        /// <returns></returns>
        private double GetPostGISVersion()
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
                    return Convert.ToDouble(vPart, CultureInfo.InvariantCulture);
                }
            }
        }

        #endregion

        #region Disposers and finalizers

        #endregion

        #region New Added Methods

        /// <summary>
        /// Select a column and return it values for display
        /// </summary>
        /// <param name="enquireColumn"></param>
        /// <param name="bbox"></param>
        /// <param name="fds"></param>
        public void GetColumnInView(string enquireColumn, Envelope bbox, FeatureDataSet fds)
        {
            GetNonSpatialColumns();

            var strList = new Collection<string>();

            using (var conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                var strSQL = "SELECT " + enquireColumn + ", \"" + GeometryColumn + "\"" + _geometryCast +
                             "::bytea AS \"_smtmp_\" ";
                strSQL += "FROM " + QualifiedTable + " WHERE ";

                if (!String.IsNullOrEmpty(_definitionQuery))
                    strSQL += DefinitionQuery + " AND ";

                if (_supportSTIntersects)
                    strSQL += "ST_Intersects(\"" + GeometryColumn + "\"," + GetBoxClause(bbox) + ")";
                else
                {
                    strSQL += "\"" + GeometryColumn + "\" && " + GetBoxClause(bbox);
                }

                using (var cmd = new NpgsqlCommand(strSQL, conn))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        var geomIndex = dr.FieldCount - 1;

                        var fdt = CreateTableFromReader(dr, geomIndex);

                        var dataTransfer = new object[geomIndex];

                        var geoReader = new PostGisReader(Factory.CoordinateSequenceFactory, Factory.PrecisionModel);

                        fdt.BeginLoadData();
                        while (dr.Read())
                        {
                            IGeometry g = null;
                            if (!dr.IsDBNull(geomIndex))
                                g = geoReader.Read((byte[])dr.GetValue(geomIndex));

                            //No geometry, no feature!
                            if (g == null)
                                continue;

                            //Get all the attribute data
                            var count = dr.GetValues(dataTransfer);
                            System.Diagnostics.Debug.Assert(count == dataTransfer.Length);

                            var fdr = (FeatureDataRow)fdt.LoadDataRow(dataTransfer, true);
                            fdr.Geometry = g;
                        }
                        fdt.EndLoadData();

                        fds.Tables.Add(fdt);
                    }
                }
            }
        }


        #endregion

    }
}
