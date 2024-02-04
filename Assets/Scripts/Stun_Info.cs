using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class that contains the basics of what a stun is. Enemy and environment stun uses ths as a superclass. Makes things more orerly for designers
public abstract class Stun_Info : MonoBehaviour
{
	protected float[] DAKTInfo;

	public abstract void CalculateStunVals();

	public float[] GetDAKTInfo()
	{
		return DAKTInfo;
	}
	
}
