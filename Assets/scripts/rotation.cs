using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    public Transform lowerBody;
    public Transform upperBody;
    public Transform upperArm;
    public Transform lowerArm;

    public Transform cube;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(lowerBody.position);
        Vector3 position = lowerBody.position + new Vector3(0f, 0.5f, 0f);
        Vector3 y_axis = new Vector3(0f, 1f, 0f);
        Vector3 x_axis = new Vector3(1f, 0f, 0f);

        //upperBody.RotateAround(position, y_axis, Time.deltaTime * 20);

        Vector3 direction = Vector3.ProjectOnPlane(upperArm.position - upperBody.position, y_axis);
        Vector3 upperArmPos = upperArm.position + new Vector3(0f, 0.25f, 0f);
        upperArm.RotateAround(upperArmPos, direction, Time.deltaTime * 40);



    }
}
