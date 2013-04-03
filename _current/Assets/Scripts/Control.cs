using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour
{
	const float INF = 1e10f;
	const float GRABTHRESH = 1.5f;
	const float RELEASETHRESH = 1.0f;
	public bool isDebug;
	public PSMoveWrapper psMoveWrapper;
	public bool isMirror = true;
	GlobalVar globalvar;
	bool isFallen = false;
	int currFloor = 0;
	int floorNumber = 6;
	int currentFloor;
	float[] floorY;
	GameObject go0;
	GameObject goPlatform;
	GameObject goFace;
	GameObject[] goHammer = new GameObject[3];
	GameObject[] goHand = new GameObject[3];
	GameObject[] goHandO = new GameObject[3];
	GameObject[] goPlayer = new GameObject[3];
	GameObject[] goChain = new GameObject[3];
	GameObject[] goPS = new GameObject[3];
	Vector3[] gemPos = new Vector3[3];
	Vector3[] handlePos = new Vector3[3];
	Quaternion[] handleRot = new Quaternion[3];
	int status;

	IEnumerator toast (string s)
	{
		GameObject.Find ("_debugbox").guiText.text = s;
		print (s);
		yield return new WaitForSeconds(1);
		GameObject.Find ("_debugbox").guiText.text = "";
	}
	
	void debugbox (string s)
	{
		if (isDebug)
			GameObject.Find ("_debugbox").guiText.text = s;
	}
	
	void makeChain (string prefix)//as it named，make chains
	{
		GameObject chain0, chain1, chain2 = null;
		chain0 = GameObject.Find (prefix + "0");
		chain1 = chain0;
		for (int i=1; i<10; i++) {//many many!
			chain2 = (GameObject)Instantiate (chain0, chain0.transform.position, chain0.transform.rotation);//built it
			chain2.transform.Translate (0, chain0.transform.localScale.y * i, 0);//move itself to right place
			//chain2.transform.Translate(0,0,0.4f*i);
			chain2.name = prefix + i;//change name
			
			HingeJoint hj = (HingeJoint)chain1.hingeJoint;//about the joint

			hj.connectedBody = chain2.rigidbody;//get the body so it can happen sth when hit others
			hj.anchor = new Vector3 (0, 0, 0.5f);//orientation, you may be curious why I call this orientation
			hj.useSpring = true;
			hj.axis = new Vector3 (0, 0, 1);
			JointSpring js = new JointSpring ();	//set a brand new joint!!!		
			js.damper = 20f;
			hj.spring = js;
			
			chain1 = chain2;
		}		
		
		chain2.name = prefix + "C";//rename the chain2 so it can give another chain name,right?
		chain2.rigidbody.isKinematic = true;//what's isKinematic
		Destroy (chain2.hingeJoint);//so chain2 dont need a joint any longer?
		
	}
	
	IEnumerator resetPlatform ()
	{		
		makeChain ("ChainPart1");
		makeChain ("ChainPart2");
		yield return new WaitForSeconds(0.2f);
		goPlatform.rigidbody.isKinematic = false;//find the platform and i still don't know what's isKinematic(to protect the worker)			
	}
	
	GameObject goSound1;
	GameObject goSound2;
	float soundSwitchTime;
	
	IEnumerator waitConnect ()
	{
		yield return new WaitForSeconds(0.2f);
		// PS Move Connection.
		if (!psMoveWrapper.isConnected) {			
			//psMoveWrapper.ipAddress = "128.2.237.66";
			psMoveWrapper.Connect ();
		}
				
		if (psMoveWrapper.isConnected) {
			debugbox ("PS Move Connected!");			
		} else {						
			debugbox ("PS Move Connection Failed.");
		}
	}
	
	IEnumerator flashClock(int t, float d)
	{
		for(int i=0;i<t;i++){
			GameObject.Find("Clock").renderer.enabled=false;
			GameObject.Find("_ClockHand").renderer.enabled=false;
			yield return new WaitForSeconds(d);
			GameObject.Find("Clock").renderer.enabled=true;
			GameObject.Find("_ClockHand").renderer.enabled=true;
			yield return new WaitForSeconds(d);
		}
	}
	
	void Awake ()
	{		
		if (go0 = GameObject.Find ("0")) {		
			globalvar = go0.GetComponent<GlobalVar> ();
			psMoveWrapper = go0.GetComponent<PSMoveWrapper> ();
			goSound1=GameObject.Find ("sound1");
		} else {
			// Initialization Once
			go0 = new GameObject ();
			go0.name = "0";
			go0.AddComponent ("GlobalVar");
			go0.AddComponent ("PSMoveWrapper");
			
			psMoveWrapper = go0.GetComponent<PSMoveWrapper> ();
			psMoveWrapper.isFixedIP = false;
			
			globalvar = go0.GetComponent<GlobalVar> ();			
			globalvar.allTime = 20f;
			globalvar.remainTime = globalvar.allTime;
			globalvar.timePeriod = 0;
			//globalvar.playerCount = 2;
			//PlayerPrefs.SetInt("playerCount",playerCount);
			globalvar.playerCount = PlayerPrefs.GetInt("playerCount");
			if(globalvar.playerCount==0)
				globalvar.playerCount=2;
			
			globalvar.isFinished = false;
			globalvar.floorHitLeft [1] = 3;
			globalvar.floorHitLeft [2] = 3;
			globalvar.floorHitLeft [3] = 2;
			globalvar.floorHitLeft [4] = 6;
			globalvar.floorHitLeft [5] = 8;
			globalvar.floorHitLeft [6] = 8;
			
			goSound1 = new GameObject ();
			DontDestroyOnLoad (goSound1);
			goSound1.name = "sound1";			
			goSound1.AddComponent<AudioSource> ();
			goSound1.audio.loop = true;
			goSound1.audio.clip = (AudioClip)Resources.Load ("SE/bg");
			print (goSound1.audio.volume);
			goSound1.audio.volume = 0.5f;
			goSound1.audio.Play ();
			
//			goSound2=new GameObject();
//			goSound2.name="sound2";
//			DontDestroyOnLoad(goSound2);
//			goSound2.AddComponent<AudioSource>();
//			goSound2.audio.loop=true;
//			goSound2.audio.clip=(AudioClip)Resources.Load("SE/TurkeyLoop");						
//			goSound2.audio.volume=0.2f;
			
			
//			goSound2.audio.Play();
//			soundSwitchTime=Time.time+1f;			
		}			
	}
	
	void Start ()
	{
		StartCoroutine (waitConnect ());
		GameObject.Find ("_currFloor").guiText.text = "";
		GameObject.Find ("_debugbox").guiText.text = "";

				
		// Find GOs.
		goHammer [1] = GameObject.Find ("Hammer1");
		goHammer [2] = GameObject.Find ("Hammer2");
		goPlayer [1] = GameObject.Find ("Player1");
		goPlayer [2] = GameObject.Find ("Player2");
		goHand [1] = GameObject.Find ("Hand1");
		goHand [2] = GameObject.Find ("Hand2");
		goHandO [1] = GameObject.Find ("Hand1O");
		goHandO [2] = GameObject.Find ("Hand2O");
		goPS [1] = GameObject.Find ("PS1");
		goPS [2] = GameObject.Find ("PS2");
		goPlatform = GameObject.Find ("Platform");				
		goFace = GameObject.Find ("Face");				
		
		// Reset everything according to globalvar.
		StartCoroutine (resetPlatform ());
		for (int i=1; i<=floorNumber; i++) {
			if (globalvar.floorHitLeft [i] == 0) {
				Destroy (GameObject.Find ("Floor" + i));
			}
		}
		
		if (globalvar.playerCount == 1) {
			goPlayer [2].active = false;
			goPlayer [2].transform.Translate(0,0,100);
		}
				
		// One-time calculation.
		floorY = new float[floorNumber + 1];
		for (int i=1; i<=floorNumber; i++) {
			floorY [i] = GameObject.Find ("Floor" + i).transform.position.y - 0.3f;
		}
		
		// Find generated GOs.
		goChain [1] = GameObject.Find ("ChainPart1C");
		goChain [2] = GameObject.Find ("ChainPart2C");
		
		// flash clock
		StartCoroutine(flashClock(3,0.3f));
	}
	
	IEnumerator delayLoad (float delay, string level)
	{
		yield return new WaitForSeconds(delay);
		if(level!="Main"){
			Destroy (go0);
			Destroy(goSound1);
		}
		Application.LoadLevel (level);
	}
	
	bool[] hammering = new bool[3];

	IEnumerator hammerStat (int i)
	{
		hammering [i] = true;
		yield return new WaitForSeconds(0.2f);
		hammering [i] = false;
	}
		
	void hammer (int i)
	{
		if (hammering [i])
			return;
		
		if (isFallen || globalvar.isTimeUp)
			return;
		
		debugbox (i + " hammer!");
		StartCoroutine (hammerStat (i));

		AudioSource.PlayClipAtPoint ((AudioClip)globalvar.hammerSound [Random.Range (0, 4)], transform.position);
		
		
		Vector3 tpos = goHammer [i].transform.position;
		tpos.z = 1.5f;
		goPS [i].transform.position = tpos;
		goPS [i].particleEmitter.Emit ();
		goPS [i].particleEmitter.emit = false;
//		goSound1.audio.volume=0f;
//		goSound2.audio.volume=1f;
//		soundSwitchTime=Time.time+1f;
		
		if (globalvar.floorHitLeft [currFloor] != 0) {
			globalvar.floorHitLeft [currFloor]--;
						
			if (globalvar.floorHitLeft [currFloor] == 0) {
				// Fixed One Level!
				Instantiate (Resources.Load ("Smoke"), new Vector3 (0, floorY [currFloor], 1.1f), Quaternion.Euler (-90, 0, 0));											
				
				GameObject.Find ("PSMagic").transform.position = new Vector3 (0, floorY [currFloor], 1.0f);
				GameObject.Find ("PSMagic1").particleEmitter.Emit ();
				GameObject.Find ("PSMagic1").particleEmitter.emit = false;
				GameObject.Find ("PSMagic2").particleEmitter.Emit ();
				GameObject.Find ("PSMagic2").particleEmitter.emit = false;
				Destroy (GameObject.Find ("Floor" + currFloor));	
				faceSet (1, 1);
				globalvar.finishedFloorNumber++;
				AudioSource.PlayClipAtPoint (globalvar.acSuccess, transform.position);
				
				if (globalvar.finishedFloorNumber == floorNumber) {
					// Fixed All Levels!
					globalvar.isFinished = true;
					debugbox ("Win!");
					
					faceSet(1,10);
					GetComponent<SideCamera>().fix=false;
					GetComponent<SideCamera>().destPos=new Vector3(0,4f,-21f);
										
					AudioSource.PlayClipAtPoint (globalvar.acWoohoo, transform.position);
					StartCoroutine (delayLoad (4f, "GoodEnd"));
				}else{
					AudioSource.PlayClipAtPoint (globalvar.acYah[Random.Range(0,2)], transform.position);
				}
			}			
		}					
	}
		
	float faceRecoverTime = INF;

	void faceSet (int n, float t)
	{
		if (n != 0)
			faceRecoverTime = Time.time + t;
		status = n;
	}

	void faceUpdate ()
	{
		if (status == 0) {
			goFace.renderer.material.mainTexture = globalvar.animFaceNormal [globalvar.animFaceNormalIndex];
			//globalvar.animFaceNormalIndex=(globalvar.animFaceNormalIndex+1)%globalvar.animFaceNormal.Length;
		} else if (status == 1) {
			goFace.renderer.material.mainTexture = globalvar.animFaceHappy [globalvar.animFaceHappyIndex];
			globalvar.animFaceHappyIndex++;
			if (globalvar.animFaceHappyIndex == globalvar.animFaceHappy.Length) {
				globalvar.animFaceHappyIndex = 0;
			}
		} else if (status == 2) {
			goFace.renderer.material.mainTexture = globalvar.texFaceFall;		
		} else if (status == 3) {
			goFace.renderer.material.mainTexture = globalvar.texFaceNervous;		
		}
		if (Time.time > faceRecoverTime) {
			faceRecoverTime = INF;
			status = 0;
		}
	}
	
	int[] grabN = new int[3];
	bool[] grabbed = new bool[3];
	float[] grabY = new float[3];
	float[] grabChainY = new float[3];

	void grab (int i, int n)
	{
		debugbox ("grabbed " + i);
		grabbed [i] = true;
		grabN [i] = n;
		grabY [i] = handlePos [i].y;
		grabChainY [i] = goChain [n].transform.position.y;		
		psMoveWrapper.SetRumble (i - 1, 5);
		
		goHandO [i].animation.Play ("Take 001");
		goHandO [i].animation ["Take 001"].speed = 3;	
	}
	
	void release (int i)
	{
		debugbox ("released " + i);
		grabbed [i] = false;
		psMoveWrapper.SetRumble (i - 1, 0);	
				
		if (!goHandO [i].animation.isPlaying)
			goHandO [i].animation ["Take 001"].time = goHandO [i].animation ["Take 001"].length;
		goHandO [i].animation.Play ("Take 001");
		goHandO [i].animation ["Take 001"].speed = -3;	
	}

	void FixedUpdate ()
	{
		if (psMoveWrapper.isConnected) {
			int count = psMoveWrapper.moveCount;
			if (count == 0)
				return;
			if (count > 2)
				count = 2;
			
			if (count > globalvar.playerCount)
				count = globalvar.playerCount;
			for (int i=1; i<=count; i++) {				
				gemPos [i] = psMoveWrapper.position [i - 1];
				handlePos [i] = psMoveWrapper.handlePosition [i - 1];	
				handleRot [i] = Quaternion.Euler (psMoveWrapper.orientation [i - 1]);

				handlePos [i].x /= 4;
				handlePos [i].y /= 4;
				handlePos [i].y -= 3;								
				handlePos [i].z = -2;
				
				goHand [i].transform.localRotation = handleRot [i];
				goHammer [i].transform.localRotation = handleRot [i];
				
				if (psMoveWrapper.acceleration [i - 1].z > 300 && psMoveWrapper.acceleration [i - 1].y > 240) {
					hammer (i);
				}
					
				if (Mathf.Abs (goPlayer [i].transform.position.x) > GRABTHRESH) {
					goHand [i].transform.position = handlePos [i] + new Vector3 (0, goPlatform.transform.position.y + 4, 0);
					goHammer [i].transform.position = handlePos [i] + new Vector3 (0, goPlatform.transform.position.y + 100, 0);
				} else {
					goHand [i].transform.position = handlePos [i] + new Vector3 (0, goPlatform.transform.position.y + 100, 0);
					goHammer [i].transform.position = handlePos [i] + new Vector3 (0, goPlatform.transform.position.y + 4, 0);					
				}
		
				Vector3 tpos = goPlayer [i].transform.position;
				Vector3 dpos = goHand [i].transform.position;
				tpos.x = Mathf.Lerp (tpos.x, dpos.x, 0.2f);
				goPlayer [i].transform.position = tpos;
								

				if (grabbed [i]) {
					int n = grabN [i];
					tpos = goChain [n].transform.position;					
					tpos.y = grabChainY [i] + (grabY [i] - handlePos [i].y);
					goChain [n].transform.position = tpos;
				
					if (Mathf.Abs (goPlayer [i].transform.position.x) < RELEASETHRESH
						|| psMoveWrapper.valueT [i - 1] == 0) {
						release (i);
					}
				} else {					
					if (psMoveWrapper.valueT [i - 1] == 255) {
						if (goPlayer [i].transform.position.x < -GRABTHRESH) {		
							grab (i, 1);
						}
						if (goPlayer [i].transform.position.x > GRABTHRESH) {		
							grab (i, 2);
						}						
					}					
				}				
			}
		}		
					
	}
	
	void playSoundName (string s)
	{
		
	}
		
	void Update ()
	{
		// Keyboard
		if (Input.GetKey (KeyCode.Q)) {
			goChain [1].transform.Translate (0, 0.03f, 0);			
		}
		if (Input.GetKey (KeyCode.R)) {
			goChain [2].transform.Translate (0, 0.03f, 0);
		}
		
		if (Input.GetKey (KeyCode.A)) {
			goPlayer [1].transform.Translate (0.02f, 0, 0);
		}
		if (Input.GetKey (KeyCode.D)) {
			goPlayer [1].transform.Translate (-0.02f, 0, 0);
		}
		
		if (Input.GetKeyDown (KeyCode.Return)) {
			GameObject.Find ("sound1").audio.Stop ();
			Application.LoadLevel ("Ending");
		}
		
		if (Input.GetKeyDown (KeyCode.Space)) {
			new WaitForSeconds (1f);
			hammer (1);
		}			
		
		if (Input.GetKeyDown (KeyCode.C)) {			
			if (psMoveWrapper.isConnected) {
				psMoveWrapper.Disconnect ();
				debugbox ("PS Move Disconnected.");
			} else {
				psMoveWrapper.Connect ();				
				if (psMoveWrapper.isConnected) {
					debugbox ("PS Move Connected!");
				} else {
					debugbox ("PS Move Connection Failed.");
				}
			}
		}
		
		if (Input.GetKeyDown (KeyCode.Backspace)) {						
			Application.LoadLevel ("Main");
		}
		
		// Calculation.
		float minDist = 999f;		
		for (int i=1; i<=floorNumber; i++) {
			float tmp = Mathf.Abs (transform.position.y - floorY [i]);
			if (tmp < minDist) {
				minDist = tmp;
				currFloor = i;
			}
		}
		
		// Dropping
//		if (goPlatform.transform.position.y > -4.5f) {
//			for (int i=1; i<=2; i++) {
//				if (!grabbed [i])
//					goChain [i].transform.Translate (0, -0.01f, 0);
//			}
//		}			

		
		// Time
		if (!globalvar.isFinished)
			globalvar.remainTime -= Time.deltaTime;
		GameObject.Find ("ClockHand").transform.localRotation = Quaternion.Euler (0, 0, Mathf.Lerp (115, 380, globalvar.remainTime / globalvar.allTime));
		if (globalvar.timePeriod == 0) {
			// 1/3 time left!
			if (globalvar.remainTime / globalvar.allTime < 1 / 3f) {
				globalvar.timePeriod = 1;
				faceSet (3, 1);
				StartCoroutine(flashClock(3,0.1f));
				
				AudioSource.PlayClipAtPoint (globalvar.acFaster, transform.position);
				goSound1.audio.clip = globalvar.bgFast;
				goSound1.audio.volume=0.8f;
				goSound1.audio.Play();
			}
		} else if (globalvar.timePeriod == 1) {
			// fail
			if (globalvar.remainTime < 0) {
				globalvar.timePeriod = 2;
				globalvar.isTimeUp=true;
				faceSet (2, 5);			
				StartCoroutine(flashClock(2,0.3f));
				
				goSound1.audio.Stop ();
				AudioSource.PlayClipAtPoint (globalvar.soundFail, transform.position);
				AudioSource.PlayClipAtPoint (globalvar.acOhNo, transform.position);
				StartCoroutine (delayLoad (4f, "BadEnd"));
			}			
		} else {
		}
		
			
		if (!isFallen) {
			Bounds bounds = goPlatform.renderer.bounds;
			float lowY = bounds.center.y - bounds.size.y / 2;
			for (int i=1; i<=globalvar.playerCount; i++) {
				if (goPlayer [i].transform.position.y < lowY - 0.5f) {
					// Fall!
					isFallen = true;				
					debugbox ("Fall!");
					if(globalvar.isTimeUp)
						continue;
					
					AudioSource.PlayClipAtPoint(globalvar.soundFall,transform.position);
					AudioSource.PlayClipAtPoint (globalvar.acGasp, transform.position);
					faceSet (2, 4);										
					StartCoroutine (delayLoad (2f, "Main"));					
				}				
			}			
		}
				
//		if(Time.time>soundSwitchTime){
//			if(goSound1.audio.volume<1)
//				goSound1.audio.volume+=0.05f;
//			if(goSound2.audio.volume>0)
//				goSound2.audio.volume-=0.05f;
//		}			
		
		// Face
		faceUpdate ();

		if (isDebug)
			GameObject.Find ("_currFloor").guiText.text = currFloor + " left: " + globalvar.floorHitLeft [currFloor];
	}
}
