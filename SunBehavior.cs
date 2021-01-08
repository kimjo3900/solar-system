using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBehavior : MonoBehaviour
{
	private Vector3 pos, earthPos, camPos, earthDist, camDist, iniScale;
	private float scaleFactor;
	private GameObject cam, earth;
	private List<Vector3> sunScales;
	
    // Start is called before the first frame update
    void Start()
    {
		earth = GameObject.Find("Earth");
        cam = GameObject.Find("Main Camera");
		iniScale = transform.localScale;
		
		sunScales = new List<Vector3>();
		
		foreach (Transform child in transform) {
			sunScales.Add(child.localScale);
		}
    }

    // Update is called once per frame
    void Update()
    {
		//Scale sun size if sun-cam distance is large
		//Should try to avoid all these statements if cam position hasn't changed since last update
		
        EarthBehavior earthObj = earth.GetComponent("EarthBehavior") as EarthBehavior;
		CamBehavior camObj = cam.GetComponent("CamBehavior") as CamBehavior;
		pos = GetPosition();
		camPos = camObj.GetPosition();
		camDist = camPos - pos;
		
		if (camDist.magnitude > 283) {
			scaleFactor = camDist.magnitude / 283;
			transform.localScale = scaleFactor * iniScale;
			
			int i = 0;
			foreach (Transform child in transform) {
				child.localScale = scaleFactor * sunScales[i];
				i++;
			}
		}
    }
	
	public Vector3 GetPosition() {
		return transform.position;
	}
}
