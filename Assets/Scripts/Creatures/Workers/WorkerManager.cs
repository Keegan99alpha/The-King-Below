using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerManager : MonoBehaviour
{
    [SerializeField]
    private bool addWorker;
    [SerializeField]
    private bool removeWorker;
    [SerializeField]
    private bool removeLastWorker;
    [SerializeField]
    private BasicWorker prefab;
    [SerializeField]
    private int workerLimit;

    [SerializeField]
    private List<BasicWorker> workers;

    private List<GameObject> items_terrains_active;

    private RaycastHit hit;
    //private Vector3 currentPos;
    private float newY;
    private Vector3 castpos;

    private BasicWorker cloner;
    private int filledIndexCheck;

    void Start()
    {
        items_terrains_active = new List<GameObject>();
        workerLimit = 10;
        castpos = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
    }

    //assign a worker to a target tile.
    public void assignWorker(GameObject target)
    {
        workers[0].workerTask(target);
        /*
        agent = workers[0].GetComponent<NavMeshAgent>();
        agent.destination = target.transform.position;
        */
    }   

    //keep track of when a worker has reached a location or finished a task.
    public void workerJobTracker(NavMeshAgent worker)
    {
        
    }

    public void worldJobTracker(GameObject item_terrain)
    {
        if (items_terrains_active.Contains(item_terrain))
        {
            items_terrains_active.Remove(item_terrain);
            //resign worker;
        }
        else
        {
            items_terrains_active.Add(item_terrain);
            assignWorker(item_terrain);
        }
    }

    private void updateWorkerCount(List<BasicWorker> workers, bool add, int index)
    {
        cloner = null;
        //Create a worker
        if (add == true)
        {
            filledIndexCheck = 0;
            //Find any empty spots in the list and fill them first.
            foreach (BasicWorker unit in workers)
            {
                //If an empty element is found, fill and return.
                if (unit == null)
                {
                    break;
                }
                filledIndexCheck++;
            }
            //If list is at it's max size and no spare slots are found, return.
            if (workers.Count == workerLimit && filledIndexCheck == workers.Count)
            {
                print("Hello");
                return;
            }
            //Otherwise, prepare a new instance of a worker.
            else
            {
                print("not at max/open slots");
                if (Physics.Raycast(castpos, transform.TransformDirection(Vector3.down), out hit))
                {
                    //fix this//
                    newY = hit.point.y + prefab.GetComponent<Renderer>().bounds.size.y/2;
                    print(newY);
                }
                cloner = Instantiate(prefab, new Vector3(transform.position.x, newY, transform.position.z), Quaternion.identity);
            }
            //If there is a spare slot
            if (filledIndexCheck < workers.Count)
            {
                workers[filledIndexCheck] = cloner;
                workers[filledIndexCheck].workerIndex = filledIndexCheck;
                return;
            }
            //If no empty slots, make a new entry at the end. 
            else
            {
                cloner.workerIndex = workers.Count;
                workers.Add(cloner);
            }
        }

        //Remove a worker
        if(add == false)
        {
            //If the index is within the lists range, delete the specific worker
            if (index >= 0 && index < workers.Count)
            {
                GameObject gameOb = workers[index].gameObject;
                if (gameOb != null)
                {
                    Destroy(gameOb);
                }
                else
                {
                    print("Already dead lad.");
                }
            }
            //If the index is negative, delete the latest worker
            else if (index < 0 && workers.Count > 0)
            {
                Destroy(transform.GetChild(transform.childCount - 1).gameObject);
            }
            //If the index is outside the range (can only be above since below would be negative), alert.
            else if (index > workers.Count)
            {
                print("Trying to remove a worker that doesn't exist yet.");
            }
            //If no workers exist and not targetting a specific worker, alert
            else if (workers.Count == 0)
            {
                print("No workers to remove.");
            }
            //Fallthrough alert
            else
            {
                print("Worker removal logic has a hole.");
            }
        }
    }

    void Update()
    {
        if(addWorker == true || Input.GetKeyDown(KeyCode.L))
        {
            addWorker = false;
            updateWorkerCount(workers, true, -1);
        }
        if (removeWorker == true)
        {
            removeWorker = false;
            updateWorkerCount(workers, false, 0);
        }
        if (removeLastWorker == true)
        {
            removeLastWorker = false;
            updateWorkerCount(workers, false, -1);
        }
        //should be refering to target transform
        /*if(transform.position == agent.destination && agent.destination != null)
        {
            print("hi");
            agent.ResetPath();
        }*/
    }
}
