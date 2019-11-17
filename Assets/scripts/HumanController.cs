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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMatrices();
        ApplyMatrices();
    }

    public void CalculateMatrices()
    {
        // figure orientation joint
        Quaternion rot1 = Quaternion.Euler(0f, baseJointAngle, 0f);
        lowerBodyMatrix =
            Matrix4x4.Translate(position) *
            Matrix4x4.Rotate(rot1);


        // torso joint
        Quaternion rot2 = Quaternion.Euler(0f, 0f, torsoJointAngle);
        upperBodyMatrix =
            lowerBodyMatrix *
            Matrix4x4.Translate(upperBodyOffset) *
            Matrix4x4.Rotate(rot2);


        // right shoulder bend joint
        Quaternion rot3 = Quaternion.Euler(0f, rightArmSwing, upperRightArmJointAngle);
        upperRightArmMatrix =
            upperBodyMatrix *
            Matrix4x4.Translate(upperRightArmOffset) *
            Matrix4x4.Rotate(rot3);


        // right elbow bend joint
        Quaternion rot4 = Quaternion.Euler(0f, 0f, lowerRightArmJointAngle);
        lowerRightArmMatrix =
            upperRightArmMatrix *
            Matrix4x4.Translate(lowerArmOffset) *
            Matrix4x4.Rotate(rot4);


        // left shoulder bend joint
        Quaternion rot5 = Quaternion.Euler(0f, leftArmSwing, upperLeftArmJointAngle);
        upperLeftArmMatrix =
            upperBodyMatrix *
            Matrix4x4.Translate(upperLeftArmOffset) *
            Matrix4x4.Rotate(rot5);


        // left elbow bend joint
        Quaternion rot6 = Quaternion.Euler(0f, 0f, lowerLeftArmJointAngle);
        lowerLeftArmMatrix =
            upperLeftArmMatrix *
            Matrix4x4.Translate(lowerArmOffset) *
            Matrix4x4.Rotate(rot6);
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

    public void setAngles(HumanAngleConfig config)
    {
        this.baseJointAngle = config.baseJointAngle;
        this.torsoJointAngle = config.torsoJointAngle;

        this.rightArmSwing = config.rightArmSwing;
        this.upperRightArmJointAngle = config.upperRightArmJointAngle;
        this.lowerRightArmJointAngle = config.lowerRightArmJointAngle;

        this.leftArmSwing = config.leftArmSwing;
        this.upperLeftArmJointAngle = config.upperLeftArmJointAngle;
        this.lowerLeftArmJointAngle = config.lowerLeftArmJointAngle;

        CalculateMatrices();
        ApplyMatrices();
    }

    public HumanAngleConfig getCurrentConfig()
    {
        return new HumanAngleConfig(
            this.baseJointAngle,
            this.torsoJointAngle,

            this.rightArmSwing,
            this.upperRightArmJointAngle,
            this.lowerRightArmJointAngle,

            this.leftArmSwing,
            this.upperLeftArmJointAngle,
            this.lowerLeftArmJointAngle);
    }
}