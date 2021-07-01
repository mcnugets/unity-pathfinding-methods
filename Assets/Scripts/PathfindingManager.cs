using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


public class PathfindingManager : MonoBehaviour
{
    
    public struct Agent
    {
        public Transform agentPos;
        public Queue<Node> getPath;
        public Agent(Transform agentPos, Queue<Node> getPath)
        {
            this.agentPos = agentPos;
            this.getPath = getPath;
        }
     

    }
    #region variables
    public Transform start;
    public Transform Player;
    public List<Transform> patrolPoints;
    private LineRenderer lr;
    //detection range of a player  
    public detectionRange detect;
    private float WaitTime;
    private pathfindingController path;
    private grid Grid;
    private Vector3 old_pos;
    private int old_method;


    #endregion
    

     void Start()
    {


       
        lineSettings();
        Grid = GetComponent<grid>();
        path = GetComponent<pathfindingController>();
        old_pos = start.transform.position;
        StartCoroutine(hasFoundThePLayer());



        


    }

 


    #region functions       
    private void lineSettings()
    {
       

        lr = GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lr.startWidth = 0.25f;
        lr.endWidth = 0.25f;
        lr.startColor = Color.green;
        lr.endColor = Color.green;
    }



    private bool is_all_visited(List<Tuple<Vector3, bool>> visitedNodes)
    {
        bool areAllVisited = false;
        int boolCounter = 0;

        for (int i = 0; i < visitedNodes.Count; i++)
        {
            if (visitedNodes[i].Item2) boolCounter++;
          
        }
        if (boolCounter == patrolPoints.Count ) areAllVisited = !areAllVisited;


        return areAllVisited;
    }

    private Node findMinValue(List<Node> nextNode)
    {

        

        Node lowestValue = nextNode[0];

        for (int i = 1; i < nextNode.Count; i++)
        {
            if (nextNode[i].H < lowestValue.H)
            {
                lowestValue = nextNode[i];
            }
        }

        return lowestValue;
    }



    private Vector3 StoredPosition(Queue<Vector3> vectorQueue)
    {
        Vector3 getvector;
        if (!vectorQueue.Contains(Player.transform.position) && isMax(vectorQueue))
        {
            vectorQueue.Enqueue(Player.transform.position);


        }
        if (!isMax(vectorQueue))
        {


            getvector = vectorQueue.Dequeue();

            return getvector;


        }

        return vectorQueue.Peek();
    }
    private bool hasChangedPos(Vector3 start, Vector3 finish)
    {
        float dx = Mathf.Abs(finish.x - start.x);
        float dy = Mathf.Abs(finish.z - start.z);

        float dPos = dx + dy;

        if (dPos == 0) return true;

        return false;

    }
    private void redneredNodePath(Queue<Node> getSequence)
    {

        Queue<Vector3> renderVector = new Queue<Vector3>();
        foreach (var get in getSequence)
        {
            renderVector.Enqueue(get.pos);
        }

        lr.positionCount = renderVector.Count;
        lr.SetPositions(renderVector.ToArray());


    }
  
    private Agent FindThePath(int index, Transform start, Vector3 targetPoint)
    {
        path.selectPathfinding(index, start.transform.position, targetPoint);
        Agent agentPath = new Agent(start, path.sequence);
        if (GameObject.Find("Parent") != null)
        {
            Destroy(GameObject.Find("Parent"));
        }
        markDiscovered(path.discovered);
        return agentPath;

    }
    private void markDiscovered(List<Node> disPosition) 
    {
        GameObject parent = new GameObject("Parent");
        shape begin = GetComponent<shape>();
        foreach (var node in disPosition)
        {
            begin.createcircle(node.pos, Grid.radius).transform.parent = parent.transform;
           
        }


    }
    private bool isMax(Queue<Vector3> getQueue)
    {
        
        if (getQueue.Count < 2)
        {

            return true;
        }
        else if (getQueue.Count == 2)
        {

            return false;
        }
        return false;
    }

    #endregion



    #region IEnumerators
  

    private IEnumerator hasFoundThePLayer()
    {
       
        path.switcher();
        yield return new WaitUntil(() => !not_set(path.method));
        


        bool within_its_range;
        bool runChase = false;
        bool runPatrol = false;
        old_method = path.method;


        IEnumerator patrol = agentPatrol( patrolPoints, start);
        IEnumerator chase = followPath(start, Player.transform.position);

        while (start.transform.position!=Player.transform.position)
        {
            float distance = Vector3.Distance(start.transform.position, Player.transform.position);
            within_its_range = distance < detect.radius? true : false;
           
            switch (within_its_range)
            {
                case true:
                    if (!runChase)
                    {
                        chase = followPath(start, Player.transform.position);
                        StopCoroutine(patrol);
                        runChase = true;
                        StartCoroutine(chase);
                        runPatrol = false;

                    }
                    break;

                case false:
                    if (!runPatrol)
                    {
                        patrol = agentPatrol(patrolPoints, start);

                        StopCoroutine(chase);
                        runPatrol = true;
                        StartCoroutine(patrol);
                        runChase = false;
                    }
                    break;

            }
            yield return null;
        }
    }

