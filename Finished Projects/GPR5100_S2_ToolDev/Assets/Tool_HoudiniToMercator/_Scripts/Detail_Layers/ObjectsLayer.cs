#if UNITY_EDITOR
namespace htm.detailLayers
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public sealed class ObjectsLayer : IDetailLayer
    {
        ///<summary>The Detail level this layer corresponds to.</summary>
        IDetailLevel handler;

        public ObjectsLayer(IDetailLevel handler)
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

            if (createdPrefab == null)
            { throw new System.NullReferenceException("Prefab creation returned null."); }

            //Assign the addressable before moving the assets, Unity wont't allow address assignetation outside of an Assets folder.
            SorterUtils.SetAddressableGroup(createdPrefab, handler.GetAddressableGroup(), handler.GetAddressablePath() + createdPrefab.name);

            //Refresh after creating the initial prefab, materials and terrrain data so they can be retrieved and moved.
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            //Move the assets to the packages folders
            MovePrefab(createdPrefab, twoDigitParentFolderName, threeDigitParentFolderName);
        }

        #region ASSET_COPYING_METHODS
        #region PREFAB_COPY_MOVING
        public GameObject CreatePrefab(Transform activeTile, string tileAssetName)
        {
            string tempDir = Application.dataPath + "/TileTemp/" + tileAssetName + ".prefab";

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
            if (!Directory.Exists(Application.dataPath + "/TileTemp/"))
            { Directory.CreateDirectory(Application.dataPath + "/TileTemp/"); }

            string oldPath = Application.dataPath + "/TileTemp/" + prefab.name + ".prefab";
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
            string oldPath = Application.dataPath + "/TileTemp/" + prefab.name + ".prefab.meta";
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
        #endregion

        #region SURFACE_LAYER_HELPER_METHODS
        public bool IsTileSuitable(Transform activeTile)
        {
            return true;
        }

        public string GetDetailPrefabPath(string tileAssetName, string twoDigitName, string threeDigitName)
        {
            return Path.GetFullPath(handler.GetPrefabsFolderPath() + Path.DirectorySeparatorChar + twoDigitName + Path.DirectorySeparatorChar
                                + threeDigitName + Path.DirectorySeparatorChar + tileAssetName + ".prefab");
        }

        ///<summary>Creates the assets and prefab folders that store seabed layer tiles if they do not exist.</summary>
        void CreateDirsIfNotExists(string activeTileName, string twoDigitName, string threeDigitName)
        {
            string prefabPath = Path.GetFullPath(handler.GetPrefabsFolderPath() + Path.DirectorySeparatorChar + twoDigitName + Path.DirectorySeparatorChar
                                 + threeDigitName + Path.DirectorySeparatorChar);
            if (!Directory.Exists(prefabPath))
            { Directory.CreateDirectory(prefabPath); }

            if (!Directory.Exists(Application.dataPath + "/TileTemp/"))
            { Directory.CreateDirectory(Application.dataPath + "/TileTemp/"); }
        }
        #endregion
    }
#endif
}