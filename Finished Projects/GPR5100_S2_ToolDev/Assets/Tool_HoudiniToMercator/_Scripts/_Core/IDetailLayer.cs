#if UNITY_EDITOR
using UnityEngine;

public interface IDetailLayer
{
    ///<summary>Returns true if the passed tile is suitable to be used with this layers' pre-requisites</summary>
    public bool IsTileSuitable(Transform activeTile);

    ///<summary>Copies, Renames and correctly handles the passed active tile based on layer format.</summary>
    public void HandleTile(Transform activeTile, string digitName, string prefix);
}
#endif