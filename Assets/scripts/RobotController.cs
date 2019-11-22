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

    RobotConfig start;
    RobotConfig end;

    bool forwardAnim = true;
    float progress = 0f;

    float singleAnimationDuration = 3.0f;

    struct RobotConfig{
        public Quaternion firstQuat;
        public Quaternion secondQuat;
        public Quaternion thirdQuat;
        public Quaternion fourthQuat;

        public RobotConfig(Quaternion q1, Quaternion q2, Quaternion q3, Quaternion q4)
        {
            firstQuat = q1;
            secondQuat = q2;
            thirdQuat = q3;
            fourthQuat = q4;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        start = new RobotConfig(
            Quaternion.Euler(0f, 0f, 0f),
            Quaternion.Euler(0f, 0f, 90f),
            Quaternion.Euler(0f, 0f, 0f),
            Quaternion.Euler(0f, 0f, 90f));

        end = new RobotConfig(
            Quaternion.Euler(0f, 180, 0f),
            Quaternion.Euler(0f, 0f, 45f),
            Quaternion.Euler(0f, 270f, 0f),
            Quaternion.Euler(0f, 0f, -45));
    }

    // Update is called once per frame
    void Update()
    {
        //var step = speed * Time.deltaTime;
        //firstJointAngle -= step;
        //thirdJointAngle += step;

        //firstJoint.rotation = Quaternion.Euler(0f, firstJointAngle, 0f);
        //secondJoint.rotation = firstJoint.rotation * Quaternion.Euler(0f, 0f, secondJointAngle);
        //thirdJoint.rotation = secondJoint.rotation * Quaternion.Euler(0f, thirdJointAngle, 0f);
        //fourthJoint.rotation = thirdJoint.rotation * Quaternion.Euler(0f, 0f, fourthJointAngle);

        float step = Time.deltaTime * (1 / singleAnimationDuration);
        if (forwardAnim)
        {
            if (progress <= 1.0f)
            {
                progress += step;
                firstJoint.rotation = Quaternion.Slerp(start.firstQuat, end.firstQuat, progress);
                secondJoint.rotation = firstJoint.rotation * Quaternion.Slerp(start.secondQuat, end.secondQuat, progress);
                thirdJoint.rotation = secondJoint.rotation * Quaternion.Slerp(start.thirdQuat, end.thirdQuat, progress);
                fourthJoint.rotation = thirdJoint.rotation * Quaternion.Slerp(start.fourthQuat, end.fourthQuat, progress);
            }
            else
            {
                firstJoint.rotation = end.firstQuat;
                secondJoint.rotation = firstJoint.rotation * end.secondQuat;
                thirdJoint.rotation = secondJoint.rotation * end.thirdQuat;
                fourthJoint.rotation = thirdJoint.rotation * end.fourthQuat;
                forwardAnim = false;
                progress = 0f;
            }
        }
        else
        {
            if (progress <= 1.0f)
            {
                progress += step;
                firstJoint.rotation = Quaternion.Slerp(end.firstQuat, start.firstQuat, progress);
                secondJoint.rotation = firstJoint.rotation * Quaternion.Slerp(end.secondQuat, start.secondQuat, progress);
                thirdJoint.rotation = secondJoint.rotation * Quaternion.Slerp(end.thirdQuat, start.thirdQuat, progress);
                fourthJoint.rotation = thirdJoint.rotation * Quaternion.Slerp(end.fourthQuat, start.fourthQuat, progress);
            }
            else
            {
                firstJoint.rotation = start.firstQuat;
                secondJoint.rotation = firstJoint.rotation * start.secondQuat;
                thirdJoint.rotation = secondJoint.rotation * start.thirdQuat;
                fourthJoint.rotation = thirdJoint.rotation * start.fourthQuat;
                forwardAnim = true;
                progress = 0f;
            }
        }
    }
}
