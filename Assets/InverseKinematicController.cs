using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematicController : MonoBehaviour
{
    public HumanController humanController;
    public Transform target;

    public float gamma = 0f;
    public float alpha = 0f;


    float armSegment = 0.3f;
    float armLength =  0.6f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            calculateInverseKinematics();
        }
    }

    void calculateInverseKinematics()
    {


        Vector3 shoulderPos = HumanController.getTranslation(humanController.upperRightArmMatrix);
        Vector3 targetPos = target.position;

        Vector3 direction = targetPos - shoulderPos;
        float baseAngle = getAngle(direction.z, direction.x);
        humanController.rightArmSwing = baseAngle - 90.0f;
        humanController.CalculateMatrices();


        // recalculated necessary values after changing first joint
        shoulderPos = HumanController.getTranslation(humanController.upperRightArmMatrix);
        direction = targetPos - shoulderPos;

        // calculate distance between shoulder point and target object
        float distance = Vector3.Distance(shoulderPos, targetPos);
        
        // if the distance is above the threshold distance quit the function
        if (distance > armLength)
        {
            Debug.Log("Distance: " + distance + " is too far from right shoulder!");
            return;
        }

        // recalculate direction after baseJoint might have changed
        Vector2 directionInXZPlane = new Vector2(direction.x, direction.z);



        // get gamma and alpha angle in isosceles triangle
        gamma = Mathf.Acos(1.0f - ( (distance * distance) / (2 * armSegment * armSegment) ));
        gamma = gamma * 180.0f / Mathf.PI;
        alpha = (180.0f - gamma) / 2.0f;

        Vector2 directionGrappingPlane = new Vector2(directionInXZPlane.magnitude, direction.y);
        Debug.Log("XZ-Y-Plane: " + directionGrappingPlane);
        float upperArmAngle = getAngle(directionGrappingPlane.x, directionGrappingPlane.y);
        Debug.Log("The atan2 upperArmAngle is: " + upperArmAngle);

        upperArmAngle = upperArmAngle + 90.0f - alpha;
        humanController.upperRightArmJointAngle = upperArmAngle;

        float lowerArmAngle = 180.0f - gamma;
        humanController.lowerRightArmJointAngle = lowerArmAngle;

        Debug.Log("Distance to object is: " + distance);
        Debug.Log("The baseAngle is: " + getAngle(direction.z, direction.x));
        Debug.Log("The upperArmAngle is: " + upperArmAngle);
        Debug.Log("The lowerArmAngle is; " + lowerArmAngle);
        humanController.ApplyMatrices();
    }

    private float getAngle(float x, float y)
    {
        float angle = Mathf.Atan2(y, x);
        float degrees = (angle * 180f) / Mathf.PI;
        return (360 + Mathf.Round(degrees)) % 360;

    }
}
