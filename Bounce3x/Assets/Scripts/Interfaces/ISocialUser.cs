using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ISocialUser{
	string username{set;get;}
	string password{set;get;}
	int score{set;get;}
	int timestamp{set;get;}
}
