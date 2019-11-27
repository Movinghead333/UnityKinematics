using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckRangeUI : MonoBehaviour
{
    public HumanController humanController;
    public Transform targetObject;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Matrix4x4 lowerBodyMatrix = Matrix4x4.Translate(humanController.position) * Matrix4x4.Rotate(humanController.currentConfig.lowerBodyQuat);
        Matrix4x4 upperBodyMatrix = lowerBodyMatrix * humanController.upperBodyOffsetMatrix * Matrix4x4.Rotate(humanController.currentConfig.upperBodyQuat);
        Matrix4x4 upperRightArmMatrix = upperBodyMatrix * humanController.upperRightArmOffsetMatrix * Matrix4x4.Rotate(humanController.currentConfig.upperRightArmJointQuat);
        float dist = Vector3.Distance(targetObject.position, HumanController.getTranslation(upperRightArmMatrix));
        text.text = "Distance: " + dist;
    }
}
