using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextComboManager : MonoBehaviour {

	public GameObject scoreComboLabel;
	private GameObject inGamePanel;
	private List<GameObject> scoreComboCollection = new List<GameObject>();

	// Use this for initialization
	void Start () {	
		inGamePanel = GameObject.Find("InGamePanel");
	}

	public void ShowTextCombo(){
		GameObject scLabel = SearchForInActiveTextCombo();
		if(scLabel == null){
			scLabel  =  Instantiate(scoreComboLabel, new Vector3(0,0,0), inGamePanel.transform.rotation ) as GameObject;
			scLabel.name = "ScoreComboLabel";
			scLabel.transform.parent = inGamePanel.transform;		
			scLabel.transform.localScale = new Vector3( 58.54361f,58.54361f,0 );
			TextComboTweener textComboTweener = scLabel.gameObject.GetComponent<TextComboTweener>();
			textComboTweener.OnTweenComplete+=OnTweenComplete;
			textComboTweener.ID = scoreComboCollection.Count;
			scoreComboCollection.Add(scLabel);
			//Debug.Log("create new text combo!");
		}
	}

	private GameObject SearchForInActiveTextCombo(){
		int textComboLen = scoreComboCollection.Count;
		GameObject found = null;

		for(int index=0;index<textComboLen;index++){
			if(scoreComboCollection[index]!=null){
				if(!scoreComboCollection[index].activeSelf){
					found = scoreComboCollection[index];
					found.SetActive(true);
					TextComboTweener textComboTweener = found.gameObject.GetComponent<TextComboTweener>();
					if(textComboTweener!=null){
						textComboTweener.Reset();
						//Debug.Log("reuse text combo!");
					}
					break;
				}
			}
		}

		return found;
	}

	private void DeactivateTextComboById(int id){
		int textComboLen = scoreComboCollection.Count;

		for(int index=0;index<textComboLen;index++){
			if(scoreComboCollection[index]!=null){
				if(scoreComboCollection[index].activeSelf){
					TextComboTweener textComboTweener = scoreComboCollection[index].gameObject.GetComponent<TextComboTweener>();
					if(textComboTweener!=null){
						if(textComboTweener.ID == id){
							scoreComboCollection[index].SetActive(false);
							break;
						}
					}
				}
			}
		}
	}

	private void OnTweenComplete(int id){
		DeactivateTextComboById(id);
	}
}
