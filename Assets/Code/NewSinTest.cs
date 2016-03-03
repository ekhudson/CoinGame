using UnityEngine;
using System.Collections;

public class NewSinTest : MonoBehaviour 
{
	public Vector3 Origin = Vector3.zero;
	public float Amplitude = 10f;
	public float Frequency = 120f;
	public float Speed = 100f;
	public float MaxDegrees = 180;
	public float Radius = 10f;
	public int LineSegments = 10;
	public float LineLength = 100f;

	private float mPreviousFrequency = 0f;
	private float mCurrentPhase = 0f;

	public float PreviousFrequency
	{
		get
		{
			return mPreviousFrequency;
		}
		set
		{
			mPreviousFrequency = value;
		}
	}

	public float CurrentPhase
	{
		get
		{
			return mCurrentPhase;
		}
		set
		{
			mCurrentPhase = value;
		}
	}

	public void OnDrawGizmos()
	{

	}

}
