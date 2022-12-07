using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplePicker : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject basketPrefab;
    public int numOfBaskets = 3;
    public float basketBottomY = -14f;
    public float basketSpacingY = 2f;
    public List<GameObject> basketList;

    private void Start()
    {
        basketList = new List<GameObject>();
        for (int i = 0; i < numOfBaskets; i++)
        {
            GameObject basket = Instantiate<GameObject>(basketPrefab); //The lives of Basket
            Vector3 pos = Vector3.zero;
            pos.y = basketBottomY + (basketSpacingY * i);
            basket.transform.position = pos;
            basketList.Add(basket);
        }
    }

    public void AppleDestroyed()
    {
        GameObject[] applesToDestroy = GameObject.FindGameObjectsWithTag("Apple"); //Gathers all the Apples in the scene
        foreach (GameObject apple in applesToDestroy)
        {
            Destroy(apple); //then destroys them to create a Reset effect
        }

        int basketIndex = basketList.Count - 1; //Removes 1 basket
        GameObject tBasketGO = basketList[basketIndex];
        basketList.RemoveAt(basketIndex);
        Destroy(tBasketGO);

        if (basketList.Count == 0)
        {
            SceneManager.LoadScene("_Scene_0");
        }
    }
}