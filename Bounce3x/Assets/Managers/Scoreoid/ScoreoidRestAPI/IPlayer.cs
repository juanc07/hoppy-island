using UnityEngine;
using System.Collections;

public interface IPlayer{
	string username{set;get;}
	string password{set;get;}
	string unique_id{set;get;}
	string first_name{set;get;}
	string last_name{set;get;}

	string email{set;get;}
	string bonus{set;get;}
	string achievements{set;get;}
	string gold{set;get;}

	string money{set;get;}
	string kills{set;get;}
	string lives{set;get;}
	string time_played{set;get;}

	string unlocked_levels{set;get;}
	string unlocked_items{set;get;}
	string inventory{set;get;}
	string last_level{set;get;}

	string current_level{set;get;}
	string current_time{set;get;}
	string current_bonus{set;get;}
	string current_kills{set;get;}

	string current_achievements{set;get;}
	string current_gold{set;get;}
	string current_unlocked_levels{set;get;}
	string current_unlocked_items{set;get;}

	string current_lives{set;get;}
	string xp{set;get;}
	string energy{set;get;}
	string boost{set;get;}

	string latitude{set;get;}
	string longtitude{set;get;}
	string game_state{set;get;}
	string platform{set;get;}

	string rank{set;get;}
	string best_score{set;get;}
	string created{set;get;}
	string updated{set;get;}

}
