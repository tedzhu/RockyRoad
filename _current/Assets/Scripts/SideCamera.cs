using UnityEngine;
using System.Collections;

public class SideCamera : MonoBehaviour
{
	public Vector3 destPos;//something ted like
	public bool fix = false;
	public GameObject fixedOn;
	
	
	void Start ()
	{
		destPos = transform.position;//the camera's position?
		
		fix = true;
		fixedOn = GameObject.Find ("Platform");	
	}
	
	void Update ()
	{ 
		transform.position = Vector3.Slerp (transform.position, destPos, 0.05f);	//move this with each frame			
		
		if (fix) {
			destPos.y = fixedOn.transform.position.y+1;//camera's y =platform's y
		} else {		//someone once wanted to move the camera~~~stupid
			if (Input.GetKeyDown (KeyCode.W)) {
				destPos.y+=0.1f;
			}
			if (Input.GetKeyDown (KeyCode.S)) {
				destPos.y-=0.1f;
			}
			if (Input.GetKeyDown (KeyCode.A)) {
				destPos.x+=0.1f;
			}
			if (Input.GetKeyDown (KeyCode.D)) {
				destPos.x-=0.1f;
			}
		}
		
	}
		
}
