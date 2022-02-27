using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPiece : DefaultClass
{
    public CenterPiece(DefaultClass above, DefaultClass below, DefaultClass left, DefaultClass right) : base(above, below, left, right)
    {
    }

    public void Reset()
    {
        dug = true;
    }

    protected override void Start()
    {
        Reset();
        startMaterial = gameObject.GetComponent<MeshRenderer>().material;
        base.Start();
        exposed = true;
    }
}
