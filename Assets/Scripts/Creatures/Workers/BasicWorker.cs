using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWorker : MonoBehaviour
{
    private int hp;
    [SerializeField]
    private float moveSpeed;
    public int workerIndex;
    private BasePieceClass target;
    // Start is called before the first frame update
    void Start()
    {
        hp = 50;
        moveSpeed = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
