using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
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
	public unitCreation create;   // removed from final game, put in different objects that need it.
	public testBuilding testbarracks = new testBuilding(); // removed after buildings are complete 
	void Awake() {
		instance = this;
	}
	

	// Use this for initialization
	void Start () {	
		
		Debug.Log("start");
		generateMap();
	//	GameManager.instance.highlightTilesAt(new Vector2(5,5) ,Color.green, 10 );
		generatePlayers();
		GameManager.instance.fogOfWar();
		create = new unitCreation(); 
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (players[currentPlayerIndex].HP > 0) players[currentPlayerIndex].TurnUpdate();
//		player p = teamList[currentTeamIndex].getCurrentPlayer();
//		if(p.actionPoints<=0)
		//{
		/*	foreach (Player player in teamList[currentTeamIndex].getMembers())
			{
				if(player.actionPoints>0)
				{
					
				}
			}*/
		//}
		else nextTurn();
		if(players[currentPlayerIndex].actionPoints<=0)
		{
			players[currentPlayerIndex].renderer.material.color = Color.black;
			currentPlayerIndex = 0;
	//		currentPlayerIndex=;
		}
		
	}
	
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
	
	void OnGUI () {
	//	if (players[currentPlayerIndex].HP > 0) players[currentPlayerIndex].TurnOnGUI();
		players[currentPlayerIndex].TurnOnGUI();
		teamList[currentTeamIndex].TurnOnGUI();
	}
	
	public void selectNewPlayer(Tile t)
	{
		
		foreach(Player p in teamList[currentTeamIndex].getMembers())
		{
			if(p.gridPosition.x==t.gridPosition.x&&p.gridPosition.y==t.gridPosition.y)
				if(p.actionPoints>0)
				{
					currentPlayerIndex = players.IndexOf(p);
					Debug.Log("selected "+p.playerName+ "has "+p.actionPoints+" action points and " + p.HP + " HP"); 
				}
		}
		if(t.gridPosition.x== testbarracks.gridPosition.x &&t.gridPosition.y == testbarracks.gridPosition.y)
		{
			Debug.Log("barracks selected");
			testbarracks.TurnOnGUI();
		}
	}
	
		


	//	public Vector2 getPlayer (Tile tile)
