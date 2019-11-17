using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    public float speed = 2.0f;
    public float turnSpeed = 90f;

    public float orientation = 0;
    public Vector2 direction = new Vector2(1.0f, 0.0f);

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
            UpdateDirection();
            controller.position.x += speed * Time.deltaTime;
            //controller.position.x += direction.x * Time.deltaTime;
            //controller.position.z -= direction.y * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            UpdateDirection();
            controller.position.x -= speed * Time.deltaTime;
            //controller.position.x -= direction.x * Time.deltaTime;
            //controller.position.z += direction.y * Time.deltaTime;
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
            orientation = controller.baseJointAngle;
            //controller.baseJointAngle -= turnSpeed * Time.deltaTime;
            orientation -= turnSpeed * Time.deltaTime;
            if (orientation < 0.0f)
            {
                orientation = 360.0f;
            }
            controller.baseJointAngle = orientation;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            orientation = controller.baseJointAngle;
            //controller.baseJointAngle += turnSpeed * Time.deltaTime;
            orientation += turnSpeed * Time.deltaTime;
            if (orientation >= 360.0f)
            {
                orientation = 0.0f;
            }
            controller.baseJointAngle = orientation;
        }
    }

    void UpdateDirection()
    {
        direction.x = Mathf.Cos(controller.baseJointAngle / 180.0f * Mathf.PI);
        direction.y = Mathf.Sin(controller.baseJointAngle / 180.0f * Mathf.PI);
    }
}
