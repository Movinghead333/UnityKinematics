using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public HumanController humanController;

    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = humanController.lowerBody.position;
        gameObject.transform.rotation = humanController.lowerBody.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Matrix4x4 cam =
            Matrix4x4.Translate(humanController.lowerBody.position) *
            Matrix4x4.Rotate(humanController.lowerBody.rotation) *
            Matrix4x4.Translate(new Vector3(-0.5f, 2.5f, 0f)) * 
            Matrix4x4.Rotate(Quaternion.Euler(60f, 90f, 0f));

        gameObject.transform.position = HumanController.getTranslation(cam);
        gameObject.transform.rotation = cam.rotation;
    }
}
