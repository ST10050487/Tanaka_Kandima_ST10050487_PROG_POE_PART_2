using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG_POE_PART_2.Classes
{
    public class EventGraph
    {
        private Dictionary<Event, List<Event>> adjacencyList = new Dictionary<Event, List<Event>>();

        public void AddEdge(Event event1, Event event2)
        {
            if (!adjacencyList.ContainsKey(event1))
                adjacencyList[event1] = new List<Event>();
            if (!adjacencyList.ContainsKey(event2))
                adjacencyList[event2] = new List<Event>();

            adjacencyList[event1].Add(event2);
            adjacencyList[event2].Add(event1);
        }

        public List<Event> GetNeighbors(Event ev)
        {
            if (adjacencyList.ContainsKey(ev))
                return adjacencyList[ev];
            return new List<Event>();
        }

        public List<Event> BFS(Event startEvent)
        {
            List<Event> visited = new List<Event>();
            Queue<Event> queue = new Queue<Event>();
            queue.Enqueue(startEvent);

            while (queue.Count > 0)
            {
                Event current = queue.Dequeue();
                if (!visited.Contains(current))
                {
                    visited.Add(current);
                    foreach (var neighbor in GetNeighbors(current))
                    {
                        if (!visited.Contains(neighbor))
                            queue.Enqueue(neighbor);
                    }
                }
            }
            return visited;
        }
    }
}
