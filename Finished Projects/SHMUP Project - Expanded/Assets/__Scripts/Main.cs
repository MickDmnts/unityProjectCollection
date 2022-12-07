using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = .5f;
    public float enemyDefaultPadding = 1.5f;
    public WeaponDefinition[] weaponDefinitions; //The WeaponDefinition initialization 

    //============= PowerUp instantiation from enemies ==================\\
    public GameObject powerUpPrefab;
    public WeaponType[] powerUpFrequency = new WeaponType[] {WeaponType.blaster, WeaponType.blaster, WeaponType.spread, WeaponType.shield}; // 50% blaster drop,
                                                                                                                                            // 25% spread drop,
                                                                                                                                            // 25% shield drop


    private BoundsCheck bndsCheck;

    private void Awake()
    {
        S = this;
        bndsCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

        //A generic Dictionary with WeaponType as the key;
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void SpawnEnemy()
    {
        //Instantiate a random Enemy Ship
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        //Position the enemy above the screen with a random x position
        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null) //If the GO has a BoundsCheck attached, take its radius instead.
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius); //if the radius<0, set it equal to its +equivelant
        }

        //Set the initial position for the spawned enemy
        Vector3 pos = Vector3.zero;
        float xMin = -bndsCheck.camWidth + enemyPadding;
        float xMax = bndsCheck.camWidth + enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndsCheck.camHeight + enemyPadding;
        go.transform.position = pos;

        //Call the SpawnEnemy function again
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }

    private void Restart()
    {
        SceneManager.LoadScene("_Scene_0");
    }

    /// <summary>
    /// Static function that gets a WeaponDefinition from the WEAP_DICT static protected field
    /// of the Main Class;
    /// </summary>
    /// <param name="wt">The WeaponType of the desired WeaponDefinition</param>
    /// <returns>The WeaponDefinition or, if there is not WeaponDefinition with WeaponType passed in,
    /// returns a new WeaponDefinition with a WeaponType of DefaultWeapon...;</returns>
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if (WEAP_DICT.ContainsKey(wt)) //check to see if there is a WeaponDefinition of the WeaponType passed in...
        {
            return WEAP_DICT[wt]; //If YES, return it...
        }

        return new WeaponDefinition(); //If NO, return a new WeaponDefinition of the DefaultWeapon
    }

    public void ShipDestroyed(Enemy e)
    {
        //Potentially generate a powerUp
        if (Random.value <= e.powerUpDropChance)
        {
            //Choose which powerUp to pick
            //Pick one from the possibilities in powerUpFrequency
            int ndx = Random.Range(0,powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];

            //Spawn a PowerUp
            GameObject go = Instantiate(powerUpPrefab) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();

            //set the powerUp to the proper weaponType
            pu.SetTypeOfPowerUp(puType);

            //Set the powerUp to the position of the destroyed ship
            pu.transform.position = e.transform.position;
        }
    }
}
