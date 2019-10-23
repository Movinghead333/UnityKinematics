using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    public Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = new Vector3(0, 1.5f, 0);
        Vector3 axis = new Vector3(1f, 0, 0);

        transform.RotateAround(position, axis, Time.deltaTime * 20);
    }
}