//	{
//		foreach (Player p in players)
//			if(p.gridPosition.x.Equals(tile.gridPosition.x) && p.gridPosition.y.Equals(tile.gridPosition.y)&&p.team == currentTeam)
//		{
//				return p.gridPosition;
//		}
//		return new Vector2(-1,-1);
//	}
	
	
	public void nextTurn() {
		Debug.Log("Team" + currentTeamIndex);
	//	fogOutMap();
		if(currentTeamIndex + 1 < teamList.Count)
		{
			currentTeamIndex++;
		}
		else{
			currentTeamIndex = 0;
		}
		
		currentTeam = teamList[currentTeamIndex];
		currentTeam.nextTurn();
	//	Player p = currentTeam.getMembers()[0];
		currentPlayerIndex=0;
		MapFog.fogOfWar(map,currentTeam,players);
		//fogOfWar();
	//	GameManager.instance.highlightTilesAt(new Vector2(5,5) ,Color.green, 10 );
	/*	if (currentTeam+ 1 < maxTeams) 
		{
			currentTeam++;
		}
		else
		{
			currentTeam=0;
		}
	*/
	//	for(int i=0; i < players.Count; i++)
	//	{
	//		if(players[i].team == currentTeam)
	//		{
	//			currentTeamPlayer.Add(players[i]);
	//		}
	//	}
		
	//players
	//	if (currentPlayerIndex + 1 < players.Count) {
	//		currentPlayerIndex++;
	//	} else {
	//		currentPlayerIndex = 0;
	//	}
		
	}
	
	public void highlightTilesAt(Vector2 originLocation, Color highlightColor, int distance) {
		List <Tile> highlightedTiles = TileHighlight.FindHighlight(map[(int)originLocation.x][(int)originLocation.y], distance);
		
		foreach (Tile t in highlightedTiles) {
			t.transform.renderer.material.color = highlightColor;
		}
	}
	
	public void removeTileHighlights() {
		for (int i = 0; i < mapSize; i++) {
			for (int j = 0; j < mapSize; j++) {
				map[i][j].transform.renderer.material.color = Color.white;
			}
		}
	}
 	
	public void moveCurrentPlayer(Tile destTile) {
		if (destTile.transform.renderer.material.color != Color.white&&destTile.transform.renderer.material.color != FogOfWar.colorOfFogOfWar)
		
			if(!isSomebodyThere(destTile)){
				removeTileHighlights();
				players[currentPlayerIndex].moving = false;
				players[currentPlayerIndex].gridPosition = destTile.gridPosition;
				players[currentPlayerIndex].moveDestination = destTile.transform.position + 1.5f * Vector3.up;
				MapFog.fogOfWar(map,currentTeam,players);
		} else {
			Debug.Log ("destination invalid");
		}
	}
	
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
	
	public void attackWithCurrentPlayer(Tile destTile) {
		if (destTile.transform.renderer.material.color != Color.white||destTile.transform.renderer.material.color != FogOfWar.colorOfFogOfWar ) {
			
			Player target = null;
			foreach (Player p in players) {
				if (p.gridPosition == destTile.gridPosition) 
				{
				//	currentTeam.playerList
					 if(currentTeam.doesContainMember(p))
					{
						Debug.Log ("He is on your team");
						
					}
					else {target = p;}
				}
			}
			
			if (target != null) {
								
				//Debug.Log ("p.x: " + players[currentPlayerIndex].gridPosition.x + ", p.y: " + players[currentPlayerIndex].gridPosition.y + " t.x: " + target.gridPosition.x + ", t.y: " + target.gridPosition.y);
				if (players[currentPlayerIndex].gridPosition.x >= target.gridPosition.x - 1 && players[currentPlayerIndex].gridPosition.x <= target.gridPosition.x + 1 &&
					players[currentPlayerIndex].gridPosition.y >= target.gridPosition.y - 1 && players[currentPlayerIndex].gridPosition.y <= target.gridPosition.y + 1) {
					players[currentPlayerIndex].actionPoints--;
					
					removeTileHighlights();
					players[currentPlayerIndex].moving = false;			
					players[currentPlayerIndex].attacking = false;
					//attack logic
					//roll to hit
			//		bool hit = Random.Range(0.0f, 1.0f) <= players[currentPlayerIndex].attackChance;
					
			//		if (hit) {
						//damage logic
						int amountOfDamage = 5;
						
						target.HP -= amountOfDamage;
						
						Debug.Log(players[currentPlayerIndex].playerName + " successfuly hit " + target.playerName + " for " + amountOfDamage + " damage!");
				//	} else {
				//		Debug.Log(players[currentPlayerIndex].playerName + " missed " + target.playerName + "!");
					}
				//}
				
				else {
					Debug.Log ("Target is not adjacent!");
				}
			}
		} else {
			Debug.Log ("destination invalid");
		}
		MapFog.fogOfWar(map,currentTeam,players);
		//fogOfWar();
	}
	
	// Does the ability of the selected player
	public void doTargetAreaAbility(Tile destTile)
	{
		// Check to see if we are looking at an ability tile
		if (destTile.transform.renderer.material.color != Color.white||destTile.transform.renderer.material.color != FogOfWar.colorOfFogOfWar ){	
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
					else{
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
	
	void generateMap() {
		Debug.Log("generateMap");
		map = new List<List<Tile>>();
		for (int i = 0; i < mapSize; i++) {
			List <Tile> row = new List<Tile>();
			for (int j = 0; j < mapSize; j++) {
				Tile tile = ((GameObject)Instantiate(TilePrefab, new Vector3(i - Mathf.Floor(mapSize/2),0, -j + Mathf.Floor(mapSize/2)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
				tile.gridPosition = new Vector2(i, j);
				row.Add (tile);
			}
			map.Add(row);
		}
		
	}
	
	void generatePlayers() {
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
		public void highlightCurrentSquare(Player P)
	{
		for (int i = 0; i < mapSize; i++) {
			for (int j = 0; j < mapSize; j++) {
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
	
/*	public void fogOfWar()
	{
		fogOutMap();
		currentTeam.teamSight();
		HidingUnseenUnits();
		RevealingSeenUnits();
	}
	
	void fogOutMap()
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
	
	public void highlightCurrentSquare(Player P)
	{
		for (int i = 0; i < mapSize; i++) {
			for (int j = 0; j < mapSize; j++) {
				if(i==P.gridPosition.x&&j==P.gridPosition.y)
				{
						map[i][j].transform.renderer.material.color = Color.white;
				}
			}
		}
	}
	
	private void HidingUnseenUnits()
	{
		foreach (Player P in players)
		{
			if(map [(int)P.gridPosition.x][(int)P.gridPosition.y].transform.renderer.material.color==colorOfFogOfWar) 
			{
				if(P != null)
				{
					P.renderer.enabled = false;
				}
				
			//	P.invisible();
			//	P.renderer.enabled = false;
				//P(UserPlayer);
				//UserPlayer temp = (UserPlayer)P;
				//P.GetComponent(MeshRenderer).enabled = false;
			//	P.GetComponent(MeshRenderer).renderer.enabled = false;
				
				Debug.Log (P.playerName + " is in fog of war");
			}
		}
	}
	
	private void RevealingSeenUnits()
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
	
	
	*/
	
	
	public Player createUserPlayer(int xCordinate, int yCordinate,int teamNumber) //this will probably be removed once the game is done.
	{
		return createNewUnit(UserPlayerPrefab,xCordinate, yCordinate, teamNumber);
	}
	
	//public Player createNewUnit(GameObject UnitPrefab, Vector3 startPosition, Vector2 gridPosition, int teamNumber)
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
