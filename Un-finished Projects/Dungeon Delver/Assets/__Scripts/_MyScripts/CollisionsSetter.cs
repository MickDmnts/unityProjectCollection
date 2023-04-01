using UnityEngine;

public class CollisionsSetter : MonoBehaviour
{
    static private BoxCollider bColl;

    static public void SetCollider(Tile tileToConfigure)
    {
        bColl = tileToConfigure.bCollider;
        bColl.enabled = true;
        char c = TileCamera.COLLISIONS[tileToConfigure.tileNum];
        switch (c)
        {
            case 'S': //Whole
                bColl.center = Vector3.zero;
                bColl.size = Vector3.one;
                break;
            case 'W': //Top
                bColl.center = new Vector3(0f, .25f, 0f);
                bColl.size = new Vector3(1f, 5f, 1f);
                break;
            case 'A': //Left
                bColl.center = new Vector3(-.25f, 0, 0);
                bColl.size = new Vector3(.5f, 1, 1);
                break;
            case 'D': //Right
                bColl.center = new Vector3(.25f, 0, 0);
                bColl.size = new Vector3(.5f, 1, 1);
                break;
            default: //_,|, etc.
                bColl.enabled = false;
                break;
        }
    }
}
