#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

using htm.detailLevels;

[Serializable]
public sealed class SorterTileHandler
{
    ///<summary>The handler that created this instance.</summary>
    HoudiniExportSorter sorter;

    ///<summary>A dictionary containing all the layer prefixes available.</summary>
    Dictionary<TileLayerType, char> layerPrefixes;
    ///<summary>A dictionary containing all the available detail levels.</summary>
    Dictionary<CustomDetailLevel, IDetailLevel> detailLevels;

    ///<summary>The automatically determined position of the first tile in the grid.</summary>
    FirstTilePos firstTilePos;
    ///<summary>The automatically determined direction of the generated tiles in the grid.</summary>
    TileExportDirection tileExportDir;
    ///<summary>A copy of the detail level for internal use.</summary>
    CustomDetailLevel internalDetailLevel;

    ///<summary>Should prefabs be the only thing that gets moved based on layer selected?</summary>
    bool movePrefabsOnly = false;
    ///<summary>The LAT cords of the first grid tile.</summary>
    double firstTileLAT = 0.0;
    ///<summary>The LON cords of the first grid tile.</summary>
    double firstTileLON = 0.0;

    double activeTileLAT;
    double activeTileLON;

    ///<summary>The handler that created this instance of TileHandler.</summary>
    public HoudiniExportSorter Sorter => sorter;

    public SorterTileHandler(HoudiniExportSorter sorter)
    {
        this.sorter = sorter;

        InitHandler();
    }

    #region HANDLER_INITIALIZATION
    ///<summary>Sets up the SorterTileHandler instance.</summary>
    void InitHandler()
    {
        internalDetailLevel = SorterUtils.DetermineDetailLevel(sorter.TileType, sorter.CustomDetailLevel);
        movePrefabsOnly = SorterUtils.CanMovePrefabsOnly(sorter.TileType);

        layerPrefixes = InitializePrefixCache(sorter.CustomLayerPrefix.ToCharArray()[0]);
        detailLevels = InitializeDetailLevels();

        if (sorter.ParentTile.transform.childCount > 0)
        { firstTilePos = DetermineFirstTilePosition(sorter.ParentTile.transform.GetChild(0)); }
        else
        { Debug.LogWarning("Not enough children tiles in the passed parent tile"); }

        if (sorter.ParentTile.transform.childCount > 1)
        { tileExportDir = DetermineTileExportDirection(sorter.ParentTile.transform.GetChild(0), sorter.ParentTile.transform.GetChild(1)); }
        else
        { Debug.Log("At least 2 children tiles are needed for automatic direction detection on the grid."); }
    }

    ///<summary>Returns a correctly initialized dictionary containing all the layer prefixes.</summary>
    Dictionary<TileLayerType, char> InitializePrefixCache(char customLayerPrefix)
    {
        Dictionary<TileLayerType, char> temp = new Dictionary<TileLayerType, char>()
        {
            {TileLayerType.Global, 'z'},
            {TileLayerType.Seabed, 'a'},
            {TileLayerType.Surface, 'b'},
            {TileLayerType.Objects, 'c'},
            {TileLayerType.CustomTerrain, customLayerPrefix},
            {TileLayerType.CustomObjects, customLayerPrefix},
        };

        return temp;
    }

    ///<summary>Returns a correctly initialized dictionary containing all the detail levels.</summary>
    Dictionary<CustomDetailLevel, IDetailLevel> InitializeDetailLevels()
    {
        Dictionary<CustomDetailLevel, IDetailLevel> temp = new Dictionary<CustomDetailLevel, IDetailLevel>()
        {
            {CustomDetailLevel.LowDetail, new LowDetail(this)},
            {CustomDetailLevel.HighDetail, new HighDetail(this)}
        };

        return temp;
    }

    ///<summary>Determines where in the corners the first tile is lying on the grid based on its position.</summary>
    FirstTilePos DetermineFirstTilePosition(Transform parentFirstChild)
    {
        Transform firstChild = parentFirstChild;

        if (firstChild == null) { throw new System.ArgumentOutOfRangeException("Parent tile does not contain any children."); }

        if (firstChild.position.x > 0)
        {
            if (firstChild.position.z > 0)
            { return FirstTilePos.TopRight; }
            else
            { return FirstTilePos.BottomRight; }
        }
        else
        {
            if (firstChild.position.z > 0)
            { return FirstTilePos.TopLeft; }
            { return FirstTilePos.BottomLeft; }
        }
    }

