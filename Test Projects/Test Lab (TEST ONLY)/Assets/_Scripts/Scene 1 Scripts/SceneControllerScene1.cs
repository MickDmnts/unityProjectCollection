using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControllerScene1 : MonoBehaviour
{
    LevelChange sceneChanger = new LevelChange();

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            sceneChanger.ChangeLevels(1);
        }
    }
}
