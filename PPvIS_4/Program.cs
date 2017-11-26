using System;
using System.Collections.Generic;
using System.Linq;

namespace PPvIS_4
{
    class Vertex<T>
    {
        public T vertex; // require ToString() method in T class, to use Console.WriteLine()
        public List<Vertex<T>> neighbour_vertexes;
        public Vertex()
        {
            neighbour_vertexes = new List<Vertex<T>>();
        }
        public Vertex(T vertex)
        {
            this.vertex = vertex;
            neighbour_vertexes = new List<Vertex<T>>(); 
        }
        public Vertex<T> Clone() // instead of using '='
        {
            var new_vertex = new Vertex<T>();
            new_vertex.vertex = this.vertex;
            new_vertex.neighbour_vertexes = new List<Vertex<T>>();
            return new_vertex;
        }
    }

    class Graph<T>
    {
        private List< Vertex<T> > vertex_set;
        public List< Vertex<T> > Vertex_set
        {
            get
            {
                return vertex_set;
            }
            set
            {
                vertex_set = value;
            }
        }

        public Graph ( List <Vertex<T>> vertex_set ){
            this.vertex_set = new List<Vertex<T>>();
            this.vertex_set = vertex_set;
        }
        public Graph( Graph<T> graph)
        {
            this.vertex_set = new List<Vertex<T>>();
            vertex_set = graph.vertex_set;
        }
        public Graph()
        {
            vertex_set = new List<Vertex<T>>();
        }

        public bool is_empty<A>() // determines whether the collection is null or contains no elements 
        {
            if (vertex_set == null)
            {
                return true;
            }
            var collection = vertex_set as ICollection<A>;
            if (collection != null)
            {
                return collection.Count < 1;
            }
            return !vertex_set.Any();
        }
        
        public void clear()
        {
            vertex_set.Clear();
        }

        public void add_vertex(Vertex<T> vertex)
        {
            vertex_set.Add(vertex);
        }

        public void delete_vertex(Vertex<T> vertex)
        {
            for (int i = 0; i < vertex_set.Count(); i++)
            {
                if (vertex_set[i].neighbour_vertexes.Contains(vertex))
                {
                    vertex_set[i].neighbour_vertexes.Remove(vertex);
                }
            }
            vertex_set.Remove(vertex);
        }

        public bool contain_vertex(Vertex<T> vertex)
        {
            return vertex_set.Contains(vertex);
        }

        public int count_vertexes()
        {
            return vertex_set.Count();
        }

        public List<Vertex<T>> get_neighbour_vertexes(Vertex<T> vertex)
        {
            return vertex.neighbour_vertexes;
        }

        public int count_neighbour_vertexes(Vertex<T> vertex)
        {
            try { return vertex.neighbour_vertexes.Count(); }
            catch (Exception e) { return 0; }            
        }

        public bool if_vertexes_are_neighboured(Vertex<T> vertex_1_temp, Vertex<T> vertex_2_temp)
        {
            Vertex<T> vertex_1 = vertex_1_temp.Clone(); Vertex<T> vertex_2 = vertex_2_temp.Clone();
            try
            {
                return vertex_1.neighbour_vertexes.Contains(vertex_2);
            }
            catch
            {
                return false;
            }
        }

        public void delete_edge(Vertex<T> vertex_1, Vertex<T> vertex_2)
        {
            if(contain_vertex(vertex_1) && contain_vertex(vertex_2))
            {
                if (vertex_1.neighbour_vertexes.Contains(vertex_2) && vertex_2.neighbour_vertexes.Contains(vertex_1))
                {
                    vertex_1.neighbour_vertexes.Remove(vertex_2);
                    vertex_2.neighbour_vertexes.Remove(vertex_1);
                }
            }
        }

        public int count_edges()
        {
            try
            {
                List<Vertex<T>> checked_vertex_set = new List<Vertex<T>>();
                int number_of_edges = 0;
                for (int i = 0; i < vertex_set.Count(); i++)
                {
                    Vertex<T> vertex = vertex_set[i];
                    for (int j = 0; j < vertex.neighbour_vertexes.Count(); j++)
                    {
                        if (checked_vertex_set.Contains(vertex.neighbour_vertexes[j]) == false)
                        {
                            number_of_edges++;
                        }
                    }
                    checked_vertex_set.Add(vertex);
                }
                return number_of_edges;
            }
            catch
            {
                return 0;
            }
            
        }

        public void add_edge(Vertex<T> vertex_1, Vertex<T> vertex_2)
        {
            if (contain_vertex(vertex_1) && contain_vertex(vertex_2) && (if_vertexes_are_neighboured(vertex_1, vertex_2) == false) )
              {
                vertex_1.neighbour_vertexes.Add(vertex_2);
                vertex_2.neighbour_vertexes.Add(vertex_1);
              }
        }

