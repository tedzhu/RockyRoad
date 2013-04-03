using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
	PSMoveWrapper psMoveWrapper;
	GameObject goCredit;
	// Use this for initialization
	void Start ()
	{
		psMoveWrapper = GetComponent<PSMoveWrapper> ();
		psMoveWrapper.Connect ();		
		if (psMoveWrapper.isConnected) {
			GameObject.Find ("IceCream").renderer.enabled = true;
		} else {
		}
		
		goCredit = GameObject.Find ("PlaneCredit");
	}
	
	const float INF = 1e10f;
	float vanishTime = INF;
	
	
	// Update is called once per frame
	void Update ()
	{
		if (goCredit.renderer.enabled) {
			bool vanish = false;
			for (int i=0; i<psMoveWrapper.moveCount; i++) {
				if (psMoveWrapper.isButtonCircle [i])
					vanish = true;
			}
			if (Input.GetKeyDown (KeyCode.S)) {
				vanish = true;
			}
			if (vanish) {
				goCredit.renderer.enabled = false;
			}
		} else {
		
			for (int i=0; i<psMoveWrapper.moveCount; i++) {
				if (psMoveWrapper.isButtonSquare [i]) {
					PlayerPrefs.SetInt ("playerCount", 1);
					Destroy (gameObject);
					Application.LoadLevel ("Beginning");
				} else if (psMoveWrapper.isButtonTriangle [i]) {
					PlayerPrefs.SetInt ("playerCount", 2);
					Destroy (gameObject);
					Application.LoadLevel ("Beginning");
				} else if (psMoveWrapper.isButtonCircle [i]) {
					goCredit.renderer.enabled = true;
				} else if (psMoveWrapper.isButtonCross [i]) {
					Application.Quit ();
				}
			}
		
			if (Input.GetKeyDown (KeyCode.Q)) {
				PlayerPrefs.SetInt ("playerCount", 1);
				Destroy (gameObject);
				Application.LoadLevel ("Beginning");
			} else if (Input.GetKeyDown (KeyCode.W)) {
				PlayerPrefs.SetInt ("playerCount", 2);
				Destroy (gameObject);
				Application.LoadLevel ("Beginning");
			} else if (Input.GetKeyDown (KeyCode.S)) {
				goCredit.renderer.enabled = true;
			} else if (Input.GetKeyDown (KeyCode.A)) {
				Application.Quit ();
			}		
		
		}		
	}
}