    ///<summary>Determines the direction the tiles are exported from Houdini based on their position on the grid</summary>
    TileExportDirection DetermineTileExportDirection(params Transform[] firstTwoChildren)
    {
        if (firstTwoChildren.Length < 1) { throw new ArgumentOutOfRangeException("Not enough children tiles passed as arguments, 2 are needed at least."); }

        Transform firstChild = firstTwoChildren[0];
        Transform secondChild = firstTwoChildren[1];

        if (firstChild == null) { throw new System.ArgumentOutOfRangeException("Parent tile does not contain any children."); }
        if (secondChild == null) { throw new System.ArgumentOutOfRangeException("Parent tile does not contain enough children for direction calculations."); }

        if (secondChild.position.z > firstChild.position.z)
        { return TileExportDirection.Up; }
        else if (secondChild.position.z < firstChild.position.z)
        { return TileExportDirection.Down; }
        else if (secondChild.position.x > firstChild.position.x)
        { return TileExportDirection.Right; }
        else
        { return TileExportDirection.Left; }
    }
    #endregion

    #region CHILDREN_TILE_NAMING
    ///<summary>Outputs the first tiles' LAT/LON coordinates relative to the bottom-left corner of the grid.</summary>
    void CalculateFirstTileCords(GameObject parentTile, FirstTilePos firstTilePos, out double lat, out double lon)
    {
        double firstTileLAT = 0.0;
        double firstTileLON = 0.0;

        switch (firstTilePos)
        {
            case FirstTilePos.TopLeft:
                firstTileLAT = sorter.Lat + ((Math.Sqrt(parentTile.transform.childCount) - 1) * sorter.TileRes);
                firstTileLON = sorter.Lon;
                break;

            case FirstTilePos.TopRight:
                firstTileLAT = sorter.Lat + ((Math.Sqrt(parentTile.transform.childCount) - 1) * sorter.TileRes);
                firstTileLON = sorter.Lon + ((Math.Sqrt(parentTile.transform.childCount) - 1) * sorter.TileRes);
                break;

            case FirstTilePos.BottomLeft:
                firstTileLAT = sorter.Lat;
                firstTileLON = sorter.Lon;
                break;

            case FirstTilePos.BottomRight:
                firstTileLAT = sorter.Lat;
                firstTileLON = sorter.Lon + ((Math.Sqrt(parentTile.transform.childCount) - 1) * sorter.TileRes);
                break;
        }

        lat = firstTileLAT;
        lon = firstTileLON;
    }

    ///<summary>Begins the tile sorting,moving and renaming sequence of the grid.</summary>
    public void SortTiles()
    {
        //Refresh the layer prefix
        layerPrefixes = InitializePrefixCache(sorter.CustomLayerPrefix.ToCharArray()[0]);

        sorter.ParentTile.name = sorter.ParentFolderName;
        GameObject parentTile = sorter.ParentTile;

        //Cache the first tile LAT and LON based on its grid position
        CalculateFirstTileCords(parentTile, firstTilePos, out firstTileLAT, out firstTileLON);

        int childCountSqrt = (int)Math.Sqrt(parentTile.transform.childCount);
        Transform activeTile;

        //Traverses the grid from top-left going downwards.
        for (int row = childCountSqrt - 1; row >= 0; row--)
        {
            for (int col = childCountSqrt - 1; col >= 0; col--)
            {
                //Tile acquisition from the grid based on its position on the grid
                activeTile = parentTile.transform.GetChild(row * childCountSqrt + col);

                if (!detailLevels[internalDetailLevel].IsTileSuitable(activeTile, Sorter.TileType))
                {
                    Debug.Log(activeTile.name + " tile is not suitable for this layer.");
                    continue;
                }

                activeTileLAT = firstTileLAT + (col * sorter.TileRes);
                activeTileLON = firstTileLON - (row * sorter.TileRes);

                Sorter.External_CalculateTileAddressableGroup(internalDetailLevel, activeTileLAT, activeTileLON);

                //Move, store, name the currently inspected tile based on its detail level.
                DetailDependentNamingConvention(activeTile, internalDetailLevel, activeTileLAT, activeTileLON, firstTileLAT, firstTileLON);
            }
        }

        string tempFolder = Path.Combine(Application.dataPath, "TileTemp");
        if (Directory.Exists(tempFolder))
        {
            Directory.Delete(tempFolder);
            File.Delete(tempFolder + ".meta");
        }

        AssetDatabase.Refresh();
    }

