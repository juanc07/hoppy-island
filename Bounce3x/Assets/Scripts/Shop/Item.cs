using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item{	
	public string id;
	public string name;
	public string price;
	public AvatarList avatarType;
	public ItemType type;
	public Transform itemTransform;
	public Transform itemShopTransform;
	
	
	public enum ItemType{
		Avatar,
		Powerup,
	}
	
	public enum AvatarList{		
		Zombie,
		Robot,
		Nyancat,
		Unicorn,
		Clown,
		Pink,
		Whale,
		Barbarian,
		Panda,
		Skeleton,
		Trex,
		Kamina,
		Samurai
	}
}
