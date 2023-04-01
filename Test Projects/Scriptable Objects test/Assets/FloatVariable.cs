using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    protected float InitialValue;

    [NonSerialized]
    private float runtimeValue;
    public float RuntimeValue { get => runtimeValue; }

    public void OnAfterDeserialize()
    {
        runtimeValue = InitialValue;
    }

    public void OnBeforeSerialize(){}

    public void SubtractValue(float value) => runtimeValue -= value;
}
