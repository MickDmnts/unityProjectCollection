using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Handles every and each castlePosition action such as:
///OnMouseEnter 
///OnMouseExit
///etc </summary>
public class PositionActivator : MonoBehaviour 
{
	public static PositionActivator S;//Singleton

	[Header("Set Dynamically")]
	public int indexInArray;

    //Private Vars\\
    [Header("Private Variables: Do not modify")]
	[SerializeField] MeshRenderer currentPosMeshRenderer;
	[SerializeField] Light currentPosLight;
    [SerializeField] int _currentPos;
	[SerializeField] bool _activePos = false;

	//Property\\
	public bool ActivePos
	{
		set
		{
			_activePos = value;
		}
	}

    //MAIN SCRIPT\\
    void Awake()
	{
		S = this;		
		_activePos = false;
	}

	void Start()
	{

		indexInArray = System.Array.IndexOf(GameController.S.castlePositions,this.gameObject);
		currentPosMeshRenderer = GetComponent<MeshRenderer>();
		currentPosLight = GetComponent<Light>();

		//Deactive the light and M.R. components
		currentPosMeshRenderer.enabled = false;
		currentPosLight.enabled = false;
	}

	//============================Mouse Events============================\\
	public void OnMouseEnter()
	{
		if(indexInArray != _currentPos && _activePos)
		{
			currentPosMeshRenderer.enabled = true;
			currentPosLight.enabled = true;				
		}
	}

	void OnMouseDown()
	{
		if (indexInArray != _currentPos && _activePos) //If the player is not in this position AND the position is active, move him to the clicked pos
		{
            //Call ReAssignCurrentPositionOfThePlayer() telling him that the player is heading to this clicked position
            ReAssignCurrentPositionOfThePlayer(this.indexInArray); 
            currentPosMeshRenderer.enabled = false;
            currentPosLight.enabled = false;

            //Player handles everything from here
            Player.S.p0 = Player.S.POS;
			Player.S.p1 = this.transform.position;
			Player.S.TimeStart = Time.time;

			//Tell the player to move
			Player.S.positionMove = true;
			Shoot.S.CanShoot = false; //While the player is moving , he can not shoot 
		}
	}
	
	void OnMouseExit()
	{
		currentPosMeshRenderer.enabled = false;
		currentPosLight.enabled = false;
	}

    //==================================Custom Functions==================================\\

    ///<summary>Called every time the player changes position with LMB click to tell every position where is the player heading.</summary>
    public void ReAssignCurrentPositionOfThePlayer(int posToGo)
    {
        foreach (GameObject tempItem in GameController.S.castlePositions)
        {
            tempItem.GetComponent<PositionActivator>()._currentPos = posToGo;
        }
    }
}
