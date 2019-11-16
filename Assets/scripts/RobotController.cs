using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public float speed = 45f;
    public Transform firstJoint;
    public Transform secondJoint;
    public Transform thirdJoint;
    public Transform fourthJoint;

    Quaternion firstJointTarget;
    Quaternion secondJointTarget;
    Quaternion FourthJointTarget;

    public float firstJointAngle = 0f;
    public float secondJointAngle = 0f;
    public float thirdJointAngle = 0f;
    public float fourthJointAngle = 0f;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var step = speed * Time.deltaTime;
        firstJointAngle -= step;
        thirdJointAngle += step;

        firstJoint.rotation = Quaternion.Euler(0f, firstJointAngle, 0f);
        secondJoint.rotation = firstJoint.rotation * Quaternion.Euler(0f, 0f, secondJointAngle);
        thirdJoint.rotation = secondJoint.rotation * Quaternion.Euler(0f, thirdJointAngle, 0f);
        fourthJoint.rotation = thirdJoint.rotation * Quaternion.Euler(0f, 0f, fourthJointAngle);

    }
}
