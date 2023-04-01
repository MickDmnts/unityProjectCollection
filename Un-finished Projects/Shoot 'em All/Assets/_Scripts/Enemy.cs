using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in inspector")]
    [SerializeField]
    int enemyMaxHealth = 100;
    public float showDamageDuration = 0.1f;

    [Header("Set Dynamically")]
    [SerializeField]
    int currentHealth;
    public Color[] originalColors;
    public Material[] materials;
    public bool showingDamage = false;
    public float damageDoneTime;

    private void Awake()
    {
        GetAllMaterialsAndOriginalColors();
        SetCurrentHealthToMax();
    }

    void GetAllMaterialsAndOriginalColors()
    {
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }

    void SetCurrentHealthToMax()
    {
        currentHealth = enemyMaxHealth;
    }

    private void Update()
    {
        if (IsEnemyDead())
        {
            Destroy(gameObject);
        }

        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }
    }

    bool IsEnemyDead()
    {
        if (currentHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DamageEnemyByValue(int value)
    {
        currentHealth -= value;
        ShowDamage();
    }

    void ShowDamage()
    {
        foreach (Material mat in materials)
        {
            mat.color = Color.red;
        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }

    void UnShowDamage()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }
}
