using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Data;

namespace RadGridViewGroupingApp
{
    public partial class MainWindow : Window
    {
        private readonly Random _random = new Random();
        private readonly List<Item> _items;

        public MainWindow()
        {
            InitializeComponent();

            _items = new List<Item>
            {
                new Item() { Name = "First", GroupName = "A group" },
                new Item() { Name = "Second", GroupName = "A group" },
                new Item() { Name = "Third", GroupName = "B group" },
                new Item() { Name = "Fourth", GroupName = "B group" },
                new Item() { Name = "Fifth", GroupName = "B group" },
            };

            var itemView = new ListCollectionView(_items);
            itemView.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));

            MyRadGridView.ItemsSource = itemView;

            Observable.Interval(TimeSpan.FromSeconds(1))
                      .Subscribe(OnNext);
        }

        private void OnNext(long l)
        {
            foreach (var item in _items)
            {
                item.Index = _random.Next(10);
            }
        }
    }
}