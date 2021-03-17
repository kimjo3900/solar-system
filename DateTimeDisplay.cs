using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Threading;
using TMPro;

public class DateTimeDisplay : MonoBehaviour
{
	public double speed = 1;
	private Text txt;
	private double s, t;
	private bool isPaused = false;
	private int year, day, hour, min;
	private string month, timeDisplay;
	private List<string> months = new List<string>() {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
	private List<int> maxDays = new List<int>() {31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365};
	private List<int> maxDaysLeapYear = new List<int>() {31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366};
	private DateTime dtStart;
	private GameObject earth, moon, pauseButton, monthDrop, dayDrop, yearDrop, hourDrop, minDrop, dateSel;
	
    // Start is called before the first frame update
    void Start()
    {
		earth = GameObject.Find("Earth");
		moon = GameObject.Find("Moon");
		pauseButton = GameObject.Find("Play/Pause Button");
		monthDrop = GameObject.Find("Month");
		dayDrop = GameObject.Find("Day");
		yearDrop = GameObject.Find("Year");
		hourDrop = GameObject.Find("Hour");
		minDrop = GameObject.Find("Min");
		txt = GetComponent<Text>();
		dateSel = GameObject.Find("Date Selection");
		dateSel.SetActive(false);
		
		// Initialize s - the number of seconds since Jan 1 00:00 of the current year
		SetS(DateTime.UtcNow);
    }

    // FixedUpdate() is called once per frame - is not called when Time.timeScale = 0
    void FixedUpdate()
    {
		// day initially represents day # of the current year
		day = (int)(s / 86400) + 1;
		
		// Determine the month based on day and then determine the day # of the current month
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
		
		// Determine the time based on s
		t = s % 86400;
		hour = (int)(t / 3600);
		t = t % 3600;
		min = (int)(t / 60);
		
		// Increment the year if necessary and reset s back to 0
		if (s >= 31536000 && !IsLeapYear(year)) {
			s = s % 31536000;
			year++;
		}
		else if (s >= 31622400 && IsLeapYear(year)) {
			s = s % 31622400;
			year++;
		}

		// Decrement the year in the case when Reverse is checked
		else if (s < 0 && !IsLeapYear(year-1)) {
			s = 31536000 + s % 31536000;
			year--;
		}

		else if (s < 0 && IsLeapYear(year-1)) {
			s = 31622400 + s % 31622400;
			year--;
		}
		
		txt.text = month + " " + day.ToString() + ", " + year.ToString() + "\n" + FormatTime(hour) + ":" + FormatTime(min);
		
		s+= speed*Time.deltaTime;
    }

	// Determine if it's a leap year based on year number
	public bool IsLeapYear(int year) {
		if (year % 4 != 0 || (year % 100 == 0 && year % 400 != 0))
			return false;
		else
			return true;
	}
	
	// Format the time to display as HH:MM
	public string FormatTime(int value) {
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
		
		PauseButton button = pauseButton.GetComponent("PauseButton") as PauseButton;
		button.ChangeImage();
	}

	// Reverse the speed
	public void Reverse() {
		speed = -speed;
	}
	
	// Open the Date/Time selection box and pause the scene if it's not already paused
	public void OpenDateSelection() {
		if (!isPaused) 
			Pause();
			
		dateSel.SetActive(true);
		
		// Assign the current date/time to the corresponding dropdown entries
		for (int i=0; i<months.Count; i++) {
			if (month.Equals(months[i])) {
				monthDrop.GetComponent<TMP_Dropdown>().value = i;
				break;
			}
		}
		
		dayDrop.GetComponent<TMP_Dropdown>().value = day-1;
		yearDrop.GetComponent<TMP_Dropdown>().value = year-2000;
		hourDrop.GetComponent<TMP_Dropdown>().value = hour;
		minDrop.GetComponent<TMP_Dropdown>().value = min;
	}
	
	// Close the Date/Time selection box and set the Date/Time
	public void CloseDateSelection() {
		dateSel.SetActive(false);
		
		// Assign the selected dropdown values to a new DateTime
		int newMonth = monthDrop.GetComponent<TMP_Dropdown>().value+1;
		int newDay = dayDrop.GetComponent<TMP_Dropdown>().value+1;
		int newYear = yearDrop.GetComponent<TMP_Dropdown>().value+2000;
		int newHour = hourDrop.GetComponent<TMP_Dropdown>().value;
		int newMin = minDrop.GetComponent<TMP_Dropdown>().value;
		
		DateTime newDate = new DateTime(newYear, newMonth, newDay, newHour, newMin, 0);
		
		// Update s
		SetS(newDate);
		
		// Update t in EarthBehavior
		EarthBehavior earthObj = earth.GetComponent("EarthBehavior") as EarthBehavior;
		earthObj.SetT(newDate);
		
		// Update Earth's and Moon's rotation
		earthObj.SetRot(earthObj.GetDT());
		MoonBehavior moonObj = moon.GetComponent("MoonBehavior") as MoonBehavior;
		moonObj.SetRot(earthObj.GetDT());
		
		// Call FixedUpdate() to update the date and time
		FixedUpdate();
	}
	
	// Set s based on the given DateTime
	public void SetS(DateTime dt) {
		year = dt.Year;
		dtStart = new DateTime(year, 1, 1);
		s = dt.Subtract(dtStart).TotalSeconds;
	}
}
