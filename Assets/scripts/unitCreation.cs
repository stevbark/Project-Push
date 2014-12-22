using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameObject))]
public class unitCreation : MonoBehaviour
{
	public  GameObject UserPlayerPrefab;
	public  GameObject AIPlayerPrefab;
	

	// Use this for initialization
	void Start ()
	{
		UserPlayerPrefab = GameManager.instance.UserPlayerPrefab;
		AIPlayerPrefab = GameManager.instance.AIPlayerPrefab;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

//	public void TurnOnGUI (int xcord, int ycord) {
	public void TurnOnGUI () {
		float buttonHeight = 50;
		float buttonWidth = 150;
	//	Debug.Log("in unit create");
		
		Rect buttonRect = new Rect(0, Screen.height - buttonHeight * 2, buttonWidth, buttonHeight);
		if (GUI.Button(buttonRect, "Recruit Unit")) {
			int costOfUnit=5;
			if(GameManager.instance.currentTeam.amountOfMoney() >=costOfUnit )
			{
		//		if(!GameManager.instance.isSomebodyThere())
				{
					GameManager.instance.currentTeam.removeMoney(costOfUnit);
		//	 		createUserPlayer(xcord,ycord,GameManager.instance.currentTeamIndex); 
					createUserPlayer(1,2,GameManager.instance.currentTeamIndex); 
				}
		//		else 
				{
					Debug.Log("somebody is there already");
				}
			}
			else
			{
				Debug.Log("Team " +GameManager.instance.currentTeamIndex + "not enough money");
			}
		}
	}
	
	public void importPrefab(GameObject Prefab)
	{
		if(Prefab !=null)
		{
	//	UserPlayerPrefab = GameObject.FindGameObjectsWithTag(Prefab.GetComponent.tag);
		}
	//	UserPlayerPrefab = (GameObject) this.GetComponent(typeof(Prefab));
	}
	

/*	Player createSoldier() // these will be created once units have been made using createNewUnit() method
	{
		
	}
	
	Player createScout()
	{
		
	}
	
	Player createMedic()
	{
		
	}
	
	Player createHeavy()
	{
		
	}
	
	Player createSniper()
	{
		
	}
	
	Player createJeep()
	{
		
	}
	
	Player createAPC()
	{
		
	}

	Player createTank()
	{
		
	}
	
		*/	

/*	public Player createUserPlayer(int xCordinate, int yCordinate,int teamNumber) //this will probably be removed once the game is done.
	{
		return createNewUnit(UserPlayerPrefab,xCordinate, yCordinate, teamNumber);
	}
*/	
	//public Player createNewUnit(GameObject UnitPrefab, Vector3 startPosition, Vector2 gridPosition, int teamNumber)
	
	public Player createUserPlayer(int x, int y, int teamNumber)  // removed once the problems with the Prefabs is solved. we should be using the code below.
	{
		return GameManager.instance.createUserPlayer( x,  y,  teamNumber);
	}
/*	public Player createNewUnit(GameObject UnitPrefab, int x, int y, int teamNumber) 
	{
		if(UnitPrefab !=null)
		{
			Player player;
			player = ((GameObject)Instantiate(UnitPrefab, new Vector3(x - Mathf.Floor(GameManager.instance.mapSize/2),1.5f, -y + Mathf.Floor(GameManager.instance.mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
			player.gridPosition = new Vector2(x,y);
			player.playerName = "Bob";				
			GameManager.instance.players.Add(player);
			GameManager.instance.teamList[teamNumber].addMember(player);
			return player;
		}
		return null;
	}	
*/
	
	
	
	
}


