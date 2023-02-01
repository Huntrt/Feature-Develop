using System;

namespace Pathfinding.AStar
{

public class Heap<T> where T: IHeapItem<T>
{
	T[] items;
	int curItemCount;

	public Heap(int maxSize)
	{
		//Reset items slot to given size
		Clear(); items = new T[maxSize];
	}

	public void Add(T item)
	{
		//Set given item index as current count
		item.HeapIndex = curItemCount;
		items[curItemCount] = item;
		SortUp(item);
		curItemCount++;
	}

	void SortUp(T item)
	{
		//Get index of parent
		int parentIndex = (item.HeapIndex - 1) / 2;

		while (true)
		{
			T parentItem = items[parentIndex];
			//Swap the given item if with parent if it has higher priority than it parent
			if(item.CompareTo(parentItem) > 0) Swap(item, parentItem); else break;
			//Reset parent index
			parentIndex = (item.HeapIndex - 1) / 2;
		}
	}

	void SortDown(T item)
	{
		while (true)
		{
			//Get the left and right child of given item's index
			int leftChild = item.HeapIndex * 2 + 1;
			int rightChild = item.HeapIndex * 2 + 2;

			int swap = 0;
			//If the left child are smaller then count
			if(leftChild < curItemCount)
			{
				//Swap with the left child
				swap = leftChild;
				//But if the right child are smaller then count
				if(rightChild < curItemCount)
				{
					//Compare if left child has lower priority than right child
					if(items[leftChild].CompareTo(items[rightChild]) < 0)
					{
						//If it does than swap with right child
						swap = rightChild;
					}
				}
				//If the swapped child has lower priority than given item
				if(item.CompareTo(items[swap]) < 0)
				{
					//Swap given item with it child
					Swap(item, items[swap]);
				}
				//If parent given item has lower priority
				else return;
			}
			//If has children
			else return;
		}
	}
	
	public T RemoveFirst()
	{
		//Save the first item
		T first = items[0]; curItemCount--;
		//Push the last item to be first
		items[0] = items[curItemCount];
		//Remove the first index
		items[0].HeapIndex = 0;
		//Sort the first
		SortDown(items[0]);
		//Return the saved first item
		return first;
	}

	void Swap(T a, T b)
	{
		//Switch item a and b inside items list
		items[a.HeapIndex] = b; items[b.HeapIndex] = a;
		//Save the item a index
		int aIndex = a.HeapIndex;
		//Switch item a index to b
		a.HeapIndex = b.HeapIndex;
		//Switch item b index to saved a index
		b.HeapIndex = aIndex;
	}

	//Re-sort need to update an item
	public void UpdateItems(T item) {SortUp(item);}

	public int Count {get {return curItemCount;}}

	public bool Contains(T item) 
	{
		//If given item index are in count
		if(item.HeapIndex < curItemCount)
		{
			//Return result of item list contain given item
			return Equals(items[item.HeapIndex], item);
		}
		//Return false if not in count
		else {return false;}
	}

	//! Clear the heap (testing)
	public void Clear() {curItemCount = 0;}
}

public interface IHeapItem<T> : IComparable<T>
{
	int HeapIndex {get; set;}
}

}//? namepsace