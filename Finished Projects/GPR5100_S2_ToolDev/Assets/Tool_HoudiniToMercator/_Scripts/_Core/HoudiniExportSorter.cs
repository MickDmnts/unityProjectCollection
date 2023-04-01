#if UNITY_EDITOR
using System.IO;
using System;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

#region TOOL_ENUMS
///<summary>The available export layers</summary>
public enum TileLayerType
{
    Global,
    Seabed,
    Surface,
    Objects,
    CustomTerrain,
    CustomObjects,
}

///<summary>The available detail levels</summary>
public enum CustomDetailLevel
{
    LowDetail,
    HighDetail,
}

///<summary>The available position in the grid the first tile can be.</summary>
public enum FirstTilePos
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight,
}

///<summary>The available directions the grid can be exported from Houdini.</summary>
public enum TileExportDirection
{
    Up,
    Right,
    Down,
    Left,
}

public enum CustomLayerFiltering
{
    Seabed,
    Surface,
    Seabed_Surface,
}
#endregion

public sealed class HoudiniExportSorter : MonoBehaviour
{
    #region CONST_VARIABLES
    ///<summary>The assets folder fixed name</summary>
    const string ASSETS_FOLDER_NAME = "assets";
    ///<summary>The prefabs folder fixed name</summary>
    const string PREFABS_FOLDER_NAME = "prefabs";
    ///<summary>The materials folder fixed name</summary>
    const string MATERIALS_FOLDER_NAME = "Materials";

    ///<summary>The output tiles name separator</summary>
    const char STR_SEPARATOR = '_';

    ///<summary>The low detail addressable group fixed name</summary>
    const string LD_ADR_GROUP = "TerrainLD_";
    ///<summary>The high detail addressable group fixed name</summary>
    const string HD_ADR_GROUP = "TerrainHD_";
    #endregion

    #region INSPECTOR_VARIABLES
    ///<summary>The initial tile that contains the sub tiles to be named and saved</summary>
    [SerializeField, Tooltip("The initial tile that contains the sub tiles to be named and saved")] GameObject parentTile;

    [Space()]
    ///<summary>The low detail Assets path.</summary>
    [SerializeField, Tooltip("The low detail Assets path.")] string lowDetailBasePath = string.Empty;
    ///<summary>The high detail Packages path</summary>
    [SerializeField, Tooltip("The high detail Packages path")] string highDetailBasePath = string.Empty;

    [Space()]
    ///<summary>The bottom left LAT coords.</summary>
    [SerializeField, Tooltip("The bottom left LAT coords.")] double lat = 0;
    ///<summary>The bottom left LON coords.</summary>
    [SerializeField, Tooltip("The bottom left LON coords.")] double lon = 0;

    [Space()]
    ///<summary>The layer the user selected to export</summary>
    [SerializeField, Tooltip("The layer the user selected to export")] TileLayerType tileType = TileLayerType.Global;
    ///<summary>The sub tile export resolution</summary>
    [SerializeField, Tooltip("The sub tile export resolution")] double tileRes = 20000;

    [Space()]
    ///<summary>These scripts will be added to the exported tiles.</summary>
    [SerializeField, Tooltip("These scripts will be added to the exported tiles.")] MonoScript[] scriptsToAdd;

    [Space()]
    ///<summary>The custom prefix the user inputs if he selects a custom layer</summary>
    [SerializeField, Tooltip("The custom prefix the user inputs if he selects a custom layer")] string customLayerPrefix = string.Empty;
    ///<summary>The detail level of the sub tiles to export</summary>
    [SerializeField, Tooltip("The detail level of the sub tiles to export")] CustomDetailLevel customDetailLevel;
    ///<summary>The filtering mode of the tiles.</summary>
    [SerializeField, Tooltip("The filtering mode of the tiles.")] CustomLayerFiltering customLayerType = CustomLayerFiltering.Seabed;
    #endregion

    #region PRIVATE_VARS
    ///<summary>The handler used to iterate and manage the sub tiles</summary>
    [SerializeField] SorterTileHandler tileHandler;

    ///<summary>The low detail Assets folder</summary>
    string ldAssetsFolder;
    ///<summary>The high detail Assets folder</summary>
    string hdAssetsFolder;

    ///<summary>The low detail Prefabs folder</summary>
    string ldPrefabsFolder;
    ///<summary>The high detail Prefabs folder</summary>
    string hdPrefabsFolder;

