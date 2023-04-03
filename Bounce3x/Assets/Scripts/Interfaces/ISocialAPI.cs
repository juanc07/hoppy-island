using System.Collections.Generic;
using System;

public interface ISocialAPI{
	//methods
	void CreateUser(string username,string password);
	void LoginUser(string username,string password);
	void SendScore(int score);
	int GetScore();
	void GetUsers();
	List<ISocialUser> Users{get;}

	//events
	event Action OnCreateUserComplete;
	event Action OnCreateUserFailed;

	event Action OnLoginUserComplete;
	event Action OnLoginUserFailed;

	event Action OnSendScoreComplete;
	event Action OnSendScoreFailed;

	event Action OnGetScoreComplete;
	event Action OnGetScoreFailed;

	event Action OnGetUsersComplete;
	event Action OnGetUsersFailed;

	event Action OnGetUsersInProgress;
	event Action OnOffline;
}