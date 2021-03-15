using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DateTimeDisplay : MonoBehaviour
{
	public double speed = 1;
	private Text txt;
	private double s, t;
	private bool isPaused = false;
	private int year;
	private int day, hour, min;
	private string month, timeDisplay;
	private List<string> months = new List<string>() {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
	private List<int> maxDays = new List<int>() {31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365};
	private List<int> maxDaysLeapYear = new List<int>() {31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366};
	private DateTime dtNow, dtStart;
	private TimeSpan tSinceStart;
	
    // Start is called before the first frame update
    void Start()
    {
		txt = GetComponent<Text>();
		
		// Compute s - the number of seconds since Jan 1 midnight of the current year
		dtNow = DateTime.UtcNow;
		year = dtNow.Year;
		dtStart = new DateTime(year, 1, 1);
		tSinceStart = dtNow.Subtract(dtStart);

		s = tSinceStart.TotalSeconds;
    }

    // Update is called once per frame
    void Update()
    {
		//day initially represents day # of the current year
		day = (int)(s / 86400) + 1;
		
		//determine the month based on day and then determine the day # of the current month
		for (int i=0; i<months.Count; i++) {
			if (!IsLeapYear(year) && day <= maxDays[i]) {
				month = months[i];
				if (i != 0)
					day -= maxDays[i-1];
				break;
			}
			else if (IsLeapYear(year) && day <= maxDaysLeapYear[i]) {
				month = months[i];
				if (i != 0)
					day -= maxDaysLeapYear[i-1];
				break;
			}
		}
		
		//determine the time based on s
		t = s % 86400;
		hour = (int)(t / 3600);
		t = t % 3600;
		min = (int)(t / 60);
		
		//increment the year if necessary and reset s back to 0
		if (s >= 31536000 && !IsLeapYear(year)) {
			s = s % 31536000;
			year++;
		}
		else if (s >= 31622400 && IsLeapYear(year)) {
			s = s % 31622400;
			year++;
		}

		//decrement the year in the case when Reverse is checked
		else if (s < 0 && !IsLeapYear(year-1)) {
			s = 31536000 + s % 31536000;
			year--;
		}

		else if (s < 0 && IsLeapYear(year-1)) {
			s = 31622400 + s % 31622400;
			year--;
		}
		
		txt.text = FormatTime(hour) + ":" + FormatTime(min) + "\n" + month + " " + day.ToString() + ", " + year.ToString();
		
		s+= speed*Time.deltaTime;

    }

	//determine if it's a leap year based on year number
	private bool IsLeapYear(int year) {
		if (year % 4 != 0 || (year % 100 == 0 && year % 400 != 0))
			return false;
		else
			return true;
	}
	
	// format the time to display as hh:mm
	private string FormatTime(int value) {
		if (value >= 10)
			return value.ToString();
		else
			return "0" + value.ToString();
	}

	// Set speed using a logarithmic scale
	public void SetSpeed(float speed) {
		if (this.speed > 0)
			this.speed = Math.Pow(6.811342874, speed);
		else
			this.speed = -Math.Pow(6.811342874, speed);
	}

	// Play/Pause the scene
	public void Pause() {
		if (!isPaused) {
			Time.timeScale = 0;
			isPaused = true;
		}
		else {
			Time.timeScale = 1;
			isPaused = false;
		}
	}

	// Reverse the speed
	public void Reverse() {
		speed = -speed;
	}
}
