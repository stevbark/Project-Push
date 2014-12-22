using UnityEngine;
using System.Collections;

public class placeHolderPlayer : Player {
// Use this for initialization

	unitCreation x = new unitCreation();

	
	void Start () {
	
	}
// Update is called once per frame
void Update () {

}
	
	
public override void TurnOnGUI () {
		float buttonHeight = 50;
		float buttonWidth = 150;
		
	//	x.TurnOnGUI(1,2);
		
		Rect buttonRect = new Rect(0, Screen.height - buttonHeight * 2, buttonWidth, buttonHeight);
		if (GUI.Button(buttonRect, "Recruit Unit")) {
			int costOfUnit=5;
			if(GameManager.instance.currentTeam.amountOfMoney() >=costOfUnit )
			{
		//		if(!GameManager.instance.isSomebodyThere())
				{
					GameManager.instance.currentTeam.removeMoney(costOfUnit);
		//	 		createUserPlayer(xcord,ycord,GameManager.instance.currentTeamIndex); 
					GameManager.instance.createUserPlayer(1,2,GameManager.instance.currentTeamIndex); 
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
		
		
		 buttonRect = new Rect(0, Screen.height - buttonHeight * 1, buttonWidth, buttonHeight);		
		if (GUI.Button(buttonRect, "End Turn")) {
			GameManager.instance.removeTileHighlights();
			actionPoints = 2;
			moving = false;
			attacking = false;			
			GameManager.instance.nextTurn();
		}
	}
}