using System;
using System.Collections.Generic;

public class Heap<T> where T : IComparable<T> {
    List<T> items = new List<T>();

    public bool hasItems {
        get { return items.Count > 0; }
    }

    public T root {
        get { return items[0]; }
    }

    public void Insert(T item) {
        items.Add(item);

        int i = items.Count - 1;

        while(i > 0) {
            int parent = (i - 1)/2;
            if (items[i].CompareTo(items[parent]) < 0) {
                T temp = items[i];
                items[i] = items[parent];
                items[parent] = temp;
                i = parent;
            } else {
                break;
            }
        }
    }

    public void DeleteRoot() {
        int i = items.Count - 1;

        items[0] = items[i];
        items.RemoveAt(i);
        i = 0;

        while (true) {
            int left = 2*i + 1;
            int right = 2*i + 2;
            int smallest = i;

            if (left < items.Count && items[left].CompareTo(items[smallest]) < 0) {
                smallest = left;
            }

            if (right < items.Count && items[right].CompareTo(items[smallest]) < 0) {
                smallest = right;
            }

            if (smallest != i) {
                T temp = items[smallest];
                items[smallest] = items[i];
                items[i] = temp;
                i = smallest;
            } else {
                break;
            }
        }
    }

    public T PopRoot() {
        T result = this.root;
        DeleteRoot();
        return result;
    }
}