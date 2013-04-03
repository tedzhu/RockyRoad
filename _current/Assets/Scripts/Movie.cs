using UnityEngine;
using System.Collections;

public class Movie : MonoBehaviour {
	public MovieTexture movieTexture;	
	public string nextScene;
	
	// Use this for initialization
	void Start () {
		movieTexture.Play();
		//movieTexture.audioClip
		//audio.clip=movieTexture.audioClip;
		audio.Play();			
	}	
	
	// Update is called once per frame
	void Update () {
		if((!movieTexture.isPlaying && !audio.isPlaying) || Input.GetKey(KeyCode.Space)){
			Application.LoadLevel(nextScene);			
		}
	}
	
	void OnGUI(){
		if(movieTexture.isPlaying )
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),movieTexture,ScaleMode.StretchToFill);
		
	}
}
