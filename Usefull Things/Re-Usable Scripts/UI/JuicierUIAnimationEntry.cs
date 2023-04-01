using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuicierUIAnimationEntry : MonoBehaviour
{
    [Header("Booleans")]
    public bool executeOnStart = false;
    public bool hasOffset = false;

    [Space(10)]
    [Header("Animation Duration")]
    public float animationDuration = 1f;

    [Header("Delay Before Animation")]
    public float animationDelay = 0;

    [Space(10)]
    [Header("Position")]
    public Vector3 startingPos;
    public AnimationCurve posCurve;

    [Space(10)]
    [Header("Scale")]
    public Vector3 startingScale;
    public AnimationCurve scaleCurve;

    Vector3 endingPos;
    Vector3 endingScale;

    private void Awake()
    {
        if (!executeOnStart)
        {
            SetEndingVariables();
            StartCoroutine(Animation());
        }
    }

    private void Start()
    {
        if (executeOnStart)
        {
            SetEndingVariables();
            StartCoroutine(Animation());
        }
    }

    void SetEndingVariables()
    {
        endingPos = transform.localPosition;
        endingScale = transform.localScale;

        if (hasOffset)
        {
            startingPos += endingPos;
        }
    }

    IEnumerator Animation()
    {
        transform.localPosition = startingPos;
        transform.localScale = startingScale;
        yield return new WaitForSecondsRealtime(animationDelay);

        //Useful vars
        float time = 0;
        float percentage = 0;
        float lastTime = Time.realtimeSinceStartup;

        do
        {
            time += Time.realtimeSinceStartup - lastTime;
            lastTime = Time.realtimeSinceStartup;
            percentage = Mathf.Clamp01(time / animationDuration);
            Vector3 temporaryPosition = Vector3.LerpUnclamped(startingPos, endingPos, posCurve.Evaluate(percentage));
            Vector3 temploraryScale = Vector3.LerpUnclamped(startingScale, endingScale, scaleCurve.Evaluate(percentage));
            transform.localPosition = temporaryPosition;
            transform.localScale = temploraryScale;
            yield return null;
        } while (percentage < 1);

        //Just in case it misses some frames 
        transform.localScale = endingScale;
        transform.localPosition = endingPos;
        yield return null;
    }
}
