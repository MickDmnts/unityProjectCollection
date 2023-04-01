using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControllerScene2 : MonoBehaviour
{
    LevelChange sceneChanger = new LevelChange();

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            sceneChanger.ChangeLevels(0);
        }
    }
}
