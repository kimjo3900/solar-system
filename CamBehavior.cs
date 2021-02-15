using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBehavior : MonoBehaviour
{
	private Vector3 pos, rot;
	private GameObject earth;
	public bool earthView;
	
    // Start is called before the first frame update
    void Start()
    {
		earth = GameObject.Find("Earth");
		rot = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
		EarthBehavior earthObj = earth.GetComponent("EarthBehavior") as EarthBehavior;
		
        if (earthView) {
			pos = earthObj.GetPosition();
			pos.z -= .3f;
			transform.position = pos;
			transform.LookAt(earthObj.GetTransform());
		}
		else {
			transform.position = new Vector3(0,3000,0);
			transform.eulerAngles = rot;
		}
    }
	
	public Vector3 GetPosition() {
		return transform.position;
	}
}
