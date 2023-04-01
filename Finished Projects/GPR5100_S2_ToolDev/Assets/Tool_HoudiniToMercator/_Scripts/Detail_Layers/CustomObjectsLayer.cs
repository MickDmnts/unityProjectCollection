#if UNITY_EDITOR
namespace htm.detailLayers
{

    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public sealed class CustomObjectsLayer : IDetailLayer
    {
        ///<summary>The Detail level this layer corresponds to.</summary>
        IDetailLevel handler;
        CustomDetailLevel detailLevel;

        GlobalLayer globalWorker;
        ObjectsLayer highDetailWorker;

        public CustomObjectsLayer(IDetailLevel handler, CustomDetailLevel detailLevel)
        {
            this.handler = handler;
            this.detailLevel = detailLevel;

            this.globalWorker = new GlobalLayer(handler);
            this.highDetailWorker = new ObjectsLayer(handler);
        }

        public void HandleTile(Transform activeTile, string digitName, string prefix)
        {
            CustomLayerFiltering filterMode = handler.TileHandler().Sorter.CustomLayerFiltering;

            if (detailLevel == CustomDetailLevel.LowDetail)
            {
                activeTile.name = digitName;
                string tileAssetName = prefix + handler.GetStrSeparator() + activeTile.name;

                //This ensures that the generated tiles will have a destination file to get saved to.
                string parentFolderName = SorterUtils.GenerateCordsRelativeFolderName(activeTile, '_', 2);

                string prefabsPath = Path.GetFullPath(Path.Combine(handler.GetPrefabsFolderPath(), parentFolderName));
                if (!Directory.Exists(prefabsPath))
                { Directory.CreateDirectory(prefabsPath); }

                GameObject createdPrefab = globalWorker.CreatePrefab(activeTile, tileAssetName, parentFolderName);

                if (createdPrefab == null)
                { throw new System.NullReferenceException("Prefab creation returned null."); }

                SorterUtils.AddScriptsToPrefab(handler.TileHandler().Sorter.ScriptsToAdd, createdPrefab.transform);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                //Create the prefab addressable
                SorterUtils.SetAddressableGroup(createdPrefab, handler.GetAddressableGroup(), handler.GetAddressablePath() + createdPrefab.name);
            }
            else if (detailLevel == CustomDetailLevel.HighDetail)
            {
                highDetailWorker.HandleTile(activeTile, digitName, prefix);
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
    }
#endif
}