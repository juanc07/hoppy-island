using UnityEngine;
using System.Collections;

public class Trap{
	public int id;
	public string name;	
	public Rigidbody obj;
	public bool isActive;
	public TrapTypes trapType;
	
	public enum TrapTypes{
		Bomb
	}
	
}
