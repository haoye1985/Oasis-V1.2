using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpMap.Forms;
using GeoAPI.Geometries;
using NetTopologySuite;

namespace Oasis_v1._1
{
    class ZoomExtent
    {
        private MapBox _mapbox;
        private List<Envelope> _zoomExtentStack = new List<Envelope>();
        private bool _storeExtentsUser = false;
        private bool _storeExtentsInternal = false;
        private int _index = 0;

        public ZoomExtent(MapBox mapbox)
        {
            _mapbox = mapbox;
            _mapbox.Map.MapViewOnChange += new SharpMap.Map.MapViewChangedHandler(_map_MapViewOnChange);
        }

        public void _map_MapViewOnChange()
        {
            if (_storeExtentsUser && _storeExtentsInternal)
                Add(_mapbox.Map.Envelope);
            else
                _storeExtentsInternal = true;
        }

        public bool StoreExtents
        {
            get { return _storeExtentsUser; }
            set
            {
                if (value) Add(_mapbox.Map.Envelope);
                _storeExtentsUser = value;
            }
        }

        public void Clear()
        {
            _zoomExtentStack.Clear();
            _index = 0;
        }

        public bool CanZoomPrevious
        {
            get
            {
                return (_index > 0);
            }
        }

        public bool CanZoomNext
        {
            get
            {
                return (_index < _zoomExtentStack.Count - 1);
            }
        }


        public void ZoomPrevious()
        {
            if (CanZoomPrevious)
            {
                _storeExtentsInternal = false;
                _index--;
                _mapbox.Map.ZoomToBox(_zoomExtentStack[_index]);
            }
        }

        public void ZoomNext()
        {
            if (CanZoomNext)
            {
                _storeExtentsInternal = false;
                _index++;
                _mapbox.Map.ZoomToBox(_zoomExtentStack[_index]);
            }
        }

        private void Add(Envelope newExtent)
        {
            if (_zoomExtentStack.Count - 1 > _index)
                _zoomExtentStack.RemoveRange(_index + 1, _zoomExtentStack.Count - _index - 1);

            // add given extent
            _zoomExtentStack.Add(newExtent);
            // correct index
            _index = _zoomExtentStack.Count - 1;
        }
    }
}
