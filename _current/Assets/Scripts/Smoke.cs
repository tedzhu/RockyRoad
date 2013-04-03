using UnityEngine;
using System.Collections;

public class Smoke : MonoBehaviour {
	GlobalVar globalvar;
	
	IEnumerator aFunc()
	{
		for(int i=0;i<10;i++){
			this.renderer.material.mainTexture=globalvar.animSmoke[i];
			yield return new WaitForSeconds(0.02f);
		}
		Destroy(gameObject);
	}
	void Start () {
		globalvar=GameObject.Find("0").GetComponent<GlobalVar> ();
		StartCoroutine(aFunc());
	}
}
