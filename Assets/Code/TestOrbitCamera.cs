using UnityEngine;
using System.Collections;

public class TestOrbitCamera : MonoBehaviour 
{
    private Vector3 mOrbitVelocty = Vector3.zero;
	
    private Transform mTransform;

	private void Start () 
    {
        mTransform = transform;
        //Screen.lockCursor = true;
        //Screen.showCursor = false;
	}
	
	
	private void Update () 
    {

	}
}
