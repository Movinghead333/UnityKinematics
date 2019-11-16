using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    public float speed = 2.0f;
    public float turnSpeed = 90f;

    public HumanController controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            controller.position.x += speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            controller.position.x -= speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            controller.position.z += speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            controller.position.z -= speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            controller.baseJointAngle -= turnSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            controller.baseJointAngle += turnSpeed * Time.deltaTime;
        }
    }
}
