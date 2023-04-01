#if UNITY_EDITOR
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

using Object = UnityEngine.Object;

public static class SorterUtils
{
    ///<summary>Return the detail level of the tile based on the Tile Type selected from the user.</summary>
    public static CustomDetailLevel DetermineDetailLevel(TileLayerType tileType, CustomDetailLevel customDetailLevel)
    {
        CustomDetailLevel detailLevel = CustomDetailLevel.LowDetail;

        switch (tileType)
        {
            case TileLayerType.Global:
                detailLevel = CustomDetailLevel.LowDetail;
                break;

            case TileLayerType.Seabed:
                detailLevel = CustomDetailLevel.HighDetail;
                break;

            case TileLayerType.Surface:
                detailLevel = CustomDetailLevel.HighDetail;
                break;

            case TileLayerType.Objects:
                detailLevel = CustomDetailLevel.HighDetail;
                break;

            case TileLayerType.CustomTerrain:
                detailLevel = customDetailLevel;
                break;

            case TileLayerType.CustomObjects:
                detailLevel = customDetailLevel;
                break;
        }

        return detailLevel;
    }

    ///<summary>Returns true/false based on the tile type selected from the user.</summary>
    public static bool CanMovePrefabsOnly(TileLayerType tileType)
    {
        bool result = false;

        switch (tileType)
        {
            case TileLayerType.Global:
                result = false;
                break;

            case TileLayerType.Seabed:
                result = false;
                break;

            case TileLayerType.Surface:
                result = false;
                break;

            case TileLayerType.Objects:
                result = true;
                break;

            case TileLayerType.CustomTerrain:
                result = false;
                break;

            case TileLayerType.CustomObjects:
                result = true;
                break;
        }

        return result;
    }

    ///<summary>Correctly formats the passed string and keeps only the numbers up to the numsToKeep value</summary>
    public static string FormatDoubleToString(string doubleAsStr, int numsToKeep)
    {
        string finalStr = string.Empty;

        bool isNegative = doubleAsStr.StartsWith('-');

        string totalName = doubleAsStr.Trim('-').PadLeft(8, '0');

        finalStr = totalName.Substring(0, numsToKeep);

        if (isNegative) finalStr = "-" + finalStr;

        return finalStr;
    }

    public static string GenerateCordsRelativeFolderName(Transform activeTile, char separator, int numsToKeep)
    {
        string[] substrings = activeTile.name.Split("_");

        string lat = substrings[0];
        string lon = substrings[1];

        if (lat.Length >= numsToKeep)
        {
            if (lat.StartsWith('-'))
            { lat = lat.Substring(0, numsToKeep + 1); }
            else
            { lat = lat.Substring(0, numsToKeep); }
        }

        if (lon.Length >= numsToKeep)
        {
            if (lon.StartsWith('-'))
            { lon = lon.Substring(0, numsToKeep + 1); }
            else
            { lon = lon.Substring(0, numsToKeep); }
        }

        //Create a folder name relative to the cords of the tile.
        string parentFolderName = lat + separator.ToString() + lon;

        return parentFolderName;
    }

    ///<summary>Converts the passed path to an asset relative path.</summary>
    public static string ConvertToAssetRelative(string filePath)
    {
        int assetsIndex = filePath.IndexOf("Assets");
        if (assetsIndex == -1)
        {
            return null;
        }

        string assetPath = filePath.Substring(assetsIndex);

        return assetPath;
    }

    ///<summary>Copies the terrain data asset from the source string to the destination string then loads the moved asset and returns it.
    /// <para>Returns null if the moved asset doesn't get loaded.</para>
    /// <para>* The paths must be inside the Assets unity folder.</para>
    /// </summary>
    public static TerrainData CopyTerrainData(string strDataSource, string strDataDest)
    {
        string destAssetRelative = SorterUtils.ConvertToAssetRelative(strDataDest);
        destAssetRelative.Replace('\\', '/');

        AssetDatabase.CopyAsset(strDataSource, SorterUtils.ConvertToAssetRelative(strDataDest));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        TerrainData dataCopy = AssetDatabase.LoadAssetAtPath<TerrainData>(destAssetRelative);

        return dataCopy;
    }

    ///<summary>Copies the material asset from the source string to the destination string then loads the moved asset and returns it.
    /// <para>Returns null if the moved asset doesn't get loaded.</para>
    /// <para>* The paths must be inside the Assets unity folder.</para>
    /// </summary>
    public static Material CopyMaterial(string strMaterialSource, string strMaterialDest)
    {
        string destAssetRelative = SorterUtils.ConvertToAssetRelative(strMaterialDest);
        destAssetRelative.Replace('\\', '/');

        AssetDatabase.CopyAsset(strMaterialSource, destAssetRelative);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Material matCopy = AssetDatabase.LoadAssetAtPath<Material>(destAssetRelative);

        return matCopy;
    }

    ///<summary>Creates an addressable group after the passed string if it does not exist.</summary>
    public static void CreateAddressableGroup(string groupName)
    {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

        if (settings)
        {
            AddressableAssetGroup group = settings.FindGroup(groupName);
            if (!group)
            {
                group = settings.CreateGroup(groupName, false, false, true, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));
            }
        }
        else
        {
            throw new System.NullReferenceException("Addressable settings could not be retrieved.");
        }
    }

    ///<summary>Marks the passed Object as addressable and assigns it in the passed group name.
    ///<para>Throws error if the passed group name does not exist</para>
    /// </summary>
    public static void SetAddressableGroup(this Object obj, string groupName, string objName)
    {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

        if (settings)
        {
            AddressableAssetGroup group = settings.FindGroup(groupName);
            if (!group)
            {
                Debug.Log("Group sreach" + groupName);
                throw new System.ArgumentException("The passed groupName does not exist, did you forgot to create it?");
            }

            string assetpath = AssetDatabase.GetAssetPath(obj);
            string guid = AssetDatabase.AssetPathToGUID(assetpath);

            AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, group, false, false);
            entry.address = objName;

            List<AddressableAssetEntry> entriesAdded = new List<AddressableAssetEntry> { entry };

            group.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, false, true);
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true, false);
        }
        else
        {
            throw new System.NullReferenceException("Addressable settings could not be retrieved.");
        }
    }

    public static void AddScriptsToPrefab(MonoScript[] scriptsArray, Transform activeTile)
    {
        for (int i = 0; i < scriptsArray.Length; i++)
        {
            string scriptName = scriptsArray[i].name;
            Type scriptType = Type.GetType(scriptName);

            activeTile.gameObject.AddComponent(scriptType);
        }
    }
}
#endif