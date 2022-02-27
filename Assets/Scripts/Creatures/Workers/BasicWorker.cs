using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicWorker : MonoBehaviour
{
    private NavMeshAgent agent;
    private int hp;
    [SerializeField]
    private float moveSpeed;
    public int workerIndex;
    private int taskID;

    public GameObject target;
    private bool atTarget;
    public bool hasTask;
    private bool taskComplete;
    private bool workerMining;
    private IEnumerator workerUpdateTimer;


    public void workerTask(GameObject targetObject)
    {
        hasTask = true;
        if (targetObject.gameObject.GetComponent<GoldNodeClass>() && (targetObject.gameObject.GetComponent<DefaultClass>().dug == false))
        {
            miningTask(targetObject);
        }
        else if (targetObject.gameObject.GetComponent<DefaultClass>())
        {
            diggingTask(targetObject);
            print("You selected a terrain piece");
        }
        else
        {
            workerMining = false;
            print("You selected something - probably");
        }
    }

    public void workerResign()
    {
        print("resigned");
        hasTask = false;
        taskID = -1;
        target = null;
    }

    public void miningTask(GameObject targetObject)
    {
        taskID = 0;
        atTarget = false;
        target = targetObject;
        agent.destination = target.transform.position;
    }

    public void diggingTask(GameObject targetObject)
    {
        taskID = 1;
        atTarget = false;
        target = targetObject;
        agent.destination = target.transform.position;
    }

    private IEnumerator workerUpdate()
    {
        while (true)
        {
            //no active task -> wait 1 second for any incoming tasks on repeat
            if (taskID == -1)
            {
                yield return new WaitForSeconds(1.0f);
            }
            //Verify the worker has reached the target before beginning to dig/mine
            while (!atTarget)
            {
                yield return new WaitForSeconds(0.2f);
                if (agent.remainingDistance == 0)
                {
                    //print("reached target");
                    atTarget = true;                    
                }
            }
            while (atTarget && hasTask)
            {
                //If the task is to mine, update the gold in the node they are mining
                if (taskID == 0)
                {
                    print("mining");
                    if (target.GetComponent<GoldNodeClass>().goldCheck())
                    {
                        target.GetComponent<GoldNodeClass>().updateGold();
                        yield return new WaitForSeconds(2.0f);
                    }
                    else
                    {
                        hasTask = false;
                    }
                }
                else if(taskID == 1)
                {
                    print("diggin");
                    yield return new WaitForSeconds(6.0f);
                    target.GetComponent<DefaultClass>().dug = true;
                    target.GetComponent<DefaultClass>().refreshPosition();                    
                    hasTask = false;
                    print("finished diggin");
                }
                else
                {
                    //hasTask = false;
                    break;
                }
                
            }
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        taskID = -1;
        hasTask = false;
        atTarget = true;
        target = null;
        hp = 50;
        moveSpeed = 10.0f;
        workerMining = false;
        agent = GetComponent<NavMeshAgent>();
        workerUpdateTimer = workerUpdate();
        StartCoroutine(workerUpdateTimer);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
