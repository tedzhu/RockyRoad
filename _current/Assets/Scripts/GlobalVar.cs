using UnityEngine;
using System.Collections;
using System.Linq;

public class GlobalVar : MonoBehaviour
{
	public Texture texFaceFall;
	public Texture texFaceNervous;
	public Texture[] animFaceHappy;
	public int animFaceHappyIndex = 0;
	public Texture[] animFaceNormal;
	public int animFaceNormalIndex = 0;
	public Texture[] animSmoke;
	public Object[] hammerSound;
	public AudioClip tadaSound;
	public int[] floorHitLeft = new int[10];
	public int finishedFloorNumber = 0;
	public float allTime;
	public float remainTime;
	public int timePeriod;
	public int playerCount;
	public bool isFinished;
	public bool isTimeUp;
	public AudioClip soundFail;
	public AudioClip soundFall;
	public AudioClip bgNormal;
	public AudioClip bgFast;
	public AudioClip acFaster;
	public AudioClip acGasp;
	public AudioClip acOhNo;
	public AudioClip[] acYah;
	public AudioClip acWoohoo;
	public AudioClip acSuccess;
	void Awake ()
	{
		print ("Global Start");
		DontDestroyOnLoad (this);
		if (Application.loadedLevelName == "0")
			Application.LoadLevel ("Main");
						
		hammerSound = Resources.LoadAll ("SE/hammer", typeof(AudioClip));	
		tadaSound = (AudioClip)Resources.Load ("SE/tada");
		soundFail= (AudioClip)Resources.Load ("SE/fail");
		soundFall= (AudioClip)Resources.Load ("SE/fall");
		bgNormal=(AudioClip)Resources.Load ("SE/bg");
		bgFast=(AudioClip)Resources.Load ("SE/bg_fast");
		acFaster=(AudioClip)Resources.Load ("SE/faster");
		acGasp=(AudioClip)Resources.Load ("SE/gasp");
		acOhNo=(AudioClip)Resources.Load ("SE/oh no");
		acYah = Resources.LoadAll ("SE/yah", typeof(AudioClip)).Cast<AudioClip> ().ToArray ();
		acWoohoo=(AudioClip)Resources.Load ("SE/woohoo");
		acSuccess=(AudioClip)Resources.Load ("SE/success");
		
		animSmoke = Resources.LoadAll ("Smoke", typeof(Texture)).Cast<Texture> ().ToArray ();
		animFaceNormal = Resources.LoadAll ("UI/normal status", typeof(Texture)).Cast<Texture> ().ToArray ();
		animFaceHappy = Resources.LoadAll ("UI/happy event", typeof(Texture)).Cast<Texture> ().ToArray ();
		
		texFaceFall = (Texture)Resources.Load ("UI/fall", typeof(Texture));
		texFaceNervous = (Texture)Resources.Load ("UI/nervous", typeof(Texture));			
	}
}
