using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun_Info : MonoBehaviour
{
	public string DAKT;
	private float[] DAKTInfo;

	void Start()
	{
		DAKTInfo = new float[4];

		string[] Info = DAKT.Split(" ");
        DAKTInfo[0] = float.Parse(Info[0]); //Damage
        DAKTInfo[1] = float.Parse(Info[1]); //Angle
        DAKTInfo[2] = float.Parse(Info[2]); //Knockback
        DAKTInfo[3] = float.Parse(Info[3]); //Time
	}

	public float[] GetDAKTInfo()
	{
		return DAKTInfo;
	}
	
}