    ///<summary>The parent folder name that stores the created tiles in XX_XX format.</summary>
    string parentFolderName;

    ///<summary>The addressable group the sub tiles will be added to.</summary>
    string addressableGroup = string.Empty;
    #endregion

    #region ACCESSORS 
    ///<summary>The output tiles name separator</summary>
    public char Str_Separator => STR_SEPARATOR;
    ///<summary>The assets folder fixed name</summary>
    public string AssetsFolderName => ASSETS_FOLDER_NAME;
    ///<summary>The prefabs folder fixed name</summary>
    public string PrefabsFolderName => PREFABS_FOLDER_NAME;
    ///<summary>The materials folder fixed name</summary>
    public string MaterialFolderName => MATERIALS_FOLDER_NAME;

    ///<summary>The addressable group the sub tiles will be added to.</summary>
    public string AddressableGroup => addressableGroup;

    ///<summary>The initial tile that contains the sub tiles to be named and saved</summary>
    public GameObject ParentTile => parentTile;
    ///<summary>The layer the user selected to export</summary>
    public TileLayerType TileType => tileType;
    ///<summary>The detail level of the sub tiles to export</summary>
    public CustomDetailLevel CustomDetailLevel => customDetailLevel;
    ///<summary>The bottom left LAT coords.</summary>
    public double Lat => lat;
    ///<summary>The bottom left LON coords.</summary>
    public double Lon => lon;
    ///<summary>The sub tile export resolution</summary>
    public double TileRes => tileRes;
    ///<summary>The custom prefix the user inputs if he selects a custom layer</summary>
    public string CustomLayerPrefix => customLayerPrefix == string.Empty ? "." : customLayerPrefix;
    ///<summary>The filtering mode of the tiles.</summary>
    public CustomLayerFiltering CustomLayerFiltering => customLayerType;

    ///<summary>The array containing the scripts to add to the exported tile.</summary>
    public MonoScript[] ScriptsToAdd => scriptsToAdd.Clone() as MonoScript[];

    ///<summary>The low detail Assets folder</summary>
    public string LdAssetsFolder => ldAssetsFolder;
    ///<summary>The low detail Prefabs folder</summary>
    public string LdPrefabsFolder => ldPrefabsFolder;
    ///<summary>The high detail Assets folder</summary>
    public string HdAssetsFolder => hdAssetsFolder;
    ///<summary>The high detail Prefabs folder</summary>
    public string HdPrefabsFolder => hdPrefabsFolder;
    ///<summary>The parent folder name that stores the created tiles in XX_XX format.</summary>
    public string ParentFolderName => parentFolderName;
    #endregion

    public void StartSorterSequence()
    {
        MoveToFixedPosition();

        //Activate the tile in case it is not active.
        if (!parentTile.activeInHierarchy)
        { parentTile.SetActive(true); }

        //Folder creation
        CheckForNeccessaryFolders();

        //Create the addressable group this tile belongs to
        CalculateTileAddressableGroup(customDetailLevel, lat, lon, out addressableGroup);
        SorterUtils.CreateAddressableGroup(addressableGroup);

        tileHandler = new SorterTileHandler(this);
        tileHandler.SortTiles();
    }

    ///<summary>Moves the parent tile to a prefixed position for correct calculations.</summary>
    void MoveToFixedPosition()
    {
        if (parentTile != null)
        {
            parentTile.transform.position = Vector3.zero;
        }
    }

