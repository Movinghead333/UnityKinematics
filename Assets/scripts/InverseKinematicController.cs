using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematicController : MonoBehaviour
{
    public HumanController humanController;
    public Transform target;

    public float maxDistance = 0.8f;

    public float gamma = 0f;
    public float alpha = 0f;


    float armSegment = 0.3f;
    float armLength =  0.6f;

    HumanAngleConfig startConfig;
    HumanAngleConfig ikConfig;

    public float interpolationDuration = 1.0f;
    public static bool interpolating = false;
    public float progress = 0f;
    private float durationMultiplier;


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
                progress += Time.deltaTime * (1 / interpolationDuration);
                humanController.setConfig(HumanAngleConfig.Lerp(
                    startConfig, ikConfig, progress));
                humanController.CalculateMatrices();
                humanController.ApplyMatrices();
            }
            else
            {
                startConfig = new HumanAngleConfig(ikConfig);
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
        ikConfig.upperRightArmJointQuat = Quaternion.identity;
        ikConfig.lowerRightArmJointQuat = Quaternion.identity;
        ikConfig.upperBodyQuat = Quaternion.identity;
        return true;
    }

    bool calculateInverseKinematics()
    {
        Matrix4x4 lowerBodyMatrix;
        Matrix4x4 upperBodyMatrix;
        Matrix4x4 upperRightArmMatrix;
        Matrix4x4 lowerRightArmMatrix;
        Matrix4x4 upperLeftArmMatrix;
        Matrix4x4 lowerLeftArmMatrix;

        HumanAngleConfig IKConfig = new HumanAngleConfig(
            Quaternion.identity, Quaternion.identity, Quaternion.identity,
            Quaternion.identity, Quaternion.identity, Quaternion.identity);

        // get target position
        Vector3 targetPos = target.position;

        // calculate lowerbodyJoint
        Vector3 lowerBodyPosition = humanController.position;
        Vector3 heading = targetPos - lowerBodyPosition;

        float headingAngle = getAngle(heading.z, heading.x) - 90.0f;
        IKConfig.lowerBodyQuat = Quaternion.Euler(0f, headingAngle, 0f);

        lowerBodyMatrix = Matrix4x4.Translate(lowerBodyPosition) * Matrix4x4.Rotate(IKConfig.lowerBodyQuat);
        upperBodyMatrix = lowerBodyMatrix * humanController.upperBodyOffsetMatrix * Matrix4x4.Rotate(IKConfig.upperBodyQuat);
        upperRightArmMatrix = upperBodyMatrix * humanController.upperRightArmOffsetMatrix * Matrix4x4.Rotate(IKConfig.upperRightArmJointQuat);



        // get the distance between shoulder joint and target object after
        // the figure has rotated towards its target object
        float shoulderTargetDistance = (targetPos - HumanController.getTranslation(
            upperRightArmMatrix)).magnitude;

        Debug.Log("shoulder target distance: " + shoulderTargetDistance);

        // if the distance is above the threshold distance quit the function
        if (shoulderTargetDistance > maxDistance)
        {
            Debug.Log("Distance: " + shoulderTargetDistance + " is too far from right shoulder!");
            IKConfig.upperRightArmJointQuat = Quaternion.identity;
            IKConfig.lowerRightArmJointQuat = Quaternion.identity;
            IKConfig.upperBodyQuat = Quaternion.identity;
            ikConfig = IKConfig;
            return true;
        }

        float torsoBendAngle = 0f;
        if (shoulderTargetDistance > armLength && shoulderTargetDistance < maxDistance)
        {
            Debug.Log("Calculating torsobend");

            // calculate torsobend
            Vector3 hipPosition = HumanController.getTranslation(upperBodyMatrix);
            Vector3 hipTargetDistVec = targetPos - hipPosition;
            float hipTargetDist = hipTargetDistVec.magnitude; // c-side
            float torsoHeight = HumanController.torsoHeight; // b-side
            float projectedArmDistance = Mathf.Sqrt( // a-side
                (2 * HumanController.armSegmentLength) * (2 * HumanController.armSegmentLength) -
                HumanController.armOffset * HumanController.armOffset) - 0.05f;
            //Debug.Log("Projected distance: " + projectedArmDistance);

            // get alpha via cosin formula: gamma = arccos( (a^2+b^2-c^2) / 2ab)
            float alphaAngle = Mathf.Acos(
                (Mathf.Pow(torsoHeight, 2f) + Mathf.Pow(hipTargetDist, 2f) - Mathf.Pow(projectedArmDistance, 2f)) /
                (2f * torsoHeight * hipTargetDist)) * Mathf.Rad2Deg;
            Debug.Log("AlphaAngle: " + alphaAngle);

            Vector3 normalizedHipTargetDistVec = Vector3.Normalize(hipTargetDistVec);
            float betaAngle = Mathf.Acos(Vector3.Dot(Vector3.up, normalizedHipTargetDistVec)) * Mathf.Rad2Deg;

            torsoBendAngle = betaAngle - alphaAngle;
            Debug.Log("torsoangle: " + torsoBendAngle);
            IKConfig.upperBodyQuat = Quaternion.Euler(0f, 0f, -torsoBendAngle);
            upperBodyMatrix = lowerBodyMatrix * humanController.upperBodyOffsetMatrix * Matrix4x4.Rotate(IKConfig.upperBodyQuat);
            upperRightArmMatrix = upperBodyMatrix * humanController.upperRightArmOffsetMatrix * Matrix4x4.Rotate(IKConfig.upperRightArmJointQuat);
        }

        // calculate shoulder swing joint
        Vector3 shoulderPos = HumanController.getTranslation(upperRightArmMatrix);
        Vector3 direction = targetPos - shoulderPos;

        float rightArmSwing = getAngle(direction.z, direction.x) - headingAngle - 90.0f;
        ikConfig.upperRightArmJointQuat = IKConfig.upperRightArmJointQuat = Quaternion.Euler(0f, rightArmSwing, 0f);
        upperRightArmMatrix = upperBodyMatrix * humanController.upperRightArmOffsetMatrix * Matrix4x4.Rotate(IKConfig.upperRightArmJointQuat);


        // recalculated necessary values after changing first joint
        shoulderPos = HumanController.getTranslation(upperRightArmMatrix);
        direction = targetPos - shoulderPos;

        // calculate distance between shoulder point and target object
        float distance = Vector3.Distance(shoulderPos, targetPos);
        
        // recalculate direction after baseJoint might have changed
        Vector2 directionInXZPlane = new Vector2(direction.x, direction.z);


        // get gamma and alpha angle in isosceles triangle
        gamma = Mathf.Acos(1.0f - ( (distance * distance) / (2 * armSegment * armSegment) ));
        gamma = gamma * 180.0f / Mathf.PI;
        alpha = (180.0f - gamma) / 2.0f;

        Vector2 directionGrappingPlane = new Vector2(directionInXZPlane.magnitude, direction.y);
        //Debug.Log("XZ-Y-Plane: " + directionGrappingPlane);
        float upperArmAngle = getAngle(directionGrappingPlane.x, directionGrappingPlane.y);
        //Debug.Log("The atan2 upperArmAngle is: " + upperArmAngle);

        upperArmAngle = upperArmAngle + 90.0f - alpha;

        float lowerArmAngle = 180.0f - gamma;

        //Debug.Log("Distance to object is: " + distance);
        //Debug.Log("The baseAngle is: " + getAngle(direction.z, direction.x));
        //Debug.Log("The upperArmAngle is: " + upperArmAngle);
        //Debug.Log("The lowerArmAngle is; " + lowerArmAngle);

        IKConfig.upperRightArmJointQuat = Quaternion.Euler(0f, 0f, torsoBendAngle) * Quaternion.Euler(0f, rightArmSwing, upperArmAngle);
        IKConfig.lowerRightArmJointQuat = Quaternion.Euler(0f, 0f, lowerArmAngle);

        ikConfig = IKConfig;

        return true;
    }

    private float getAngle(float x, float y)
    {
        float angle = Mathf.Atan2(y, x);
        float degrees = (angle * 180f) / Mathf.PI;
        return (360 + Mathf.Round(degrees)) % 360;
    }
}
