using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Team : MonoBehaviour
{
	public List<Player> playerList = new List<Player>();
	Player currentPlayer = new Player();
	private finance currentMoney = new finance();
	// Use this for initialization
	
	
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void SetUpTeam(List<Player> players, int startingMoney)
	{
		foreach(Player P in players)
		{
			playerList.Add(P);
		}
		
		currentMoney.setMoney(startingMoney);
	}
	
	public void addMoney(int amountAdded)
	{
		currentMoney.addMoney(amountAdded);
	}
	
	public bool removeMoney(int amountRemoved) // tells whether amount removed can be afforded
	{
		return currentMoney.removeMoney(amountRemoved);
	}
	public int amountOfMoney()
	{
		return currentMoney.amountOfMoney();
	}

	public void addMember(Player player)
	{
		playerList.Add(player);
	}
	
	public void removePlayer(Player player)
	{
		playerList.Remove(player);
	}
	
	public List<Player> getMembers()
	{
		return playerList;
	}
	
	public bool doesContainMember(Player player)
	{
		return playerList.Contains(player);
	}
	
	public bool setCurrentPlayer(Player p)
	{
		if(doesContainMember(p))
		{
			currentPlayer = playerList[playerList.IndexOf(p)];
			return true;
		}
				
		return false;
		
	}
	public Player getCurrentPlayer()
	{
		return currentPlayer;
	}
	
	public void teamSight()
	{
		foreach(Player P in playerList)
		{
			P.sight();
			Debug.Log (P.playerName + " can see");
		}
	}
	
/*	public void proveOnTeam()  // this is to see who is on the team
	{
		foreach(Player P in playerList)
		{
			P.renewActionPoints();	
			Debug.Log("player" + P.playerName + "is on this Team");
			P.sight();
		}
	}
*/	
	public void nextTurn () 
	{
		foreach(Player P in playerList)
		{
			P.renewActionPoints();	
			Debug.Log("player" + P.playerName + "is on this Team");
			P.moving = false;
			P.attacking = false;
		}
		
		
	}
	
	public void TurnOnGUI () 
	{
		
		float buttonHeight = 50;
		float buttonWidth = 150;
		Rect buttonRect = new Rect (0,0,buttonWidth,buttonHeight);
		if (GUI.Button(buttonRect, "money = "+ currentMoney.amountOfMoney() )) 
		{
		}
		
		buttonRect = new Rect(0, Screen.height - buttonHeight * 1, buttonWidth, buttonHeight);		
		
		if (GUI.Button(buttonRect, "End Turn")) {		
			GameManager.instance.nextTurn();
		}
		
	}
	
	
		
//	*/
//	void onMouseDown()
//	{
//		foreach(Player p in playerList)
//		{
			//if(p.gridPosition.x.Equals(tile.gridPosition.x) && p.gridPosition.y.Equals(tile.gridPosition.y))
		//	{
		//		currentPlayer = p;
		//		Debug.Log ("unit selected");
		//	}
//		}
//	}
	
	
}

