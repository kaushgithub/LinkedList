# LinkedList
Q1) Understand and articulate what the code does, when you would recommend using it versus not
using it
1) It is a generic singly LinkedList class which provides interfaces for creating a sequence of elements
with a link to next element and iterate through the elements of the LinkedList. It provides four more
interfaces for iterating a singly LinkedList without using additional memory which is
a) Union of two singly LinkedLists
b) Intersection of node data of two singly LinkedLists.
d) Order the nodes based on some comparer.
4) Join two lists based on some condition.

Q2) When I would use the above data structure?
1) This data structure can be used to store elements sequentially and it supports enumerator to iterate
through elements. As mentioned above, the strength of this data structure lies in memory utilization
when iterating through the list after performing union, intersection etc. of two lists. With little more
space for storing the links, we are just playing around with links while iterating through elements.
2) We can extend this data structure to behave like C# LinkedList class and make insertion, deletion
operations constant time and also remove nodes, reinsert nodes, either in the same list or another list,
which results in no additional memory created on heap.

Q3) When would I not use the above data structure?
1) The above things could be achieved through C# list class. But it has its disadvantages like resizing
array underneath as we append more elements. If resizing etc. is not of big concern (use of extra
memory, performance), I would not use the LinkedList class above as we have additional overhead of
storing the links and traversing the links.
2) When I want a completely new list of all the elements after operations like union, intersection and
not just iterating through the elements.
Q4) Things I like about the design
1) The LinkedList class is generic - Code reuse.
2) The node class is encapsulated as private class for the LinkedList class which is very appropriate in this
scenario. But I have changed it to be in its own class because of some refactoring which is explained in
the refactoring the code section below.

3) Making the Items property virtual - In future, if I decide to inherit this class and create a doubly linked
list or something, I would want to change the implementation of this property. Also, this design already
does override this property.
Q5) Things I do not like about the design
1) The design uses a lot of inner classes. I am fine with making data members like the node class private
classes but having so many inner classes seems like a poor design. I will list the disadvantages below of
having so many inner/private classes:
a) The UnionEnumerator class is made private and tightly coupled with the LinkedList class. I strongly
think this private class can be taken out of the linked list class and be made more generic for sequential
collections and just reuse it for this LinkedList class. There is a lot of logic sitting inside the
UnionEnumerator class which makes me think, move it out. Also, I have explained more about this
below in the refactor section.
b) Unit testing cannot be done on these inner classes. The only way to test the MoveNext() of
UnionEnumerator class is through public LinkedList&lt;T&gt; Union(LinkedList&lt;T&gt; list) which is not ok. There is
so much logic in the UnionEnumerator class which I think should be tested independently.
c) Also, sometimes it is harder to read the code with so many inner classes.
2) Generally I would keep my constructors simple. Just assigning the parameters passed. Here the
constructor does lot more with building the list. I am ok with it in this scenario. But keeping it simple is
one thing I would recommend. May be build the list when we do some operation on the class or have an
initialize method and build the list after object construction.
3) public virtual int Count method in LinkedList class- having this virtual is not necessary I think. I am not
sure if I would ever try to override this method. But it is ok to have it virtual except for a minor
performance penalty.
4) Also, I thought about the private BuildList method being static. I do not think it is necessary here but
not hurting anything either.

Q6) Refactoring the code for my implementation
I have implemented the intersect method. The first thing that struck me was that I could reuse the code
in UnionEnumerator. So I created the following classes (Please find attached the new files with this
email):
a) Created an abstract class BaseEnumerator.cs - Moved most of the logic from private class
UnionEnumerator. Made the movenext method abstract.
b) Created specialized classes UnionEnumerator and IntersectEnumerator inheriting the above abstract
class and implement the movenext which is the crux of functions union, intersect etc. In a similar fashion
we can implement orderby and join enumerators.
c) EnumeratorFactory.cs - Created a static factory class EnumeratorFactory which returns the correct
specialized enumerator class based on the operation &quot;union&quot;, &quot;intersection&quot; etc.

d) Also, we can separate out UnionEnumerable class and IntersectEnumerable classes and reuse some of
the code. This is not implemented but can be done in a very similar fashion as in steps a and b above
e) As talked before, we could make the BaseEnumerator class be more generic (for all sequential
collections) and not tie it to the LinkedList class. This is not implemented either.
f) I have commented the code in Linkedlist.cs after refactoring with comments like //Kaushik -
Refactored into its own class to facilitate the new BaseEnumerator class
g) Added new tests for Intersect operation and now all the Intersect operation tests pass after my
implementation.
e) Not implemented but can add some tests for separated Enumerator classes.
