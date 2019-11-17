using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAngleConfig
{
    public float baseJointAngle = 0f;
    public float torsoJointAngle = 0f;

    public float rightArmSwing = 0f;
    public float upperRightArmJointAngle = 0f;
    public float lowerRightArmJointAngle = 0f;

    public float leftArmSwing = 0f;
    public float upperLeftArmJointAngle = 0f;
    public float lowerLeftArmJointAngle = 0f;

    public HumanAngleConfig(
        float baseJointAngle = 0f,
        float torsoJointAngle = 0f,

        float rightArmSwing = 0f,
        float upperRightArmJointAngle = 0f,
        float lowerRightArmJointAngle = 0f,

        float leftArmSwing = 0f,
        float upperLeftArmJointAngle = 0f,
        float lowerLeftArmJointAngle = 0f)
    {
        this.baseJointAngle = baseJointAngle;
        this.torsoJointAngle = torsoJointAngle;

        this.rightArmSwing = rightArmSwing;
        this.upperRightArmJointAngle = upperRightArmJointAngle;
        this.lowerRightArmJointAngle = lowerRightArmJointAngle;

        this.leftArmSwing = leftArmSwing;
        this.upperLeftArmJointAngle = upperLeftArmJointAngle;
        this.lowerLeftArmJointAngle = lowerLeftArmJointAngle;
    }

    public HumanAngleConfig(HumanAngleConfig newConfig)
    {
        this.baseJointAngle = newConfig.baseJointAngle;
        this.torsoJointAngle = newConfig.torsoJointAngle;

        this.rightArmSwing = newConfig.rightArmSwing;
        this.upperRightArmJointAngle = newConfig.upperRightArmJointAngle;
        this.lowerRightArmJointAngle = newConfig.lowerRightArmJointAngle;

        this.leftArmSwing = newConfig.leftArmSwing;
        this.upperLeftArmJointAngle = newConfig.upperLeftArmJointAngle;
        this.lowerLeftArmJointAngle = newConfig.lowerLeftArmJointAngle;
    }

    public static HumanAngleConfig Lerp(HumanAngleConfig start, HumanAngleConfig end, float gradient)
    {
        return new HumanAngleConfig(
            Mathf.Lerp(start.baseJointAngle, end.baseJointAngle, gradient),
            Mathf.Lerp(start.torsoJointAngle, end.torsoJointAngle, gradient),
            Mathf.Lerp(start.rightArmSwing, end.rightArmSwing, gradient),
            Mathf.Lerp(start.upperRightArmJointAngle, end.upperRightArmJointAngle, gradient),
            Mathf.Lerp(start.lowerRightArmJointAngle, end.lowerRightArmJointAngle, gradient),
            Mathf.Lerp(start.leftArmSwing, end.leftArmSwing, gradient),
            Mathf.Lerp(start.upperLeftArmJointAngle, end.upperLeftArmJointAngle, gradient),
            Mathf.Lerp(start.lowerLeftArmJointAngle, end.lowerLeftArmJointAngle, gradient)
            );
    }
}
