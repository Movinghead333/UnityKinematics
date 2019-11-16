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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // figure orientation joint
        Quaternion rot1 = Quaternion.Euler(0f, baseJointAngle, 0f);
        Matrix4x4 lowerBodyMatrix =
            Matrix4x4.Translate(position) *
            Matrix4x4.Rotate(rot1);

        lowerBody.position = getTranslation(lowerBodyMatrix);
        lowerBody.rotation = lowerBodyMatrix.rotation;


        // torso joint
        Quaternion rot2 = Quaternion.Euler(0f, 0f, torsoJointAngle);
        Matrix4x4 upperBodyMatrix =
            lowerBodyMatrix *
            Matrix4x4.Translate(upperBodyOffset) *
            Matrix4x4.Rotate(rot2);

        upperBody.position = getTranslation(upperBodyMatrix);
        upperBody.rotation = upperBodyMatrix.rotation;

        // right shoulder bend joint
        Quaternion rot3 = Quaternion.Euler(0f, rightArmSwing, upperRightArmJointAngle);
        Matrix4x4 upperRightArmMatrix =
            upperBodyMatrix *
            Matrix4x4.Translate(upperRightArmOffset) *
            Matrix4x4.Rotate(rot3);

        upperRightArm.position = getTranslation(upperRightArmMatrix);
        upperRightArm.rotation = upperRightArmMatrix.rotation;

        // right elbow bend joint
        Quaternion rot4 = Quaternion.Euler(0f, 0f, lowerRightArmJointAngle);
        Matrix4x4 lowerRightArmMatrix =
            upperRightArmMatrix *
            Matrix4x4.Translate(lowerArmOffset) *
            Matrix4x4.Rotate(rot4);

        lowerRightArm.position = getTranslation(lowerRightArmMatrix);
        lowerRightArm.rotation = lowerRightArmMatrix.rotation;

        // left shoulder bend joint
        Quaternion rot5 = Quaternion.Euler(0f, leftArmSwing, upperLeftArmJointAngle);
        Matrix4x4 upperLeftArmMatrix =
            upperBodyMatrix *
            Matrix4x4.Translate(upperLeftArmOffset) *
            Matrix4x4.Rotate(rot5);

        upperLeftArm.position = getTranslation(upperLeftArmMatrix);
        upperLeftArm.rotation = upperLeftArmMatrix.rotation;

        // left elbow bend joint
        Quaternion rot6 = Quaternion.Euler(0f, 0f, lowerLeftArmJointAngle);
        Matrix4x4 lowerLeftArmMatrix =
            upperLeftArmMatrix *
            Matrix4x4.Translate(lowerArmOffset) *
            Matrix4x4.Rotate(rot6);

        lowerLeftArm.position = getTranslation(lowerLeftArmMatrix);
        lowerLeftArm.rotation = lowerLeftArmMatrix.rotation;
    }

    Vector3 getTranslation(Matrix4x4 mat)
    {
        return new Vector3(mat.m03, mat.m13, mat.m23);
    }
}
