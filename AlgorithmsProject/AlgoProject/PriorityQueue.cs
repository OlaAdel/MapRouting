using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoProject
{
    public class PriorityQueue<T> where T : IComparable<T>//IComparable give the ability to compare 2 object of PriorityQueue
    {
        private List<T> data;
        public PriorityQueue()
        {
            this.data = new List<T>();
        }
        public void Enqueue(T item)
        {
            data.Add(item);
            int child_index = data.Count - 1; // start at end
            while (child_index > 0)
            {
                int parent_index = (child_index - 1) / 2;
                if (data[child_index].CompareTo(data[parent_index]) >= 0) //equal (data[child_index] - data[parent_index])>=0 
                {
                    break; // child item is larger than (or equal) parent so we're done
                }
                T temp = data[child_index];
                data[child_index] = data[parent_index];
                data[parent_index] = temp;
                child_index = parent_index;
            }
        }
        public T Dequeue()
        {
            // assume priority queue is NOT EMPTY
            int last_index = data.Count - 1;
            T frontItem = data[0];
            data[0] = data[last_index];
            data.RemoveAt(last_index);

            --last_index;
            int parent_index = 0;
            while (true)
            {
                int child_index = parent_index * 2 + 1; // left child index of parent
                if (child_index > last_index)
                {
                    break;  //no children
                }
                int right_child = child_index + 1;
                if (right_child <= last_index && data[right_child].CompareTo(data[child_index]) < 0) // if there is a right_child (child_index + 1) and it's smaller than left child use the right_child instead
                {
                    child_index = right_child;
                }
                if (data[parent_index].CompareTo(data[child_index]) <= 0)// equal (data[parent_index]-data[child_index])<=0
                {
                    break; // parent is smaller than (or equal) to smallest child so done
                }
                T temp = data[parent_index];
                data[parent_index] = data[child_index];
                data[child_index] = temp; // swap parent and child
                parent_index = child_index;
            }
            return frontItem;
        }
        public T Peek()
        {
            T frontItem = data[0];
            return frontItem;
        }
        public int Count()
        {
            return data.Count;
        }
        public bool IsConsistent()
        {
            // is the heap property true for all data?
            if (data.Count == 0)
            {
                return true;
            }
            int last_index = data.Count - 1;
            for (int parent_index = 0; parent_index < data.Count; ++parent_index)
            {
                int left_child_index = 2 * parent_index + 1;
                int right_child_index = 2 * parent_index + 2;
                if (left_child_index <= last_index && data[parent_index].CompareTo(data[left_child_index]) > 0)
                {
                    return false;  // if left child exists and it's greater than parent then false.
                }
                if (right_child_index <= last_index && data[parent_index].CompareTo(data[right_child_index]) > 0)
                {
                    return false; // check the right child too.
                }
            }
            return true;
        }

    }
}