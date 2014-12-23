using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FogOfWar : MonoBehaviour
{
	public static readonly Color colorOfFogOfWar= Color.grey;
	// Use this for initialization
    // this will be called by game manager when the game in intialized and it is the same as other times the game is being fogged out. As a result this does not need to do anything
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	
	
	public void fogOfWar(List<List<Tile>> map , Team currentTeam, List <Player> players )
	{
		fogOutMap(map);
		currentTeam.teamSight();
		HidingUnseenUnits(map,players);
		RevealingSeenUnits(map,players);
	}
	
	// set all squares to the fog of war color
	private void fogOutMap(List <List<Tile>> map)
	{
		for(int i=0; i<map.Count;i++)
		{
			List<Tile> mapRow= map[i];
			for(int j=0; j<mapRow.Count;j++)
			{
				mapRow[j].transform.renderer.material.color = colorOfFogOfWar;
			}
		}
	}
	
	// hides any characters who's squares are colored the fog of war color because those characters cannot be seen.
	private void HidingUnseenUnits(List <List<Tile>> map,List <Player> players)
	{
		foreach (Player P in players)
		{
			if(map [(int)P.gridPosition.x][(int)P.gridPosition.y].transform.renderer.material.color==colorOfFogOfWar) 
			{
				if(P != null)
				{
					P.renderer.enabled = false;
				}
		
				
				Debug.Log (P.playerName + " is in fog of war");
			}
		}
	}
	
    // this wil reveal units that can be seen
	public void RevealingSeenUnits(List <List<Tile>> map,List <Player> players)
	{
		foreach (Player P in players)
		{
			if(map [(int)P.gridPosition.x][(int)P.gridPosition.y].transform.renderer.material.color==Color.white) 
			{
				if(P != null)
				{
					P.renderer.enabled = true;
				}
				
				Debug.Log (P.playerName + " is NOT in the fog of war");
			}
		}
	}
}

