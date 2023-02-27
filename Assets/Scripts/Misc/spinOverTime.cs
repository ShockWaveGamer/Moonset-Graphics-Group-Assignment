using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinOverTime : MonoBehaviour
{

    public float spinSpeed = 1.0f;

    [SerializeField] direction SpinDirection = new direction();

    // Update is called once per frame
    void Update()
    {
        switch (SpinDirection)
        {
            case direction.xp:
                transform.Rotate(Vector3.right * spinSpeed * Time.deltaTime);
                break;
            case direction.yp:
                transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
                break;
            case direction.zp:
                transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
                break;
            case direction.xn:
                transform.Rotate(Vector3.left * spinSpeed * Time.deltaTime);
                break;
            case direction.yn:
                transform.Rotate(Vector3.down * spinSpeed * Time.deltaTime);
                break;
            case direction.zn:
                transform.Rotate(Vector3.back * spinSpeed * Time.deltaTime);
                break;
        }
    }

}
enum direction
{
    xp,
    yp,
    zp,
    xn,
    yn,
    zn
       
};
