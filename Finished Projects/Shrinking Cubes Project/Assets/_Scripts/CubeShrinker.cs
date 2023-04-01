using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeShrinker : MonoBehaviour
{
    public GameObject cubePrefab; //The cube prefab
    public float timeUntilExit = 2f; //Timer to exit play mode
    public List<GameObject> cubesList; //The list of the cubes
    public List<GameObject> removeList;

    private GameObject _obj; //The G.O. cube
    private int _numberOfCubes = 0; //Number of cubes
    private bool _stillCreating = true; //Checks if cubes are still being created
    private bool _stoppedCreating = false; //When true, stop creating cubes
    private bool _allBlack = false;
    private bool _endAnim = false;


    void Start () {
        cubesList = new List<GameObject>(); //Inits the List of Cubes
        removeList = new List<GameObject>();
	}
	
	
	void Update () {

        if (_stillCreating == true)
        {
            _numberOfCubes++; //Increment the number of cubes

            _obj = Instantiate(cubePrefab); //Instantiate the cube
            _obj.name = "Cube " + _numberOfCubes; //Name the cube
            cubesList.Add(_obj); //populates the list with the cubes created

            Color c = new Color(Random.value, Random.value, Random.value); //Changes the color
            _obj.GetComponent<Renderer>().material.color = c;

            _obj.transform.position = Random.insideUnitSphere; //Inst/tes the cubes in a sphere-like maner

        }

        if (_numberOfCubes == 180 && _stillCreating == true)
        {
            _stillCreating = false;
            _stoppedCreating = true;

            foreach (GameObject cubes in cubesList)
            {
                cubes.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            }
        }

        if (_stoppedCreating == true) //Start the black color switcher
        {
            StartCoroutine(ColorSwitch());
            _stoppedCreating = false; 
        }

        if (_endAnim == true) //End the animation
        {
            timeUntilExit -= Time.deltaTime;
            foreach (GameObject cubeToRemove in removeList)
            {
                cubesList.Remove(cubeToRemove);
                Destroy(cubeToRemove);
            }

            removeList.Clear();

            if (timeUntilExit < 0)
            {
                Application.Quit();
            }
        }
	}

    IEnumerator ColorSwitch() { //After all the cubes are frozen, they start turning black
        foreach (GameObject cube in cubesList)
        {
            Color c = new Color(0f,0,0f); //Black
            cube.GetComponent<Renderer>().material.color = c;
            yield return new WaitForSeconds(0.02f);
        }
        _allBlack = true;
    }

    void CubeShrink(GameObject cube) { //After the cubes have turned black, start turning them yellow and shrink them

        cube.GetComponent<Renderer>().material.color = new Color(204f, 5f, 0f); //Yellow

        Vector3 pushCubeUp = new Vector3(0f, 0.2f, 0f);
        cube.GetComponent<Transform>().localPosition += pushCubeUp;

        float scalingFactor = 0.95f;
        float cubeScale = cube.transform.localScale.x;
        cubeScale *= scalingFactor;
        cube.transform.localScale = Vector3.one * cubeScale;

        if (cubeScale <= 0.1f)
        {
            removeList.Add(cube);
        }
    }
}
