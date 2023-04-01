using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class TillingManager : MonoBehaviour
{
    [Header("Set in inspector")]
    public int xOffset = 2;

    [Header("Set dynamically")]
    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;
    public bool reverseScale = false;

    private float spriteWidth = 0f;
    private Camera mainCamera;
    private Transform myTransform;
    private SpriteRenderer spriteRenderer;
    private ParallaxManager parallaxManager;

    private void Awake()
    {
        SetMainCameraReference();
        SetMyTransform();
        SetSpriteRendererReference();
        SetParallaxManagerObject();
    }

    private void Start()
    {
        GrabSpriteBounds();
    }

    private void Update()
    {
        CheckIfNecessaryToInstantiateBuddy();
    }

    void CheckIfNecessaryToInstantiateBuddy()
    {
        if (hasALeftBuddy == false || hasARightBuddy == false)
        {
            float cameraHorizontalExtend = mainCamera.orthographicSize * Screen.width / Screen.height;

            float edgeVisiblePositionToTheRight = (myTransform.position.x + spriteWidth / 2) - cameraHorizontalExtend;
            float edgeVisiblePositionToTheLeft = (myTransform.position.x - spriteWidth / 2) + cameraHorizontalExtend;

            if (mainCamera.transform.position.x >= edgeVisiblePositionToTheRight - xOffset && hasARightBuddy == false)
            {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }
            else if (mainCamera.transform.position.x <= edgeVisiblePositionToTheLeft + xOffset && hasALeftBuddy == false)
            {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
    }

    void MakeNewBuddy(int directionToInstantiate)
    {
        Vector3 whereToInstantiate = new Vector3(myTransform.position.x + spriteWidth * directionToInstantiate, myTransform.position.y, myTransform.position.z);
        Transform newBuddy = (Transform)Instantiate(myTransform, whereToInstantiate, myTransform.rotation);
        parallaxManager.AddBackgroundToTheList(newBuddy);

        if (reverseScale == true)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y,newBuddy.localScale.z);
        }

        SetNewBuddyParentTransform(newBuddy);

        if (directionToInstantiate > 0)
        {
            newBuddy.GetComponent<TillingManager>().hasALeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<TillingManager>().hasARightBuddy = true;
        }
    }

    void GrabSpriteBounds()
    {
        spriteWidth = spriteRenderer.sprite.bounds.size.x;
    }

    void SetMainCameraReference()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void SetMyTransform()
    {
        if (myTransform == null)
            myTransform = this.gameObject.transform;
    }

    void SetSpriteRendererReference()
    {
        if (spriteRenderer == null)
            spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    void SetNewBuddyParentTransform(Transform newBuddy)
    {
        newBuddy.parent = myTransform.parent;
    }

    void SetParallaxManagerObject()
    {
        parallaxManager = GameObject.Find("ParallaxManager").GetComponent<ParallaxManager>();
        if (parallaxManager == null)
        {
            Debug.Log("Can\'t find parallax manager");
        }
    }
}
