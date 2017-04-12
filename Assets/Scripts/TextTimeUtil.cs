/*
    作者：ChenXingHua
*/
using UnityEngine;
using System.Collections;

public class TextTimeUtil{

	public static string TransTime(long time)
	{
		if (time > 0)
		{
			float ti = (float)time;
			int src = (int)(ti / 1000);
			int hr = 0;
			int min = 0;
			int sec = 0;
			if (src > 60)
			{
				min = src / 60;
				sec = src % 60;
			}
			else
			{
				sec = src;
			}


			if (min > 60)
			{
				hr = min / 60;
				min = min % 60;
			}

			return TimeToStr(hr) + ":" + TimeToStr(min) + ":" + TimeToStr(sec);

		}

		return "00:00:00";
	}

	static string TimeToStr(int time)
	{
		if (time > 9)
		{
			return time.ToString();
		}
		else
		{
			return "0" + time.ToString();
		}
	}


	public static long GetTime(long time){
		long realtime = 0;
		if (time > 0) {
			float ti = (float)time;
			int src = (int)(ti / 1000);
			int hr = 0;
			int min = 0;
			int sec = 0;
			if (src > 60) {
				min = src / 60;
				sec = src % 60;
			} else {
				sec = src;
			}


			if (min > 60) {
				hr = min / 60;
				min = min % 60;
			}

			return (hr * 3600 + min * 60 + sec) * 1000;
		} else {
			return 0;
		}
	}

	public static string CutString(string text,int len){
		if (len < text.Length) {
			string str = text.Substring (0, len) + "...";
			return str;
		} else {
			return text;
		}
	}
}
