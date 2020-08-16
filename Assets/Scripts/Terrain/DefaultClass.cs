using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultClass : BasePieceClass
{
    [SerializeField]
    private bool isDug;
    protected float dugScale;
    protected float defaultScale;

    public DefaultClass(DefaultClass above, DefaultClass below, DefaultClass left, DefaultClass right) : base(above, below, left, right)
    {
    }

    public DefaultClass(bool dug, bool border, bool corner, DefaultClass above, DefaultClass below, DefaultClass left, DefaultClass right) : base(border, corner, above, below, left, right)
    {
        this.dug = dug;
    }

    public bool dug
    {
        get;
        set;
    }

    protected virtual void Awake()
    {
        dug = isDug;
    }

    protected virtual void Start()
    {
        dugScale = 0.2f;
        defaultScale = 1.0f;
        if (dug == true)
        {
            refreshPosition();
        }
    }

    public void refreshPosition()
    {
        //toggle the display dug for debug and the scale and position of the tile.
        if (dug == true)
        {
            isDug = true;
            gameObject.layer = 8;
            //change scale and then move the tile down half the scale. e.g. a scale of 0.2 takes 0.4 from either end for a total of 0.8, but it should only move down
            //one sides worth to stay at the same floor height.
            transform.localScale = new Vector3(defaultScale, dugScale, defaultScale);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - ((defaultScale - transform.localScale.y) / 2), transform.localPosition.z);
        }
        else
        {
            isDug = false;
            gameObject.layer = 0;
            //move back up the same amount it moved down, then reset the scale.
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + ((defaultScale - transform.localScale.y) / 2), transform.localPosition.z);
            transform.localScale = new Vector3(defaultScale, defaultScale, defaultScale);
        }
        transform.parent.hasChanged = true;
    }

    void Update()
    {
        //after the generator has run, establish what this tile is.
        if (corner == true)
        {
            gameObject.GetComponent<MeshRenderer>().material = cornerMaterial;
        }
        else if (border == true)
        {
            gameObject.GetComponent<MeshRenderer>().material = borderMaterial;
        }
    }
}
