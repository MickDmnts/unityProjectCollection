using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthHandler : MonoBehaviour
{
    public int playerMaxHealth = 100;

    [Header("Set Dynamically")]
    public GameObject playerObject;
    public int currentHealth;
    public HealthBarHandler healthBarHandler;

    private void Awake()
    {
        FindPlayerObjectInHierarchy();
        AssignHealthBarScriptReference();
        SetCurrentHealthEqualToMax();
    }

    void FindPlayerObjectInHierarchy()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player").gameObject;
    }

    void AssignHealthBarScriptReference()
    {
        healthBarHandler = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBarHandler>();
    }

    void SetCurrentHealthEqualToMax()
    {
        currentHealth = playerMaxHealth;
    }

    private void Start()
    {
        healthBarHandler.SetMaxHealth(playerMaxHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DamagePlayerByAmount(20);
            healthBarHandler.SetHealth(currentHealth);
            IsPlayerDead();
        }
    }

    void DamagePlayerByAmount(int damageAmount)
    {
        currentHealth -= damageAmount;
    }

    void IsPlayerDead()
    {
        if (currentHealth <= 0)
        {
            Destroy(playerObject);
            Debug.LogError("PLAYER DIED");
        }
    }
}
