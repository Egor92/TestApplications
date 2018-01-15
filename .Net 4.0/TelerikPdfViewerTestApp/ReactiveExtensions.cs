using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;

namespace TelerikPdfViewerTestApp
{
    public class CollectionItemsObservablePublishingInfo<TItem, TValue>
    {
        public CollectionItemsObservablePublishingInfo(IReactiveCollection<TItem> collection, TItem item, TValue newValue)
        {
            NewValue = newValue;
            Item = item;
            Collection = collection;
        }

        public IReactiveCollection<TItem> Collection { get; private set; }
        public TItem Item { get; private set; }
        public TValue NewValue { get; private set; }
    }

    public static partial class ReactiveExtensions
    {
        public static IObservable<CollectionItemsObservablePublishingInfo<TItem, TValue>> GetItemsObservable<TItem, TValue>(this IReactiveCollection<TItem> reactiveCollection,
                                                                                                                            Func<TItem, IObservable<TValue>> itemToObservable)
        {
            if (reactiveCollection == null)
                throw new ArgumentNullException("reactiveCollection");
            if (itemToObservable == null)
                throw new ArgumentNullException("itemToObservable");

            return Observable.Create<CollectionItemsObservablePublishingInfo<TItem, TValue>>(observer =>
            {
                var subscriptionsByItem = new Dictionary<TItem, IDisposable>();

                Action<TItem> subscribeToItemObservable = item =>
                {
                    if (Equals(item, null))
                        return;

                    var isItemObservableAlreadySubscripted = subscriptionsByItem.ContainsKey(item);
                    if (isItemObservableAlreadySubscripted)
                        return;

                    var observable = itemToObservable(item);
                    var subscription = observable.Subscribe(value =>
                    {
                        observer.OnNext(new CollectionItemsObservablePublishingInfo<TItem, TValue>(reactiveCollection, item, value));
                    });
                    subscriptionsByItem[item] = subscription;
                };

                Action<TItem> unsubscribeToItemObservable = item =>
                {
                    subscriptionsByItem[item].Dispose();
                    subscriptionsByItem.Remove(item);
                };

                var collectionSubscription = new CompositeDisposable()
                {
                    reactiveCollection.ItemsAdded.Subscribe(subscribeToItemObservable),
                    reactiveCollection.ItemsRemoved.Subscribe(unsubscribeToItemObservable),
                    reactiveCollection.ShouldReset.Subscribe(_ =>
                    {
                        var items = subscriptionsByItem.Keys.ToList();
                        items.ForEach(unsubscribeToItemObservable);
                        foreach (var item in reactiveCollection)
                        {
                            subscribeToItemObservable(item);
                        }
                    }),
                };

                foreach (var item in reactiveCollection)
                {
                    subscribeToItemObservable(item);
                }

                return Disposable.Create(() =>
                {
                    collectionSubscription.Dispose();
                    foreach (var subscription in subscriptionsByItem.Values)
                    {
                        subscription.Dispose();
                    }
                });
            });
        }
    }
}