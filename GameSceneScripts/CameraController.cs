using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Transform targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = PlayerController.instance.gameObject.transform;

    }


    void LateUpdate()
    {

        transform.position = new Vector3(targetPosition.position.x, targetPosition.position.y, transform.position.z);
    }
}
