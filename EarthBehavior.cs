using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EarthBehavior : MonoBehaviour
{
	private double speed, xPos, zPos, t, dT, E;
	private float scaleFactor;
	private Vector3 camPos, camDist, sunDist, iniScale, norm;
	private Material mat;
	private GameObject cam, date;
	private const double tRev = 31556925.445;	// number of seconds in a tropical year which is 365.24219265 days
	private const double rotPerSec = .00417807; // number of degrees of rotation about Earth's axis per second; rotates 360 degrees in one sidereal day which is slightly less than a solar day
	private DateTime dtPeri = new DateTime(2020, 1, 5, 7, 47, 0);	// DateTime of Jan 2020 perihelion - Jan 5, 2020 07:47 UTC
	
    // Start is called before the first frame update
    void Start()
    {
		mat = GetComponent<Renderer>().material;
        cam = GameObject.Find("Main Camera");
		date = GameObject.Find("Date & Time");
		iniScale = transform.localScale;
		
		// Initialize t - the number of seconds since Jan 2020 perihelion
		SetT(DateTime.UtcNow);
		
		//initialize Earth's rotation
		transform.Rotate(Vector3.up, -116.75f - (float)(rotPerSec * t));
    }

    // Update is called once per frame
    void Update()
    {
		//Get current speed multiplier from Date & Time
		DateTimeDisplay dateObj = date.GetComponent("DateTimeDisplay") as DateTimeDisplay;
		speed = dateObj.speed;
		
		//Rotation about axis
		transform.Rotate(Vector3.up, (float)(-rotPerSec * speed * Time.deltaTime));
		
		//Orbit
		E = t/tRev*2*Math.PI;
		
		for (int i=0; i<3; i++) {
			E = NewtonsMethod(E);
		}
		
		xPos = 1495.96329*Math.Cos(E) - 24.9825869;
		zPos = 1495.75469*Math.Sqrt(1-(Math.Pow(1495.96329*Math.Cos(E), 2)/2237906.17));
		if (t % tRev > tRev/2 && t>0 || t % tRev > -tRev/2 && t<0)
			zPos = -zPos;
		
		transform.position = new Vector3((float)xPos, 0, (float)zPos);
		
		//Illuminate earth surface that's facing the sun
		sunDist = -transform.position; 	//sunDist is earth-to-sun vector
		mat.SetVector("_SunDir", sunDist.normalized);
		
		// Adjust scale based on view mode
		CamBehavior camObj = cam.GetComponent("CamBehavior") as CamBehavior;
		
		if (camObj.GetView() == 3) {
			scaleFactor = 1000;
			transform.localScale = scaleFactor * iniScale;
		}
		else {
			transform.localScale = iniScale;
		}
		
		t+= speed*Time.deltaTime;
    }
	
	// Get the Earth's position
	public Vector3 GetPosition() {
		return transform.position;
	}
	
	// Get the object's transform
	public Transform GetTransform() {
		return transform;
	}
	
	// Get t
	public double GetT() {
		return t;
	}
	
	// Set t based on the given DateTime
	public void SetT(DateTime dt) {
		dT = t;
		t = dt.Subtract(dtPeri).TotalSeconds;
		dT = t - dT;
	}
	
	// Set the Earth's rotation - only to be called when changing the date through the Date Selection menu
	public void SetRot(double t) {
		transform.Rotate(Vector3.up, (float)(-rotPerSec * t));
	}
	
	// Get dT - the difference in seconds between the new time and previous time
	public double GetDT() {
		return dT;
	}
	
	// Method for accurately computing the Earth's position as a function of t
	public double NewtonsMethod(double E) {
		return E - (5022440.67*(E-.0167*Math.Sin(E))-t) / (5022440.67*(1-.0167*Math.Cos(E)));
	}

}
