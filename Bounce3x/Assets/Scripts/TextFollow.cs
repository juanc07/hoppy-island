using UnityEngine;
using System.Collections;

public class TextFollow : MonoBehaviour{	
	public Camera mainCamera;
	public Camera NGUICamera;
	public GameObject target;
	
	private GameObject inGamePanel;
	private UILabel comboLabel;
	
	// Use this for initialization
	void Start (){		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void FollowTarget(){	
		mainCamera = NGUITools.FindCameraForLayer(target.layer);
		NGUICamera = NGUITools.FindCameraForLayer(gameObject.layer);
		
		Vector3 pos = mainCamera.WorldToViewportPoint(target.transform.position);
		pos = NGUICamera.ViewportToWorldPoint(pos);
		
		pos.z = 0f;
		transform.position = pos;
		
		Debug.Log("text is folling");
	}
	
	private void LateUpdate(){		
		FollowTarget();
		
	}
}
