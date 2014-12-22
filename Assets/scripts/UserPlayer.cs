using UnityEngine;
using System.Collections;

public class UserPlayer : Player {
	//public GameObject personPrefab; //this prefab will be removed once different units are created.
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log("Current player Index is " +GameManager.instance.currentPlayerIndex);
		if (GameManager.instance.players[GameManager.instance.currentPlayerIndex] == this) {
			transform.renderer.material.color = Color.green;
		}else {
			transform.renderer.material.color = Color.white;
		}
		if(actionPoints == 0 )
		{
			this.renderer.material.color = Color.blue;
		}
		if (HP <= 0) {
			//transform.rotation = Quaternion.Euler(new Vector3(90,0,0));
			//transform.renderer.material.color = Color.red;
			GameManager.instance.removePlayer(this);
			Destroy(this.gameObject);
		}
		
	}
	public void onMouseDown()
	{
		
	//	if(team==currentTeam)
		{
//			GameManager.Instantiate.setCurrentPlayer(this);
		}
			
	}
	public override void TurnUpdate ()
	{
		//highlight
		//
		//
		
		if (Vector3.Distance(moveDestination, transform.position) > 0.1f) {
			transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime;
			
			if (Vector3.Distance(moveDestination, transform.position) <= 0.1f) {
				transform.position = moveDestination;
				actionPoints--;
			}
		}
		
		base.TurnUpdate ();
	}
	
	public override void sight()
	{
		GameManager.instance.highlightTilesAt(gridPosition, Color.white, sightRange); 
		GameManager.instance.highlightCurrentSquare(this);
		
	}
	
	public override void TurnOnGUI () {
		float buttonHeight = 50;
		float buttonWidth = 150;
		
		Rect buttonRect = new Rect(0, Screen.height - buttonHeight * 4, buttonWidth, buttonHeight);
		
		
		//move button
		if (GUI.Button(buttonRect, "Move")) {
			if (!moving) {
				GameManager.instance.removeTileHighlights();
				GameManager.instance.fogOfWar();
				moving = true;
				attacking = false;
				abilityActive = false;
				GameManager.instance.highlightTilesAt(gridPosition, Color.blue, movementPerActionPoint);
			} else {
				moving = false;
				attacking = false;
				abilityActive = false;
				GameManager.instance.removeTileHighlights();
				GameManager.instance.fogOfWar();
			}
		}
		
		//attack button
		buttonRect = new Rect(0, Screen.height - buttonHeight * 3, buttonWidth, buttonHeight);
		
		if (GUI.Button(buttonRect, "Attack")) {
			if (!attacking) {
				GameManager.instance.removeTileHighlights();
				GameManager.instance.fogOfWar();
				moving = false;
				attacking = true;
				abilityActive = false;
				GameManager.instance.highlightTilesAt(gridPosition, Color.red, attackRange);
			} else {
				moving = false;
				attacking = false;
				abilityActive = false;
				GameManager.instance.removeTileHighlights();
				GameManager.instance.fogOfWar();
			}
		}
		
		buttonRect = new Rect(0, Screen.height - buttonHeight * 2, buttonWidth, buttonHeight);
		
		if(GUI.Button(buttonRect, abilityString))
		{
			if(!abilityActive){
				GameManager.instance.removeTileHighlights ();
				GameManager.instance.fogOfWar();
				moving = false;
				attacking = false;
				abilityActive = true;
				GameManager.instance.highlightTilesAt (gridPosition, Color.yellow, abilityRange);
			}
			else{
				moving = false;
				attacking = false;
				abilityActive = false;
				GameManager.instance.removeTileHighlights();
				GameManager.instance.fogOfWar();
			}
		}
		
		
	//	Rect abutton = new Rect (this.gridPosition.x - buttonWidth/2, this.gridPosition.y- buttonHeight/2, buttonWidth, buttonHeight);
		
	//	GUI.Button(abutton, "test");
		
		base.TurnOnGUI ();
	}
}
