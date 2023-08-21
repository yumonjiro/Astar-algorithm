using System;
class PriorityQueue<T, U> where U: IComparable<U>
{

    public readonly (T, U)[] Array;
    public int Count = 0;
    public PriorityQueue(int capacity)
    {
        Array = new (T,U)[capacity];
    }

    public void Enqueue(T item, U priority)
    {
        Count += 1;
        Array[Count-1] = (item, priority);
        int itemIndex = Count-1;

        while(true)
        {
            
            int parent = (itemIndex-1)/2;
            if(Array[parent].Item2.CompareTo(priority) < 0)
            {
                // Swap two values
                this.Swap(parent, itemIndex);
                itemIndex = parent;
            }
            else
            {
                break;
            }
        }


    }
    private void Swap(int a, int b)
    {
        (this.Array[b], this.Array[a]) = (this.Array[a], this.Array[b]);
    }

    // return item with greatest priority(biggest)
    public T Dequeue()
    {
        this.Swap(0, this.Count -1);
        this.Count -= 1;
        
        this.DownHeap();
        
        //最後は末尾＋１　＝最大値になっている
        // return best item
        return this.Array[this.Count].Item1;
    }

    private void DownHeap()
    {
        var parent = 0;
        while (true)
        {
            var child = 2 * parent + 1;
            if (child > this.Count - 1) break;

            if (child < this.Count - 1 && (this.Array[child].Item2.CompareTo(this.Array[child + 1].Item2)) < 0)
            {
                // 左より右の子のほうが大きい値の場合、右を入れ替え対象にする
                child += 1;
            }

            if (this.Array[parent].Item2.CompareTo(this.Array[child].Item2) < 0)
            {
                this.Swap(parent, child);
                parent = child;
            }
            else
            {
                break;
            }
        }
    }
}