    #region INITIAL_FOLDER_CREATION
    ///<summary>Checks if the Assets and Prefabs folders exist for Low and High details.</summary>
    void CheckForNeccessaryFolders()
    {
        if (!String.IsNullOrEmpty(lowDetailBasePath) || !String.IsNullOrWhiteSpace(lowDetailBasePath))
        {
            ldPrefabsFolder = Path.Combine(lowDetailBasePath, PREFABS_FOLDER_NAME);
            ldAssetsFolder = Path.Combine(lowDetailBasePath, ASSETS_FOLDER_NAME);

            CheckForParentTileFolders();

            string finalLdString = Path.Combine(ldAssetsFolder, parentFolderName, MATERIALS_FOLDER_NAME);

            //Assets check
            if (!Directory.Exists(ldAssetsFolder))
            { CreateAssetsFolder(lowDetailBasePath + Path.DirectorySeparatorChar); }

            //Prefabs check
            if (!Directory.Exists(ldPrefabsFolder))
            { CreatePrefabFolder(lowDetailBasePath + Path.DirectorySeparatorChar); }

            //Materials check
            if (!Directory.Exists(finalLdString))
            { Directory.CreateDirectory(finalLdString); }
        }

        if (!String.IsNullOrEmpty(highDetailBasePath) || !String.IsNullOrWhiteSpace(highDetailBasePath))
        {
            hdPrefabsFolder = Path.Combine(highDetailBasePath, PREFABS_FOLDER_NAME);
            hdAssetsFolder = Path.Combine(highDetailBasePath, ASSETS_FOLDER_NAME);

            CheckForParentTileFolders();

            string finalHdString = Path.Combine(hdAssetsFolder, parentFolderName, MATERIALS_FOLDER_NAME);

            //Assets check
            if (!Directory.Exists(hdAssetsFolder))
            { CreateAssetsFolder(highDetailBasePath + Path.DirectorySeparatorChar); }

            //Prefabs check
            if (!Directory.Exists(hdPrefabsFolder))
            { CreatePrefabFolder(highDetailBasePath + Path.DirectorySeparatorChar); }

            //Materials check
            if (!Directory.Exists(finalHdString))
            { Directory.CreateDirectory(finalHdString); }
        }
    }

    ///<summary>Creates a folder named Assets at the specified path</summary>
    void CreateAssetsFolder(string path)
    {
        string finalPath = Path.Combine(path, ASSETS_FOLDER_NAME + Path.DirectorySeparatorChar);

        DirectoryInfo finalFolder = Directory.CreateDirectory(finalPath);
    }

    ///<summary>Creates a folder names Prefabs at the specified path.</summary>
    void CreatePrefabFolder(string path)
    {
        string finalPath = Path.Combine(path, PREFABS_FOLDER_NAME + Path.DirectorySeparatorChar);

        DirectoryInfo finalFolder = Directory.CreateDirectory(finalPath);
    }

    ///<summary>Checks if the main folders storing the tiles are present, if not it creates them.</summary>
    void CheckForParentTileFolders()
    {
        string finalLat = SorterUtils.FormatDoubleToString(lat.ToString(), 2);
        string finalLon = SorterUtils.FormatDoubleToString(lon.ToString(), 2);

        //Cache the initial parent folder name.
        parentFolderName = finalLat + STR_SEPARATOR + finalLon;

        //Create the needed base folders based on 
        switch (tileType)
        {
            case TileLayerType.Global:
                CreateLDFolders(parentFolderName);
                break;

            case TileLayerType.Seabed:
                CreateHDFolders(parentFolderName);
                break;

            case TileLayerType.Surface:
                CreateHDFolders(parentFolderName);
                break;

            case TileLayerType.Objects:
                CreateHDFolders(parentFolderName);
                break;

            case TileLayerType.CustomTerrain:

                if (IsPassedDetailSelected(CustomDetailLevel.LowDetail))
                {
                    if (!String.IsNullOrEmpty(lowDetailBasePath) || !String.IsNullOrWhiteSpace(lowDetailBasePath))
                    {
                        CreateLDFolders(parentFolderName);
                    }
                    else { throw new ArgumentNullException("lowDetailBasePath is not valid"); }
                }

                if (IsPassedDetailSelected(CustomDetailLevel.HighDetail))
                {
                    if (!String.IsNullOrEmpty(highDetailBasePath) || !String.IsNullOrWhiteSpace(highDetailBasePath))
                    {
                        CreateHDFolders(parentFolderName);
                    }
                    else { throw new ArgumentNullException("highDetailBasePath is not valid"); }
                }

                break;

            case TileLayerType.CustomObjects:

                if (IsPassedDetailSelected(CustomDetailLevel.LowDetail))
                {
                    if (!String.IsNullOrEmpty(lowDetailBasePath) || !String.IsNullOrWhiteSpace(lowDetailBasePath))
                    {
                        CreateLDFolders(parentFolderName);
                    }
                    else { throw new ArgumentNullException("lowDetailBasePath is not valid"); }
                }

                if (IsPassedDetailSelected(CustomDetailLevel.HighDetail))
                {
                    if (!String.IsNullOrEmpty(highDetailBasePath) || !String.IsNullOrWhiteSpace(highDetailBasePath))
                    {
                        CreateHDFolders(parentFolderName);
                    }
                    else { throw new ArgumentNullException("highDetailBasePath is not valid"); }
                }
                break;
        }
    }

