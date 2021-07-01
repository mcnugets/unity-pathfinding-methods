using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;




public enum pf
{

    E,
    A_star,
    Dijkstra,
    Greedy,
    BFS
}
public class pathfindingController : pathfinding

{
    
    public Button[] butt;
    public GameObject panel;
    private bool setPanel;
    
    int index;
    public int method
    {
        set { index = value; }
        get { return index; }
        
    }
  
  
    
    // The topics that has been covered:
    // delegeate method that takes fucntions as a parameter Actio,UnityAction and etc
    // lambda expression ()=>
    // Unity Action and Unity Event
    void Start()
    {
       

        
        setPanel = false;

  
    }
  
    public void openPanel()
    {

        setPanel = !setPanel;
        panel.SetActive(setPanel);
    }
    
    public void switcher()
    {
        
        for (int i = 0; i < butt.Length; i++)
        {

            func(i);
        }

    }
    private void func(int index) 
    {
        butt[index].onClick.AddListener( ()=> method = index+1);

       
    }
   
 
    
    
    public void selectPathfinding(int index, Vector3 start, Vector3 target)
    {


        switch ((pf)index)
        {
            case pf.E:
                print("DEFAULT");
                break;
            case pf.A_star:
                print("a star");
                A_Star(start, target);
                break;
            case pf.Dijkstra:
                print("Dijkstra");
                Dijkstra(start, target);
                break;
            case pf.Greedy:
                print("Greedy");
                Greedy(start, target);
                break;
            case pf.BFS:
                print(" BreadthFirstSearch");
                BreadthFirstSearch(start, target);
                break;
            default:
                print("CHOOSE PATHFINDING METHOD");
                break;
        }


    }
   




}
