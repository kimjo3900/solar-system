using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateTimeDisplay : MonoBehaviour
{
	public double speed = 1;
	private Text txt;
	private double s;
	private bool isLeapYear;
	private int year = 2020;
	private int days;
	private string month;
	private List<string> months = new List<string>() {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
	private List<int> maxDays = new List<int>() {31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365};
	private List<int> maxDaysLeapYear = new List<int>() {31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366};
	
    // Start is called before the first frame update
    void Start()
    {
		txt = GetComponent<Text>();
		
		//s represents number of seconds since Jan 1 of the current year
		s = 355620;
    }

    // Update is called once per frame
    void Update()
    {
		
		//determine if it's a leap year based on year number
		if (year % 4 != 0 || (year % 100 == 0 && year % 400 != 0))
			isLeapYear = false;
		else
			isLeapYear = true;
		
		//days initially represents day # of the current year
		days = (int)(s / 86400) + 1;
		
		//determine the month based on days and then determine the day # of the current month
		for (int i=0; i<months.Count; i++) {
			if (!isLeapYear && days <= maxDays[i]) {
				month = months[i];
				if (i != 0)
					days -= maxDays[i-1];
				break;
			}
			else if (isLeapYear && days <= maxDaysLeapYear[i]) {
				month = months[i];
				if (i != 0)
					days -= maxDaysLeapYear[i-1];
				break;
			}
		}
		
		//determine the time based on s. s % 86400 gives # seconds since midnight.
		
		//increment the year if necessary and reset s back to 0
		if (s >= 31622400 && isLeapYear) {
			s = s % 31622400;
			year++;
		}
		if (s >= 31536000 && !isLeapYear) {
			s = s % 31536000;
			year++;
		}
		
		
		
		txt.text = month + " " + days.ToString() + ", " + year.ToString();
		
		s+= speed*Time.deltaTime;
    }
}
