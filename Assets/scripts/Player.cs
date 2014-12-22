using UnityEngine;
using System.Collections;


public class Player : MonoBehaviour {

	public Vector2 gridPosition = Vector2.zero;
	public Vector3 moveDestination;
	public float moveSpeed = 10.0f;
	
	public int movementPerActionPoint = 5;
	public int attackRange = 1;
	
	public bool moving = false;
	public bool attacking = false;
	
	
	public string playerName = "George";
	public int HP = 25;
	public int maxHP = 25;
	
	public int sightRange =3;
	public float attackChance = 0.75f;
	public float defenseReduction = 0.15f;
	public int damageBase = 5;
	public float damageRollSides = 6; //d6
	private int actionsPerTurn = 2;
	public int actionPoints = 2;
	public int team;
	
	public string abilityString = "Placeholder";
	public bool abilityActive = false;
	public int abilityRange = 2;
	public int abilityAmount = 0;
	public string unitType = "";
	
	void Awake () {
		moveDestination = transform.position;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
		void Update () 
	{
		if(actionPoints <= 0)
		{
			renderer.material.color = Color.black;
		}
		else
		{
			renderer.material.color = Color.white;
		}
	}
	
	public void invisible()
	{
		if(this.renderer !=null)
		{
			this.renderer.enabled =false;
		}
	}
	public virtual void TurnUpdate () {
	//	if (actionPoints <= 0) {
	//		actionPoints = 2;
	//		moving = false;
	//		attacking = false;			
	//		GameManager.instance.nextTurn();  //remove this later or it will skip rest of turn when first person finishes
	//	}
	}
	
	public void renewActionPoints()
	{
		actionPoints=actionsPerTurn;
	}
	
	
	
	public virtual void TurnOnGUI () {
		
	}
	
	
	public virtual void sight()
	{
	}

}
