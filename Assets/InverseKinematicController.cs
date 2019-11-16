using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematicController : MonoBehaviour
{
    public HumanController humanController;
    public Transform target;

    double armLength = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            calculateInverseKinematics();
        }
    }

    void calculateInverseKinematics()
    {
        Vector3 shoulderPos = humanController.upperRightArm.position;
        Vector3 targetPos = target.position;

        // calculate distance between shoulder point and target object
        double distance = Vector3.Distance(shoulderPos, targetPos);
        
        // if the distance is above the threshold distance quit the function
        if (distance > armLength)
        {
            Debug.Log("Distance: " + distance + " is too far from right shoulder!");
            //return;
        }

        Vector3 direction = targetPos - shoulderPos;

        double angle = getAngle(direction);

        humanController.baseJointAngle = (float)angle - 90f;



        Debug.Log("The angle is: " + getAngle(direction));
    }

    private double getAngle(Vector3 directionVector)
    {
        float angle = Mathf.Atan2(directionVector.x, directionVector.z);
        float degrees = (angle * 180f) / Mathf.PI;
        return (360 + Mathf.Round(degrees)) % 360;
    }
}
