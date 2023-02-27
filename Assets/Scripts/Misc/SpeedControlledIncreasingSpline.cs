using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//WRITTEN BY: Mason Desjarlais

public class SpeedControlledIncreasingSpline : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    protected Transform[] points;

    [SerializeField]
    [Range(1, 32)]
    protected int sampleRate = 16;

    [SerializeField]
    protected float speed;

    #endregion

    #region Variables

    Vector3 previousPos;

    #endregion

    //NOTE: WHEN TOGGLING SPEED MOD, HIGHLIGHT BOTH CAR AND LOOKPOINT IN INSPECTOR
    //      OTHERWISE IT WILL SPIN LIKE CRAZY

    public bool speedModActive = true;
    public float speedModifier = 0.01f;

    public int delayAmount = 0;
    protected float Timer;

    [System.Serializable]
    protected class SamplePoint
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

    protected float distance = 0f;
    protected float accumDistance = 0f;
    protected int currentIndex = 0;
    protected int currentSample = 0;

    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        transform.position = points[0].position;
        InitiateCatmulRomSpline();
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, FollowSpline(), ref velocity, 1);
    }

    protected void InitiateCatmulRomSpline()
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
            Vector3 previousPos = CatmulRom.Vector3(p0, p1, p2, p3, 0);
            for (int sample = 1; sample <= sampleRate; ++sample)
            {
                float t = (float)sample / sampleRate;
                Vector3 currentPos = CatmulRom.Vector3(p0, p1, p2, p3, t);
                accumDistance += (previousPos - currentPos).magnitude;
                segment.Add(new SamplePoint(t, accumDistance));
                previousPos = currentPos;
            }
            table.Add(segment);
        }
    }

    protected Vector3 FollowSpline()
    {
        distance += speed * Time.deltaTime;
        Timer += Time.deltaTime;

        if (speedModActive == true && Timer >= delayAmount)
        {
            Timer = 0f;
            speed = speed + speedModifier;
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

        return CatmulRom.Vector3(p0, p1, p2, p3, GetAdjustedT());
    }

    protected float GetAdjustedT()
    {
        SamplePoint current = table[currentIndex][currentSample];
        SamplePoint next = table[currentIndex][currentSample + 1];

        return Mathf.Lerp(current.samplePosition, next.samplePosition,
            (distance - current.accumulatedDistance) / (next.accumulatedDistance - current.accumulatedDistance)
        );
    }

    /*protected void LookForward()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - previousPos);
        previousPos = transform.position;
    }*/

    public float Speed
    {
        get { return speed; }
    }
}
