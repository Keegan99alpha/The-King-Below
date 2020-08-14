﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestClickManager : MonoBehaviour
{
    [SerializeField]
    private WorkerManager workers;
    private NavMeshSurface mapMesh;
    // Start is called before the first frame update
    void Start()
    {
        mapMesh = GameObject.Find("Blocks").GetComponent<NavMeshSurface>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mapMesh.BuildNavMesh();
        }
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject target = hit.transform.gameObject;
                //for dirt tiles
                if (target.CompareTag("Terrain") && target.GetComponent<DefaultClass>().dug == false)
                {
                    target.GetComponent<DefaultClass>().dug = true;
                    target.GetComponent<DefaultClass>().refreshPosition();
                }
                //for gold tiles
                if (target.CompareTag("GoldNode") && target.GetComponent<GoldNodeClass>().mining == false && target.GetComponent<GoldNodeClass>().dug == false)
                {
                    target.GetComponent<GoldNodeClass>().mining = true;
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        { // if right button pressed...
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject target = hit.transform.gameObject;
                if (target.CompareTag("Terrain") && target.GetComponent<DefaultClass>().dug == true)
                {
                    target.GetComponent<DefaultClass>().dug = false;
                    target.GetComponent<DefaultClass>().refreshPosition();
                }
                if (target.CompareTag("GoldNode") && target.GetComponent<GoldNodeClass>().mining == true)
                {
                    target.GetComponent<GoldNodeClass>().mining = false;
                }
            }
        }
    }
}
