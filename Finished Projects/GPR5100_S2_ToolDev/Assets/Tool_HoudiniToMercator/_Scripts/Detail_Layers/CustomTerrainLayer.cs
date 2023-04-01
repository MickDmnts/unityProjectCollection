#if UNITY_EDITOR
namespace htm.detailLayers
{
    using UnityEngine;

    public sealed class CustomTerrainLayer : IDetailLayer
    {
        ///<summary>The Detail level this layer corresponds to.</summary>
        IDetailLevel handler;

        ///<summary>The detail level to produce tiles for.</summary>
        CustomDetailLevel detailLevel;

        GlobalLayer globalWorker;
        SurfaceLayer surfaceWorker;
        SeabedLayer seabedWorker;

        public CustomTerrainLayer(IDetailLevel handler, CustomDetailLevel detailLevel)
        {
            this.handler = handler;
            this.detailLevel = detailLevel;

            this.globalWorker = new GlobalLayer(handler);
            this.surfaceWorker = new SurfaceLayer(handler);
            this.seabedWorker = new SeabedLayer(handler);
        }

        public void HandleTile(Transform activeTile, string digitName, string prefix)
        {
            CustomLayerFiltering filterMode = handler.TileHandler().Sorter.CustomLayerFiltering;

            if (detailLevel == CustomDetailLevel.LowDetail)
            {
                globalWorker.HandleTile(activeTile, digitName, prefix);
            }
            else if (detailLevel == CustomDetailLevel.HighDetail)
            {
                switch (detailLevel)
                {
                    case CustomDetailLevel.LowDetail:
                        globalWorker.HandleTile(activeTile, digitName, prefix);
                        break;

                    case CustomDetailLevel.HighDetail:
                        switch (filterMode)
                        {
                            case CustomLayerFiltering.Seabed:
                                {
                                    seabedWorker.HandleTile(activeTile, digitName, prefix);
                                }
                                break;

                            case CustomLayerFiltering.Surface:
                                {
                                    surfaceWorker.HandleTile(activeTile, digitName, prefix);
                                }
                                break;

                            case CustomLayerFiltering.Seabed_Surface:
                                {
                                    //The save path we need is the surface one but with another filter mode.
                                    surfaceWorker.HandleTile(activeTile, digitName, prefix);
                                }
                                break;
                        }
                        break;
                }
            }
        }

        //The below methods all return null/empty values due to the Layer
        //using other layers as its workers.
        //Although, this leaves space for full extensibility of the Layer.
        #region CUSTOM_DETAIL_HELPER_METHODS
        public bool IsTileSuitable(Transform activeTile)
        {
            CustomLayerFiltering filterMode = handler.TileHandler().Sorter.CustomLayerFiltering;
            bool result = false;

            switch (filterMode)
            {
                case CustomLayerFiltering.Seabed:
                    {
                        if (activeTile.transform.localPosition.y <= -1)
                        {
                            result = true;
                        }
                    }
                    break;

                case CustomLayerFiltering.Surface:
                    {
                        if (activeTile.transform.localPosition.y >= 0)
                        {
                            result = true;
                        }
                    }
                    break;

                case CustomLayerFiltering.Seabed_Surface:
                    {
                        result = true;
                    }
                    break;
            }

            return result;
        }
        #endregion

        #region SEABED_LOW_DETAIL
        #endregion
    }
#endif
}