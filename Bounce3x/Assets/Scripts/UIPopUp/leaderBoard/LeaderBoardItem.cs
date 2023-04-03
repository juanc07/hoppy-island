using UnityEngine;
using System.Collections;

public class LeaderBoardItem : MonoBehaviour {

	public bool restrictWithinPanel = false;
	public Transform target;
	UIPanel mPanel;
	Bounds mBounds;

	// Use this for initialization
	void Start () {
	
	}

	void FindPanel ()
	{
		mPanel = (target != null) ? UIPanel.Find(target.transform, false) : null;
		if (mPanel == null) restrictWithinPanel = false;
	}

	public virtual Vector3 CalculateConstrainOffset (Vector2 min, Vector2 max, int offsetRectY)
	{
		float offsetX = mPanel.clipRange.z * 0.5f;
		float offsetY = mPanel.clipRange.w * 0.5f;
		
		Vector2 minRect = new Vector2(min.x, min.y + offsetRectY);
		Vector2 maxRect = new Vector2(max.x, max.y - offsetRectY);
		Vector2 minArea = new Vector2( mPanel.clipRange.x - offsetX, mPanel.clipRange.y - offsetY);
		Vector2 maxArea = new Vector2(mPanel.clipRange.x + offsetX, mPanel.clipRange.y + offsetY);
		
		if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
		{

			minArea.x += mPanel.clipSoftness.x;
			minArea.y += mPanel.clipSoftness.y;
			maxArea.x -= mPanel.clipSoftness.x;
			maxArea.y -= mPanel.clipSoftness.y;
		}
		return NGUIMath.ConstrainRect(minRect, maxRect, minArea, maxArea);
	}

	public bool ConstrainTargetToBounds (Transform target, ref Bounds targetBounds, int offsetRectY )
	{
		Vector3 offset = CalculateConstrainOffset(targetBounds.min, targetBounds.max, offsetRectY );		
		if (offset.magnitude > 0f){
			return true;
		}
		return false;
	}

	// Update is called once per frame
	void Update (){
		/*
		if (restrictWithinPanel && mPanel == null) FindPanel();
		if (restrictWithinPanel) mBounds = NGUIMath.CalculateRelativeWidgetBounds(mPanel.cachedTransform, target);

		if (restrictWithinPanel && mPanel.clipping != UIDrawCall.Clipping.None && !ConstrainTargetToBounds(target, ref mBounds,100)){
			//Debug.Log("within panel");
			//this.gameObject.SetActive(true);
			this.gameObject.transform.GetComponentInChildren<UISprite>().enabled=true;
			UILabel[] labels = this.gameObject.transform.GetComponentsInChildren<UILabel>();
			foreach(UILabel label in labels ){
				label.enabled = true;
			}
		}else{
			//Debug.Log("outside in panel");
			//this.gameObject.activeSelf = false;
			//this.gameObject.transform.GetComponentInChildren<UISprite>().enabled=false;
			bel[] labels = this.gameObject.transform.GetComponentsInChildren<UILabel>();
			foreach(UILabel label in labels ){
				label.enabled = false;
			}
		}
		*/
	}
}
