using UnityEngine;
using System.Collections;
 
[System.Serializable]
public class CameraLocation
{
    public string name = "New Camera";
    public float fov = Camera.main.fov;
    public Vector3 position = Camera.main.transform.position;
    public Quaternion rotation = Camera.main.transform.rotation;
}