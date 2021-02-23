using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedMultiDisplay : MonoBehaviour
{
	private Text txt;
	private GameObject slider;
	private float val;
	private List<string> times = new List<string>() {"1 sec", "7 sec", "1 min", "5 min", "30 min", "4 hours", "1 day", "1 week", "2 months", "1 year"};

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<Text>();
		slider = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
		val = slider.GetComponent<Slider>().value;
        txt.text = "1 sec = " + times[(int)val];
    }
}