    private IEnumerator agentPatrol(List<Transform> patrolingPoints, Transform start)
    {



       
      //  currentNode.Enqueue(start);
        
        bool hasFoundClosestPoint = false;
        List<Tuple<Vector3, bool>> visited  = new List<Tuple<Vector3, bool>>();
        visited.Add(Tuple.Create(start.transform.position, true));

        while (start.transform.position!=Player.transform.position)
        {
            Transform current = start;
            List<Node> nextNode = new List<Node>();
            print("FIRST WHILE LOOP CHECK");
            if (is_all_visited(visited))
                visited.Clear();
           

            if (!hasFoundClosestPoint)
            {
               

                for (int i = 0; i < patrolingPoints.Count; i++)
                {
                    Node N = new Node(patrolingPoints[i].transform.position, true);

                  
                    if (visited.Contains(Tuple.Create(N.pos, true)))
                        continue;


                  
                    N.H = Vector3.Distance(current.position, patrolingPoints[i].transform.position); // in case i forget what the intital issue was
                  
                    nextNode.Add(N);
                    hasFoundClosestPoint = !hasFoundClosestPoint;
                }
            }
         
            Node closest = findMinValue(nextNode);
            Agent agentCurrentPos = FindThePath(path.method,current, closest.pos);
            Transform agentTransform = agentCurrentPos.agentPos;
            bool hasReachedClosest = false;
            bool hasReached = false;



           

            while (!hasReachedClosest)
            {
                if (agentTransform.transform.position == closest.pos) hasReachedClosest = true;

                Node nextPoint = agentCurrentPos.getPath.Dequeue();


                var nodeState = Tuple.Create(closest.pos, true);

                while (!hasReached)
                {
                    if (path.method != old_method)
                    {
                        old_method = path.method;
                        current.transform.position = old_pos;
                        hasFoundClosestPoint = false;
                        hasReachedClosest = true;
                        visited.Clear();
                        break;


                    }


                    redneredNodePath(agentCurrentPos.getPath);
                    agentTransform.transform.position = Vector3.MoveTowards(agentTransform.transform.position, nextPoint.pos, +Time.deltaTime);
                    if (agentTransform.transform.position == nextPoint.pos) hasReached = true;
                    yield return hasReached;
                }
                hasReached = false;
                //once startcoroutine completed its execution, it might fail to restart again



                Node getCurrentNode = Grid.getNodeposition(agentTransform.transform.position);
                Node getClosestNode = Grid.getNodeposition(closest.pos);

                if (getCurrentNode.Equals(getClosestNode))
                {

                    hasFoundClosestPoint = false;
                    if (!visited.Contains(nodeState)) visited.Add(nodeState);

                    break;
                }
                yield return null;

            }
            yield return null;
        }
    }

    private bool not_set(int index)
    {
        if (index > 0) return false;

        return true;
    }
   


    private IEnumerator followPath( Transform Start, Vector3 Final)
    {
        
        bool hasReached = false;

        Agent agent = FindThePath(path.method, start, Final);
        Queue<Vector3> queueVector = new Queue<Vector3>();
        Transform agentTransform = agent.agentPos;
        
   
        while (agent.getPath.Count != 0)
        {

            Node target = agent.getPath.Dequeue();
            while (!hasReached)
            {

                
                Vector3 prevPos = StoredPosition(queueVector);
                if (path.method != old_method)
                {
                    Start.transform.position = old_pos;
                    old_method = path.method;
                }
                if (!hasChangedPos(prevPos, Player.transform.position) || Start.transform.position == old_pos)
                {
                    agent = FindThePath(path.method, Start, Player.transform.position);
                    hasReached = true;
                }
             

                redneredNodePath(agent.getPath);
    
                float time = +Time.deltaTime;
                agentTransform.transform.position = Vector3.MoveTowards(agentTransform.transform.position, target.pos, time);

                if (agentTransform.transform.position == target.pos) hasReached = true;
                yield return hasReached;
            }
            if (hasReached) hasReached = false;



            yield return null;
        }
        
    }
    #endregion

    private void Update()
    {
        if (path.method == 1) print("method===> " + path.method);
        if (path.method == 2) print("method===> " + path.method);
    }


}

