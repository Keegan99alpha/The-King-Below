using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
