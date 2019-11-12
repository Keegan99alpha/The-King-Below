using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldNodeClass : DefaultClass
{
    public Material goldNode;
    [SerializeField]
    private int goldAmount;
    [SerializeField]
    private int miningAmount;
    [SerializeField]
    private bool miningShow; 

    private IEnumerator mineTimer;

    public GoldNodeClass(bool dug, bool mining, int gold, DefaultClass above, DefaultClass below, DefaultClass left, DefaultClass right) : base(above, below, left, right)
    {
        this.dug = dug;
        this.gold = gold;
        this.mining = mining;
    }

    public int gold
    {
        get;
        set;
    }

    public bool mining
    {
        get;
        set;
    }
    private IEnumerator miningTime()
    {
        while (true)
        {
            while (mining)
            {
                print("Mining");
                yield return new WaitForSeconds(2.0f);
                gold -= miningAmount;
                if (gold <= 0 && dug == false)
                {
                    StopCoroutine(miningTime());
                    gold = 0;
                    dug = true;
                    mining = false;
                    refreshPosition();
                }
            }
            yield return null;
        }
    }

    new void Start()
    {
        dugScale = 0.2f;
        defaultScale = 1.0f;
        gameObject.GetComponent<MeshRenderer>().material = goldNode;
        gold = 500;
        miningAmount = 5;
        mineTimer = miningTime();
        StartCoroutine(mineTimer);
    }

    void Update()
    {
        goldAmount = gold;
        miningShow = mining;
    }
}
