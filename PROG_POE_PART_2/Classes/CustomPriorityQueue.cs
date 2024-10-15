using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG_POE_PART_2.Classes
{
    public class CustomPriorityQueue<T>
    {
        private SortedDictionary<int, Queue<T>> _dictionary = new SortedDictionary<int, Queue<T>>();

        public void Enqueue(T item, int priority)
        {
            if (!_dictionary.ContainsKey(priority))
            {
                _dictionary[priority] = new Queue<T>();
            }
            _dictionary[priority].Enqueue(item);
        }

        public T Dequeue()
        {
            if (_dictionary.Count == 0)
            {
                throw new InvalidOperationException("Queue is empty.");
            }

            var firstPair = _dictionary.First();
            var item = firstPair.Value.Dequeue();

            // Remove the entry if the queue is empty
            if (firstPair.Value.Count == 0)
            {
                _dictionary.Remove(firstPair.Key);
            }

            return item;
        }

        public bool IsEmpty => !_dictionary.Any();
    }

}
