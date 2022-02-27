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
    [SerializeField]
    private Logistics logist;

    private IEnumerator mineTimer;

    public GoldNodeClass(bool mining, int gold, DefaultClass above, DefaultClass below, DefaultClass left, DefaultClass right) : base(above, below, left, right)
    {
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

    /*move mining timer to individual workers, maintain gold log update inside node.
    private IEnumerator miningTime()
    {
        //while engaged, take away gold and send it to the player inventory at set intervals. If depleted, dig the node.
        while (true)
        {
            //check if this is doubled code
            while (mining)
            {
                yield return new WaitForSeconds(2.0f);
                gold -= miningAmount;
                logist.playerGold += miningAmount;
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
    }*/
    public bool goldCheck()
    {
        if(gold > 0)
        {
            if(gold < miningAmount)
            {
                miningAmount = gold;
            }
            return (true);
        }
        else
        {
            return (false);
        }
    }

    public void updateGold()
    {
        gold -= miningAmount;
        logist.playerGold += miningAmount;
        if (gold <= 0 && dug == false)
        {
            gold = 0;
            dug = true;
            refreshPosition();
        }
    }

    new void Start()
    {
        //Find the player inventory manager and get the script used to track gold
        logist = GameObject.Find("UnderlordManager").GetComponent<Logistics>();
        dugScale = 0.2f;
        defaultScale = 1.0f;
        gameObject.GetComponent<MeshRenderer>().material = goldNode;
        gold = 500;
        miningAmount = 5;
        startMaterial = gameObject.GetComponent<MeshRenderer>().material;
        placeDigSlots();
        //mineTimer = miningTime();
        //StartCoroutine(mineTimer);
    }

    void Update()
    {
        goldAmount = gold;
        miningShow = mining;
    }
}
