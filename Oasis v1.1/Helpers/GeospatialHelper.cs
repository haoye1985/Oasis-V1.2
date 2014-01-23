using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpMap.Data;
using SharpMap.Layers;
using GeoAPI.Geometries;

namespace Oasis_v1._1
{
    public class Geospatial
    {
        public static bool CheckPointInNode(Coordinate coord, Envelope box)
        {
            bool _isChecked;

            if (box.Contains(coord))
            {
                _isChecked = true;
            }
            else
            {
                _isChecked = false;
            }
            return _isChecked;
        }



        public static FeatureDataRow FindGeoNearPoint(GeoAPI.Geometries.IPoint point, VectorLayer layer, double amountGrow)
        {
            var box = new Envelope(point.Coordinate);
            box.ExpandBy(amountGrow);

            var fds = new FeatureDataSet();
            layer.DataSource.ExecuteIntersectionQuery(box, fds);

            FeatureDataRow result = null;
            var minDistance = double.MaxValue;

            foreach (FeatureDataTable fdt in fds.Tables)
            {
                foreach (FeatureDataRow fdr in fdt.Rows)
                {
                    if (fdr.Geometry != null)
                    {
                        var distance = point.Distance(fdr.Geometry);
                        if (distance < minDistance)
                        {
                            result = fdr;
                            minDistance = distance;
                        }
                    }
                }
            }

            return result;
        }
    }
}
