using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HumanAngleConfig
{
    public Quaternion baseJointQuat;
    public Quaternion torsoJointQuat;

    public Quaternion upperRightArmJointQuat;
    public Quaternion lowerRightArmJointQuat;

    public Quaternion upperLeftArmJointQuat;
    public Quaternion lowerLeftArmJointQuat;

    public HumanAngleConfig(
        Quaternion baseJointQuat,
        Quaternion torsoJointQuat,

        Quaternion upperRightArmJointQuat,
        Quaternion lowerRightArmJointQuat,

        Quaternion upperLeftArmJointQuat,
        Quaternion lowerLeftArmJointQuat)
    {
        this.baseJointQuat          = baseJointQuat;
        this.torsoJointQuat         = torsoJointQuat;

        this.upperRightArmJointQuat = upperRightArmJointQuat;
        this.lowerRightArmJointQuat = lowerRightArmJointQuat;

        this.upperLeftArmJointQuat  = upperLeftArmJointQuat;
        this.lowerLeftArmJointQuat  = lowerLeftArmJointQuat;
    }

    public HumanAngleConfig(HumanAngleConfig newConfig)
    {
        this.baseJointQuat          = newConfig.baseJointQuat;
        this.torsoJointQuat         = newConfig.torsoJointQuat;

        this.upperRightArmJointQuat = newConfig.upperRightArmJointQuat;
        this.lowerRightArmJointQuat = newConfig.lowerRightArmJointQuat;

        this.upperLeftArmJointQuat  = newConfig.upperLeftArmJointQuat;
        this.lowerLeftArmJointQuat  = newConfig.lowerLeftArmJointQuat;
    }

    public static HumanAngleConfig Lerp(HumanAngleConfig start, HumanAngleConfig end, float gradient)
    {
        //Debug.Log("Start angle: " + start.baseJointQuat + " Endconfig angle: " + end.baseJointQuat);
        return new HumanAngleConfig(
            Quaternion.Slerp(start.baseJointQuat,          end.baseJointQuat, gradient),
            Quaternion.Slerp(start.torsoJointQuat,         end.torsoJointQuat, gradient),
            Quaternion.Slerp(start.upperRightArmJointQuat, end.upperRightArmJointQuat, gradient),
            Quaternion.Slerp(start.lowerRightArmJointQuat, end.lowerRightArmJointQuat, gradient),
            Quaternion.Slerp(start.upperLeftArmJointQuat,  end.upperLeftArmJointQuat, gradient),
            Quaternion.Slerp(start.lowerLeftArmJointQuat,  end.lowerLeftArmJointQuat, gradient)
            );
    }
}
