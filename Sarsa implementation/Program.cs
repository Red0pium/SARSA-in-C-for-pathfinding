using Sarsa_implementation;
using System.Collections.Generic;
using System.Runtime.Serialization;

internal class Program
{
    public enum Action
    {
        Up,
        Down,
        Left,
        Right
    }
    private static void Main(string[] args)
    {
        //Obstacle o1 = new Obstacle("X", 1, 1, -100.0);
        //Obstacle o2 = new Obstacle("X", 3, 1, -100.0);
        //Obstacle o3 = new Obstacle("O", 0, 2, 10.0);
        //Obstacle o4 = new Obstacle("O", 0, 1, 10.0);
        //
        //List<Obstacle> obstacles = new List<Obstacle>();
        //obstacles.Add(o1);
        //obstacles.Add(o2);
        //obstacles.Add(o3);
        //obstacles.Add(o4);
        //
        //
        //Grid test = new Grid(5, 7, 0, 0, 3, 3, obstacles);
        
        List < Obstacle > obstacles = new List<Obstacle>();

        // Define a couple of obstacles
        obstacles.Add(new Obstacle("X", 2, 2, -100.0)); 
        obstacles.Add(new Obstacle("X", 1, 3, -100.0));
        obstacles.Add(new Obstacle("X", 0, 2, -100.0));

        // Create the grid
        int gridRows = 5;
        int gridColumns = 7;
        int startX = 0; // Starting position X
        int startY = 0; // Starting position Y
        int goalX = 0; // Goal position X
        int goalY = 3; // Goal position Y

        Grid testGrid = new Grid(gridRows, gridColumns, startX, startY, goalX, goalY, obstacles);


        SarsaAgent Agent = new SarsaAgent(testGrid, 0.5,0.8);

        State state1 = new State(1, 1);
        State state2 = new State(1, 1);
         

        Dictionary<(State, Action), double> qValues = new Dictionary<(State, Action), double>();
        qValues[(state1, Action.Up)] = 3.0;

        Console.Write(state1.Equals(state2));



        //Agent.Train(100,1.0,0.1,0.3);


        //Agent.SaveAgent("C:\\Users\\Matbr\\Desktop\\Mathys\\Esilv 2023-2024\\AI Algorithm\\Forum\\Agent\\Test2.json");
        Agent.LoadAgent("C:\\Users\\Matbr\\Desktop\\Mathys\\Esilv 2023-2024\\AI Algorithm\\Forum\\Agent\\Test2.json");
        Agent.Train(100, 1.0, 0.1, 0.3);
        Agent.TestAgent(5);
        Agent.SaveAgent("C:\\Users\\Matbr\\Desktop\\Mathys\\Esilv 2023-2024\\AI Algorithm\\Forum\\Agent\\AgentFort.json");
        //SarsaAgent Agent2 = new SarsaAgent(testGrid, 0.5, 0.8);
        //Agent2.LoadAgent("C:\\Users\\Matbr\\Desktop\\Mathys\\Esilv 2023-2024\\AI Algorithm\\Forum\\Agent\\Test2.json");
        //Agent2.TestAgent(5);

    }
}