using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
   public  Vector3 pos;
   public bool unwalkable;
    public float G = 0f;
    public float H = 0f;
    public float F = 0f;
    

    public Node(Vector3 pos, bool unwalkable) 
    {

  
        this.pos = pos;
        this.unwalkable = unwalkable;
    }
  public float f_cost 
    {

        get { return F; } 
        set { F = value; } 
    }public float H_cost 
    {

        get { return F; } 
        set { F = value; } 
    }public float G_cost 
    {

        get { return F; } 
        set { F = value; } 
    }

    public override bool Equals(object obj)
    {
        Node node = obj as Node;
        if (node == null)
            return false;

        return node.pos == pos;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 13;
            hash = hash * 27 + pos.x.GetHashCode();
            hash = hash * 27 + pos.y.GetHashCode();
            hash = hash * 27 + pos.z.GetHashCode();


            return hash;
        }



    }

    public override string ToString()
    {
        return base.ToString();
    }
}
