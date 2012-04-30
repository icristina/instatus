using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Instatus.Web
{
    public enum WebContentType
    {
        Atom,
        Rss,
        Jpeg,
        Gif,
        Bmp,
        Tiff,
        Png,
        Avi,
        Mpeg,
        Txt,
        Html,
        Rtf,
        Doc,
        Excel,
        Pdf,
        Mp3,
        Json,
        Xml,
        Csv,
        Js
    }

    public static class WebMimeType
    {   
        private class Definition {
            public WebContentType ContentType { get; set; }
            public string[] MimeTypes { get; set; }
            public string[] Extensions { get; set; }
            public Category Category { get; set; }

            public Definition() {}

            public Definition(WebContentType contentType, string[] mimeTypes, string extension, Category category) {
                ContentType = contentType;
                MimeTypes = mimeTypes;
                Extensions = new string[] { extension };
                Category = category;
            }

            public Definition(WebContentType contentType, string mimeType, string extension, Category category) 
                : this(contentType, new string[] { mimeType }, extension, category) {

            }
        }

        private enum Category {
            Syndication,
            Photo,
            Video,
            Document,
            Text,
            Audio,
            Data
        }
        
        private static List<Definition> definitions = new List<Definition>() {
            new Definition(WebContentType.Atom, "application/atom+xml", "atom", Category.Syndication),
            new Definition(WebContentType.Rss, "application/rss+xml", "rss", Category.Syndication),
            new Definition(WebContentType.Jpeg, new string[] { "image/jpeg", "image/pjpeg" }, "jpg", Category.Photo),
            new Definition(WebContentType.Png, "image/png", "png", Category.Photo),
            new Definition(WebContentType.Tiff, "image/tiff", "tiff", Category.Photo),
            new Definition(WebContentType.Gif, "image/gif", "gif", Category.Photo),
            new Definition(WebContentType.Avi, "video/avi", "avi", Category.Video),
            new Definition(WebContentType.Mpeg, "video/mpeg", "mpeg", Category.Video),
            new Definition(WebContentType.Txt, "text/plain", "txt", Category.Text),
            new Definition(WebContentType.Html, "text/html", "htm", Category.Text),
            new Definition(WebContentType.Rtf, "text/richtext", "rtf", Category.Text),
            new Definition(WebContentType.Doc, "application/msword", "doc", Category.Document),
            new Definition(WebContentType.Pdf, "application/pdf", "pdf", Category.Document),
            new Definition(WebContentType.Mp3, "audio/mpeg", "mp3", Category.Audio),
            new Definition(WebContentType.Json, "application/json", "json", Category.Data),
            new Definition(WebContentType.Xml, new string[] { "text/xml", "application/xml" }, "xml", Category.Data),
            new Definition(WebContentType.Csv, new string[] { "text/csv" }, "csv", Category.Data),
            new Definition(WebContentType.Js, new string[] { "text/javascript" }, "js", Category.Data)
        };

        public static string ToMimeType(this WebContentType contentType)
        {
            return definitions.First(d => d.ContentType == contentType).MimeTypes.First();
        }

        public static string ToExtension(this WebContentType contentType)
        {
            return definitions.First(d => d.ContentType == contentType).Extensions.First();
        }

        public static string GetExtension(string mimeType) {
            return definitions.First(d => d.MimeTypes.Any(m => m == mimeType)).Extensions.First();
        }

        private static bool IsInCategory(string mimeType, Category category)
        {
            return definitions.Any(d => d.Category == category && d.MimeTypes.Any(m => m == mimeType));
        }

        public static bool IsContentType(this WebContentType contentType, string mimeType)
        {
            return definitions.Any(d => d.ContentType == contentType && d.MimeTypes.Any(m => m == mimeType));
        }

        private static bool IsRelativePathInCategory(string relativePath, Category category)
        {
            var extension = Path.GetExtension(relativePath);

            if (extension.IsEmpty())
                return false;

            var normalizedExtension = extension.ToLower().Substring(1);
            
            return definitions.Any(d => d.Category == category && d.Extensions.Any(e => e == normalizedExtension));
        }

        public static bool IsRelativePathPhoto(string relativePath)
        {
            return IsRelativePathInCategory(relativePath, Category.Photo);
        }

        public static bool IsPhoto(string mimeType)
        {
            return IsInCategory(mimeType, Category.Photo);
        }

        public static bool IsVideo(string mimeType)
        {
            return IsInCategory(mimeType, Category.Video);
        }

        public static bool IsDocument(string mimeType)
        {
            return IsInCategory(mimeType, Category.Document);
        }

        public static WebContentType GetContentType(string mimeType)
        {
            return definitions.First(d => d.MimeTypes.Any(m => m == mimeType)).ContentType;
        }
    }
}