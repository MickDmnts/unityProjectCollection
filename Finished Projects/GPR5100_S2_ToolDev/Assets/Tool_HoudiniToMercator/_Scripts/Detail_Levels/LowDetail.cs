#if UNITY_EDITOR
namespace htm.detailLevels
{
    using System.Collections.Generic;
    using htm.detailLayers;
    using UnityEngine;

    public sealed class LowDetail : IDetailLevel
    {
        ///<summary>The SorterTileHandler parent</summary>
        SorterTileHandler handler;

        string detailAddrPath = "terrain/ld/";

        ///<summary>A dictionary containing all the detail level layers</summary>
        Dictionary<TileLayerType, IDetailLayer> detailLevelLayers;

        ///<summary>Returns the Detail Level Sorter Tile Handler</summary>
        public SorterTileHandler TileHandler() => handler;

        private LowDetail() { }

        public LowDetail(SorterTileHandler handler)
        {
            this.handler = handler;

            detailLevelLayers = CreateLevelLayers();
        }

        ///<summary>Returns an initialized dictionary containing this Detail Level layers.</summary>
        Dictionary<TileLayerType, IDetailLayer> CreateLevelLayers()
        {
            Dictionary<TileLayerType, IDetailLayer> layers = new Dictionary<TileLayerType, IDetailLayer>{
            {TileLayerType.Global, new GlobalLayer(this)},
            {TileLayerType.CustomTerrain, new CustomTerrainLayer(this, CustomDetailLevel.LowDetail)},
            {TileLayerType.CustomObjects, new CustomObjectsLayer(this, CustomDetailLevel.LowDetail)},
            };

            return layers;
        }

        ///<summary>Call to handle the passed tile based on its corresponding layer type.</summary>
        public void HandleDetailAssets(TileLayerType layer, Transform activeTile, string digitName)
        {
            if (detailLevelLayers != null)
            {
                if (detailLevelLayers.ContainsKey(layer))
                {
                    detailLevelLayers[layer].HandleTile(activeTile, digitName, handler.GetLayerPrefix(layer));
                }
            }
        }

        ///<summary>
        /// Returns true if the passed tile is suitable for this details' layer type, otherwise false.
        /// <para>Throws error if the passed layer is not supported from this detail level.</para>
        /// </summary>
        public bool IsTileSuitable(Transform activeTile, TileLayerType layer)
        {
            if (detailLevelLayers != null)
            {
                if (detailLevelLayers.ContainsKey(layer))
                {
                    return detailLevelLayers[layer].IsTileSuitable(activeTile);
                }
            }

            throw new System.ArgumentException("The passed layer is not supported from this detail level.");
        }

        #region UTILITIES
        ///<summary>Get the detail level's parent folder path.</summary>
        public string GetParentFolderPath() => handler.Sorter.ParentFolderName;

        ///<summary>Get the detail level's assets folder path.</summary>
        public string GetAssetsFolderPath() => handler.Sorter.LdAssetsFolder;

        ///<summary>Get the detail level's prefabs folder path.</summary>
        public string GetPrefabsFolderPath() => handler.Sorter.LdPrefabsFolder;

        ///<summary>Get the detail level's materials folder path.</summary>
        public string GetMaterialFolderPath() => handler.Sorter.MaterialFolderName;

        ///<summary>Returns the layer type the user has selected from the inspector.</summary>
        public TileLayerType GetTileLayerType() => handler.Sorter.TileType;

        ///<summary>The string separator set from the parent sorter.</summary>
        public string GetStrSeparator() => handler.Sorter.Str_Separator.ToString();

        public string GetAddressableGroup() => handler.Sorter.AddressableGroup;

        public string GetAddressablePath() => detailAddrPath;
        #endregion
    }
#endif
}