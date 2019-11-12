using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePieceClass : MonoBehaviour
{
    //default dirt materials
    public Material normalMaterial;
    public Material borderMaterial;
    public Material cornerMaterial;

    //contructor for tiles that don't care about edges/borders
    public BasePieceClass(BasePieceClass above, BasePieceClass below, BasePieceClass left, BasePieceClass right)
    {
        this.above = above;
        this.below = below;
        this.left = left;
        this.right = right;
    }

    //contructor for tiles that need to know about edges/borders
    public BasePieceClass(bool border, bool corner, BasePieceClass above, BasePieceClass below, BasePieceClass left, BasePieceClass right)
    {
        this.border = border;
        this.corner = corner;
        this.above = above;
        this.below = below;
        this.left = left;
        this.right = right;
    }
   
    public bool border
    {
        get;
        set;
    }
    public bool corner
    {
        get;
        set;
    }
    public BasePieceClass above
    {
        get;
        set;
    }
    public BasePieceClass below
    {
        get;
        set;
    }
    public BasePieceClass left
    {
        get;
        set;
    }
    public BasePieceClass right
    {
        get;
        set;
    }

    void Start()
    {
        
    }

    void Update()
    {

    }
}
