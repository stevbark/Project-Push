using UnityEngine;
using System.Collections;

public class finance : MonoBehaviour
{
	private int money = 100;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void setMoney(int StartingMoney)
	{
		if(StartingMoney>=0)
		{
			money = StartingMoney;
		}
	}
	
	public void addMoney(int amountAdded)
	{
		money+=amountAdded;
	}
	
	public bool removeMoney(int amountRemoved) // tells whether amount removed can be afforded
	{
		if(amountRemoved<=money)
		{
			money -=amountRemoved;
			return true;
		}
		return false; 
	}
	
	public int amountOfMoney()
	{
		return money;
	}
}

