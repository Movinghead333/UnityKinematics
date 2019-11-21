using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    public Transform lowerBody;
    public Transform upperBody;
    public Transform upperRightArm;
    public Transform lowerRightArm;
    public Transform upperLeftArm;
    public Transform lowerLeftArm;

    public float baseJointAngle = 0f;
    public float torsoJointAngle = 0f;

    public float rightArmSwing = 0f;
    public float upperRightArmJointAngle = 0f;
    public float lowerRightArmJointAngle = 0f;

    public float leftArmSwing = 0f;
    public float upperLeftArmJointAngle = 0f;
    public float lowerLeftArmJointAngle = 0f;

    public Vector3 position = new Vector3(0f, 0f, 0f);

    Vector3 upperBodyOffset = new Vector3(0f, 1f, 0f);
    Vector3 upperRightArmOffset = new Vector3(0f, 0.5f, -0.2f);
    Vector3 upperLeftArmOffset = new Vector3(0f, 0.5f, 0.2f);
    Vector3 lowerArmOffset = new Vector3(0f, -0.3f, 0f);

    public Matrix4x4 lowerBodyMatrix;
    public Matrix4x4 upperBodyMatrix;
    public Matrix4x4 upperRightArmMatrix;
    public Matrix4x4 lowerRightArmMatrix;
    public Matrix4x4 upperLeftArmMatrix;
    public Matrix4x4 lowerLeftArmMatrix;

    HumanAngleConfig currentConfig;

    // Start is called before the first frame update
    void Start()
    {
        currentConfig = new HumanAngleConfig(
            Quaternion.Euler(0f, baseJointAngle, 0f),
            Quaternion.Euler(0f, 0f, torsoJointAngle),
            Quaternion.Euler(0f, rightArmSwing, upperRightArmJointAngle),
            Quaternion.Euler(0f, 0f, lowerRightArmJointAngle),
            Quaternion.Euler(0f, leftArmSwing, upperLeftArmJointAngle),
            Quaternion.Euler(0f, 0f, lowerLeftArmJointAngle));
        CalculateMatrices();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyMatrices();
    }

    public void CalculateMatrices()
    {
        // figure orientation joint
        //Quaternion rot1 = Quaternion.Euler(0f, baseJointAngle, 0f);
        lowerBodyMatrix =
            Matrix4x4.Translate(position) *
            Matrix4x4.Rotate(currentConfig.baseJointQuat);


        // torso joint
        //Quaternion rot2 = Quaternion.Euler(0f, 0f, torsoJointAngle);
        upperBodyMatrix =
            lowerBodyMatrix *
            Matrix4x4.Translate(upperBodyOffset) *
            Matrix4x4.Rotate(currentConfig.torsoJointQuat);


        // right shoulder bend joint
        //Quaternion rot3 = Quaternion.Euler(0f, rightArmSwing, upperRightArmJointAngle);
        upperRightArmMatrix =
            upperBodyMatrix *
            Matrix4x4.Translate(upperRightArmOffset) *
            Matrix4x4.Rotate(currentConfig.upperRightArmJointQuat);


        // right elbow bend joint
        //Quaternion rot4 = Quaternion.Euler(0f, 0f, lowerRightArmJointAngle);
        lowerRightArmMatrix =
            upperRightArmMatrix *
            Matrix4x4.Translate(lowerArmOffset) *
            Matrix4x4.Rotate(currentConfig.lowerRightArmJointQuat);


        // left shoulder bend joint
        //Quaternion rot5 = Quaternion.Euler(0f, leftArmSwing, upperLeftArmJointAngle);
        upperLeftArmMatrix =
            upperBodyMatrix *
            Matrix4x4.Translate(upperLeftArmOffset) *
            Matrix4x4.Rotate(currentConfig.upperLeftArmJointQuat);


        // left elbow bend joint
        //Quaternion rot6 = Quaternion.Euler(0f, 0f, lowerLeftArmJointAngle);
        lowerLeftArmMatrix =
            upperLeftArmMatrix *
            Matrix4x4.Translate(lowerArmOffset) *
            Matrix4x4.Rotate(currentConfig.lowerLeftArmJointQuat);
    }

    public void ApplyMatrices()
    {
        lowerBody.position = getTranslation(lowerBodyMatrix);
        lowerBody.rotation = lowerBodyMatrix.rotation;

        upperBody.position = getTranslation(upperBodyMatrix);
        upperBody.rotation = upperBodyMatrix.rotation;

        upperRightArm.position = getTranslation(upperRightArmMatrix);
        upperRightArm.rotation = upperRightArmMatrix.rotation;

        lowerRightArm.position = getTranslation(lowerRightArmMatrix);
        lowerRightArm.rotation = lowerRightArmMatrix.rotation;

        upperLeftArm.position = getTranslation(upperLeftArmMatrix);
        upperLeftArm.rotation = upperLeftArmMatrix.rotation;

        lowerLeftArm.position = getTranslation(lowerLeftArmMatrix);
        lowerLeftArm.rotation = lowerLeftArmMatrix.rotation;
    }

    public static Vector3 getTranslation(Matrix4x4 mat)
    {
        return new Vector3(mat.m03, mat.m13, mat.m23);
    }

    public void setConfig(HumanAngleConfig newConfig)
    {
        currentConfig = newConfig;
    }

    public HumanAngleConfig getCurrentConfig()
    {
        return new HumanAngleConfig(
            this.lowerBodyMatrix.rotation,
            this.upperBodyMatrix.rotation,
            this.upperRightArmMatrix.rotation,
            this.lowerRightArmMatrix.rotation,
            this.upperLeftArmMatrix.rotation,
            this.lowerLeftArmMatrix.rotation);
    }
}