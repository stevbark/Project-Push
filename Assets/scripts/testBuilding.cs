using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameObject))]
public class testBuilding : MonoBehaviour
{
	public Vector2 gridPosition = Vector2.zero;
	public unitCreation create; 
	
	// Use this for initialization
	void Start ()
	{
		create = new unitCreation();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void TurnOnGUI () 
	{ 
		float buttonHeight = 50;
		float buttonWidth = 150;
		Rect buttonRect = new Rect(0, Screen.height - buttonHeight * 2, buttonWidth, buttonHeight);
		if (GUI.Button(buttonRect, "Recruit Unit")) {
			int costOfUnit=5;
			if(GameManager.instance.currentTeam.amountOfMoney() >=costOfUnit )
			{
		//		if(!GameManager.instance.isSomebodyThere())
				{
					GameManager.instance.currentTeam.removeMoney(costOfUnit);
		//	 		createUserPlayer(xcord,ycord,GameManager.instance.currentTeamIndex); 
//					createUserPlayer(1,2,GameManager.instance.currentTeamIndex); 
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
	//	Debug.Log("execute GUI");
	//	create.TurnOnGUI((int)gridPosition.x,(int)gridPosition.y);
//		create.TurnOnGUI();
	
}