    ///<summary>Creates a parentFolderName named folder inside the assets and prefabs low detail directories.</summary>
    void CreateLDFolders(string parentFolderName)
    {
        string finalAssetsString = Path.GetFullPath(Path.Combine(ldAssetsFolder, parentFolderName));
        string finalPrefabsString = Path.GetFullPath(Path.Combine(ldPrefabsFolder, parentFolderName));

        if (!Directory.Exists(finalAssetsString))
        { Directory.CreateDirectory(finalAssetsString); }

        if (!Directory.Exists(finalPrefabsString))
        { Directory.CreateDirectory(finalPrefabsString); }
    }

    ///<summary>Creates a parentFolderName named folder inisde the assets and prefabs high detail directories.</summary>
    void CreateHDFolders(string parentFolderName)
    {
        string finalAssetsString = Path.GetFullPath(Path.Combine(hdAssetsFolder, parentFolderName));
        string finalPrefabsString = Path.GetFullPath(Path.Combine(hdPrefabsFolder, parentFolderName));

        if (!Directory.Exists(finalAssetsString))
        { Directory.CreateDirectory(finalAssetsString); }

        if (!Directory.Exists(finalPrefabsString))
        { Directory.CreateDirectory(finalPrefabsString); }
    }
    #endregion

    #region UTILITIES
    ///<summary>This method can be used from external scripts to calculate and create the addressable group of the passed lat/lon on tool runtime.</summary>
    public void External_CalculateTileAddressableGroup(CustomDetailLevel customDetailLevel, double lat, double lon)
    {
        CalculateTileAddressableGroup(customDetailLevel, lat, lon, out addressableGroup);
        SorterUtils.CreateAddressableGroup(addressableGroup);
    }

    ///<summary>Sets the passed addressable string based on the passed lat/lon values. Outputs a string of "Terrain?D_X_X" format.</summary>
    void CalculateTileAddressableGroup(CustomDetailLevel customDetailLevel, double lat, double lon, out string addressableGroup)
    {
        string tempStr = string.Empty;

        //Set the initial prefix
        switch (customDetailLevel)
        {
            case CustomDetailLevel.LowDetail:
                tempStr += LD_ADR_GROUP;
                break;

            case CustomDetailLevel.HighDetail:
                tempStr += HD_ADR_GROUP;
                break;
        }

        if (lat > 0)
        { PositiveLat(ref tempStr, lat); }
        else
        { NegativeLat(ref tempStr, lat); }

        tempStr += "_";

        if (lon > 0)
        { PositiveLon(ref tempStr, lon); }
        else
        { NegativeLon(ref tempStr, lon); }

        addressableGroup = tempStr;
    }

    ///<summary>Sets the passed string to 0,1,2 based on the positive lat global value.</summary>
    void PositiveLat(ref string tempStr, double lat)
    {
        string str = string.Empty;

        if (lat == 0)
        {
            str += "0";
            tempStr += str;
            return;
        }

        if (lat < 10000000)
        {
            str = "0";
        }
        else if (lat == 10000000)
        {
            str = "1";
        }
        else { str = "2"; }

        tempStr += str;
    }

    ///<summary>Sets the passed string to 0,-1,-2 based on the negative lat global value.</summary>
    void NegativeLat(ref string tempStr, double lat)
    {
        string str = string.Empty;

        if (lat == 0)
        {
            str += "0";
            tempStr += str;
            return;
        }

        if (lat > -10000000)
        {
            str = "0";
        }
        else if (lat == -10000000)
        {
            str = "-1";
        }
        else { str += "-2"; }

        tempStr += str;
    }

    ///<summary>Sets the passed string to 0,1,2 based on the positive lon global value.</summary>
    void PositiveLon(ref string tempStr, double lon)
    {
        string str = string.Empty;

        if (lon == 0)
        {
            tempStr += "0";
            tempStr += str;
            return;
        }

        if (lon < 10000000)
        {
            str += "0";
        }
        else if (lon == 10000000)
        {
            str += "1";
        }
        else { str += "2"; }

        tempStr += str;
    }

