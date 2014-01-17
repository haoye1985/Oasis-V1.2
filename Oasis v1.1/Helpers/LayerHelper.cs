using System;
using SharpMap.Forms;
using SharpMap.Styles;
using SharpMap.Layers;
using GeoAPI.CoordinateSystems.Transformations;
using GeoAPI.Geometries;
using BruTile.Web;

namespace Oasis_v1._1
{
    public enum TileWebObjectType
    {
        /// <summary>
        /// 
        /// </summary>
        GoogleMap,
        /// <summary>
        /// 
        /// </summary>
        OpenStreetMap,
        /// <summary>
        /// 
        /// </summary>
        BingMap
    }

    public static class LayerHelper
    {
        private static TileAsyncLayer maplayer;

        public static void LoadTileWebLayer(MapBox mapbox, TileWebObjectType tileType)
        {

            if (mapbox != null)
            {
                try
                {
                    switch (tileType)
                    {
                        case TileWebObjectType.GoogleMap:
                            maplayer = new TileAsyncLayer(new GoogleTileSource(GoogleMapType.GoogleMap), "TileLayer - Google");
                            break;

                        case TileWebObjectType.OpenStreetMap:
                            maplayer = new TileAsyncLayer(new OsmTileSource(), "TileLayer - OSM");
                            break;

                        case TileWebObjectType.BingMap:
                            maplayer = new TileAsyncLayer(new BingTileSource(BingRequest.UrlBing, "", BingMapType.Roads), "TileLayer - Bing");
                            break;

                        default:
                            break;
                    }

                    mapbox.Map.BackgroundLayer.Add(maplayer);
                    IMathTransform mathTransform = ProjectionHelper.Wgs84toGoogleMercator.MathTransform;
                    Envelope geom = GeometryTransform.TransformBox(new Envelope(-9.205626, -9.123736, 38.690993, 38.740837), mathTransform);
                    mapbox.Map.ZoomToBox(geom);
                    mapbox.Map.Zoom = 20000;
                    mapbox.Refresh();
                }
                catch
                {
                    throw new Exception("The mapbox doesnot exist!");
                }
 
            }
        }



    }
}