        public static bool operator ==(Graph<T> graph_1, Graph<T> graph_2)
        {
            if (graph_1.Vertex_set.Count() == graph_2.Vertex_set.Count())
                return true;
            else
                return false;
        }
        public static bool operator !=(Graph<T> graph_1, Graph<T> graph_2)
        {
            if (graph_1.Vertex_set.Count() != graph_2.Vertex_set.Count())
                return true;
            else
                return false;
        }
        public Graph<T> Clone()
        {
            var new_graph = new Graph<T>();
            new_graph.vertex_set = this.vertex_set;
            return new_graph;
        }

    }

    class Iterator_for_vertexes<T>
    {
        public Graph<T> graph;
        public Vertex<T> current_vertex;
        public int current_ID;

        public Iterator_for_vertexes(ref Graph<T> graph, Vertex<T> current_vertex)
        {
            if (graph.Vertex_set.Contains(current_vertex)) {
                this.current_vertex = current_vertex;
                for (int i = 0; i < graph.Vertex_set.Count(); i++)
                {
                    if (current_vertex == graph.Vertex_set[i])
                    {
                        current_ID = i;
                    }
                }
             this.graph = graph;
            }             
        }

        public Iterator_for_vertexes(ref Graph<T> graph)
        {
            this.graph = graph;
            current_ID = 0;
            current_vertex = graph.Vertex_set[0];
        }

        public Vertex<T> get_current_vertex()
        {
            return graph.Vertex_set[current_ID];
        }

        public void next_vertex()
        {
            current_ID++;            
            if (current_ID >= graph.Vertex_set.Count())
            {
                current_vertex = graph.Vertex_set[graph.Vertex_set.Count()];
                current_ID = graph.Vertex_set.Count() - 1; 
            }
            else { current_vertex = graph.Vertex_set[current_ID]; }            
        }

        public void previous_vertex()
        {
            current_ID--;
            if (current_ID < 0)
            {
                current_ID = 0;
                current_vertex = graph.Vertex_set[current_ID];
            }
            else { current_vertex = graph.Vertex_set[current_ID]; }
        }
    }

    class Iterator_for_neighbour_vertexes<T>
    {
        public Graph<T> graph;
        private Vertex<T> main_vertex;
        public Vertex<T> current_vertex;
        public int current_ID;

        public Iterator_for_neighbour_vertexes(ref Graph<T> graph, Vertex<T> main_vertex)
        {
            if (graph.Vertex_set.Contains(main_vertex))
            {
            this.main_vertex = main_vertex;
            this.current_vertex = main_vertex.neighbour_vertexes[0];
            current_ID = 0;
            this.graph = graph;
            }
        }

        public Iterator_for_neighbour_vertexes(ref Graph<T> graph)
        {
            this.graph = graph;
            current_ID = 0;
            main_vertex = graph.Vertex_set[0];
            current_vertex = main_vertex.neighbour_vertexes[0];
        }

        public Vertex<T> get_current_vertex()
        {
            return current_vertex;
        }

        public void next_vertex()
        {
            current_ID++;
            if (current_ID >= main_vertex.neighbour_vertexes.Count())
            {
                current_ID = main_vertex.neighbour_vertexes.Count() - 1;
                current_vertex = main_vertex.neighbour_vertexes[current_ID];
            }
            else { current_vertex = main_vertex.neighbour_vertexes[current_ID]; }
        }

        public void previous_vertex()
        {
            current_ID--;
            if (current_ID < 0)
            {
                current_ID = 0;
                current_vertex = main_vertex.neighbour_vertexes[current_ID];
            }
            else { current_vertex = main_vertex.neighbour_vertexes[current_ID]; }
        }
    }

    class Person
    {
       public string name;
       public string surname;
       public Person(string name,string surname)
        {
            this.name = name; this.surname = surname;
        }
        public override string ToString()
        {
            return string.Concat(name, " ", surname);
        }
    }

