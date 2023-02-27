using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmullGrapple : MonoBehaviour
{
	public Transform[] points;
	public float speed = 1f;
	public int delayAmount = 0;
	protected float Timer;
	[Range(1, 32)]
	public int sampleRate = 16;

	[System.Serializable]
	class SamplePoint
	{
		public float samplePosition;
		public float accumulatedDistance;

		public SamplePoint(float samplePosition, float distanceCovered)
		{
			this.samplePosition = samplePosition;
			this.accumulatedDistance = distanceCovered;
		}
	}
	//list of segment samples makes it easier to index later
	//imagine it like List<SegmentSamples>, and segment sample is a list of SamplePoints
	List<List<SamplePoint>> table = new List<List<SamplePoint>>();

	float distance = 0f;
	float accumDistance = 0f;
	int currentIndex = 0;
	int currentSample = 0;

	private void Start()
	{
		//make sure there are 4 points, else disable the component
		if (points.Length < 4)
		{
			enabled = false;
		}

		int size = points.Length;

		//calculate the speed graph table
		Vector3 prevPos = points[0].position;
		for (int i = 0; i < size; ++i)
		{
			List<SamplePoint> segment = new List<SamplePoint>();

			Vector3 p0 = points[(i + points.Length - 1) % points.Length].position;
			Vector3 p1 = points[i].position;
			Vector3 p2 = points[(i + 1) % points.Length].position;
			Vector3 p3 = points[(i + 2) % points.Length].position;

			//calculate samples
			segment.Add(new SamplePoint(0f, accumDistance));
			Vector3 previousPos = CalcCatVec3(p0, p1, p2, p3, 0);
			for (int sample = 1; sample <= sampleRate; ++sample)
			{
				float t = (float)sample / sampleRate;
				Vector3 currentPos = CalcCatVec3(p0, p1, p2, p3, t);
				accumDistance += (previousPos - currentPos).magnitude;
				segment.Add(new SamplePoint(t, accumDistance));
				previousPos = currentPos;
			}
			table.Add(segment);
		}
	}

	private void Update()
	{
		distance += speed * Time.deltaTime;
		Timer += Time.deltaTime;

		if (Timer >= delayAmount)
		{
			Timer = 0f;
			speed = speed + 0.0001f;
		}

		//check if we need to update our samples
		if (distance > table[currentIndex][currentSample + 1].accumulatedDistance)
		{
			if (currentSample + 1 >= table[currentIndex].Count - 1)
			{
				if (currentIndex < table.Count - 1)
				{
					currentIndex++;
				}
				else
				{
					currentIndex = 0;
					distance = 0;
				}
				currentSample = 0;
			}
			else
			{
				currentSample++;
			}

		}

		Vector3 p0 = points[(currentIndex - 1 + points.Length) % points.Length].position;
		Vector3 p1 = points[currentIndex].position;
		Vector3 p2 = points[(currentIndex + 1) % points.Length].position;
		Vector3 p3 = points[(currentIndex + 2) % points.Length].position;

		transform.position = CalcCatVec3(p0, p1, p2, p3, GetAdjustedT());


	}

	float GetAdjustedT()
	{
		SamplePoint current = table[currentIndex][currentSample];
		SamplePoint next = table[currentIndex][currentSample + 1];

		return Mathf.Lerp(current.samplePosition, next.samplePosition,
			(distance - current.accumulatedDistance) / (next.accumulatedDistance - current.accumulatedDistance)
		);
	}


	private void OnDrawGizmos()
	{
		Vector3 a, b, p0, p1, p2, p3;
		for (int i = 0; i < points.Length; i++)
		{
			a = points[i].position;
			p0 = points[(points.Length + i - 1) % points.Length].position;
			p1 = points[i].position;
			p2 = points[(i + 1) % points.Length].position;
			p3 = points[(i + 2) % points.Length].position;
			for (int j = 1; j <= sampleRate; ++j)
			{
				b = CalcCatVec3(p0, p1, p2, p3, (float)j / sampleRate);
				Gizmos.DrawLine(a, b);
				a = b;
			}
		}
	}

	private float CalcCatFloat(float p0, float p1, float p2, float p3, float t)
	{
		return 0.5f * (2.0f * p1 + t * (-p0 + p2) + t * t * (2.0f * p0 - 5.0f * p1 + 4.0f * p2 - p3) + t * t * t * (-p0 + 3.0f * p1 - 3.0f * p2 + p3));
	}

	private Vector3 CalcCatVec3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		float x = CalcCatFloat(p0.x, p1.x, p2.x, p3.x, t);
		float y = CalcCatFloat(p0.y, p1.y, p2.y, p3.y, t);
		float z = CalcCatFloat(p0.z, p1.z, p2.z, p3.z, t);
		return new Vector3(x, y, z);
	}

}
