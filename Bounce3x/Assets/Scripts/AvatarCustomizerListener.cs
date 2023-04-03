using UnityEngine;
using System.Collections;

public class AvatarCustomizerListener : MonoBehaviour {
	
	private AvatarCustomizer avatarCustomizer;
	
	// Use this for initialization
	void Start () {
		avatarCustomizer =this.gameObject.GetComponent<AvatarCustomizer>();
		Messenger.AddListener<Item>( GameEvent.SelectShopItem, onSelectShopItem);
	}
	
	private void onSelectShopItem(Item item){
		if(this==null)return;
		avatarCustomizer.ChangeAvatarByAvatarType(item.avatarType);		
		//Debug.Log( "AvatarCustomizerListener onSelectShopItem " + item.name );		
	}
}
