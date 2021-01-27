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

    private GameObject target;
    private GameObject lastTarget;
    private bool atTarget;
    private bool taskComplete;
    private bool workerMining;
    private IEnumerator workerUpdateTimer;


    public void workerTask(GameObject targetObject)
    {
        if (targetObject.gameObject.GetComponent<GoldNodeClass>())
        {
            miningTask(targetObject);
        }
        else if (targetObject.gameObject.GetComponent<DefaultClass>())
        {
            atTarget = false;
            target = targetObject;
            agent.destination = target.transform.position;
            print("You selected a terrain piece");
        }
        else
        {
            workerMining = false;
            print("You selected something - probably");
        }
    }

    public void miningTask(GameObject targetObject)
    {
        taskID = 0;
        atTarget = false;
        workerMining = false;
        lastTarget = target;
        target = targetObject;
        agent.destination = target.transform.position;
        if (lastTarget != null)
        {
            lastTarget.GetComponent<GoldNodeClass>().mining = false;
        }
        //print("You selected a gold node");
    }

    private IEnumerator workerUpdate()
    {
        //set up a target variable to store selected gold tile and then use the click management code to toggle mining.
        while (true)
        {
            if (taskID == -1)
            {
                yield return new WaitForSeconds(1.0f);
            }
            while (!atTarget)
            {
                yield return new WaitForSeconds(0.2f);
                if (agent.remainingDistance == 0)
                {
                    print("reached target");
                    atTarget = true;
                    if (taskID == 0)
                    {
                        print("asked to mine");
                        workerMining = true;
                    }
                    else
                    {
                        workerMining = false;
                    }
                }
            }
            while (atTarget && workerMining)
            {
                print("mining");
                target.GetComponent<GoldNodeClass>().updateGold();
                yield return new WaitForSeconds(2.0f);
            }
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        taskID = -1;
        atTarget = true;
        target = null;
        lastTarget = null;
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
