using SelectPdf;
using System.Linq;

namespace TelerikPdfViewerTestApp
{
    public static class BookmarkFactory
    {
        public static Bookmark[] CreateBookmarks(PdfDocument pdfDocument)
        {
            return pdfDocument.Bookmarks.OfType<PdfBookmark>()
                              .Select(CreateBookmarks)
                              .ToArray();
        }

        private static Bookmark CreateBookmarks(PdfBookmark pdfBookmark)
        {
            var bookmark = new Bookmark(pdfBookmark);
            var childrenBookmarks = pdfBookmark.ChildNodes.OfType<PdfBookmark>()
                                               .Select(x => new Bookmark(x));
            bookmark.ChildrenBookmarks.AddRange(childrenBookmarks);
            return bookmark;
        }
    }
}