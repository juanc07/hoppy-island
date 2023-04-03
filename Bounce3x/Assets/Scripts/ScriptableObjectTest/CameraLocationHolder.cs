using System.Collections.Generic;
using UnityEngine;
 
public class CameraLocationHolder : ScriptableObject {    
	public List<CameraLocation> content;	
    public CameraLocationHolder(List<CameraLocation> content)
    {
        this.content = content;
    }
}