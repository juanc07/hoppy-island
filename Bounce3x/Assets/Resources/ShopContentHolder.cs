using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopContentHolder : ScriptableObject {
	public List<Item> content = new List<Item>();
	
	public ShopContentHolder(List<Item> content)
    {
        this.content = content;
    }
}
