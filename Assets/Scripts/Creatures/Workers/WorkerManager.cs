using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    [SerializeField]
    private BasicWorker[] workers;
    private BasicWorker[] oldWorkers;
    [SerializeField]
    private bool addWorkers;
    [SerializeField]
    private BasicWorker prefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void updateWorkers()
    {
        for (int i = 0; i < workers.Length; i++)
        {
            print(workers[i]);
            if (workers[i] == null)
            {
                workers[i] = Instantiate(prefab, new Vector3(0, 3, 0), Quaternion.identity, gameObject.transform);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(addWorkers == true)
        {
            //fix
            oldWorkers = workers;
            workers = new BasicWorker[workers.Length + 1];
            workers = oldWorkers;
            addWorkers = false;
            updateWorkers();
        }       
    }
}
