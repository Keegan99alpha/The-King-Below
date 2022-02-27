using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickManager : MonoBehaviour
{
    // Start is called before the first frame update
    private WorkerManager workers;
    private GameObject mapBlocks;
    private NavMeshSurface mapMesh;
    // Start is called before the first frame update
    void Start()
    {
        workers = (WorkerManager)FindObjectOfType(typeof(WorkerManager));
        mapBlocks = GameObject.Find("MapGenerator");
        mapMesh = mapBlocks.GetComponent<NavMeshSurface>();
        mapMesh.BuildNavMesh();
        print(mapBlocks.transform.hasChanged);
        /*foreach (Transform child in mapBlocks.transform)
        {
            child.hasChanged = false;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        //update navmesh anytime a block piece notifies the parent container its changed 
        if (mapBlocks.transform.hasChanged == true)
        {
            mapMesh.BuildNavMesh();
            mapBlocks.transform.hasChanged = false;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject target = hit.transform.gameObject;
                if (target.CompareTag("Terrain") || target.CompareTag("GoldNode"))
                {
                    workers.worldJobTracker(hit.transform.gameObject);
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject target = hit.transform.gameObject;
                //for dirt tiles
                if(target.CompareTag("Terrain") && target.GetComponent<DefaultClass>().dug == false)
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
