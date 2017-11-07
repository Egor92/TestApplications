using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace VisualTreeHelperTest
{
    public static class VisualTreeHelperExtended
    {
        public static DependencyObject GetParent(this DependencyObject source)
        {
            var parent = VisualTreeHelper.GetParent(source);
            if (parent == null)
            {
                var frameworkElement = source as FrameworkElement;
                if (frameworkElement != null)
                    parent = frameworkElement.Parent;

                var frameworkContentElement = source as FrameworkContentElement;
                if (frameworkContentElement != null)
                    parent = frameworkContentElement.Parent;
            }
            return parent;
        }

        public static T GetAncestor<T>(this DependencyObject source, bool includingItself = false)
            where T : DependencyObject
        {
            return GetAncestor<T>(source, x => false, includingItself);
        }

        public static T GetAncestor<T>(this DependencyObject source,
                                       Func<DependencyObject, bool> abortCondition,
                                       bool includingItself = false)
            where T : DependencyObject
        {
            return (T)source.GetAncestor(typeof(T), abortCondition, includingItself);
        }

        public static DependencyObject GetAncestor(this DependencyObject source, Type ancestorType, bool includingItself = false)
        {
            return source.GetAncestor(ancestorType, null, includingItself);
        }

        public static DependencyObject GetAncestor(this DependencyObject source,
                                                   Type ancestorType,
                                                   Func<DependencyObject, bool> abortCondition,
                                                   bool includingItself = false)
        {
            if (!typeof(DependencyObject).IsAssignableFrom(ancestorType))
                throw new ArgumentException("AncestorType must be assignable to 'System.Windows.DependencyObject'", "ancestorType");

            abortCondition = abortCondition ?? (x => false);
            if (abortCondition(source))
                return null;

            if (includingItself)
            {
                if (ancestorType.IsInstanceOfType(source))
                    return source;
            }

            var parent = source.GetParent();
            if (parent == null)
                return null;

            return parent.GetAncestor(ancestorType, abortCondition, true);
        }

        public static IEnumerable<T> GetDescendants<T>(this DependencyObject source, bool includingItself = false)
            where T : DependencyObject
        {
            return GetDescendants<T>(source, x => true, includingItself);
        }

        public static IEnumerable<T> GetDescendants<T>(this DependencyObject source, Func<T, bool> condition, bool includingItself = false)
            where T : DependencyObject
        {
            if (source == null)
                throw new ArgumentNullException("source");

            condition = condition ?? (x => true);
            var descendants = new List<T>();

            if (includingItself)
            {
                var t = source as T;
                if (t != null && (condition(t)))
                    descendants.Add(t);
            }

            CollectDescendants(source, condition, descendants);
            return descendants;
        }

        private static void CollectDescendants<T>(this DependencyObject source, Func<T, bool> condition, ICollection<T> descendants)
            where T : DependencyObject
        {
            var children = source.GetChildren().ToList();

            var targetDescendants = children.OfType<T>().Where(condition);
            foreach (var targetDescendant in targetDescendants)
            {
                descendants.Add(targetDescendant);
            }

            foreach (var child in children)
            {
                CollectDescendants(child, condition, descendants);
            }
        }

        public static T GetByName<T>(this DependencyObject source, string name)
            where T : FrameworkElement
        {
            return GetFirstDescendant<T>(source, x => x.Name == name, int.MaxValue, true);
        }

        public static T GetFirstDescendant<T>(this DependencyObject source,
                                              Func<T, bool> condition,
                                              int maxNestingLevel,
                                              bool includingItself = false)
            where T : DependencyObject
        {
            if (source == null)
                throw new ArgumentNullException("source");

            condition = condition ?? (x => true);

            if (includingItself)
            {
                maxNestingLevel--;
                var targetDescendant = source as T;
                if (targetDescendant != null && (condition(targetDescendant)))
                    return targetDescendant;
            }

            maxNestingLevel--;
            if (maxNestingLevel < 0)
                return null;

            return source.GetChildren()
                         .Select(x => x.GetFirstDescendant(condition, maxNestingLevel, true))
                         .FirstOrDefault(x => x != null);
        }

        public static IEnumerable<DependencyObject> GetChildren(this DependencyObject source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var childrenCount = VisualTreeHelper.GetChildrenCount(source);
            for (int i = 0; i < childrenCount; i++)
            {
                yield return VisualTreeHelper.GetChild(source, i);
            }
        }
    }
}
