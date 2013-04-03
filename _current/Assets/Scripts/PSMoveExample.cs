#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414

using UnityEngine;
using System.Collections;
using System;

public class PSMoveExample : MonoBehaviour {
	
	public PSMoveWrapper psMoveWrapper;
	
	public GameObject gem, handle;
	public GameObject gem2, handle2;
	public int inx;
	public int iny;
	public int inz;
	public int inxl;
	public int inyl;
	public int inzl;
	public float xtest;
	public bool isMirror = true;
	public Vector3 inPosition;
	public Vector3 inPosition2;
	public bool inVec=false;
	public bool inVec2=false;
	
	public float zOffset = 0;
	Quaternion temp = new Quaternion(0,0,0,0);
	Quaternion temp2 = new Quaternion(0,0,0,0);	
	
	#region GUI Variables
	string connectStr = "Connect";
	string cameraStr = "Camera Switch On";
	string rStr = "0", gStr = "0", bStr = "0";
	string rumbleStr = "0";
	#endregion
	
	public float tempY;
	bool stopcon;
	bool test1; // for the start calibration
	bool test2;
	// Use this for initialization
	void Start () {
		stopcon= false;
		test1=false;
		test2=false;
		inx=3;
		iny=8;
		inz=16;
		inxl=6;
		inyl=12;
		inzl=20;
	}
	
