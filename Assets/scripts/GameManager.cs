/*
 * Based on Paul Metcalf's Unity3d tutorial.
 * 
 * Heavily modified for Project Push by Stephen Barkley-Yeung
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * The central game object. Contains the game state. One per game.
 */
public class GameManager : MonoBehaviour
{
    // one and only game Manager. 
	public static GameManager instance;
	public static FogOfWar MapFog = new FogOfWar();
	
	public GameObject TilePrefab;
	public GameObject UserPlayerPrefab;
	public GameObject AIPlayerPrefab;
	public GameObject TestBuildingPrefab;
	public GameObject MedicPrefab;
	
	public int mapSize = 11;
	public List <List<Tile>> map = new List<List<Tile>>();
	public List <Player> players = new List<Player>();
	public List <Player> currentTeamPlayer = new List<Player>();
	public List <Team> teamList = new List<Team>();
	public int currentPlayerIndex = 0;
	public int currentTeamIndex=0;
	public Team currentTeam =  null;
	public int maxTeams=2;
	public unitCreation create;   // TODO: remove from final game, put in different objects that need it.
	public testBuilding testbarracks = new testBuilding(); // TODO: remove after buildings are complete 
	void Awake()
    {
		instance = this;
	}
	

	// Use this for initialization
	void Start ()
    {	
		
		Debug.Log("start");
		generateMap();
		generatePlayers();
		GameManager.instance.fogOfWar();
		create = new unitCreation(); 
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (players[currentPlayerIndex].HP > 0)
        {
            players[currentPlayerIndex].TurnUpdate();
        }
        else nextTurn();
		if(players[currentPlayerIndex].actionPoints<=0)
		{
			players[currentPlayerIndex].renderer.material.color = Color.black;
			currentPlayerIndex = 0;
		}
		
	}
	
    // Called when a player dies
	public void removePlayer(Player p)
	{
		if(players.Contains (p))
		{
			players.Remove(p);
		}
		foreach(Team t in teamList) 
		{
			if(t.doesContainMember(p))
			{
				t.removePlayer(p);
			}
		}
		currentPlayerIndex=0;
		
	}
	
	// tuns on the GUI for the first player on a team
    void OnGUI () 
    {
		players[currentPlayerIndex].TurnOnGUI();
		teamList[currentTeamIndex].TurnOnGUI();
	}
	

	public void selectNewPlayer(Tile t)
	{
		
		foreach(Player p in teamList[currentTeamIndex].getMembers())
		{
            // checks if the player is on the team 
			if(p.gridPosition.x==t.gridPosition.x&&p.gridPosition.y==t.gridPosition.y)
                // checks if the player still has actions left. If the player is out of actions, they cannot be selected.
                if(p.actionPoints>0)
				{
					currentPlayerIndex = players.IndexOf(p);
					Debug.Log("selected "+p.playerName+ "has "+p.actionPoints+" action points and " + p.HP + " HP"); 
				}
		}
        // incomplete
		if(t.gridPosition.x== testbarracks.gridPosition.x &&t.gridPosition.y == testbarracks.gridPosition.y)
		{
			Debug.Log("barracks selected");
			testbarracks.TurnOnGUI();
		}
	}
	
		
	
	// updates the team counter and sets the fog of war for that team.
	public void nextTurn()
    {
		Debug.Log("Team" + currentTeamIndex);
		if(currentTeamIndex + 1 < teamList.Count)
		{
			currentTeamIndex++;
		}
		else
        {
			currentTeamIndex = 0;
		}
		
		currentTeam = teamList[currentTeamIndex];
		currentTeam.nextTurn();
		currentPlayerIndex=0;
		MapFog.fogOfWar(map,currentTeam,players);

	
	}
	
    // highlights every square within a number of squares from the center. Used for attacks and skills. 
	public void highlightTilesAt(Vector2 originLocation, Color highlightColor, int distance)
    {
		List <Tile> highlightedTiles = TileHighlight.FindHighlight(map[(int)originLocation.x][(int)originLocation.y], distance);
		
		foreach (Tile t in highlightedTiles) 
        {
			t.transform.renderer.material.color = highlightColor;
		}
	}
	
    // resets the entire map to white. 
	public void removeTileHighlights()
    {
		for (int i = 0; i < mapSize; i++)
        {
			for (int j = 0; j < mapSize; j++) 
            {
				map[i][j].transform.renderer.material.color = Color.white;
			}
		}
	}
 	

