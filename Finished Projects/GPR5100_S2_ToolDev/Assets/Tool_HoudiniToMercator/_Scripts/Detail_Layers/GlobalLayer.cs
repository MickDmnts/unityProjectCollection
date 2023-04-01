#if UNITY_EDITOR
namespace htm.detailLayers
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public sealed class GlobalLayer : IDetailLayer
    {
        IDetailLevel handler;

        public GlobalLayer(IDetailLevel handler)
        {
            this.handler = handler;
        }

        public void HandleTile(Transform activeTile, string digitName, string prefix)
        {
            activeTile.name = digitName;
            string tileAssetName = prefix + handler.GetStrSeparator() + activeTile.name;

            //This ensures that the generated tiles will have a destination file to get saved to.
            string parentFolderName = SorterUtils.GenerateCordsRelativeFolderName(activeTile, '_', 2);
            CreateNeededFolders(parentFolderName);

            //Create a tile copy
            GameObject createdPrefab = CreatePrefab(activeTile, tileAssetName, parentFolderName);

            if (createdPrefab == null)
            { throw new System.NullReferenceException("Prefab creation returned null."); }

            SorterUtils.AddScriptsToPrefab(handler.TileHandler().Sorter.ScriptsToAdd, createdPrefab.transform);

            //Asset copying
            Terrain tileTerrain = createdPrefab.GetComponent<Terrain>();
            TerrainCollider terrainCol = createdPrefab.GetComponent<TerrainCollider>();

            string strDataSource = AssetDatabase.GetAssetPath(tileTerrain.terrainData);
            string strMaterialSource = AssetDatabase.GetAssetPath(tileTerrain.materialTemplate);

            string strDataDest = GetDetailAssetPath(tileAssetName, parentFolderName);
            string strMaterialDest = GetDetailMaterialPath(tileAssetName, parentFolderName);

            //Copy the assets at the passed folders
            tileTerrain.materialTemplate = SorterUtils.CopyMaterial(strMaterialSource, strMaterialDest);

            tileTerrain.terrainData = SorterUtils.CopyTerrainData(strDataSource, strDataDest);
            terrainCol.terrainData = tileTerrain.terrainData;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            //Create the prefab addressable
            SorterUtils.SetAddressableGroup(createdPrefab, handler.GetAddressableGroup(), handler.GetAddressablePath() + createdPrefab.name);
        }

        void CreateNeededFolders(string parentFolderName)
        {
            string assetPath = Path.GetFullPath(Path.Combine(handler.GetAssetsFolderPath(), parentFolderName));
            if (!Directory.Exists(assetPath))
            { Directory.CreateDirectory(assetPath); }

            string prefabsPath = Path.GetFullPath(Path.Combine(handler.GetPrefabsFolderPath(), parentFolderName));
            if (!Directory.Exists(prefabsPath))
            { Directory.CreateDirectory(prefabsPath); }

            string materialsPath = Path.GetFullPath(Path.Combine(handler.GetAssetsFolderPath(), parentFolderName, handler.GetMaterialFolderPath()));
            if (!Directory.Exists(materialsPath))
            { Directory.CreateDirectory(materialsPath); }
        }

        #region GLOBAL_LAYER_HELPER_METHODS
        public bool IsTileSuitable(Transform activeTile)
        {
            return true;
        }

        public string GetDetailPrefabPath(string tileAssetName, string newParentFolder)
        {
            return Path.GetFullPath(Path.Combine(handler.GetPrefabsFolderPath(), newParentFolder, tileAssetName + ".prefab"));
        }

        public string GetDetailAssetPath(string tileAssetName, string newParentFolder)
        {
            return Path.GetFullPath(Path.Combine(handler.GetAssetsFolderPath(), newParentFolder, tileAssetName + ".asset"));
        }

        public string GetDetailMaterialPath(string tileAssetName, string newParentFolder)
        {
            return Path.GetFullPath(Path.Combine(handler.GetAssetsFolderPath(), newParentFolder, handler.GetMaterialFolderPath(), tileAssetName + ".mat"));
        }

        public GameObject CreatePrefab(Transform activeTile, string tileAssetName, string parentFolderName)
        {
            string strPrefabDest = GetDetailPrefabPath(tileAssetName, parentFolderName);

            //This preserves the original objects from being tied together
            GameObject copy = GameObject.Instantiate(activeTile.gameObject, activeTile.position, activeTile.rotation);
            GameObject created = PrefabUtility.SaveAsPrefabAsset(copy, SorterUtils.ConvertToAssetRelative(strPrefabDest));
            GameObject.DestroyImmediate(copy);

            return created;
        }
        #endregion
    }
#endif
}