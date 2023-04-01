#if UNITY_EDITOR
namespace htm.detailLayers
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public sealed class SurfaceLayer : IDetailLayer
    {
        ///<summary>The Detail level this layer corresponds to.</summary>
        IDetailLevel handler;

        ///<summary>The name of the temporary folder storing the tiles before getting moved to the external folder.</summary>
        string tempFolderName = "TileTemp";

        public SurfaceLayer(IDetailLevel handler)
        {
            this.handler = handler;
        }

        public void HandleTile(Transform activeTile, string digitName, string prefix)
        {
            activeTile.name = digitName;
            string tileAssetName = prefix + handler.GetStrSeparator() + activeTile.name;

            //This ensures that the generated tiles will have a destination file to get saved to.
            string twoDigitParentFolderName = SorterUtils.GenerateCordsRelativeFolderName(activeTile, '_', 2);
            string threeDigitParentFolderName = SorterUtils.GenerateCordsRelativeFolderName(activeTile, '_', 3);
            CreateDirsIfNotExists(activeTile.name, twoDigitParentFolderName, threeDigitParentFolderName);

            //Create the prefab from the passed tile
            GameObject createdPrefab = CreatePrefab(activeTile, tileAssetName);

            if (createdPrefab == null)
            { throw new System.NullReferenceException("Prefab creation returned null."); }

            SorterUtils.AddScriptsToPrefab(handler.TileHandler().Sorter.ScriptsToAdd, createdPrefab.transform);

            #region ASSET_CREATION_AND_MOVING
            //Cache the created prefabs' components so the original ones do not get changed/damaged. 
            Terrain tileTerrain = createdPrefab.GetComponent<Terrain>();
            TerrainCollider terrainCol = createdPrefab.GetComponent<TerrainCollider>();

            //cache the original asset paths to create copies from them.
            string strMaterialSource = AssetDatabase.GetAssetPath(tileTerrain.materialTemplate);
            string strDataSource = AssetDatabase.GetAssetPath(tileTerrain.terrainData);

            //create Copy of the material
            Material matAtTempFolder = CreateMaterialCopy(ref tileTerrain, tileAssetName, strMaterialSource);

            //create Copy of the terrain data
            TerrainData tdAtTempFolder = CreateTerrainDataCopy(ref tileTerrain, ref terrainCol, tileAssetName, strDataSource);

            //Assign the addressable before moving the assets, Unity wont't allow address assignetation outside of an Assets folder.
            SorterUtils.SetAddressableGroup(createdPrefab, handler.GetAddressableGroup(), handler.GetAddressablePath() + createdPrefab.name);

            //Refresh after creating the initial prefab, materials and terrrain data so they can be retrieved and moved.
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            //Move the assets to the packages folders
            MovePrefab(createdPrefab, twoDigitParentFolderName, threeDigitParentFolderName);
            MoveMaterial(matAtTempFolder, twoDigitParentFolderName, threeDigitParentFolderName);
            MoveTerrainData(tdAtTempFolder, twoDigitParentFolderName, threeDigitParentFolderName);
            #endregion
        }

        #region ASSET_COPYING_METHODS
        #region PREFAB_COPY_MOVING
        public GameObject CreatePrefab(Transform activeTile, string tileAssetName)
        {
            string tempDir = Path.Combine(Application.dataPath, tempFolderName, tileAssetName + ".prefab");

            //This preserves the original objects from being tied together
            GameObject copy = GameObject.Instantiate(activeTile.gameObject, activeTile.position, activeTile.rotation);
            PrefabUtility.SaveAsPrefabAssetAndConnect(copy, tempDir, InteractionMode.AutomatedAction);
            GameObject.DestroyImmediate(copy);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            string path = SorterUtils.ConvertToAssetRelative(tempDir);
            GameObject temp = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            return temp;
        }

        ///<summary>Moves the passed prefab asset to the layers Package folder using System.IO
        /// <para>Moves the .meta file too</para>
        /// </summary>
        void MovePrefab(GameObject prefab, string twoDigitName, string threeDigitName)
        {
            string tempFolder = Path.Combine(Application.dataPath, tempFolderName);
            if (!Directory.Exists(tempFolder))
            { Directory.CreateDirectory(tempFolder); }

            string oldPath = Path.Combine(Application.dataPath, tempFolderName, prefab.name + ".prefab");
            oldPath = SorterUtils.ConvertToAssetRelative(oldPath);

            string newPath = GetDetailPrefabPath(prefab.name, twoDigitName, threeDigitName);
            newPath = newPath.Replace('\\', '/');

            if (File.Exists(newPath))
            { File.Delete(newPath); }

            if (!File.Exists(newPath))
            { File.Move(oldPath, newPath); }

            //Move its meta file along with it
            MoveMetaFile(prefab, twoDigitName, threeDigitName);
        }

        ///<summary>Moves the passed prefab asset meta to the layers Package folder using System.IO</summary>
        void MoveMetaFile(GameObject prefab, string twoDigitName, string threeDigitName)
        {
            string oldPath = Path.Combine(Application.dataPath, tempFolderName, prefab.name + ".prefab.meta");
            oldPath = SorterUtils.ConvertToAssetRelative(oldPath);

            string newPath = GetDetailPrefabPath(prefab.name, twoDigitName, threeDigitName);
            newPath += ".meta";
            newPath = newPath.Replace('\\', '/');

            if (File.Exists(newPath))
            { File.Delete(newPath); }

            if (!File.Exists(newPath))
            { File.Move(oldPath, newPath); }

            //Refresh after moving the asset and its meta
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        #endregion

        #region MATERIAL_COPY_MOVING
        ///<summary>Copies the terrain to the temporary Assets folder, assigns it to the passed Terrain and returns it.</summary>
        Material CreateMaterialCopy(ref Terrain tileTerrain, string tileAssetName, string strMaterialSource)
        {
            string matTempFolder = Path.Combine(Application.dataPath, tempFolderName, tileAssetName + ".mat");
            Material matAtTempFolder = SorterUtils.CopyMaterial(strMaterialSource, matTempFolder);
            tileTerrain.materialTemplate = matAtTempFolder;

            return matAtTempFolder;
        }

        ///<summary>Moves the passed material asset to the layers Package folder using System.IO
        /// <para>Moves the .meta file too</para>
        /// </summary>
        void MoveMaterial(Material material, string twoDigitName, string threeDigitName)
        {
            string oldPath = Path.Combine(Application.dataPath, tempFolderName, material.name + ".mat");
            oldPath = SorterUtils.ConvertToAssetRelative(oldPath);

            string newPath = GetDetailMaterialPath(material.name, twoDigitName, threeDigitName);
            newPath = newPath.Replace('\\', '/');

            if (File.Exists(newPath))
            { File.Delete(newPath); }

            if (!File.Exists(newPath))
            { File.Move(oldPath, newPath); }

            //Move the meta file as well
            MoveMaterialMeta(material, twoDigitName, threeDigitName);
        }

        ///<summary>Moves the passed material asset meta to the layers Package folder using System.IO</summary>
        void MoveMaterialMeta(Material material, string twoDigitName, string threeDigitName)
        {
            string oldPath = Path.Combine(Application.dataPath, tempFolderName, material.name + ".mat.meta");
            oldPath = SorterUtils.ConvertToAssetRelative(oldPath);

            string newPath = GetDetailMaterialPath(material.name, twoDigitName, threeDigitName);
            newPath += ".meta";
            newPath = newPath.Replace('\\', '/');

            if (File.Exists(newPath))
            { File.Delete(newPath); }

            if (!File.Exists(newPath))
            { File.Move(oldPath, newPath); }

            //Refresh after moving the asset and its meta
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        #endregion

        #region TERRAIN_DATA_COPY_MOVING
        ///<summary>Copies the terrain data to the temporary Assets folder, assigns it to the passed Terrain and Terrain Collider and returns it.</summary>
        TerrainData CreateTerrainDataCopy(ref Terrain tileTerrain, ref TerrainCollider terrainCollider, string tileAssetName, string strDataSource)
        {
            string tdTempFolder = Path.Combine(Application.dataPath, tempFolderName, tileAssetName + ".asset");

            TerrainData tdAtTempFolder = SorterUtils.CopyTerrainData(strDataSource, tdTempFolder);
            tileTerrain.terrainData = tdAtTempFolder;
            terrainCollider.terrainData = tdAtTempFolder;

            return tdAtTempFolder;
        }

        ///<summary>Moves the passed TerrainData asset to the layers Package folder using System.IO
        /// <para>Moves the .meta file too</para>
        /// </summary>
        void MoveTerrainData(TerrainData terrainData, string twoDigitName, string threeDigitName)
        {
            string oldPath = Path.Combine(Application.dataPath, tempFolderName, terrainData.name + ".asset");
            oldPath = SorterUtils.ConvertToAssetRelative(oldPath);

            string newPath = GetDetailAssetPath(terrainData.name, twoDigitName, threeDigitName);
            newPath = newPath.Replace('\\', '/');

            if (File.Exists(newPath))
            { File.Delete(newPath); }

            if (!File.Exists(newPath))
            { File.Move(oldPath, newPath); }

            //Move the meta file as well
            MoveTerrainDataMeta(terrainData, twoDigitName, threeDigitName);
        }

        ///<summary>Moves the passed TerrainData asset meta to the layers Package folder using System.IO</summary>
        void MoveTerrainDataMeta(TerrainData terrainData, string twoDigitName, string threeDigitName)
        {
            string oldPath = Path.Combine(Application.dataPath, tempFolderName, terrainData.name + ".asset.meta");
            oldPath = SorterUtils.ConvertToAssetRelative(oldPath);

            string newPath = GetDetailAssetPath(terrainData.name, twoDigitName, threeDigitName);
            newPath += ".meta";
            newPath = newPath.Replace('\\', '/');

            if (File.Exists(newPath))
            { File.Delete(newPath); }

            if (!File.Exists(newPath))
            { File.Move(oldPath, newPath); }

            //Refresh after moving the asset and its meta
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        #endregion
        #endregion

        #region SURFACE_LAYER_HELPER_METHODS
        public bool IsTileSuitable(Transform activeTile)
        {
            if (activeTile.transform.localPosition.y >= 0)
            {
                return true;
            }

            //Debug.Log(activeTile.transform.localPosition.y);
            return false;
        }

        public string GetDetailAssetPath(string tileAssetName, string twoDigitName, string threeDigitName)
        {
            return Path.Combine(handler.GetAssetsFolderPath(), twoDigitName, threeDigitName, tileAssetName + ".asset");
        }

        public string GetDetailMaterialPath(string tileAssetName, string twoDigitName, string threeDigitName)
        {
            return Path.Combine(handler.GetAssetsFolderPath(), twoDigitName, threeDigitName, handler.GetMaterialFolderPath(), tileAssetName + ".mat");
        }

        public string GetDetailPrefabPath(string tileAssetName, string twoDigitName, string threeDigitName)
        {
            return Path.Combine(handler.GetPrefabsFolderPath(), twoDigitName, threeDigitName, tileAssetName + ".prefab");
        }

        ///<summary>Creates the assets and prefab folders that store seabed layer tiles if they do not exist.</summary>
        void CreateDirsIfNotExists(string activeTileName, string twoDigitName, string threeDigitName)
        {
            string assetPath = Path.Combine(handler.GetAssetsFolderPath(), twoDigitName, threeDigitName);
            if (!Directory.Exists(assetPath))
            { Directory.CreateDirectory(assetPath); }

            string prefabPath = Path.Combine(handler.GetPrefabsFolderPath(), twoDigitName, threeDigitName);
            if (!Directory.Exists(prefabPath))
            { Directory.CreateDirectory(prefabPath); }

            string materialsPath = Path.Combine(handler.GetAssetsFolderPath(), twoDigitName, threeDigitName, handler.GetMaterialFolderPath());
            if (!Directory.Exists(materialsPath))
            { Directory.CreateDirectory(materialsPath); }

            string tempFolder = Path.Combine(Application.dataPath, tempFolderName);
            if (!Directory.Exists(tempFolder))
            { Directory.CreateDirectory(tempFolder); }
        }
        #endregion
    }
#endif
}