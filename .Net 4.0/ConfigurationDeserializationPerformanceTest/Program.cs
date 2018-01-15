#define LOAD
//#define COPIER
//#define MEMORY_STREAM

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Intecom.Configuration;
using Intecom.Configuration.Utils;

namespace ConfigurationDeserializationPerformanceTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            var configuration = Configuration.Load(@"D:\Data\ARMT\Configurations\LazGor\conf.xml", null);
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            var originalElementCount = configuration.mapList(x => x, typeof (ElementBase)).Count();
#if MEMORY_STREAM
            var memoryStream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, configuration);
#endif
#if COPIER
            var treeCopier = new TreeCopier<ElementBase>();
#endif
            for (int i = 0; i < 20; i++)
            {
                stopwatch.Restart();
#if LOAD
                var clonedConfiguration = Configuration.Load(@"D:\Data\ARMT\Configurations\LazGor\conf.xml", null);
#elif COPIER
                var clonedConfiguration = treeCopier.MakeCopy(configuration);
#elif MEMORY_STREAM
                memoryStream.Position = 0;
                var clonedConfiguration = (Intecom.Configuration.Configuration)formatter.Deserialize(memoryStream);
#endif
                stopwatch.Stop();
                var clonedElementCount = clonedConfiguration.mapList(x => x, typeof (ElementBase)).Count();
                Console.WriteLine(stopwatch.ElapsedMilliseconds);
            }
#if MEMORY_STREAM
            memoryStream.Dispose();
#endif
            Console.WriteLine("Finished");
            Console.Read();
        }
    }

    public class TreeCopier<TNode>
        where TNode : ElementBase
    {
        private readonly Dictionary<Type, FieldInfo[]> _fieldInfosByType = new Dictionary<Type, FieldInfo[]>();

        public TNode MakeCopy(TNode root)
        {
            var nodes = GetNodes(root);
            var memberwiseCloneMethod = typeof (object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
            var clonedNodesByOriginalNodes = new Dictionary<TNode, TNode>();
            foreach (var node in nodes)
            {
                var clonedNode = (TNode)memberwiseCloneMethod.Invoke(node, null);
                clonedNodesByOriginalNodes[node] = clonedNode;
            }
            foreach (var node in nodes)
            {
                var fieldInfos = GetFieldInfos(node);
                foreach (var fieldInfo in fieldInfos.Where(x => typeof (TNode).IsAssignableFrom(x.FieldType)))
                {
                    if (typeof (TNode).IsAssignableFrom(fieldInfo.FieldType))
                    {
                        var originalNode = (TNode)fieldInfo.GetValue(node);
                        if (originalNode == null)
                            continue;

                        TNode clonedNode;
                        if (clonedNodesByOriginalNodes.TryGetValue(originalNode, out clonedNode))
                        {
                            fieldInfo.SetValue(node, clonedNode);
                        }
                    }
                    else if (typeof (IEnumerable<TNode>).IsAssignableFrom(fieldInfo.FieldType))
                    {
                        var originalNodes = (IEnumerable<TNode>)fieldInfo.GetValue(node);
                        if (originalNodes == null)
                            continue;

                        var collectionType = originalNodes.GetType();
                        var clonedCollection = Activator.CreateInstance(collectionType) as ICollection<TNode>;
                        if (clonedCollection == null)
                            continue;

                        foreach (var originalNode in originalNodes)
                        {
                            TNode clonedNode;
                            if (clonedNodesByOriginalNodes.TryGetValue(originalNode, out clonedNode))
                            {
                                clonedCollection.Add(clonedNode);
                            }
                        }
                        fieldInfo.SetValue(node, clonedCollection);
                    }
                }
            }

            TNode clonedRoot;
            if (!clonedNodesByOriginalNodes.TryGetValue(root, out clonedRoot))
                return null;

            return clonedRoot;
        }

        private IList<TNode> GetNodes(TNode root)
        {
            return (IList<TNode>)root.mapList(x => x, typeof (TNode));
        }

        private FieldInfo[] GetFieldInfos(TNode node)
        {
            var nodeType = node.GetType();
            FieldInfo[] fieldInfos;
            if (_fieldInfosByType.ContainsKey(nodeType))
            {
                fieldInfos = _fieldInfosByType[nodeType];
            }
            else
            {
                fieldInfos = nodeType.GetFields();
                _fieldInfosByType[nodeType] = fieldInfos;
            }
            return fieldInfos;
        }
    }
}