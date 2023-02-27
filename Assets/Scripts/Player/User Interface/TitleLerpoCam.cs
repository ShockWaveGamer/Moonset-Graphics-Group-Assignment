using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLerpoCam : MonoBehaviour
{
    public float baseRot; // rot = 0
    public float Q_Rot; // rot = 1
    public float C_Rot; // rot = 2
    public float S_Rot; // rot = 3
    public float P_Rot; // rot = 4

    public int _rot = 0;
    float targetRot;
    Vector3 _angle;
    float zangle;
    public float LerpSpeed = 1.0f;

    public void setRot(int rot)
    {
        _rot = rot;
    }
    // Start is called before the first frame update
    void Start()
    {
        _rot = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (_rot)
        {
            case 0:
                targetRot = baseRot;
                break;
            case 1:
                targetRot = Q_Rot;
                break;
            case 2:
                targetRot = C_Rot;
                break;
            case 3:
                targetRot = S_Rot;
                break;
            case 4:
                targetRot = P_Rot;
                break;
            default:
                targetRot = baseRot;
                break;
        }

        // lerp the x rotation of this gameobject to targetRot degrees with a speed of LerpSpeed
        //_angle = Mathf.Lerp(transform.rotation.x, targetRot, LerpSpeed * Time.fixedDeltaTime);
        //transform.eulerAngles = transform.eulerAngles + new Vector3(_angle, 113.5f, 0);

        zangle = Mathf.Lerp(zangle, targetRot, LerpSpeed * Time.deltaTime);
        //_angle = Vector3.Lerp(new Vector3(transform.eulerAngles.x,113.5f,0), new Vector3(targetRot,113.5f,0), LerpSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(zangle, 113.5f, 0);
        if (_angle.y != 113.5f)
        {
            _angle.y = 113.5f;
        }
        if (_angle.z != 0)
        {
            _angle.z = 0;
        }
        Debug.Log(_angle);
    }


    
}
