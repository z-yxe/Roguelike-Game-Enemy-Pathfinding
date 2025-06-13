using System;
using System.Collections.Generic;

// For A* pathfinding optimization
public class BinaryHeap<T> where T : IComparable<T>
{
    private List<T> items;
    private readonly int maxSize;
    public int Count => items.Count;

    // Initializes the heap
    public BinaryHeap(int maxSize)
    {
        this.maxSize = maxSize;
        items = new List<T>(maxSize);
    }

    // Adds node to heap
    public void Add(T item)
    {
        items.Add(item);
        SortUp(items.Count - 1);
    }

    // Removes lowest-cost node
    public T RemoveFirst()
    {
        if (items.Count <= 0)
            throw new InvalidOperationException("Heap is empty");

        T firstItem = items[0];
        items[0] = items[items.Count - 1];
        items.RemoveAt(items.Count - 1);

        if (items.Count > 0)
            SortDown(0);

        return firstItem;
    }

    // Updates node priority
    public void UpdateItem(T item)
    {
        int index = items.IndexOf(item);
        if (index != -1)
        {
            SortUp(index);
            SortDown(index);
        }
    }

    // Empties the heap
    public void Clear()
    {
        items.Clear();
    }

    // Sorts node upwards
    private void SortUp(int index)
    {
        while (true)
        {
            int parentIndex = (index - 1) / 2;

            if (parentIndex < 0 || items[parentIndex].CompareTo(items[index]) <= 0)
                break;

            SwapItems(index, parentIndex);
            index = parentIndex;
        }
    }

    // Sorts node downwards
    private void SortDown(int index)
    {
        while (true)
        {
            int minIndex = index;
            int leftChild = index * 2 + 1;
            int rightChild = index * 2 + 2;

            if (leftChild < items.Count && items[leftChild].CompareTo(items[minIndex]) < 0)
                minIndex = leftChild;

            if (rightChild < items.Count && items[rightChild].CompareTo(items[minIndex]) < 0)
                minIndex = rightChild;

            if (minIndex == index)
                break;

            SwapItems(index, minIndex);
            index = minIndex;
        }
    }

    // Swaps two nodes
    private void SwapItems(int indexA, int indexB)
    {
        T temp = items[indexA];
        items[indexA] = items[indexB];
        items[indexB] = temp;
    }
}
