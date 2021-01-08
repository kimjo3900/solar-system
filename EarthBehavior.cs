using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EarthBehavior : MonoBehaviour
{
	private double speed;
	private float scaleFactor;
	private Vector3 pos, camPos, sunPos, camDist, sunDist, iniScale, norm;
	private GameObject cam, sun, date;
	private Material mat;
	private const double tRev = 31556925.445; //number of seconds in a tropical year aka 365.24219 days
	private const double rotPerSec = .00417807; //number of degrees of rotation about Earth's axis per second 
	private double xPos, zPos, t, E;
	
	
    // Start is called before the first frame update
    void Start()
    {
		mat = GetComponent<Renderer>().material;
		sun = GameObject.Find("Sun");
        cam = GameObject.Find("Main Camera");
		date = GameObject.Find("Date & Time");
		iniScale = transform.localScale;
		
		//later, initialize t to be a certain number of seconds since 2020 perihelion - January 5, 2020 2:47AM EST
		//Transform.forward points out from Dhaka, Bangladesh which is 11 hours ahead of EST
		//perihelion oocurs on Jan 5, 2020 1:47PM BST
		
		//define t=0 to be Jan 5, 2020 2:47AM EST perihelion
		t = 0;
		//t = 30423777.445; 	//Dec 2020 solstice
		
		//initialize Earth's rotation for t=0
		transform.Rotate(Vector3.up, -114.643f);
    }

    // Update is called once per frame
    void Update()
    {
		//Get current speed multiplier from Date & Time
		DateTimeDisplay dateObj = date.GetComponent("DateTimeDisplay") as DateTimeDisplay;
		speed = dateObj.speed;
		
		//Rotation about axis
		transform.Rotate(Vector3.up,(float)(-rotPerSec * speed * Time.deltaTime));
		
		//Orbit
		E = t/tRev*2*Math.PI;
		
		for (int i=0; i<3; i++) {
			E = NewtMethod(E);
		}
		
		xPos = 1495.96329*Math.Cos(E) - 24.9825869;
		zPos = 1495.75469*Math.Sqrt(1-(Math.Pow(1495.96329*Math.Cos(E), 2)/2237906.17));
		if (t % tRev > tRev/2)
			zPos = -zPos;
		
		
		transform.position = new Vector3((float)xPos, 0, (float)zPos);
		//transform.position = new Vector3(1470.981f, 0, 0);
		
		
		pos = GetPosition();
		norm = new Vector3(-pos.z, 0, pos.x);
		
		Debug.Log("t=" + t.ToString() + ", sunDir=" + (-transform.position).ToString("F4") + ", forward=" + transform.forward.ToString("F4") + ", angle= " + Vector3.Angle(-transform.position,transform.forward).ToString("F4"));
		
		//Illuminate earth surface that's facing the sun
		sunDist = -transform.position; 	//sunDist is earth-to-sun vector
		mat.SetVector("_SunDir", sunDist.normalized);
		
		//Scale earth size if earth-cam distance is large
		SunBehavior sunObj = sun.GetComponent("SunBehavior") as SunBehavior;
		CamBehavior camObj = cam.GetComponent("CamBehavior") as CamBehavior;
		camPos = camObj.GetPosition();
		camDist = camPos - pos;
		
		if (!camObj.earthView) {
			scaleFactor = camDist.magnitude / 10;
			transform.localScale = scaleFactor * iniScale;
		}
		else {
			transform.localScale = iniScale;
		}
		
		t+= speed*Time.deltaTime;
    }
	
	public Vector3 GetPosition() {
		return transform.position;
	}
	
	public Transform GetTransform() {
		return transform;
	}
	
	public double NewtMethod(double E) {
		return E - (5022440.67*(E-.0167*Math.Sin(E))-t) / (5022440.67*(1-.0167*Math.Cos(E)));
	}
}
