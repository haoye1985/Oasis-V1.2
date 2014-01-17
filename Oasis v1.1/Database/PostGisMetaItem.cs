using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Oasis_v1._1
{
    public class PostGisMetaItem : INotifyPropertyChanged
    {
        #region Fields

        private string _name;
        private string _dbName;
        private string _dbSchema;
        private string _dbTable;
        private string _dbGeometry;
        private string _dbCoordDimen;
        private string _dbSRID;
        private string _dbGeometryType;

        #endregion

        #region Constructors

        public PostGisMetaItem() {
        }

        public PostGisMetaItem(string name) {
            this._name = name;
        }

        public PostGisMetaItem(string dbname, string dbschema, string dbtable, string dbgeometry, string dbcoorddimen, string dbsrid, string dbgeometrytype)
        {
            this._dbName = dbname;
            this._dbSchema = dbschema;
            this._dbTable = dbtable;
            this._dbGeometry = dbgeometry;
            this._dbCoordDimen = dbcoorddimen;
            this._dbSRID = dbsrid;
            this._dbGeometryType = dbgeometrytype;
        }

        public PostGisMetaItem(PostGisMetaItem other)
        {
            this._dbName = other.DBName;
            this._dbSchema = other.DBSchema;
            this._dbTable = other.DBTable;
            this._dbGeometry = other.DBGeometry;
            this._dbCoordDimen = other.DBCoordDimen;
            this._dbSRID = other.DB_SRID;
            this._dbGeometryType = other.DBGeometryType;
        }

        #endregion

        #region Properties

        public string DBName
        {
            get { return _dbName; }
            set { _dbName = value; }
        }

        public string DBSchema
        {
            get { return _dbSchema; }
            set { _dbSchema = value; }
        }

        public string DBTable
        {
            get { return _dbTable; }
            set { _dbTable = value; }
        }

        public string DBGeometry
        {
            get { return _dbGeometry; }
            set { _dbGeometry = value; }
        }

        public string DBCoordDimen
        {
            get { return _dbCoordDimen; }
            set { _dbCoordDimen = value; }
        }

        public string DB_SRID
        {
            get { return _dbSRID; }
            set { _dbSRID = value; }
        }

        public string DBGeometryType
        {
            get { return _dbGeometryType; }
            set { _dbGeometryType = value; }
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
