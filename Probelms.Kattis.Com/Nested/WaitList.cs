﻿using System.Collections.Generic;
using System.Linq;

namespace Probelms.Kattis.Com.Nested
{
    public class WaitList<TItem, TKey>
    {
        class Node<T>
        {
            private int dependencyCount;
            public T Item { get; private set; }

            public Node(T item, int dependencyCount)
            {
                Item = item;
                this.dependencyCount = dependencyCount;
            }

            public bool DecreaseDependencyCount()
            {
                dependencyCount--;
                return (dependencyCount == 0);
            }
        }

        private readonly Dictionary<TKey, List<Node<TItem>>> dependencies = new Dictionary<TKey, List<Node<TItem>>>();

        public void Add(TItem item, ICollection<TKey> pendingDependencies)
        {
            var node = new Node<TItem>(item, pendingDependencies.Count);

            foreach (var dependency in pendingDependencies)
            {
                Add(dependency, node);
            }
        }

        public IEnumerable<TItem> Remove(TKey key)
        {
            List<Node<TItem>> nodeList;
            var found = dependencies.TryGetValue(key, out nodeList);

            if (found)
            {
                dependencies.Remove(key);
                return nodeList.Where(x => x.DecreaseDependencyCount()).Select(x => x.Item);
            }

            return null;
        }

        private void Add(TKey key, Node<TItem> node)
        {      
            List<Node<TItem>> nodeList;
            var found = dependencies.TryGetValue(key, out nodeList);

            if (!found)
            {
                nodeList = new List<Node<TItem>>();
                dependencies.Add(key, nodeList);
            }

            nodeList.Add(node);
        }

        public int Count
        {
            get { return dependencies.Count; }
        }
    }
}
