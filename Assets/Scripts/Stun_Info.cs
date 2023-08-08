using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun_Info : MonoBehaviour
{
	public string TAKD;
	private float[] TAKDInfo;

	void Start()
	{
		TAKDInfo = new float[4];

		string[] Info = TAKD.Split(" ");
        TAKDInfo[0] = float.Parse(Info[0]); //TimeDelay
        TAKDInfo[1] = float.Parse(Info[1]); //Angle
        TAKDInfo[2] = float.Parse(Info[2]); //Knockback
        TAKDInfo[3] = float.Parse(Info[3]); //Damage
	}

	public float[] GetTAKDInfo()
	{
		return TAKDInfo;
	}
	
}
