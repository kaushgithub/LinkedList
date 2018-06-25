
namespace SetComprehension
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class LinkedList<T>
    {
        private readonly Node head;

        public LinkedList()
        {
            this.head = null;
        }

        public LinkedList(T one, params T[] others)
        {
            if (others == null)
            {
                throw new ArgumentNullException("others");
            }
            Node previousNode = BuildList(others);
            this.head = new Node(previousNode) { Data = one };
        }

        public virtual int Count
        {
            get
            {
                int result = 0;
                foreach(T data in Items)
                {
                    result++;
                }
                return result;
            }
        }

        private static Node BuildList(T[] items)
        {
            Node newNode = null,
                previousNode = null;
    
            // go backwards to build the linked list
            for (int i = items.Length - 1; i >= 0; i--)
            {
                newNode = new Node(previousNode) { Data = items[i] };
                previousNode = newNode;
            }
            return previousNode;
        }

        public LinkedList<T> Union(LinkedList<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            return new EnumerableLinkedList(new UnionEnumerable(this, list));
        }

        public LinkedList<T> Intersect(LinkedList<T> list, Func<T, T, bool> comparer)
        {
            throw new NotImplementedException();
        }

        public LinkedList<X> Join<S, X>(LinkedList<S> list, Func<T, S, bool> comparer, Func<T, S, X> map)
        {
            throw new NotImplementedException();
        }
        
        public LinkedList<T> OrderBy(IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> Items
        {
            get
            {
                Node node = head;
                while (node != null)
                {
                    yield return node.Data;
                    node = node.Next;
                }
            }
        }

        private class EnumerableLinkedList : LinkedList<T>
        {
            private IEnumerable<T> enumerable;
            
            public EnumerableLinkedList(IEnumerable<T> enumerable)
            {
                this.enumerable = enumerable;
            }

            public override IEnumerable<T> Items
            {
                get
                {
                    return enumerable;
                }
            }
        }

        private class UnionEnumerable : IEnumerable<T>
        {
            UnionEnumerator enumerator;

            public UnionEnumerable(LinkedList<T> first, LinkedList<T> second)
            {
                this.enumerator = new UnionEnumerator(first, second);
            }

            public IEnumerator<T> GetEnumerator()
            {
                return this.enumerator;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.enumerator;
            }

            private class UnionEnumerator : IEnumerator<T>
            {
                private readonly LinkedList<T> first;
                private readonly LinkedList<T> second;
                private Node next;
                private bool? onFirst;

                public UnionEnumerator(LinkedList<T> first, LinkedList<T> second) 
                {
                    this.first = first;
                    this.second = second;
                }

                public T Current
                {
                    get 
                    { 
                        return this.next.Data; 
                    }
                }

                public void Dispose()
                {
                }

                object IEnumerator.Current
                {
                    get { return this.next.Data; }
                }

                public bool MoveNext()
                {
                    if (!this.onFirst.HasValue)
                    {
                        this.onFirst = true;
                        this.next = this.first.head;
                    }
                    else if (this.next != null)
                    {
                        this.next = this.next.Next;
                    }

                    if (this.next == null && this.onFirst.Value)
                    {
                        this.onFirst = false;
                        this.next = this.second.head;
                    }
                    return this.next != null;
                }

                public void Reset()
                {
                    this.next = this.first.head;
                    this.onFirst = true;
                }
            }
        }

        private class Node
        {
            public Node()
            {
                this.Next = null;
            }

            public Node(Node next)
            {
                this.Next = next;
            }

            public T Data { get; set; }
            public Node Next { get; private set; }
        }
    }
}
