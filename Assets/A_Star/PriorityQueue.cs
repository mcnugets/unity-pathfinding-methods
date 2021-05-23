using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*



*/




public class PriorityQueue
{
    


    public List<Node> priority = new List<Node>();


    /// <summary>
    /// adds and then sort an object to a queue
    /// </summary>
    /// <param name="Neighbour"></param>
    public void add(Node Neighbour)
    {
      

        List<Node> ref_ = priority;
       
        for (int i = 0; i < ref_.Count; i++)
        {
            if (ref_[i].Equals(Neighbour))
            {

           
                ref_.RemoveAt(i);
                priority = ref_;

            }
        }
     
        for (int i = 0; i < priority.Count; i++)
        {
          
           

            if (Neighbour.F < ref_[i].F || Neighbour.F == ref_[i].F && Neighbour.H < ref_[i].H)
            {
                Node oldvalue = ref_[i];
                ref_[i] = Neighbour;

                Neighbour = oldvalue;
              

            }

            if (i == priority.Count-1)
            {
                ref_.Add(Neighbour);
                break;
            }
        }

        if (ref_.Count == 0) ref_.Add(Neighbour);
        priority = ref_;

       


    }
    public List<Node> getAllValues()
    {

        return priority;
    } 

    /// <summary>
    /// removes element with the top priority 
    /// </summary>
    /// <returns></returns>
    public Node poll() 
    {
       
        if (isEmpty())
        {
            return null;
        }
        else
        {
            Node getValue = priority[0];
            priority.RemoveAt(0);
            return getValue;
        }
    }

    public bool isEmpty()
    {


        if (priority.Count == 0) return true;


        return false;

    }

    /// <summary>
    /// returns the element with top priority without removing it 
    /// </summary>
    /// <returns></returns>
    public Node peek() 
    {
        if (isEmpty()) 
        {
            Debug.Log("NULL LIST");
            return null;
        }
        else 
        {
            return priority[0];
        }
         
            
        
    }
    public bool ContainsKey(Vector3 nodePos)
    {
        
        foreach (var node in priority)
        {
            Vector3 localPos = node.pos;
            if (nodePos == localPos)
            {
                return true;
            }
        }
        return false;
    }



    public bool Contains(Node value) 
    {
        foreach (var item in priority)
        {
            if (value == item) 
            {
                return true;
            }
        }
        return false;
    }
    public int Count() 
    {
        return priority.Count;
    }

    


}
public class errorMessage
{
 
    public string Message { get; set; }
    public bool Validation { get; set; }
}
