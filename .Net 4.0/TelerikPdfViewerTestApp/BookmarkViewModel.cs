using System.Collections.Generic;
using System.Windows.Input;
using ReactiveUI;
using SelectPdf;
using Telerik.Windows.Controls;
using System.Linq;

namespace TelerikPdfViewerTestApp
{
    public struct Bookmark
    {
        #region Ctor

        public Bookmark(PdfBookmark pdfBookmark)
            : this()
        {
            Title = pdfBookmark.Text;
            Left = pdfBookmark.Destination.Location.X;
            Top = pdfBookmark.Destination.Location.Y;
            Zoom = pdfBookmark.Destination.ZoomFactor;
            PageIndex = pdfBookmark.Destination.Page.PageIndex;
            ChildrenBookmarks = new List<Bookmark>();
        }

        #endregion

        public string Title { get; private set; }
        public double? Left { get; private set; }
        public double? Top { get; private set; }
        public double? Zoom { get; private set; }
        public int PageIndex { get; private set; }
        public List<Bookmark> ChildrenBookmarks { get; private set; }
    }

    public class BookmarkViewModel : ReactiveObject
    {
        #region Fields

        private readonly IViewModelFactory _viewModelFactory;
        private readonly IMessageBus _messageBus;
        private readonly Bookmark _bookmark;

        #endregion

        #region Ctor

        public BookmarkViewModel(IViewModelFactory viewModelFactory, IMessageBus messageBus, Bookmark bookmark)
        {
            _viewModelFactory = viewModelFactory;
            _messageBus = messageBus;
            _bookmark = bookmark;
        }

        #endregion

        #region Title

        public string Title
        {
            get { return _bookmark.Title; }
        }

        #endregion

        #region ChildrenBookmarkVMs

        private BookmarkViewModel[] _childrenBookmarkVMs;

        public BookmarkViewModel[] ChildrenBookmarkVMs
        {
            get { return _childrenBookmarkVMs ?? (_childrenBookmarkVMs = CreateChildrenBookmarkVMs()); }
        }

        private BookmarkViewModel[] CreateChildrenBookmarkVMs()
        {
            return _bookmark.ChildrenBookmarks.Select(_viewModelFactory.CreateBookmarkVM)
                            .ToArray();
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
            _messageBus.SendMessage(_bookmark, AppEvents.NavigationRequested);
        }
    }
}