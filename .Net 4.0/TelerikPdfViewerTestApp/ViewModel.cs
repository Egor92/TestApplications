using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using ReactiveUI;
using SelectPdf;
using Telerik.Windows.Controls;
using System.Linq;

namespace TelerikPdfViewerTestApp
{
    public class ViewModel : ReactiveObject
    {
        #region Ctor

        public ViewModel()
        {
            SubscribeToDocumentSourceChanges();
        }

        #endregion

        #region DocumentSource

        private string _DocumentSource;

        public string DocumentSource
        {
            get { return _DocumentSource; }
            private set { this.RaiseAndSetIfChanged(x => x.DocumentSource, value); }
        }

        private IDisposable SubscribeToDocumentSourceChanges()
        {
            return this.ObservableForProperty(x => x.DocumentSource)
                       .Subscribe(_ => OnDocumentSourceChanged());
        }

        private void OnDocumentSourceChanged()
        {
            BookmarkVMs.Clear();

            if (DocumentSource == null)
                return;

            var pdfDocument = new PdfDocument(DocumentSource);
            var bookmarkVMs = pdfDocument.Bookmarks.OfType<PdfBookmark>()
                                         .Select(CreateBookmarkVM);
            BookmarkVMs.AddRange(bookmarkVMs);
        }

        private static BookmarkViewModel CreateBookmarkVM(PdfBookmark pdfBookmark)
        {
            var bookmark = new Bookmark
            {
                Title = pdfBookmark.Text,
                Left = pdfBookmark.Destination.Location.X,
                Top = pdfBookmark.Destination.Location.Y,
                Zoom = pdfBookmark.Destination.ZoomFactor,
                PageIndex = pdfBookmark.Destination.Page.PageIndex,
            };
            return new BookmarkViewModel(bookmark);
        }

        #endregion

        #region BookmarkName

        private string _BookmarkName;

        public string BookmarkName
        {
            get { return _BookmarkName; }
            set { this.RaiseAndSetIfChanged(x => x.BookmarkName, value); }
        }

        #endregion

        #region BookmarkVMs

        private readonly ReactiveCollection<BookmarkViewModel> _bookmarkVMs = new ReactiveCollection<BookmarkViewModel>();

        public ReactiveCollection<BookmarkViewModel> BookmarkVMs
        {
            get { return _bookmarkVMs; }
        }

        #endregion

        #region NavigationRequested

        public IObservable<Bookmark> NavigationRequested
        {
            get
            {
                return BookmarkVMs.GetItemsObservable(x => x.NavigationRequested)
                                  .Select(x => x.NewValue);
            }
        }

        #endregion

        #region OpenCommand

        private ICommand _openCommand;

        public ICommand OpenCommand
        {
            get { return _openCommand ?? (_openCommand = new DelegateCommand(Open)); }
        }

        private void Open(object _)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true)
                return;

            DocumentSource = openFileDialog.FileName;
        }

        #endregion

        #region CloseCommand

        private ICommand _closeCommand;

        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new DelegateCommand(Close)); }
        }

        private void Close(object arg)
        {
            DocumentSource = null;
        }

        #endregion

        #region NavigateToBookmarkCommand

        private ICommand _navigateToBookmarkCommand;

        public ICommand NavigateToBookmarkCommand
        {
            get { return _navigateToBookmarkCommand ?? (_navigateToBookmarkCommand = GetNavigateToBookmarkCommand()); }
        }

        private ICommand GetNavigateToBookmarkCommand()
        {
            var command = new DelegateCommand(NavigateToBookmark, CanNavigateToBookmark);
            var observable = GetCanNavigateToBookmarkObservable();
            observable.Subscribe(x => command.InvalidateCanExecute());
            return command;
        }

        private IObservable<object> GetCanNavigateToBookmarkObservable()
        {
            return this.ObservableForProperty(x => x.BookmarkName);
        }

        private void NavigateToBookmark(object _)
        {
            var bookmarkVM = BookmarkVMs.FirstOrDefault(x => x.Title == BookmarkName);
            if (bookmarkVM == null)
            {
                MessageBox.Show("Bookmark not found");
                return;
            }

            bookmarkVM.Navigate();
        }

        private bool CanNavigateToBookmark(object _)
        {
            return BookmarkVMs.Any(x => x.Title == BookmarkName);
        }

        #endregion
    }
}