    ///<summary>Appropriately Handles the passed tile based on its detail level and layer.</summary>
    void DetailDependentNamingConvention(Transform activeTile, CustomDetailLevel internalDetailLevel, double activeTileLAT, double activeTileLON, double firstTileLAT, double firstTileLON)
    {
        string finalName = GenerateResolutionBasedName(Sorter.TileRes, activeTileLAT, activeTileLON);

        switch (internalDetailLevel)
        {
            case CustomDetailLevel.LowDetail:
                {
                    if (detailLevels == null) { throw new System.NullReferenceException("Detail levels dictionary is not set."); }
                    if (!detailLevels.ContainsKey(CustomDetailLevel.LowDetail)) { throw new KeyNotFoundException("Low Detail is not a Detail Level key."); }

                    if (sorter.TileType == TileLayerType.Global)
                    {
                        detailLevels[CustomDetailLevel.LowDetail].HandleDetailAssets(TileLayerType.Global, activeTile, finalName);
                    }
                    else if (sorter.TileType == TileLayerType.CustomTerrain)
                    {
                        detailLevels[CustomDetailLevel.LowDetail].HandleDetailAssets(TileLayerType.CustomTerrain, activeTile, finalName);
                    }
                    else if (sorter.TileType == TileLayerType.CustomObjects)
                    {
                        detailLevels[CustomDetailLevel.LowDetail].HandleDetailAssets(TileLayerType.CustomObjects, activeTile, finalName);
                    }
                }
                break;

            case CustomDetailLevel.HighDetail:
                {
                    if (detailLevels == null) { throw new System.NullReferenceException("Detail levels dictionary is not set."); }
                    if (!detailLevels.ContainsKey(CustomDetailLevel.HighDetail)) { throw new KeyNotFoundException("High Detail is not a Detail Level key."); }

                    if (sorter.TileType == TileLayerType.Seabed)
                    {
                        detailLevels[CustomDetailLevel.HighDetail].HandleDetailAssets(TileLayerType.Seabed, activeTile, finalName);
                    }
                    else if (sorter.TileType == TileLayerType.Surface)
                    {
                        detailLevels[CustomDetailLevel.HighDetail].HandleDetailAssets(TileLayerType.Surface, activeTile, finalName);
                    }
                    else if (sorter.TileType == TileLayerType.Objects)
                    {
                        detailLevels[CustomDetailLevel.HighDetail].HandleDetailAssets(TileLayerType.Objects, activeTile, finalName);
                    }
                    else if (sorter.TileType == TileLayerType.CustomTerrain)
                    {
                        detailLevels[CustomDetailLevel.HighDetail].HandleDetailAssets(TileLayerType.CustomTerrain, activeTile, finalName);
                    }
                    else if (sorter.TileType == TileLayerType.CustomObjects)
                    {
                        detailLevels[CustomDetailLevel.HighDetail].HandleDetailAssets(TileLayerType.CustomObjects, activeTile, finalName);
                    }
                }
                break;
        }
    }

    ///<summary>Returns a three or four digit string composed of the LAT/LON doubles, based on the resolution passed.</summary>
    string GenerateResolutionBasedName(double res, double activeTileLAT, double activeTileLON)
    {
        double digitsToSubtract = Math.Floor(Math.Log10(res));

        if (activeTileLAT % res != 0 || activeTileLON % res != 0)
            digitsToSubtract--;

        int digitsToKeep = 8 - (int)digitsToSubtract;

        string finalName = SorterUtils.FormatDoubleToString(activeTileLAT.ToString(), digitsToKeep) + sorter.Str_Separator + SorterUtils.FormatDoubleToString(activeTileLON.ToString(), digitsToKeep);

        return finalName;
    }
    #endregion

    #region UTILITIES
    ///<summary>Returns the parent tiles' name at a three digit format.</summary>
    public string GetParentTileThreeDigit()
    {
        return SorterUtils.FormatDoubleToString(Sorter.Lat.ToString(), 3) + sorter.Str_Separator.ToString() + SorterUtils.FormatDoubleToString(Sorter.Lon.ToString(), 3);
    }

    ///<summary>Returns the passed layer's prefix.</summary>
    public string GetLayerPrefix(TileLayerType layer)
    {
        if (layerPrefixes != null)
        {
            if (layerPrefixes.ContainsKey(layer))
            {
                return layerPrefixes[layer].ToString();
            }
        }

        return default(char).ToString();
    }
    #endregion
}
#endif