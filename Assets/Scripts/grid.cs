using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid : MonoBehaviour
{
    [HideInInspector]
    public Node[,] Grid;

    public Vector2 GimzoSize;
    public float radius;
    private float diameter;
    
    public LayerMask unwalkable;

    int NodeNumberX;

    int NodeNumberY;
    
 

    // Start is called before the first frame update
    void Start()
    {

      
      
        diameter = 2 * radius;

        NodeNumberX = (int)(GimzoSize.x / diameter);
        NodeNumberY = (int)(GimzoSize.y / diameter);

        Grid = new Node[NodeNumberX, NodeNumberY];



        Vector3 BottomLeft = transform.position - (Vector3.right * GimzoSize.x / 2 + Vector3.forward * GimzoSize.y / 2);

        for (int x = 0; x < NodeNumberX; x++)
        {
            for (int y = 0; y < NodeNumberY; y++)
            {

                bool isWalkable = false;
                Vector3 nodePos = BottomLeft + Vector3.right * (x * diameter + radius) + Vector3.forward * (y * diameter + radius);
                if (Physics.CheckSphere(nodePos, radius, unwalkable)) isWalkable = true;
                Grid[x, y] = new Node(nodePos, isWalkable);

            }

        }




    }

    public Node getNodeposition(Vector3 currentNode)
    {
        
       
        float percentX = Mathf.Clamp01((GimzoSize.x / 2 + currentNode.x) / GimzoSize.x);
        float percentY = Mathf.Clamp01((GimzoSize.y / 2 + currentNode.z) / GimzoSize.y);

        int indexX = Mathf.RoundToInt((NodeNumberX - 1) * percentX);
        int indexY = Mathf.RoundToInt((NodeNumberY - 1) * percentY);


        return Grid[indexX, indexY];

    }
    private bool isUnwalkable(Node[,] getNode, Vector3 getPosition)
    {
        foreach (var n in getNode)
        {
            if (n.pos == getPosition)
            {
                return n.unwalkable;
            }
        }
        return false;

    }


    public List<Node> Neighbours(Node currentNode)
    {
        List<Node> Direction = new List<Node>();

        float getX = currentNode.pos.x;
        float getY = currentNode.pos.z;
        float maxGridX = Grid[NodeNumberX - 1, NodeNumberY - 1].pos.x;
        float minGridX = Grid[NodeNumberX - NodeNumberX, NodeNumberY - NodeNumberY].pos.x;
        float maxGridY = Grid[NodeNumberX - 1, NodeNumberY - 1].pos.z;
        float minGridY = Grid[NodeNumberX - NodeNumberX, NodeNumberY - NodeNumberY].pos.z;

        Vector3[] neighbours = neighbourArray(currentNode);
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (getX > minGridX && getX < maxGridX && getY > minGridY && getY < maxGridY)
            {
                Direction.Add(new Node(neighbours[i], isUnwalkable(Grid, neighbours[i])));
            }
        }




        return Direction;

    }
    private Vector3[] neighbourArray(Node current)
    {
        Vector3[] neighbours = new Vector3[8];

        neighbours[0] = new Vector3(current.pos.x, current.pos.y, current.pos.z + 1f);
        neighbours[1] = new Vector3(current.pos.x + 1f, current.pos.y, current.pos.z);
        neighbours[2] = new Vector3(current.pos.x, current.pos.y, current.pos.z - 1f);
        neighbours[3] = new Vector3(current.pos.x - 1f, current.pos.y, current.pos.z);
        neighbours[4] = new Vector3(current.pos.x - 1f, current.pos.y, current.pos.z + 1f);
        neighbours[5] = new Vector3(current.pos.x + 1f, current.pos.y, current.pos.z + 1f);
        neighbours[6] = new Vector3(current.pos.x + 1f, current.pos.y, current.pos.z - 1f);
        neighbours[7] = new Vector3(current.pos.x - 1f, current.pos.y, current.pos.z - 1f);

        return neighbours;
    }



    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GimzoSize.x, 1, GimzoSize.y));

        if (Grid != null)
        {
            foreach (var n in Grid)
            {


                Gizmos.color = Color.white;
                if (n.unwalkable)
                {

                    Gizmos.color = Color.red;
                }

                Gizmos.DrawCube(n.pos, Vector3.one * (diameter - 0.1f));

            }
            List<Node> getNodePos = new List<Node>();

            
                
             

            ;


        }

    }
}
