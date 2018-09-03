using ReactiveUI;

namespace TelerikPdfViewerTestApp
{
    public interface IViewModelFactory
    {
        BookmarkViewModel CreateBookmarkVM(Bookmark bookmark);
    }

    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IMessageBus _messageBus;

        #region Ctor

        public ViewModelFactory(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        #endregion

        #region Implementation of IViewModelFactory

        public BookmarkViewModel CreateBookmarkVM(Bookmark bookmark)
        {
            return new BookmarkViewModel(this, _messageBus, bookmark);
        }

        #endregion
    }
}