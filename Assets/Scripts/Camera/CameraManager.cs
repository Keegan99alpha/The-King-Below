using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    private float scrollScale = 0.1f;
    private Vector3 pos;
    private Vector3 lastPos;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        lastPos = pos;
        if (Input.mouseScrollDelta.y != 0)
        {
            transform.Translate(new Vector3(0, 0, Input.mouseScrollDelta.y * scrollScale));
            if (transform.position.y > 5.0f)
            {
                pos = lastPos;            
                transform.position = pos;
            }
            else if (transform.position.y < 2.0f)
            {
                pos = lastPos;
                transform.position = pos;
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += (new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += (new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += (new Vector3(0, 0, -speed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += (new Vector3(0, 0, speed * Time.deltaTime));
        }
        pos = transform.position;
    }
}