    ///<summary>Sets the passed string to 0,-1,-2 based on the negative lon global value.</summary>
    void NegativeLon(ref string tempStr, double lon)
    {
        string str = string.Empty;

        if (lon == 0)
        {
            tempStr += "0";
            tempStr += str;
            return;
        }

        if (lon > -10000000)
        {
            str += "0";
        }
        else if (lon == -10000000)
        {
            str += "-1";
        }
        else { str += "-2"; }

        tempStr += str;
    }
    #endregion

    #region INSPECTOR_VALIDATION
    private void OnValidate()
    {
        lon = WrapValueAround(lon);

        tileRes = Math.Clamp(tileRes, 1000, 20000000);

        customLayerPrefix = FormatCustomPrefixName(customLayerPrefix);

        customDetailLevel = SorterUtils.DetermineDetailLevel(tileType, customDetailLevel);
        CalculateTileAddressableGroup(customDetailLevel, lat, lon, out addressableGroup);
    }

    ///<summary>If the value passed is greater or equal to 20m, returns -20m, else the value passed.</summary>
    double WrapValueAround(double value)
    {
        if (value >= 20000000) return -20000000;
        else return value;
    }

    ///<summary>Correctly formats the customPrefix to be used as a layer prefix.</summary>
    string FormatCustomPrefixName(string inspectorString)
    {
        if (customLayerPrefix.Length > 1)
        { return customLayerPrefix.ToCharArray()[0].ToString(); }

        return inspectorString;
    }
    #endregion

    #region EDITOR_SCRIPT_VALIDATION
    ///<summary>Returns true if the child count of the parent tile is above 2</summary>
    public bool IsParentTileAcceptable()
    {
        if (parentTile != null)
        {
            return parentTile.transform.childCount >= 2;
        }

        return false;
    }

    public string GenerateSampleName(string customLayerPrefix)
    {
        double digitsToSubtract = Math.Floor(Math.Log10(tileRes));

        if (lat % tileRes != 0 || lon % tileRes != 0)
            digitsToSubtract--;

        int digitsToKeep = 8 - (int)digitsToSubtract;
        return customLayerPrefix + Str_Separator + SorterUtils.FormatDoubleToString(lat.ToString(), digitsToKeep) +
                    Str_Separator +
                    SorterUtils.FormatDoubleToString(lon.ToString(), digitsToKeep);
    }

    ///<summary>Returns false if the passed string's last character is a symbol, true otherwise.</summary>
    public bool IsPathValid(string inputPath)
    {
        if (String.IsNullOrEmpty(inputPath) || String.IsNullOrWhiteSpace(inputPath)) return false;

        string lastChar = inputPath.ToCharArray()[inputPath.Length - 1].ToString();

        return !Regex.IsMatch(lastChar, @"[^A-Za-z0-9\s]");
    }

    ///<summary>Returns true if the user has selected a Custom layer type, false otherwise.</summary>
    public bool IsCustomLayerType() => tileType == TileLayerType.CustomTerrain || tileType == TileLayerType.CustomObjects;

    ///<summary>Returns true if the editor GameObject is null.</summary>
    public bool IsTileEmpty() => parentTile == null;

    ///<summary>Returns false is the passed string is empty or null, true otherwise.</summary>
    public bool IsPrefixValid(string text)
    {
        return !String.IsNullOrEmpty(text);
    }

    public bool IsPassedDetailSelected(CustomDetailLevel detailLevel)
    {
        return customDetailLevel == detailLevel;
    }

    public string[] GetScriptsToAdd()
    {
        List<string> scripts = new List<string>();

        for (int i = 0; i < ScriptsToAdd.Length; i++)
        {
            scripts.Add(ScriptsToAdd[i].name);
        }

        return scripts.ToArray();
    }

    public void PopulateScriptsToAddArray(string[] scriptsToAdd)
    {
        List<MonoScript> scripts = new List<MonoScript>();

        for (int i = 0; i < scriptsToAdd.Length; i++)
        {
            Type type = Type.GetType(scriptsToAdd[i]);

            MonoBehaviour behaviour = (MonoBehaviour)gameObject.AddComponent(type);

            if (behaviour != null)
            {
                MonoScript script = MonoScript.FromMonoBehaviour(behaviour);
                scripts.Add(script);
            }

            if (type != null)
            { DestroyImmediate(gameObject.GetComponent(type)); }
        }

        this.scriptsToAdd = scripts.ToArray();

        AssetDatabase.Refresh();
    }

    public bool IsInAssetFolder(string path)
    {
        return path.Contains("Assets");
    }
    #endregion
}
#endif