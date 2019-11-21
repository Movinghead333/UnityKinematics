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

    HumanAngleConfig startConfig;
    HumanAngleConfig ikConfig;

    public bool interpolating = false;
    public float progress = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (interpolating)
        {
            if (progress < 1.0f)
            {
                progress += Time.deltaTime;
                HumanAngleConfig interpolatedConfig = HumanAngleConfig.Lerp(
                    startConfig, ikConfig, progress);
                humanController.setConfig(interpolatedConfig);
            }
            else
            {
                progress = 0.0f;
                interpolating = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                interpolating = calculateInverseKinematics();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                interpolating = ResetRightArm();
            }
        }
    }

    bool ResetRightArm()
    {
        startConfig = humanController.getCurrentConfig();
        ikConfig = new HumanAngleConfig(startConfig);
        ikConfig.upperRightArmJointQuat = Quaternion.Euler(0f, 0f, 0f);
        ikConfig.lowerRightArmJointQuat = Quaternion.Euler(0f, 0f, 0f);
        return true;
    }

    bool calculateInverseKinematics()
    {
        startConfig = humanController.getCurrentConfig();

        // get target position
        Vector3 targetPos = target.position;

        // calculate lowerbodyJoint
        Vector3 lowerBodyPosition = HumanController.getTranslation(humanController.lowerBodyMatrix);
        Vector3 heading = targetPos - lowerBodyPosition;

        float headingAngle = getAngle(heading.z, heading.x) - 90.0f;
        humanController.baseJointAngle = headingAngle;
        humanController.CalculateMatrices();

        // calculate shoulder swing joint
        Vector3 shoulderPos = HumanController.getTranslation(humanController.upperRightArmMatrix);
        Vector3 direction = targetPos - shoulderPos;

        float rightArmSwing = getAngle(direction.z, direction.x) - humanController.baseJointAngle - 90.0f;
        humanController.rightArmSwing = rightArmSwing;
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
            humanController.upperRightArmJointAngle = 0f;
            humanController.lowerRightArmJointAngle = 0f;
            return false;
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

        ikConfig = new HumanAngleConfig(
            humanController.lowerBodyMatrix.rotation,
            humanController.upperBodyMatrix.rotation,
            humanController.upperRightArmMatrix.rotation,
            humanController.lowerRightArmMatrix.rotation,
            humanController.upperLeftArmMatrix.rotation,
            humanController.lowerLeftArmMatrix.rotation);

        //public Matrix4x4 lowerBodyMatrix;
        //public Matrix4x4 upperBodyMatrix;
        //public Matrix4x4 upperRightArmMatrix;
        //public Matrix4x4 lowerRightArmMatrix;
        //public Matrix4x4 upperLeftArmMatrix;
        //public Matrix4x4 lowerLeftArmMatrix;

        return true;
    }

    private float getAngle(float x, float y)
    {
        float angle = Mathf.Atan2(y, x);
        float degrees = (angle * 180f) / Mathf.PI;
        return (360 + Mathf.Round(degrees)) % 360;
    }
}
