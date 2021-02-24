using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SpeedMultiDisplay : MonoBehaviour
{
	private Text txt;
	private GameObject date;
	private int speed;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<Text>();
		date = GameObject.Find("Date & Time");
    }

    // Update is called once per frame
    void Update()
    {
		DateTimeDisplay dateObj = date.GetComponent("DateTimeDisplay") as DateTimeDisplay;
		speed = (int)Math.Abs(dateObj.speed);
		
		txt.text = "1 sec = ";
		if (speed < 60) 
			txt.text += speed + " sec";
		else if (speed < 3600)
			txt.text += speed/60 + " min";
		else if (speed < 7200)
			txt.text += speed/3600 + " hour";
		else if (speed < 86400)
			txt.text += speed/3600 + " hours";
		else if (speed < 172800)
			txt.text += speed/86400 + " day";
		else if (speed < 604800)
			txt.text += speed/86400 + " days";
		else if (speed < 1209600)
			txt.text += speed/604800 + " week";
		else if (speed < 2629728)
			txt.text += speed/604800 + " weeks";
		else if (speed < 5259456)
			txt.text += speed/2629728 + " month";
		else if (speed < 31500000)
			txt.text += speed/2629728 + " months";
		else
			txt.text += speed/31500000 + " year";
    }
}
