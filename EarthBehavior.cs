using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EarthBehavior : MonoBehaviour
{
	private double speed, xPos, zPos, t, E;
	private float scaleFactor;
	private Vector3 camPos, camDist, sunDist, iniScale, norm;
	private Material mat;
	private GameObject cam, date;
	private const double tRev = 31556925.445;	// number of seconds in a tropical year which is 365.24219265 days
	private const double rotPerSec = .00417807; // number of degrees of rotation about Earth's axis per second; rotates 360 degrees in one sidereal day which is slightly less than a solar day
	private DateTime dtNow, dtPeri;
	private TimeSpan tSincePeri;
	
    // Start is called before the first frame update
    void Start()
    {
		mat = GetComponent<Renderer>().material;
        cam = GameObject.Find("Main Camera");
		date = GameObject.Find("Date & Time");
		iniScale = transform.localScale;
		
		// Compute t - the number of seconds since Jan 5, 2020 5:47 UTC (2020 perihelion)
		dtNow = DateTime.UtcNow;
		dtPeri = new DateTime(2020, 1, 5, 7, 47, 0);
		tSincePeri = dtNow.Subtract(dtPeri);

		t = tSincePeri.TotalSeconds;
		
		//initialize Earth's rotation for t=0
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
	
	public Vector3 GetPosition() {
		return transform.position;
	}
	
	public Transform GetTransform() {
		return transform;
	}
	
	public double GetT() {
		return t;
	}
	
	public double NewtonsMethod(double E) {
		return E - (5022440.67*(E-.0167*Math.Sin(E))-t) / (5022440.67*(1-.0167*Math.Cos(E)));
	}

}
