using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBehavior : MonoBehaviour
{
	private Vector3 iniScale;
	private float scaleFactor;
	private GameObject cam;
	private List<Vector3> sunScales = new List<Vector3>();
	
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
		iniScale = transform.localScale;
		
		foreach (Transform child in transform) {
			sunScales.Add(child.localScale);
		}
    }

    // Update is called once per frame
    void Update()
    {
		CamBehavior camObj = cam.GetComponent("CamBehavior") as CamBehavior;
		
		// Adjust scale based on view mode
		int i = 0;
		
		if (camObj.GetView() == 3) {
			scaleFactor = 20;
			transform.localScale = scaleFactor * iniScale;
			foreach (Transform child in transform) {
				child.localScale = scaleFactor * sunScales[i];
				i++;
			}
		}
		else if (camObj.GetView() == 0) {
			scaleFactor = 10;
			transform.localScale = scaleFactor * iniScale;
			foreach (Transform child in transform) {
				child.localScale = scaleFactor * sunScales[i];
				i++;
			}
		}
		else {
			transform.localScale = iniScale;
			foreach (Transform child in transform) {
				child.localScale = sunScales[i];
			}
		}
    }
	
	public Vector3 GetPosition() {
		return transform.position;
	}

	public Transform GetTransform() {
		return transform;
	}
}
