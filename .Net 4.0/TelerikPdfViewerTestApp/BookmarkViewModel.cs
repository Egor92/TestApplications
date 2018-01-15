using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
using ReactiveUI;
using Telerik.Windows.Controls;

namespace TelerikPdfViewerTestApp
{
    public struct Bookmark
    {
        public string Title { get; set; }
        public double? Left { get; set; }
        public double? Top { get; set; }
        public double? Zoom { get; set; }
        public int PageIndex { get; set; }
    }

    public class BookmarkViewModel : ReactiveObject
    {
        #region Fields

        private readonly Bookmark _bookmark;

        #endregion

        #region Ctor

        public BookmarkViewModel(Bookmark bookmark)
        {
            _bookmark = bookmark;
        }

        #endregion

        #region Title

        public string Title
        {
            get { return _bookmark.Title; }
        }

        #endregion

        #region NavigationRequested

        private readonly Subject<Bookmark> _navigationRequested = new Subject<Bookmark>();

        public IObservable<Bookmark> NavigationRequested
        {
            get { return _navigationRequested.AsObservable(); }
        }

        #endregion

        #region NavigateCommand

        private ICommand _navigateCommand;

        public ICommand NavigateCommand
        {
            get { return _navigateCommand ?? (_navigateCommand = new DelegateCommand(Navigate)); }
        }

        private void Navigate(object _)
        {
            Navigate();
        }

        #endregion

        public void Navigate()
        {
            _navigationRequested.OnNext(_bookmark);
        }
    }
}