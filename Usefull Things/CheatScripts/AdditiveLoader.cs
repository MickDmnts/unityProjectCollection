using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WGR.Core
{
    /*public enum GameScenes
    {
        DoNotUnload = -1,

        GameEntryScene = 0,
        MainMenu = 1,
        UI_Scene = 2,
        PlayerScene = 3,
        PlayerHub = 4,
        //Level0 = 5,
        Level1 = 5,
        Level2 = 6,
        Level3 = 7,
        Level4 = 8,
        Level5 = 9,
        BossRoom = 10,
    }*/

    public class AdditiveLoader : MonoBehaviour
    {
        /// <summary>
        /// When the ActiveScene field is SET it automatically 
        /// calls the OnSceneChanged() event;
        /// </summary>
        private GameScenes[] _activeScenes;
        public GameScenes[] ActiveScene
        {
            get { return _activeScenes; }
            private set
            {
                _activeScenes = value;

                GameManager.S.GameEventHandler.OnSceneChanged();
            }
        }

        public void LoadScene(GameScenes toBeUnloaded = GameScenes.DoNotUnload, params GameScenes[] toBeLoaded)
        {
            //INIT LOADING SCREEN HERE

            if (toBeUnloaded != GameScenes.DoNotUnload)
            {
                SceneManager.UnloadSceneAsync((int)toBeUnloaded);
            }

            foreach (GameScenes scene in toBeLoaded)
            {
                SceneManager.LoadScene((int)scene, LoadSceneMode.Additive);
            }

            //STOP LOADING SCREEN HERE
            //we should also notify the other components that the scene has changed

            //DEBUG - change later
            ActiveScene = toBeLoaded;
        }
    }
}