	void Update() {
					
		
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//**initial position gathering
		if(!psMoveWrapper.isButtonSquare[0]){
			inVec=false;
		}
		if(!inVec && psMoveWrapper.isButtonSquare[0]){
			inPosition=psMoveWrapper.position[0];
			inVec=true;
		}
		if(!psMoveWrapper.isButtonSquare[1]){
			inVec2=false;
		}
		if(!inVec2 && psMoveWrapper.isButtonSquare[1]){
			inPosition2=psMoveWrapper.position[1];
			inVec2=true;
		}
		//****************************
		Vector3 gemPos, handlePos;
		Vector3 gemPos2, handlePos2;
		//**Vector 3 3-dimension position calculation 
		if(psMoveWrapper.isButtonSquare[0]){
			gemPos = GameObject.Find("SphereHammer").transform.position+(psMoveWrapper.position[0]-inPosition)/8;
			gemPos.Set(gemPos.x,gemPos.y,gemPos.z*(-1));// the ball position
			handlePos = GameObject.Find("SphereHammer").transform.position+(psMoveWrapper.position[0]-inPosition)/8;// the handle position
			handlePos.Set(handlePos.x,handlePos.y,handlePos.z*(-1));
		
		}else{
			gemPos = GameObject.Find("SphereHammer").transform.position;// the ball position
			handlePos = GameObject.Find("SphereHammer").transform.position;// the handle position
		}
		if(psMoveWrapper.isButtonSquare[1]){
			gemPos2 = GameObject.Find("SphereHammer2").transform.position+(psMoveWrapper.position[1]-inPosition2)/8;
			gemPos2.Set(gemPos2.x,gemPos2.y,gemPos2.z*(-1));// the ball position
			handlePos2 = GameObject.Find("SphereHammer2").transform.position+(psMoveWrapper.position[1]-inPosition2)/8;// the handle position
			handlePos2.Set(handlePos2.x,handlePos2.y,handlePos2.z*(-1));
		
		}else{
			gemPos2 = GameObject.Find("SphereHammer2").transform.position;// the ball position
			handlePos2 = GameObject.Find("SphereHammer2").transform.position;// the handle position
		}
		//*********************************************
		//gemPos = GameObject.Find("Plane").transform.position;// the ball position
		//handlePos = GameObject.Find("Plane").transform.position;// the handle position
		if(isMirror) {
			gem.transform.localPosition = gemPos;
			handle.transform.localPosition = handlePos;
			handle.transform.localRotation = Quaternion.Euler(psMoveWrapper.orientation[0]);
			gem2.transform.localPosition = gemPos2;
			handle2.transform.localPosition = handlePos2;
			handle2.transform.localRotation = Quaternion.Euler(psMoveWrapper.orientation[1]);
		}
		else {
			gemPos.z = -gemPos.z + zOffset;
			handlePos.z = -handlePos.z + zOffset;
			gem.transform.localPosition = gemPos;
			handle.transform.localPosition = handlePos;		
			handle.transform.localRotation = Quaternion.LookRotation(gemPos - handlePos);
			handle.transform.Rotate(new Vector3(0,0,psMoveWrapper.orientation[0].z));

		/* using quaternion rotation directly
		 * the rotations on the x and y axes are inverted - i.e. left shows up as right, and right shows up as left. This code fixes this in case 
		 * the object you are using is facing away from the screen. Comment out this code if you do want an inversion along these axes
		 * 
		 * Add by Karthik Krishnamurthy*/
			
			temp = psMoveWrapper.qOrientation[0];
			temp.x = -psMoveWrapper.qOrientation[0].x;
			temp.y = -psMoveWrapper.qOrientation[0].y;
			handle.transform.localRotation = temp;
			//  user2 
			gemPos2.z = -gemPos2.z + zOffset;
			handlePos2.z = -handlePos2.z + zOffset;
			gem2.transform.localPosition = gemPos2;
			handle2.transform.localPosition = handlePos2;		
			handle2.transform.localRotation = Quaternion.LookRotation(gemPos2 - handlePos2);
			handle2.transform.Rotate(new Vector3(0,0,psMoveWrapper.orientation[1].z));

		/* using quaternion rotation directly
		 * the rotations on the x and y axes are inverted - i.e. left shows up as right, and right shows up as left. This code fixes this in case 
		 * the object you are using is facing away from the screen. Comment out this code if you do want an inversion along these axes
		 * 
		 * Add by Karthik Krishnamurthy*/
			
			temp2 = psMoveWrapper.qOrientation[1];
			temp2.x = -psMoveWrapper.qOrientation[1].x;
			temp2.y = -psMoveWrapper.qOrientation[1].y;
			handle2.transform.localRotation = temp2;
		
		}
			
		if(psMoveWrapper.position[0].x<inxl&&psMoveWrapper.position[0].x>inx&&psMoveWrapper.position[0].z<inzl&&psMoveWrapper.position[0].z>inz&&psMoveWrapper.position[0].y>iny &&psMoveWrapper.position[0].y<inyl){
			test1=true;
			test2=true;
			
		}
		if(psMoveWrapper.acceleration[0].z>300 && psMoveWrapper.acceleration[0].y>240 &&psMoveWrapper.position[0].z<15){
				stopcon=true;
				audio.Play();
		}
		if(psMoveWrapper.acceleration[1].z>300 && psMoveWrapper.acceleration[1].y>240 &&psMoveWrapper.position[1].z<15){
				stopcon=true;
				GameObject.Find("Gem").audio.Play();
		}
	/*	if(psMoveWrapper.wasPressed(0, PSMoveWrapper.CIRCLE)) {
		}*/
		
		if(psMoveWrapper.valueT[0]==255){
			psMoveWrapper.SetRumble(0, 1);
		}
		if(psMoveWrapper.valueT[0]==0){
			psMoveWrapper.SetRumble(0, 0);
		}
	}
	