	public void moveCurrentPlayer(Tile destTile) 
    {
        // can only move to a location that is highlighted. 
		if (destTile.transform.renderer.material.color != Color.white&&destTile.transform.renderer.material.color != FogOfWar.colorOfFogOfWar)
        { 
		    // units cannot overlap with other units or move off the map. After moving, need to remove the highlights and update the fog of war
            if (!isSomebodyThere(destTile))
            {
                removeTileHighlights();
                players[currentPlayerIndex].moving = false;
                players[currentPlayerIndex].gridPosition = destTile.gridPosition;
                players[currentPlayerIndex].moveDestination = destTile.transform.position + 1.5f * Vector3.up;
                MapFog.fogOfWar(map, currentTeam, players);
            }
		} 
        else 
        {
			Debug.Log ("destination invalid");
		}
	}
	
    // checks if the tile is occupied by a player
	private bool isSomebodyThere(Tile destTile)
	{
		foreach(Player P in players)
		{
			if(destTile.gridPosition.x == P.gridPosition.x && destTile.gridPosition.y == P.gridPosition.y)
			{
				return true;
			}
				
		}
		return false;
	}
	
	public void attackWithCurrentPlayer(Tile destTile)
    {
        // Checks to see if the tiles are not white or covered with fog since the ability needs to be in range and visible. 
		if (destTile.transform.renderer.material.color != Color.white||destTile.transform.renderer.material.color != FogOfWar.colorOfFogOfWar )
        {
			
			Player target = null;
			foreach (Player p in players)
            {
				if (p.gridPosition == destTile.gridPosition) 
				{
                    // cannot attack teammates
					 if(currentTeam.doesContainMember(p))
					{
						Debug.Log ("He is on your team");
						
					}
					else {target = p;}
				}
			}
			// if target exists
			if (target != null)
            {
								
				//Debug.Log ("p.x: " + players[currentPlayerIndex].gridPosition.x + ", p.y: " + players[currentPlayerIndex].gridPosition.y + " t.x: " + target.gridPosition.x + ", t.y: " + target.gridPosition.y);
				if (players[currentPlayerIndex].gridPosition.x >= target.gridPosition.x - 1 && players[currentPlayerIndex].gridPosition.x <= target.gridPosition.x + 1 &&
					players[currentPlayerIndex].gridPosition.y >= target.gridPosition.y - 1 && players[currentPlayerIndex].gridPosition.y <= target.gridPosition.y + 1)
                {
					players[currentPlayerIndex].actionPoints--;
					
					removeTileHighlights();
					players[currentPlayerIndex].moving = false;			
					players[currentPlayerIndex].attacking = false;

                    // attacks will always hit and currently deal set amount of damage.
					int amountOfDamage = 5;
						
					target.HP -= amountOfDamage;
						
					Debug.Log(players[currentPlayerIndex].playerName + " successfuly hit " + target.playerName + " for " + amountOfDamage + " damage!");
				}
				else
                {
					Debug.Log ("Target is not adjacent!");
				}
			}
		} 
        else 
        {
			Debug.Log ("destination invalid");
		}
		MapFog.fogOfWar(map,currentTeam,players);
	}
	
	// Does the ability of the selected player
	public void doTargetAreaAbility(Tile destTile)
	{
		// Check to see if we are looking at an ability tile
		if (destTile.transform.renderer.material.color != Color.white||destTile.transform.renderer.material.color != FogOfWar.colorOfFogOfWar )
        {	
			// Choose a target
			Player target = null;
			foreach(Player p in players)
			{
				if (p.gridPosition == destTile.gridPosition) 
				{
				//	currentTeam.playerList
					if(currentTeam.doesContainMember(p))
					{
						target = p;
					}
				}
			}
			// If we found one
			if(target != null)
			{
				// Check to see if our current player is a medic
				if(players[currentPlayerIndex].unitType.Equals ("Medic"))	
				{
					// If our target needs to be head, heal them
					if(target.HP < target.maxHP)
					{
						//removeTileHighlights();
						players[currentPlayerIndex].actionPoints--;
						players[currentPlayerIndex].abilityActive = false;
						target.HP += players[currentPlayerIndex].abilityAmount;
						
						if(target.HP > target.maxHP)
						{
							target.HP = target.maxHP;
						}
					}
					else
                    {
						Debug.Log ("This unit is at full health");
						players[currentPlayerIndex].abilityActive = false;
					}
				}
			}
			else
			{
				Debug.Log ("Target is not valid");
			}
		}
		MapFog.fogOfWar (map, currentTeam, players);
	}
	
