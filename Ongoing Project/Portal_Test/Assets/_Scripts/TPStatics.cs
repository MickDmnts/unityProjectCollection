using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TPStatics : MonoBehaviour
{
    [SerializeField] public Slider tp_slider;
    [SerializeField] public static int activeTeleporters = 0;
    [SerializeField] public static float timer = 0f;
    [SerializeField] public Transform[] tp_points;
}
