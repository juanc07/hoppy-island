using UnityEngine;
using System.Collections;

public class Player :IPlayer {
	public string username{set;get;}
	public string password{set;get;}

	public string unique_id{set;get;}
	public string first_name{set;get;}
	public string last_name{set;get;}
	
	public string email{set;get;}
	public string bonus{set;get;}
	public string achievements{set;get;}
	public string gold{set;get;}
	
	public string money{set;get;}
	public string kills{set;get;}
	public string lives{set;get;}
	public string time_played{set;get;}
	
	public string unlocked_levels{set;get;}
	public string unlocked_items{set;get;}
	public string inventory{set;get;}
	public string last_level{set;get;}
	
	public string current_level{set;get;}
	public string current_time{set;get;}
	public string current_bonus{set;get;}
	public string current_kills{set;get;}
	
	public string current_achievements{set;get;}
	public string current_gold{set;get;}
	public string current_unlocked_levels{set;get;}
	public string current_unlocked_items{set;get;}
	
	public string current_lives{set;get;}
	public string xp{set;get;}
	public string energy{set;get;}
	public string boost{set;get;}
	
	public string latitude{set;get;}
	public string longtitude{set;get;}
	public string game_state{set;get;}
	public string platform{set;get;}
	
	public string rank{set;get;}
	public string best_score{set;get;}
	public string created{set;get;}
	public string updated{set;get;}
}
