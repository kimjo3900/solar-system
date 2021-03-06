﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MoonBehavior : MonoBehaviour
{
	private double speed, t, T, L_0, l, lp, D, F, r, dL, L, S, h, N, B, x, y, z;
	private float scaleFactor;
	private Vector3 earthDist, earthPos, camPos, camDist, iniScale;
	private Material mat;
	private GameObject cam, earth, date;
	private const double rotPerSec = 0.00015250414366;	// number of degrees of rotation about Moon's axis per second
	private const double tIni = 0.200104704;			// Julian centuries since J2000 (Jan 1, 2000, 12:00 UTC)
	private const double secPerCent = 3155760000;		// number of seconds in Julian century
	private const double arcs = 206264.806247; 			// Arcseconds per radian
	
    // Start is called before the first frame update
    void Start()
    {
		mat = GetComponent<Renderer>().material;
		earth = GameObject.Find("Earth");
        cam = GameObject.Find("Main Camera");
		date = GameObject.Find("Date & Time");
		iniScale = transform.localScale;
		
		// Initialize Moon's rotation
		EarthBehavior earthObj = earth.GetComponent("EarthBehavior") as EarthBehavior;
		t = earthObj.GetT();
		transform.Rotate(Vector3.up, -(float)(rotPerSec * t));
    }

    // Update is called once per frame
    void Update()
    {
		// Get current speed multiplier from Date & Time
		DateTimeDisplay dateObj = date.GetComponent("DateTimeDisplay") as DateTimeDisplay;
		speed = dateObj.speed;
		
		// Rotation about axis
		transform.Rotate(Vector3.up, (float)(-rotPerSec * speed * Time.deltaTime));
		
		// Get t from EarthBehavior
		EarthBehavior earthObj = earth.GetComponent("EarthBehavior") as EarthBehavior;
		t = earthObj.GetT();
		
		T = tIni + t/secPerCent;   
		L_0 = Frac(0.606433 + 1336.851344*T);	  			// Mean longitude
		l = 2*Math.PI*Frac(0.374897 + 1325.552410*T);   	// Moon's mean anomaly
		lp = 2*Math.PI*Frac(0.993133 + 99.997361*T);   		// Sun's mean anomaly
		D = 2*Math.PI*Frac(0.827361 + 1236.853086*T);   	// Diff. long. Moon-Sun
		F = 2*Math.PI*Frac(0.259086 + 1342.227825*T);   	// Argument of latitude 

		// Compute Earth-Moon distance
		r = 3.85 - 0.20905*Math.Cos(l) - 0.03699*Math.Cos(2*D-l) - 0.02956*Math.Cos(2*D) - 0.0057*Math.Cos(2*l) + 0.00246*Math.Cos(2*l-2*D) - 0.00205*Math.Cos(lp-2*D) - 0.00171*Math.Cos(l+2*D) - 0.00152*Math.Cos(l+lp-2*D);
		
		CamBehavior camObj = cam.GetComponent("CamBehavior") as CamBehavior;
		
		// Greatly exaggerate Earth-Moon distance if in top-down view
		if (camObj.GetView() == 3) {
			r *= 100;
		}
		
		// Compute right ascension 
		dL = 22640*Math.Sin(l) - 4586*Math.Sin(l-2*D) + 2370*Math.Sin(2*D) + 769*Math.Sin(2*l) - 668*Math.Sin(lp) - 412*Math.Sin(2*F) - 212*Math.Sin(2*l-2*D) - 206*Math.Sin(l+lp-2*D) + 192*Math.Sin(l+2*D)
			 - 165*Math.Sin(lp-2*D) + 148*Math.Sin(l-lp) - 125*Math.Sin(D) - 110*Math.Sin(l+lp) - 55*Math.Sin(2*F-2*D);
		L = 2*Math.PI*Frac(L_0 + dL/1296000);

		// Compute declination
		S = F + (dL + 412*Math.Sin(2*F) + 541*Math.Sin(lp)) / arcs;
		h = F - 2*D;
		N = -526*Math.Sin(h) + 44*Math.Sin(l+h) - 31*Math.Sin(h-l) - 25*Math.Sin(F-2*l) - 23*Math.Sin(lp+h) + 21*Math.Sin(F-l) + 11*Math.Sin(h-lp);
		B = (18520*Math.Sin(S) + N) / arcs;

		// Compute (x,y,z) position of moon w.r.t. Earth
		y = r*Math.Sin(B);
		x = Math.Sqrt((Math.Pow(r, 2)-Math.Pow(y, 2))) * Math.Sin(L-0.250434);
		z = -Math.Sqrt((Math.Pow(r, 2)-Math.Pow(y, 2))) * Math.Cos(L-0.250434);

		earthDist = new Vector3((float)x, (float)y, (float)z);
		earthPos = earthObj.GetPosition();

		transform.position = earthPos + earthDist;
		
        // Illuminate moon surface that's facing the sun
		mat.SetVector("_LightDir", -transform.position.normalized / 2);

		// Adjust scale based on view mode
		if (camObj.GetView() == 3) {
			scaleFactor = 1000;
			transform.localScale = scaleFactor * iniScale;
		}
		else if (camObj.GetView() == 0) {
			scaleFactor = 10;
			transform.localScale = scaleFactor * iniScale;
		}
		else if (camObj.GetView() == 1) {
			transform.localScale = iniScale;
		}
		else {
			scaleFactor = 78;
			transform.localScale = scaleFactor * iniScale;
		}
    }

	// Return the fraction part of a number
	public double Frac(double x) {
		return x-Math.Floor(x);
	}
	
	// Get the Moon's position
	public Vector3 GetPosition() {
		return transform.position;
	}
	
	// Get the object's transform
	public Transform GetTransform() {
		return transform;
	}
	
	// Set the Moon's rotation
	public void SetRot(double t) {
		transform.Rotate(Vector3.up, -(float)(rotPerSec * t));
	}
}