    class Program
    {
        static void Main()
        {
            List<Vertex<int>> vertex_set_for_graph_1 = new List<Vertex<int>>();
            Vertex<int> vertex_1 = new Vertex<int>(0);  vertex_set_for_graph_1.Add(vertex_1);
            Vertex<int> vertex_2 = new Vertex<int>(1);  vertex_set_for_graph_1.Add(vertex_2);
            Vertex<int> vertex_3 = new Vertex<int>(2);  vertex_set_for_graph_1.Add(vertex_3);
            Vertex<int> vertex_4 = new Vertex<int>(3);  vertex_set_for_graph_1.Add(vertex_4);
            Graph<int> Graph_1 = new Graph<int>(vertex_set_for_graph_1); Graph_1.add_vertex(new Vertex<int>(4));
            Console.WriteLine("Is Graph_1 empty: {0}, data type: {1}", Graph_1.is_empty<Vertex<int>>(), vertex_1.GetType());
            Console.WriteLine("Number of vertexes: {0}", Graph_1.count_vertexes());
            Console.WriteLine("Number of edges: {0}", Graph_1.count_edges());
            Iterator_for_vertexes<int> vertex_iterator = new Iterator_for_vertexes<int>(ref Graph_1);
            Console.WriteLine("Testing Iterator, 0 vertex: {0}", vertex_iterator.current_vertex.vertex);
            vertex_iterator.next_vertex(); vertex_iterator.next_vertex();
            Console.WriteLine("Testing Iterator, 2 vertex later: {0}", vertex_iterator.current_vertex.vertex);            
            Graph_1.add_edge(Graph_1.Vertex_set[0], Graph_1.Vertex_set[1]); Graph_1.add_edge(Graph_1.Vertex_set[0], Graph_1.Vertex_set[2]); Graph_1.add_edge(Graph_1.Vertex_set[0], Graph_1.Vertex_set[3]);
            Graph_1.add_edge(Graph_1.Vertex_set[2], Graph_1.Vertex_set[1]); Graph_1.add_edge(Graph_1.Vertex_set[2], Graph_1.Vertex_set[3]);
            Console.WriteLine("Number of edges: {0}", Graph_1.count_edges());
            vertex_iterator.previous_vertex(); vertex_iterator.previous_vertex();
            Console.WriteLine("Number of neighbour vertexes for vertex {0}: {1}", vertex_iterator.current_vertex.vertex, Graph_1.count_neighbour_vertexes(vertex_iterator.current_vertex));
            Iterator_for_neighbour_vertexes<int> neighbour_vertex_iterator = new Iterator_for_neighbour_vertexes<int>(ref Graph_1, vertex_iterator.current_vertex);
            Console.WriteLine("Testing Neighbour_Iterator, vertex: {0} && first neighbour: {1}", vertex_iterator.current_vertex.vertex, neighbour_vertex_iterator.current_vertex.vertex);
            neighbour_vertex_iterator.next_vertex(); neighbour_vertex_iterator.next_vertex(); 
            Console.WriteLine("Testing Neighbour_Iterator, vertex: {0} && third neighbour: {1}", vertex_iterator.current_vertex.vertex, neighbour_vertex_iterator.current_vertex.vertex);
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------------------");

            List<Vertex<Person>> vertex_set_for_graph_2 = new List<Vertex<Person>>();
            Person Person_0 = new Person("Vyacheslav", "Kniha"); Person Person_1 = new Person("Gabriel", "Gesus"); Person Person_2 = new Person("Rahim", "Sterling");
            Vertex<Person> person_1 = new Vertex<Person>(Person_0); vertex_set_for_graph_2.Add(person_1);
            Vertex<Person> person_2 = new Vertex<Person>(Person_1); vertex_set_for_graph_2.Add(person_2);
            Vertex<Person> person_3 = new Vertex<Person>(Person_2); vertex_set_for_graph_2.Add(person_3);
            Graph<Person> Graph_2 = new Graph<Person>(vertex_set_for_graph_2);
            Console.WriteLine("Is Graph_2 empty: {0}, data type: {1}", Graph_2.is_empty<Vertex<int>>(), person_1.GetType());
            Console.WriteLine("Number of vertexes: {0}", Graph_2.count_vertexes());
            Console.WriteLine("Number of edges: {0}", Graph_2.count_edges());
            Iterator_for_vertexes<Person> vertex_iterator_2 = new Iterator_for_vertexes<Person>(ref Graph_2);
            Console.WriteLine("Testing Iterator, 0 vertex: {0}", vertex_iterator_2.current_vertex.vertex);
            vertex_iterator_2.next_vertex(); vertex_iterator_2.next_vertex();
            Console.WriteLine("Testing Iterator, 2 vertex later: {0}", vertex_iterator_2.current_vertex.vertex);
            Graph_2.add_edge(Graph_2.Vertex_set[0], Graph_2.Vertex_set[1]); Graph_2.add_edge(Graph_2.Vertex_set[0], Graph_2.Vertex_set[2]);
            Graph_2.add_edge(Graph_2.Vertex_set[1], Graph_2.Vertex_set[2]);
            Console.WriteLine("Number of edges: {0}", Graph_2.count_edges());
            vertex_iterator_2.previous_vertex(); vertex_iterator_2.previous_vertex();
            Console.WriteLine("Number of neighbour vertexes for vertex {0}: {1}", vertex_iterator_2.current_vertex.vertex, Graph_2.count_neighbour_vertexes(vertex_iterator_2.current_vertex));
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------------------");

            Graph<Person> Graph_3 = Graph_2.Clone(); 
            Console.WriteLine("size_Graph_2 == size_Graph_3 : {0}", Graph_2 == Graph_3);
            Console.ReadLine();
        }
    }
}
