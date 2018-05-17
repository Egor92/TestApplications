using System;
using System.Collections.Generic;

namespace DocumentStore
{
    public class DocumentStore
    {
        private readonly List<string> _documents = new List<string>();

        public DocumentStore(int capacity)
        {
            Capacity = capacity;
        }
        
        public int Capacity { get; private set; }

        public IEnumerable<string> Documents
        {
            get { return _documents.ToArray(); }
        }

        public void AddDocument(string document)
        {
            if (_documents.Count >= Capacity)
                throw new InvalidOperationException();

            _documents.Add(document);
        }

        public override string ToString()
        {
            return String.Format("Document store: {0}/{1}", _documents.Count, Capacity);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            DocumentStore documentStore = new DocumentStore(2);
            documentStore.AddDocument("item");
            Console.WriteLine(documentStore);
        }
    }
}