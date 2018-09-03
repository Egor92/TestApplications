using System;
using System.Threading;
using ReactiveUI;
using Telerik.Windows.Documents.Fixed.Model.Navigation;
using System.Reactive.Linq;

namespace TelerikPdfViewerTestApp
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var messageBus = new MessageBus();
            var viewModelFactory = new ViewModelFactory(messageBus);
            var viewModel = new ViewModel(viewModelFactory);
            DataContext = viewModel;

            messageBus.Listen<Bookmark>(AppEvents.NavigationRequested)
                      .ObserveOn(SynchronizationContext.Current)
                      .Subscribe(OnNavigationRequested);
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