	//void OnCollisionEnter(Collision co){
	//	if(co.gameObject.tag=="Respawn"){
	//		stopcon=true;
	//		audio.Play();
	//	}
	//}
	void OnGUI() {// the control menu I guess
		GUI.Label(new Rect(10, 10, 150, 100),  "PS Move count : " + psMoveWrapper.moveCount); // HOW 
		GUI.Label(new Rect(140, 10, 150, 100),  "PS Nav count : " + psMoveWrapper.navCount);
		GUI.Label(new Rect(270, 10, 150, 100),  "0_X : " + psMoveWrapper.position[0].x);
		GUI.Label(new Rect(340, 10, 150, 100),  "0_Y : " + psMoveWrapper.position[0].y);
		GUI.Label(new Rect(420, 10, 150, 100),  "0_Z : " + psMoveWrapper.position[0].z);
		if(test1&&test2){
				//stopcon=false;
				GUI.Label(new Rect(840, 10, 150, 100),  "test1 : " + test1);
				//if(tempY-psMoveWrapper.position[0].y>0){
				//	stopcon=true;
				//	
				//}		
		}
		if(stopcon){
			GUI.Label(new Rect(740, 10, 150, 100),  "Stopcon : " + stopcon);
			//GUI.Label(new Rect(660, 10, 150, 100),  "Testx : " + xtest);
			//stopcon=false;
		}
		
		//if(psMoveWrapper.acceleration[0].y> 120){
		//	GUI.Label(new Rect(500, 10, 150, 100),  "0_acX : " + psMoveWrapper.acceleration[0].x);
		//}
		//if(psMoveWrapper.acceleration[0].y > 120){
			//GUI.Label(new Rect(580, 10, 150, 100),  "0_acY : " + psMoveWrapper.acceleration[0].y);
		//}
		//if(psMoveWrapper.acceleration[0].y > 120){
		//	}
		
		if(GUI.Button(new Rect(20, 40, 100, 35), connectStr)) {// if not connected => connect the PSMove
			if(connectStr == "Connect") {
				psMoveWrapper.Connect();
				if(psMoveWrapper.isConnected) {
					connectStr = "Disconnect";// if not connected => show connect the PSMove
				}
			}
			else {
				psMoveWrapper.Disconnect();
				connectStr = "Connect";// if connected , then show disconnect
				Reset();
			}
		}
		
		if(psMoveWrapper.isConnected) {
			//camera stream on/off
			if(GUI.Button(new Rect(5, 80, 130, 35), cameraStr)) {
				if(cameraStr == "Camera Switch On") {
					psMoveWrapper.CameraFrameResume();
					cameraStr = "Camera Switch Off";
				}
				else {
					psMoveWrapper.CameraFramePause();
					cameraStr = "Camera Switch On";
				}
			}
			
			//color and rumble for move number 0
			if(psMoveWrapper.moveConnected[0]) {// if it is connected (psmove)
				//Set Color and Track
				//GUI.Label(new Rect(300, 50, 200,20), "R,G,B are floats that fall in 0 ~ 1");
				//GUI.Label(new Rect(260, 20, 20, 20), "R");
				//rStr = GUI.TextField(new Rect(280, 20, 60, 20), rStr);
				//GUI.Label(new Rect(350, 20, 20, 20), "G");
				//gStr = GUI.TextField(new Rect(370, 20, 60, 20), gStr);
				//GUI.Label(new Rect(440, 20, 20, 20), "B");
				//bStr = GUI.TextField(new Rect(460, 20, 60, 20), bStr); //*** create a text field where user can edit
				if(GUI.Button(new Rect(550, 30, 160, 35), "SetColorAndTrack")) {
					try {
						//float r = float.Parse(rStr);
						//float g = float.Parse(gStr);
						//float b = float.Parse(bStr);
						//psMoveWrapper.SetColorAndTrack(0, new Color(r,g,b)); // set color
					}
					catch(Exception e) {
						Debug.Log("input problem");
					}
				}
				//Rumble
				rumbleStr = GUI.TextField(new Rect(805, 20, 40, 20), rumbleStr);
				GUI.Label(new Rect(800, 50, 200,20), "0 ~ 19");
				if(GUI.Button(new Rect(870, 30, 100, 35), "Rumble")) {
					try {
						int rumbleValue = int.Parse(rumbleStr);
						psMoveWrapper.SetRumble(0, rumbleValue); //****set the rumble
					}
					catch(Exception e) {
						Debug.Log("input problem");
					}
				}
			}
			
			//move controller information
			for(int i=0; i<PSMoveWrapper.MAX_MOVE_NUM; i++) // MAX_MOVE_NUM
			{
				if(psMoveWrapper.moveConnected[i]) { // !!!! ********** P
					string display = "PS Move #" + i + 
						"\nPosition:\t\t"+psMoveWrapper.position[i] + 
						"\nVelocity:\t\t"+psMoveWrapper.velocity[i] + 
						"\nAcceleration:\t\t"+psMoveWrapper.acceleration[i] + 
						"\nOrientation:\t\t"+psMoveWrapper.orientation[i] + 
						"\nAngular Velocity:\t\t"+psMoveWrapper.angularVelocity[i] + 
						"\nAngular Acceleration:\t\t"+psMoveWrapper.angularAcceleration[i] + 
						"\nHandle Position:\t\t"+psMoveWrapper.handlePosition[i] + 
						"\nHandle Velocity:\t\t"+psMoveWrapper.handleVelocity[i] + 
						"\nHandle Acceleration:\t\t"+psMoveWrapper.handleAcceleration[i] +
						"\n" +
						"\nTrigger Value:\t\t" + psMoveWrapper.valueT[i] +
						"\nButtons:\t\t" + GetButtonStr(i) +
						"\nSphere Color:\t\t" + psMoveWrapper.sphereColor[i] +
						"\nIs Tracking:\t\t" + psMoveWrapper.isTracking[i] +
						"\nTracking Hue:\t\t" + psMoveWrapper.trackingHue[i];
					GUI.Label(new Rect( 10 + 650 * (i/2), 120+310*(i%2), 300, 400),   display);
				}
			}
			for(int j = 0; j < PSMoveWrapper.MAX_NAV_NUM; j++) {
				if(psMoveWrapper.navConnected[j]) {	
					string navDisplay = "PS Nav #" + j + 
						"\nAnalog X:\t\t" + psMoveWrapper.valueNavAnalogX[j] +
						"\nAnalog Y:\t\t" + psMoveWrapper.valueNavAnalogY[j] +
						"\nL2 Value:\t\t" + psMoveWrapper.valueNavL2[j] +
						"\nButtons:\t\t" + GetNavButtonStr(j);
					GUI.Label(new Rect(400, 100 + 95 * j, 150, 95),   navDisplay);
				}
			}
		}
		
		
	}
	
