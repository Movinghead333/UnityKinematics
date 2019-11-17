using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public HumanController humanController;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = humanController.lowerBody.position;
        gameObject.transform.rotation = humanController.lowerBody.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = humanController.lowerBody.position;
        gameObject.transform.rotation = humanController.lowerBody.rotation
            * Quaternion.Euler(0.0f, 90.0f, 0.0f); ;
        gameObject.transform.Translate(new Vector3(0.0f, 2.0f, -1.0f));
    }
}
