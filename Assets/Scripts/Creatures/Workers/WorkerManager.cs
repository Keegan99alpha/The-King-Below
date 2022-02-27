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

    public List<GameObject> items_terrains_active;
    public List<GameObject> items_terrains_reachable_slots;
    public List<GameObject> items_terrains_reachable_unfilled;


    private RaycastHit hit;
    //private Vector3 currentPos;
    private float newY;
    private Vector3 castpos;

    private BasicWorker cloner;
    private int filledIndexCheck;

    private GameObject random;

    private IEnumerator workerCheckUpTimer;

    void Start()
    {
        items_terrains_active = new List<GameObject>();
        workerLimit = 10;
        castpos = new Vector3(transform.position.x, Mathf.Infinity, transform.position.z);
        workerCheckUpTimer = workerCheckUp();
        StartCoroutine(workerCheckUpTimer);
    }

    public void aquireReachableSlots()
    {

    }

    //keep track of all worker task.
    public void workerJobTracker()
    {
        //check each worker for a finished task - if it declares it no longer has a task but has a target object, remove object from world task list.
        foreach (BasicWorker alive in workers)
        {
            if (items_terrains_active.Contains(alive.target) && !alive.hasTask)
            {
                items_terrains_active.Remove(alive.target);
            }
        }
        if (items_terrains_active.Count >= 1)
        {
            foreach (BasicWorker alive in workers)
            {
                if (alive.hasTask)
                {
                    //gotta call job tracker when the worker finishes to actually resign them, otherwise this doesn't run until new tasks are added
                    //removed !items_terrains_active.Contains(alive.target) || as it was seemingly redundant

                    //if worker has a task but it's not longer in the task list (also represented by selected status), resign it and give it a different task.
                    if (alive.target.GetComponent<DefaultClass>().selected == false)
                    {
                        alive.workerResign();
                        alive.workerTask(pickRandomTask());
                        print("Assigned task after resigning old task");
                    }
                }
                //if worker has no task, provide one.
                else if (!alive.hasTask)
                {
                    alive.workerTask(pickRandomTask());
                    print("Assigned task due to no current task");
                }
            }
        }
    }

    public GameObject pickRandomTask()
    {
        if (items_terrains_active.Count == 0)
        {
            Debug.LogError("Cannot pick task: There are no active tasks.");
            return null;
        }
        return items_terrains_active[Random.Range(0, items_terrains_active.Count)];
    }

    //keep track of user selected worker tasks
    public void worldJobTracker(GameObject item_terrain)
    {
        if (items_terrains_active.Contains(item_terrain))
        {
            item_terrain.GetComponent<DefaultClass>().selected = false;
            item_terrain.GetComponent<DefaultClass>().toggleHighlight();
            items_terrains_active.Remove(item_terrain);
        }
        else
        {
            items_terrains_active.Add(item_terrain);
            item_terrain.GetComponent<DefaultClass>().selected = true;
            item_terrain.GetComponent<DefaultClass>().toggleHighlight();
            //assignWorker(item_terrain);
        }
        workerJobTracker();
    }

    private IEnumerator workerCheckUp()
    {
        while (true)
        {
            workerJobTracker();
            yield return new WaitForSeconds(0.5f);
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
                    newY = hit.point.y + prefab.GetComponent<Renderer>().bounds.size.y / 2;
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
        if (add == false)
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
        if (addWorker == true || Input.GetKeyDown(KeyCode.L))
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

        //move out of update later
        //workerJobTracker();
    }
}
