using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBehavior : MonoBehaviour
{
	private Vector3 rot;
	private GameObject earth, sun, moon;
	private int view = 3;	//Set initial view to top-down view
	
    // Start is called before the first frame update
    void Start()
    {
		earth = GameObject.Find("Earth");
		sun = GameObject.Find("Sun");
		moon = GameObject.Find("Moon");
    }

    // Update is called once per frame
    void Update()
    {
		EarthBehavior earthObj = earth.GetComponent("EarthBehavior") as EarthBehavior;
		SunBehavior sunObj = sun.GetComponent("SunBehavior") as SunBehavior;
		MoonBehavior moonObj = moon.GetComponent("MoonBehavior") as MoonBehavior;
		
		switch (view) {
			// Sun View
			case 0:	
				transform.position = Vector3.MoveTowards(earthObj.GetPosition(), sunObj.GetPosition(), .15f);
				transform.LookAt(sunObj.GetTransform());
				break;
				
			// Earth View
			case 1:	
				transform.position = earthObj.GetPosition();
				transform.position -= new Vector3(0, 0, .15f);
				transform.LookAt(earthObj.GetTransform());
				break;
				
			// Moon View	
			case 2: 
				transform.position = Vector3.MoveTowards(earthObj.GetPosition(), moonObj.GetPosition(), .15f);
				transform.LookAt(moonObj.GetTransform());
				break;
				
			// Top-down view
			case 3: 	
				transform.position = new Vector3(0, 3500, 0);
				transform.LookAt(sunObj.GetTransform());
				break;
		}
	}
	
	public Vector3 GetPosition() {
		return transform.position;
	}

	public void SetView(int view) {
		this.view = view;
	}
	
	public int GetView() {
		return view;
	}
}