    // Creates the map. Currently an 11 by 11 sqare map. 
	void generateMap()
    {
		Debug.Log("generateMap");
		map = new List<List<Tile>>();
		for (int i = 0; i < mapSize; i++)
        {
			List <Tile> row = new List<Tile>();
			for (int j = 0; j < mapSize; j++) 
            {
				Tile tile = ((GameObject)Instantiate(TilePrefab, new Vector3(i - Mathf.Floor(mapSize/2),0, -j + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
				tile.gridPosition = new Vector2(i, j);
				row.Add (tile);
			}
			map.Add(row);
		}
		
	}
	
    // creates the units that start on the board
	void generatePlayers() 
    {
		teamList.Add(new Team());
		teamList.Add(new Team());
		UserPlayer player;
		players.Add(new placeHolderPlayer());
		
	/*	player = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(0 - Mathf.Floor(mapSize/2),1.5f, -0 + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
		player.gridPosition = new Vector2(0,0);
		player.playerName = "Bob";
		teamList[0].addMember(player);
		players.Add(player);
	*/	
		
		//create.importPrefab( UserPlayerPrefab);
	//	player = (UserPlayer)createUserPlayer(new Vector3(0 - Mathf.Floor(mapSize/2),1.5f, -0 + Mathf.Floor(mapSize/2)), new Vector2(0,0),0);
		player = (UserPlayer)createUserPlayer(0,0,0);
	//	teamList[0].addMember(player);
	//	player.playerName = "Bob";
	//	players.Add(player);
		
		player = (UserPlayer)createUserPlayer(1,5,1);
	//	teamList[1].addMember(player);
	//	player.playerName = "Bob";
	//	players.Add(player);
		
		
		player = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3((mapSize-1) - Mathf.Floor(mapSize/2),1.5f, -(mapSize-1) + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
		player.gridPosition = new Vector2(mapSize-1,mapSize-1);
		
		player = ((GameObject)Instantiate(MedicPrefab, new Vector3((mapSize-1) - Mathf.Floor(mapSize/2),1.5f, -(mapSize-1) + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<Medic>();
		player.gridPosition = new Vector2(mapSize-1,mapSize-1);
		player.playerName += " Kyle";
		teamList[0].addMember(player);
		players.Add(player);

		
		player = ((GameObject)Instantiate(MedicPrefab, new Vector3(4 - Mathf.Floor(mapSize/2),1.5f, -4 + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<Medic>();
		player.gridPosition = new Vector2(4,4);
		player.playerName += " Lars";
		teamList[1].addMember(player);
		players.Add(player);
		
		// this is a test building and will be removed once the building class is complete
	//	testBuilding x = new testBuilding();
	//	x = ((GameObject)Instantiate(TestBuildingPrefab,  new Vector3((1) - Mathf.Floor(mapSize/2),1.5f, -(1) + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<testBuilding>();
	//	x.gridPosition = new Vector2(1,1);
	//	testbarracks  =x;
		
		
	//	bool x = players.Contains (player);
	//	bool x =players.Contains(teamList[0].playerList[0]);
		
		currentTeam = teamList[0];
		
	//	bool x = teamList[0].playerList.Contains(teamList[0].playerList[0]);
	//	Debug.Log(x);
		
	//	player = ((GameObject)Instantiate(ScoutPrefab, new Vector3(6 - Mathf.Floor(mapSize/2),1.5f, -6 + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<Scout>();
	//	player.gridPosition = new Vector2(6,6);
		
	//	players.Add(player);
		
		//AIPlayer aiplayer = ((GameObject)Instantiate(AIPlayerPrefab, new Vector3(6 - Mathf.Floor(mapSize/2),1.5f, -4 + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<AIPlayer>();
		
		//players.Add(aiplayer);
	}

    // colors the selected unit's square white
	public void highlightCurrentSquare(Player P)
	{
		for (int i = 0; i < mapSize; i++)
        {
			for (int j = 0; j < mapSize; j++)
            {
				if(i==P.gridPosition.x&&j==P.gridPosition.y)
				{
						map[i][j].transform.renderer.material.color = Color.white;
				}
			}
		}
	}
	public void fogOfWar()
	{
		MapFog.fogOfWar(map,currentTeam,players);
	}
	
	
	// creates a new player object. will be replaced with create New Unit once the game is complete. 
	public Player createUserPlayer(int xCordinate, int yCordinate,int teamNumber) //this will probably be removed once the game is done.
	{
		return createNewUnit(UserPlayerPrefab,xCordinate, yCordinate, teamNumber);
	}
	

    // Creates a new unit. Still incomplete
	public Player createNewUnit(GameObject UnitPrefab, int x, int y, int teamNumber) 
	{

		Player player;
		player = ((GameObject)Instantiate(UnitPrefab, new Vector3(x - Mathf.Floor(mapSize/2),1.5f, -y + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
		player.gridPosition = new Vector2(x,y);
		player.playerName = "Bob";				
		players.Add(player);
		teamList[teamNumber].addMember(player);
		player.sight();
		MapFog.RevealingSeenUnits(map,players);
		return player;

	}
	
}
