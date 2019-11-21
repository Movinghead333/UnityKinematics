using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationInterpolationTest : MonoBehaviour
{
    public Transform objectTransform;
    public Matrix4x4 objectMatrix;

    bool interpolating = false;
    float progress = 0f;

    Quaternion baseRot;
    Quaternion targetRot;
    Vector3 offset = new Vector3(1f, 0.5f, 1f);


    // Start is called before the first frame update
    void Start()
    {
        baseRot = Quaternion.Euler(0f, 0f, 0f);
        targetRot = Quaternion.Euler(0f, 350f, 0f);
        objectMatrix =
            Matrix4x4.Translate(offset) *
            Matrix4x4.Rotate(baseRot);

        objectTransform.position = getTranslation(objectMatrix);
        objectTransform.rotation = objectMatrix.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !interpolating)
        {
            TestRotation();
        }
        if (interpolating)
        {
            if (progress > 1.0f)
            {
                interpolating = false;
                progress = 0f;
                return;
            }

            progress += Time.deltaTime;
            Quaternion newQuat = Quaternion.Slerp(baseRot, targetRot, progress);

            objectMatrix = Matrix4x4.Translate(offset) *
                           Matrix4x4.Rotate(newQuat);

            objectTransform.position = getTranslation(objectMatrix);
            objectTransform.rotation = objectMatrix.rotation;
        }
    }

    void TestRotation()
    {
        interpolating = true;
    }

    static Vector3 getTranslation(Matrix4x4 mat)
    {
        return new Vector3(mat.m03, mat.m13, mat.m23);
    }
}
