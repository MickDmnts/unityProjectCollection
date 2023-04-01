using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager S;

    [Header("Set in Inspector")]
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Slider loadingSlider;

    public void SetSingleton()
    {
        S = this;
    }

    private void Awake()
    {
        SetSingleton();
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(LoadAsynchronously(scene));
    }

    IEnumerator LoadAsynchronously(string scene)
    {
        AsyncOperation sceneOperation = SceneManager.LoadSceneAsync(scene);


        loadingPanel.SetActive(true);

        while (!sceneOperation.isDone)
        {
            float progress = Mathf.Clamp01(sceneOperation.progress / .9f);
            loadingSlider.value = progress;
            yield return null;
        }

    }
}
