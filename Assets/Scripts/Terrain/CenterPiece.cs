using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPiece : DefaultClass
{
    public CenterPiece(bool dug, bool border, bool corner, DefaultClass above, DefaultClass below, DefaultClass left, DefaultClass right) : base(dug, border, corner, above, below, left, right)
    {
        this.dug = dug;
    }

    public void Reset()
    {
        dug = true;
    }

    protected override void Start()
    {
        Reset();
        base.Start();
    }
}