	private string GetButtonStr(int num) {
		string result = "";
		if(psMoveWrapper.isButtonMove[num]) {
			result += "MOVE ";
		}
		if(psMoveWrapper.isButtonCircle[num]) {
			result += "CIRCLE ";
		}
		if(psMoveWrapper.isButtonSquare[num]) {
			result += "SQUARE ";
		}
		if(psMoveWrapper.isButtonCross[num]) {
			result += "CROSS ";
		}
		if(psMoveWrapper.isButtonTriangle[num]) {
			result += "TRIANGLE ";
		}
		if(psMoveWrapper.isButtonStart[num]) {
			result += "START ";
		}
		if(psMoveWrapper.isButtonSelect[num]) {
			result += "SELECT ";
		}
		return result;
	}

	private string GetNavButtonStr(int num) {
		string result = "";
		if(psMoveWrapper.isNavButtonCircle[num]) {
			result += "CIRCLE ";
		}
		if(psMoveWrapper.isNavButtonCross[num]) {
			result += "CROSS ";
		}
		if(psMoveWrapper.isNavUp[num]) {
			result += "UP ";
		}
		if(psMoveWrapper.isNavDown[num]) {
			result += "DOWN ";
		}
		if(psMoveWrapper.isNavLeft[num]) {
			result += "LEFT ";
		}
		if(psMoveWrapper.isNavRight[num]) {
			result += "RIGHT ";
		}
		if(psMoveWrapper.isNavButtonL1[num]) {
			result += "L1 ";
		}
		if(psMoveWrapper.isNavButtonL3[num]) {
			result += "L3 ";
		}
		return result;
	}	
	
	private void Reset() {
		cameraStr = "Camera Switch On";
		rStr = "0"; 
		gStr = "0"; 
		bStr = "0";
		rumbleStr = "0";
	}
}
