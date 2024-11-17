using System;
using System.Collections.Generic;

namespace PROG_POE_PART_2.Classes
{
    public class EventHeap
    {
        private List<Event> heap = new List<Event>();

        public int Count => heap.Count; // Add this property

        public void Insert(Event ev)
        {
            heap.Add(ev);
            HeapifyUp(heap.Count - 1);
        }

        public Event ExtractMax()
        {
            if (heap.Count == 0)
                throw new InvalidOperationException("Heap is empty");

            Event max = heap[0];
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            HeapifyDown(0);
            return max;
        }

        private void HeapifyUp(int index)
        {
            while (index > 0 && heap[index].Priority > heap[Parent(index)].Priority)
            {
                Swap(index, Parent(index));
                index = Parent(index);
            }
        }

        private void HeapifyDown(int index)
        {
            int maxIndex = index;
            int left = LeftChild(index);
            int right = RightChild(index);

            if (left < heap.Count && heap[left].Priority > heap[maxIndex].Priority)
                maxIndex = left;
            if (right < heap.Count && heap[right].Priority > heap[maxIndex].Priority)
                maxIndex = right;

            if (index != maxIndex)
            {
                Swap(index, maxIndex);
                HeapifyDown(maxIndex);
            }
        }

        private int Parent(int index) => (index - 1) / 2;
        private int LeftChild(int index) => 2 * index + 1;
        private int RightChild(int index) => 2 * index + 2;

        private void Swap(int index1, int index2)
        {
            Event temp = heap[index1];
            heap[index1] = heap[index2];
            heap[index2] = temp;
        }
    }
}
