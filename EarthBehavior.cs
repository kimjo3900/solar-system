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
	private const double tRev = 31556925.445; // number of seconds in a tropical year which is 365.24219265 days
	private const double rotPerSec = .00417807; // number of degrees of rotation about Earth's axis per second - note that a sidereal day is the time it takes for the Earth to make one complete rotation about its axis
												// w.r.t. fixed stars and is 23h 56m 4.09s, which is less than the length of one solar day
	
    // Start is called before the first frame update
    void Start()
    {
		mat = GetComponent<Renderer>().material;
        cam = GameObject.Find("Main Camera");
		date = GameObject.Find("Date & Time");
		iniScale = transform.localScale;
		
		// t is the number of seconds since Jan 5, 2020 2:47AM EST perihelion
		// Later, calculuate t based on the current date/time by subtracting DateTime(2020, 1, 5, 2, 47, 0) from DateTime.Now
		t = 0;
		
		//initialize Earth's rotation for t=0
		//Transform.forward points out from Dhaka, Bangladesh which is 11 hours ahead of EST
		//perihelion oocurs on Jan 5, 2020 1:47PM BST
		transform.Rotate(Vector3.up, -116.75f);
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
			E = NewtMethod(E);
		}
		
		xPos = 1495.96329*Math.Cos(E) - 24.9825869;
		zPos = 1495.75469*Math.Sqrt(1-(Math.Pow(1495.96329*Math.Cos(E), 2)/2237906.17));
		if (t % tRev > tRev/2)
			zPos = -zPos;
		
		transform.position = new Vector3((float)xPos, 0, (float)zPos);
		
		//Debug.Log(-pos);
		
		//Illuminate earth surface that's facing the sun
		sunDist = -transform.position; 	//sunDist is earth-to-sun vector
		mat.SetVector("_SunDir", sunDist.normalized);
		
		//Scale earth size if earth-cam distance is large
		CamBehavior camObj = cam.GetComponent("CamBehavior") as CamBehavior;
		camPos = camObj.GetPosition();
		camDist = camPos - transform.position;
		
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
	
	public double GetT() {
		return t;
	}
	
	public double NewtMethod(double E) {
		return E - (5022440.67*(E-.0167*Math.Sin(E))-t) / (5022440.67*(1-.0167*Math.Cos(E)));
	}
}
