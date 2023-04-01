#if UNITY_EDITOR
namespace htm.detailLayers
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public sealed class SeabedLayer : IDetailLayer
    {
        ///<summary>The Detail level this layer corresponds to.</summary>
        IDetailLevel handler;

        ///<summary>The name of the temporary folder storing the tiles before getting moved to the external folder.</summary>
        string tempFolderName = "TileTemp";

        public SeabedLayer(IDetailLevel handler)
        {
            this.handler = handler;
        }

        public void HandleTile(Transform activeTile, string digitName, string prefix)
        {
            activeTile.name = digitName;
            string tileAssetName = prefix + handler.GetStrSeparator() + activeTile.name;

            //This ensures that the generated tiles will have a destination file to get saved to.
            string parentFolderName = SorterUtils.GenerateCordsRelativeFolderName(activeTile, '_', 2);
            CreateDirsIfNotExists(activeTile.name, parentFolderName);

            //Create the prefab from the passed tile
            GameObject createdPrefab = CreatePrefab(activeTile, tileAssetName, parentFolderName);

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
            MovePrefab(createdPrefab, parentFolderName);
            MoveMaterial(matAtTempFolder, parentFolderName);
            MoveTerrainData(tdAtTempFolder, parentFolderName);
            #endregion
        }

        #region ASSET_COPYING_METHODS
        #region PREFAB_COPY_MOVING
        GameObject CreatePrefab(Transform activeTile, string tileAssetName, string parentFolderName)
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
        void MovePrefab(GameObject prefab, string newParentFolder)
        {
            string tempPath = Path.Combine(Application.dataPath, tempFolderName);
            if (!Directory.Exists(tempPath))
            { Directory.CreateDirectory(tempPath); }

            string oldPath = Path.Combine(Application.dataPath, tempFolderName, prefab.name + ".prefab");
            oldPath = SorterUtils.ConvertToAssetRelative(oldPath);

            string newPath = GetDetailPrefabPath(prefab.name, newParentFolder);

            newPath = newPath.Replace('\\', '/');

            if (File.Exists(newPath))
            { File.Delete(newPath); }

            if (!File.Exists(newPath))
            { File.Move(oldPath, newPath); }

            //Move its meta file along with it
            MoveMetaFile(prefab, newParentFolder);
        }

        ///<summary>Moves the passed prefab asset meta to the layers Package folder using System.IO</summary>
        void MoveMetaFile(GameObject prefab, string parentFolderName)
        {
            string oldPath = Path.Combine(Application.dataPath, tempFolderName, prefab.name + ".prefab.meta");
            oldPath = SorterUtils.ConvertToAssetRelative(oldPath);

            string newPath = Path.GetFullPath(Path.Combine(handler.GetPrefabsFolderPath(), parentFolderName, prefab.name + ".prefab.meta"));
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
        void MoveMaterial(Material material, string parentFolderName)
        {
            string oldPath = Path.Combine(Application.dataPath, tempFolderName, material.name + ".mat");
            oldPath = SorterUtils.ConvertToAssetRelative(oldPath);

            string newPath = GetDetailMaterialPath(material.name, parentFolderName);
            newPath = newPath.Replace('\\', '/');

            if (File.Exists(newPath))
            { File.Delete(newPath); }

            if (!File.Exists(newPath))
            { File.Move(oldPath, newPath); }

            //Move the meta file as well
            MoveMaterialMeta(material, parentFolderName);
        }

        ///<summary>Moves the passed material asset meta to the layers Package folder using System.IO</summary>
        void MoveMaterialMeta(Material material, string parentFolderName)
        {
            string oldPath = Path.Combine(Application.dataPath, tempFolderName, material.name + ".mat.meta");
            oldPath = SorterUtils.ConvertToAssetRelative(oldPath);

            string newPath = GetDetailMaterialPath(material.name, parentFolderName);
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
        void MoveTerrainData(TerrainData terrainData, string parentFolderName)
        {
            string oldPath = Path.Combine(Application.dataPath, tempFolderName, terrainData.name + ".asset");
            oldPath = SorterUtils.ConvertToAssetRelative(oldPath);

            string newPath = GetDetailAssetPath(terrainData.name, parentFolderName);
            newPath = newPath.Replace('\\', '/');

            if (File.Exists(newPath))
            { File.Delete(newPath); }

            if (!File.Exists(newPath))
            { File.Move(oldPath, newPath); }

            //Move the meta file as well
            MoveTerrainDataMeta(terrainData, parentFolderName);
        }

        ///<summary>Moves the passed TerrainData asset meta to the layers Package folder using System.IO</summary>
        void MoveTerrainDataMeta(TerrainData terrainData, string parentFolderName)
        {
            string oldPath = Path.Combine(Application.dataPath, tempFolderName, terrainData.name + ".asset.meta");
            oldPath = SorterUtils.ConvertToAssetRelative(oldPath);

            string newPath = GetDetailAssetPath(terrainData.name, parentFolderName);
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

        #region SEABED_LAYER_HELPER_METHODS
        public bool IsTileSuitable(Transform activeTile)
        {
            if (activeTile.transform.localPosition.y <= -1)
            {
                return true;
            }

            //Debug.Log(activeTile.transform.localPosition.y);
            return false;
        }

        string GetDetailPrefabPath(string tileAssetName, string newParentFolder)
        {
            return Path.GetFullPath(Path.Combine(handler.GetPrefabsFolderPath(), newParentFolder, tileAssetName + ".prefab"));
        }

        string GetDetailAssetPath(string tileAssetName, string newParentFolder)
        {
            return Path.GetFullPath(Path.Combine(handler.GetAssetsFolderPath(), newParentFolder, tileAssetName + ".asset"));
        }

        string GetDetailMaterialPath(string tileAssetName, string newParentFolder)
        {
            return Path.GetFullPath(Path.Combine(handler.GetAssetsFolderPath(), newParentFolder, handler.GetMaterialFolderPath(), tileAssetName + ".mat"));
        }

        ///<summary>Creates the assets and prefab folders that store seabed layer tiles if they do not exist.</summary>
        void CreateDirsIfNotExists(string activeTileName, string parentFolderName)
        {
            string assetPath = Path.GetFullPath(Path.Combine(handler.GetAssetsFolderPath(), parentFolderName));
            if (!Directory.Exists(assetPath))
            { Directory.CreateDirectory(assetPath); }

            string prefabPath = Path.GetFullPath(Path.Combine(handler.GetPrefabsFolderPath(), parentFolderName));
            if (!Directory.Exists(prefabPath))
            { Directory.CreateDirectory(prefabPath); }

            string materialsPath = Path.GetFullPath(Path.Combine(handler.GetAssetsFolderPath(), parentFolderName, handler.GetMaterialFolderPath()));
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