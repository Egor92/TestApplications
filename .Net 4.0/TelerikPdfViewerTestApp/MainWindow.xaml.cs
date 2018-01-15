using System;
using Telerik.Windows.Documents.Fixed.Model.Navigation;

namespace TelerikPdfViewerTestApp
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new ViewModel();
            DataContext = viewModel;

            viewModel.NavigationRequested.Subscribe(OnNavigationRequested);
        }

        private void OnNavigationRequested(Bookmark bookmark)
        {
            var location = new Location
            {
                Left = bookmark.Left,
                Top = bookmark.Top,
                Zoom = bookmark.Zoom,
                Page = PdfViewer.Document.Pages[bookmark.PageIndex],
            };
            PdfViewer.GoToDestination(location);
        }
    }
}