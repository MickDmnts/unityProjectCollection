using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{ 
    public void ChangeLevels(int sceneToLoad)
    {
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(activeScene);
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
