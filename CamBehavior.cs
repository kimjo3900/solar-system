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
		if (earthView) {
			earth = GameObject.Find("Earth");
			transform.eulerAngles = new Vector3(0,0,0);
		}
    }

    // Update is called once per frame
    void Update()
    {
        if (earthView) {
			EarthBehavior earthObj = earth.GetComponent("EarthBehavior") as EarthBehavior;
			pos = earthObj.GetPosition();
			pos.z -= .2f;
			transform.position = pos;
		}
    }
	
	public Vector3 GetPosition() {
		return transform.position;
	}
}
