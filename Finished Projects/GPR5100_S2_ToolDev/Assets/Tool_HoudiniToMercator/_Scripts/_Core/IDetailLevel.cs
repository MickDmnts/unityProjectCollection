#if UNITY_EDITOR
using UnityEngine;

public interface IDetailLevel
{
    ///<summary>Returns the Sorter Tile Handler reference</summary>
    public SorterTileHandler TileHandler();

    ///<summary>
    /// Returns true if the passed tile is suitable for this details' layer type, otherwise false.
    /// <para>Throws error if the passed layer is not supported from this detail level.</para>
    /// </summary>
    public bool IsTileSuitable(Transform activeTile, TileLayerType layer);

    ///<summary>Call to handle the passed tile based on its corresponding layer type.</summary>
    public void HandleDetailAssets(TileLayerType layer, Transform activeTile, string digitName);

    ///<summary>Get the detail level's parent folder path.</summary>
    public string GetParentFolderPath();

    ///<summary>Get the detail level's assets folder path.</summary>
    public string GetAssetsFolderPath();

    ///<summary>Get the detail level's prefabs folder path.</summary>
    public string GetPrefabsFolderPath();

    ///<summary>Get the detail level's materials folder path.</summary>
    public string GetMaterialFolderPath();

    ///<summary>Returns the layer type the user has selected from the inspector.</summary>
    public TileLayerType GetTileLayerType();

    ///<summary>The string separator set from the parent sorter.</summary>
    public string GetStrSeparator();

    ///<summary>Returns the details addressable group.</summary>
    public string GetAddressableGroup();

    public string GetAddressablePath();
}
#endif