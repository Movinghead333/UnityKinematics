using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    public float speed = 1.5f;
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
        if (InverseKinematicController.interpolating)
        {
            return;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            controller.position.x += speed * Time.deltaTime;
            controller.CalculateMatrices();
            controller.ApplyMatrices();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            controller.position.x -= speed * Time.deltaTime;
            controller.CalculateMatrices();
            controller.ApplyMatrices();
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            controller.position.z += speed * Time.deltaTime;
            controller.CalculateMatrices();
            controller.ApplyMatrices();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            controller.position.z -= speed * Time.deltaTime;
            controller.CalculateMatrices();
            controller.ApplyMatrices();
        }

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 direction = controller.currentConfig.baseJointQuat * new Vector3(1, 0, 0);
            controller.position.x += direction.x * speed * Time.deltaTime;
            controller.position.z += direction.z * speed * Time.deltaTime;
            controller.CalculateMatrices();
            controller.ApplyMatrices();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Vector3 direction = controller.currentConfig.baseJointQuat * new Vector3(1, 0, 0);
            controller.position.x -= direction.x * speed * Time.deltaTime;
            controller.position.z -= direction.z * speed * Time.deltaTime;
            controller.CalculateMatrices();
            controller.ApplyMatrices();
        }

        if (Input.GetKey(KeyCode.A))
        {
            Vector3 direction = controller.currentConfig.baseJointQuat * new Vector3(1, 0, 0);
            controller.position.x -= direction.z * speed * Time.deltaTime;
            controller.position.z += direction.x * speed * Time.deltaTime;
            controller.CalculateMatrices();
            controller.ApplyMatrices();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Vector3 direction = controller.currentConfig.baseJointQuat * new Vector3(1, 0, 0);
            controller.position.x += direction.z * speed * Time.deltaTime;
            controller.position.z -= direction.x * speed * Time.deltaTime;
            controller.CalculateMatrices();
            controller.ApplyMatrices();
        }

        if (Input.GetKey(KeyCode.Q))
        {
            Quaternion rot = Quaternion.Euler(0f, -turnSpeed * Time.deltaTime, 0f);
            controller.currentConfig.baseJointQuat *= rot;
            controller.CalculateMatrices();
            controller.ApplyMatrices();
        }
        else if (Input.GetKey(KeyCode.E))
        {
            Quaternion rot = Quaternion.Euler(0f, turnSpeed * Time.deltaTime, 0f);
            controller.currentConfig.baseJointQuat *= rot;
            controller.CalculateMatrices();
            controller.ApplyMatrices();
        }

        
    }
}
