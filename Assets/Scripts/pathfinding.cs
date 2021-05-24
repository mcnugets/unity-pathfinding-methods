using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;


public class pathfinding : MonoBehaviour
{

    
    private grid Grid;
    public List<Node> discovered;
    public Queue<Node> sequence;
    private Dictionary<Node, Node> prev;
    private PriorityQueue priorityQueue;

  


    //Octile distance/Euclidean Distance
    //double hVDistance = 1.0;
    //double dDistance = 1.4;

    /* for Manhattan Distances,
    double horizontalVerticalDistance = 1.0;
    double diagonalDistance = 2.0;

    for Chebyshev Distances,
    double horizontalVerticalDistance = 1.0;
    double diagonalDistance = 1.0; */




    public float vectorDistance(Vector3 v1, Vector3 v2)
    {
        
        float x_2 = v2.x;
        float x_1 = v1.x;
        float y_2 = v2.z;
        float y_1 = v1.z;
       
       
        return Mathf.Sqrt(Mathf.Pow((x_2 - x_1), 2) + Mathf.Pow((y_2 - y_1), 2)); ;
    }
    private Queue<Node> reverseQueue(Queue<Node> getReversed) 
    {
        Stack<Node> convertQueue = new Stack<Node>();
        while (getReversed.Count != 0) 
        {
            convertQueue.Push(getReversed.Dequeue());
        }
        while (convertQueue.Count != 0) 
        {
            getReversed.Enqueue(convertQueue.Pop());
        }
        return getReversed;
    }
    private Node reconctruct_path(Dictionary<Node, Node> cameFrom, Node current, Node start)
    {

        sequence = new Queue<Node>();

        while (!sequence.Contains(start))
        {

            sequence.Enqueue(current);
            current = cameFrom[current];
        }
        sequence = reverseQueue(sequence);
        
        return current;
    }
    private float Heuristics(Node current, Node finish, Node Neighbour)
    {
        float dx = Mathf.Abs(Neighbour.pos.x - finish.pos.x);
        float dy = Mathf.Abs(Neighbour.pos.z - finish.pos.z);
        float D = vectorDistance(current.pos, Neighbour.pos);

        float OctileDistance = D * Mathf.Min(dx, dy) + Mathf.Abs(dx - dy);

        return OctileDistance;
    }



    #region pathfinding algorithms

    protected Node Greedy(Vector3 StartingPos, Vector3 FinishPos)
    {
        priorityQueue = new PriorityQueue();
        prev = new Dictionary<Node, Node>();
        discovered = new List<Node>();
        Grid = GetComponent<grid>();
        Node start = Grid.getNodeposition(StartingPos);
        Node finish = Grid.getNodeposition(FinishPos);
        discovered.Add(start);
        prev.Add(start, null);
        if (!start.unwalkable)
            priorityQueue.add(new Node(start.pos, false));




        while (!priorityQueue.isEmpty())
        {
            Node current = priorityQueue.poll();
            discovered.Add(current);

            if (current.Equals(finish))
                return reconctruct_path(prev, current, start);


            List<Node> neighbours = Grid.Neighbours(current);

            foreach (var next in neighbours)
            {

                if (next.unwalkable || discovered.Contains(next)) continue;

                next.H = Heuristics(current, finish, next);

                priorityQueue.add(next);
                if (!prev.ContainsKey(next))
                {
                    prev.Add(next, current);
                }
            }






        }

        return finish;
    }




    protected Node Dijkstra(Vector3 StartingPos, Vector3 FinishPos)
    {

        priorityQueue = new PriorityQueue();
        prev = new Dictionary<Node, Node>();
        discovered = new List<Node>();
        Grid = GetComponent<grid>();
        Node start = Grid.getNodeposition(StartingPos);
        Node finish = Grid.getNodeposition(FinishPos);
        discovered.Add(start);
        prev.Add(start, null);
        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        if (!start.unwalkable)
            priorityQueue.add(new Node(start.pos, false));


        foreach (var Graph in Grid.Grid)
        {

            dist.Add(Graph, float.PositiveInfinity);
        }
  
        
        while (!priorityQueue.isEmpty())
        {
            Node current = priorityQueue.poll();
            discovered.Add(current);


            if (current.Equals(finish))
                return reconctruct_path(prev, current, start);
         

            List<Node> neighbour = Grid.Neighbours(current);

          


            foreach (var next in neighbour)
            {

                if (next.unwalkable || discovered.Contains(next)) continue;

                float newGcost = current.G + 1f;
                
               
                if (newGcost < dist[next] )
                {

                    dist[next] = newGcost;
                    next.G = dist[next];
                    priorityQueue.add(next);

                    if (!prev.ContainsKey(next))
                        prev.Add(next, current);


                }

            }
          



        }
        return finish;
    }


    protected Node A_Star(Vector3 StartingPos, Vector3 FinishPos)
    {

        priorityQueue = new PriorityQueue();
        prev = new Dictionary<Node, Node>();
        discovered = new List<Node>();
        Grid = GetComponent<grid>();
        Node start = Grid.getNodeposition(StartingPos);
        Node finish = Grid.getNodeposition(FinishPos);
        discovered.Add(start);
        prev.Add(start, null);
        Dictionary<Node, float> gScore = new Dictionary<Node, float>();  
       if(!start.unwalkable)
            priorityQueue.add(new Node(start.pos, false));

  
        foreach (var gridPos in Grid.Grid)
        {
            gScore.Add(gridPos, float.PositiveInfinity);
        }


        while (!priorityQueue.isEmpty())
        {
            Node current = priorityQueue.poll();
            discovered.Add(current);


            List<Node> neighbours = Grid.Neighbours(current);

            if (current.Equals(finish))
                return reconctruct_path(prev, current, start);
       
            

            foreach (var N in neighbours)
            {
                Vector3 u = current.pos;
                Vector3 n = N.pos;

                if (N.unwalkable || discovered.Contains(N)) continue;


                float newGcost = current.G + vectorDistance(u, n);



                if (newGcost < gScore[N])
                {


                    //g-> is the cost so far that was calculated from starting node to current node
                    //h-> is heuristics which is responsible for estimated distance between current and finish node
                    gScore[N] = newGcost;
                    N.G = gScore[N];
                    N.H = Heuristics(current, finish, N);
                    //f(n)=g(n)+h(n)
                    N.F = N.G + N.H;
                    priorityQueue.add(N);

                    if (!prev.ContainsKey(N))
                        prev.Add(N, current);



                }
            }


        }
      

    

        return finish;
    }

    
 

    protected Node BreadthFirstSearch(Vector3 StartingPos, Vector3 FinishPos)
    {
        Grid = GetComponent<grid>();
        Node start = Grid.getNodeposition(StartingPos);
        Node finish = Grid.getNodeposition(FinishPos);
        Queue<Node> queueOfNodes = new Queue<Node>();
        prev = new Dictionary<Node, Node>();
        discovered = new List<Node>();
        queueOfNodes.Enqueue(start);
        discovered.Add(start);
        prev.Add(start, null);


       


        while (queueOfNodes.Count != 0)
        {

            Node current = queueOfNodes.Dequeue();
            discovered.Add(current);


            if (current.Equals(finish))
                return reconctruct_path(prev, current, start);

            List<Node> w = Grid.Neighbours(current);




            foreach (var next in w)
            {
                if (next.unwalkable || discovered.Contains(next)) continue;
                if (!prev.ContainsKey(next))
                {
                    queueOfNodes.Enqueue(next);
                    prev.Add(next, current);
                    

                }

            }

        }
        
        return finish;
    }


    #endregion


}


    



