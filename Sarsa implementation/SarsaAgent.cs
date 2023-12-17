using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sarsa_implementation
{

    public enum Action
    {
        Up,
        Down,
        Left,
        Right
    }

    internal class SarsaAgent
    {
        private Grid grid;
        private Dictionary<(int, int , Action), double> qValues;
        private double alpha; // Learning rate
        private double gamma; // Discount factor
        private Random random;
        private State state;

        public SarsaAgent(Grid grid, double alpha, double gamma)
        {
            this.grid = grid;
            this.alpha = alpha;
            this.gamma = gamma;
            qValues = new Dictionary<(int, int, Action), double>();
            random = new Random();
            this.state = new State(grid.xbegining, grid.ybegining);
            Array actions = Enum.GetValues(typeof(Action));
        }

        private Action ChooseAction(State state, double epsilon)
        {
            // With probability epsilon, choose a random action (exploration)
            if (random.NextDouble() < epsilon)
            {
                Array actions = Enum.GetValues(typeof(Action));
                return (Action)actions.GetValue(random.Next(actions.Length));
            }
            else
            {
                // Otherwise, choose the best action based on current Q-values (exploitation)
                return BestActionForState(state);
            }
        }

        private Action BestActionForState(State state)
        {
            Action bestAction = Action.Up;
            double maxQValue = double.MinValue;

            foreach (Action action in Enum.GetValues(typeof(Action)))
            {
                if (!IsPossible(action))
                {
                    continue; // Skip actions that are not possible
                }

                double qValue = GetQValue(state, action);
                if (qValue > maxQValue)
                {
                    maxQValue = qValue;
                    bestAction = action;
                }
            }

            return bestAction;
        }


        private double GetQValue(State state, Action action)
        {
            if (!qValues.ContainsKey((state.X, state.Y , action)))
                qValues[(state.X, state.Y , action)] = 0; // Initialize unseen Q-values to a small positive value to encourage exploration

            return qValues[(state.X, state.Y, action)];
        }

        private void UpdateQValue(State state, Action action, double reward, State nextState, Action nextAction)
        {
            double oldQValue = GetQValue(state, action);
            double nextQValue = GetQValue(nextState, nextAction);

            // SARSA update formula
            double newQValue = oldQValue + alpha * (reward + gamma * nextQValue - oldQValue);
            qValues[(state.X, state.Y, action)] = newQValue;
        }

        private bool IsPossible(Action action)
        {
            if (action == Action.Left)
            {
                if (this.state.Y == 0 || this.grid.Content[this.state.X,this.state.Y-1].value <= -100.0)
                    return false;
                return true;
            }
            else if (action == Action.Up) 
            {
                if (this.state.X == 0 || this.grid.Content[this.state.X-1, this.state.Y].value <= -100.0)
                    return false;
                return true;
            }
            else if (action == Action.Right)
            {
                if (this.state.Y == this.grid.ncol-1 || this.grid.Content[this.state.X, this.state.Y+1].value <= -100.0)
                    return false;
                return true;
            }
            else if (action == Action.Down)
            {
                if (this.state.X == this.grid.nrow - 1 || this.grid.Content[this.state.X + 1, this.state.Y].value <= -100.0)
                    return false;
                return true;
            }
            return false;

        }
        
        private State NextState(Action action)
        {
            if(IsPossible(action))
            {
                if (action == Action.Up)
                {
                    return new State(this.state.X - 1, this.state.Y);
                }
                else if (action == Action.Down)
                {
                    return new State(this.state.X + 1, this.state.Y);
                }
                else if (action == Action.Left)
                {
                    return new State(this.state.X, this.state.Y - 1);
                }
                else if (action == Action.Right)
                {
                    return new State(this.state.X, this.state.Y + 1);
                }
            }
            return this.state;
        }

        public void Train(int episodes, double initialEpsilon, double minEpsilon, double epsilonDecay)
        {
            double epsilon = initialEpsilon;

            for (int episode = 0; episode < episodes; episode++)
            {
                State currentState = new State(this.grid.xbegining, this.grid.ybegining);
                this.state = currentState;
                Action currentAction = ChooseAction(currentState, epsilon);
                



                //grid.PrintGrid(state);
        
                while (!grid.IsTerminalState(currentState))
                {
                    // Execute the action and observe the next state and reward
                    State nextState = NextState(currentAction);
                    double reward = -1.0+grid.Content[state.X,state.Y].value;


                    //grid.PrintGrid(nextState);

                    Action nextAction = ChooseAction(nextState,epsilon);
                    

                    UpdateQValue(currentState, currentAction, reward, nextState, nextAction);

                    currentState = nextState;
                    this.state = currentState;
                    currentAction = nextAction;
                    

                    epsilon = Math.Max(minEpsilon, epsilon * epsilonDecay);
                }
            }
        }

        public void SaveAgent(string filePath)
        {
            var entries = qValues.Select(kvp => new QValueEntry(kvp.Key.Item1, kvp.Key.Item2,kvp.Key.Item3, kvp.Value)).ToList();
            var json = JsonConvert.SerializeObject(entries, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void LoadAgent(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var entries = JsonConvert.DeserializeObject<List<QValueEntry>>(json);
                qValues = entries.ToDictionary(entry => (entry.X, entry.Y, entry.Action), entry => entry.QValue);
            }
            else
            {
                Console.WriteLine("No saved agent found at the specified path.");
            }
        }

        public void TestAgent(int testEpisodes)
        {
            
            Grid testGrid = grid;
            int successCount = 0;
            double totalReward = 0.0;

            for (int episode = 0; episode < testEpisodes; episode++)
            {
                State currentState = new State(testGrid.xbegining, testGrid.ybegining);
                double episodeReward = 0.0;
                state= currentState;
                Console.WriteLine("###############################");
                grid.PrintGrid(currentState);

                while (!testGrid.IsTerminalState(currentState))
                {
                    Action action = BestActionForState(currentState); // Choose the best action based on learned Q-values
                    State nextState = NextState(action); // Calculate the next state based on the action
                    state= nextState;
                    double reward = -1.0 + grid.Content[state.X, state.Y].value; // Get the reward for the current action

                    grid.PrintGrid(nextState);

                    episodeReward += reward;
                    currentState = nextState;

                    if (testGrid.IsTerminalState(currentState)) // Check if the goal state is reached
                    {
                        successCount++;
                        break;
                    }
                }

                totalReward += episodeReward;
            }

            double successRate = (double)successCount / testEpisodes;
            double averageReward = totalReward / testEpisodes;

            Console.WriteLine($"Test Results: Success Rate = {successRate * 100:F2}%, Average Reward = {averageReward:F2}");
        }
    }

}
