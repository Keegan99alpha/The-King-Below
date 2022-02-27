using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultClass : BasePieceClass
{
    [SerializeField]
    private bool isDug;
    protected float dugScale;
    protected float defaultScale;
    private bool wakeUp;
    //bool to tell when a tile can be reached
    protected bool exposed;
    [SerializeField]
    protected GameObject slotPrefab;

    private GameObject diggingSlot1;
    private GameObject diggingSlot2;
    private GameObject diggingSlot3;
    private GameObject diggingSlot4;


    //inherit BasePieceClass default constructor, unused for DefaultClass tiles
    //useful for specialised tiles that need access to DefaultClasses dug and selected attribute
    public DefaultClass(DefaultClass above, DefaultClass below, DefaultClass left, DefaultClass right) : base(above, below, left, right)
    {
    }

    //inherit BasePieceClass secondary contrusctor for map edge tiles, adding selection and digging to instatiation options
    public DefaultClass(bool dug, bool selected, bool border, bool corner, DefaultClass above, DefaultClass below, DefaultClass left, DefaultClass right) : base(border, corner, above, below, left, right)
    {
        this.selected = selected;
        this.dug = dug;
    }

    public bool dug
    {
        get;
        set;
    }

    public bool selected
    {
        get;
        set;
    }

    public Material startMaterial
    {
        get;
        set;
    }


    protected virtual void Awake()
    {
        dug = isDug;
    }

    public void placeDigSlots()
    {
        diggingSlot1 = Instantiate(slotPrefab, transform.position + new Vector3(0.7f, 0, 0), Quaternion.identity, gameObject.transform);
        diggingSlot1.SetActive(false);

        diggingSlot2 = Instantiate(slotPrefab, transform.position + new Vector3(0, 0, 0.7f), Quaternion.identity, gameObject.transform);
        diggingSlot2.SetActive(false);

        diggingSlot3 = Instantiate(slotPrefab, transform.position + new Vector3(-0.7f, 0, 0), Quaternion.identity, gameObject.transform);
        diggingSlot3.SetActive(false);

        diggingSlot4 = Instantiate(slotPrefab, transform.position + new Vector3(0, 0, -0.7f), Quaternion.identity, gameObject.transform);
        diggingSlot4.SetActive(false);
    }

    public void exposureCheck()
    {
        if(((DefaultClass)above).dug && ((DefaultClass)above).exposed || 
            ((DefaultClass)below).dug && ((DefaultClass)below).exposed || 
            ((DefaultClass)left).dug && ((DefaultClass)left).exposed ||
            ((DefaultClass)right).dug && ((DefaultClass)right).exposed)
        {
            print("this node is exposed " + gameObject.transform);
            exposed = true;
            //find which neighbours are true for these conditions, set active the slot for those neighbours
        }
        else
        {
            print("this node is isolated " + gameObject.transform);
        }
    }

    protected virtual void Start()
    {
        dugScale = 0.2f;
        defaultScale = 1.0f;
        wakeUp = false;
        exposed = false;

        placeDigSlots();

        if (dug == true)
        {
            refreshPosition();
        }

        //after the generator has run, establish what this tile is.
        if (corner == true)
        {
            gameObject.GetComponent<MeshRenderer>().material = cornerMaterial;
        }
        else if (border == true)
        {
            gameObject.GetComponent<MeshRenderer>().material = borderMaterial;
        }
        //Save the tiles default appearance
        startMaterial = gameObject.GetComponent<MeshRenderer>().material;
    }

    public void refreshPosition()
    {
        //toggle the display dug for debug and the scale and position of the tile.
        if (dug == true)
        {
            if (gameObject.layer == 0)
            {
                isDug = true;
                gameObject.layer = 8;
                //change scale and then move the tile down half the scale. e.g. a scale of 0.2 takes 0.4 from either end for a total of 0.8, but it should only move down
                //one sides worth to stay at the same floor height.
                transform.localScale = new Vector3(defaultScale, dugScale, defaultScale);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - ((defaultScale - transform.localScale.y) / 2), transform.localPosition.z);
            }
        }
        else
        {
            isDug = false;
            gameObject.layer = 0;
            //move back up the same amount it moved down, then reset the scale.
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + ((defaultScale - transform.localScale.y) / 2), transform.localPosition.z);
            transform.localScale = new Vector3(defaultScale, defaultScale, defaultScale);
        }
        selected = false;
        toggleHighlight();
        //exposureCheck();
        //set check to run from all neighbours.
        ((DefaultClass)above).exposureCheck();
        ((DefaultClass)below).exposureCheck();
        ((DefaultClass)left).exposureCheck();
        ((DefaultClass)right).exposureCheck();


        transform.parent.hasChanged = true;
    }

    //turn on/off highlight on terrain selected for work
    public void toggleHighlight()
    {
        if (selected)
        {
            gameObject.GetComponent<MeshRenderer>().material = selectedTileMaterial;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = startMaterial;
        }
    }
}
