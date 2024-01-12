using ftreel.Entities;
using ftreel.Services;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ftreel.Utils
{
    public class MailSingleton
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public Document? Document { get; set; }
        private static MailSingleton Instance = new MailSingleton();

        private MailSingleton() { 
            Subject = "{Category} : Nouveau document";
            Body = "Bonjour, un fichier {Title} a été ajouté dans la catégorie " +
                "{Category} à laquelle vous êtes abonné.\n\nVous le trouverez à l'url suivante :\n\n " +
                "{Url}";
            Document = null;
        }
        public static MailSingleton GetInstanceMailSingleton()
        {
            return Instance;
        }
        public void ReplacePlaceHolders()
        {
            if (Document != null)
            {
                Subject = ReplacePlaceholder(Subject, "Url", Document.GetDocumentUrl());
                Subject = ReplacePlaceholder(Subject, "Title", Document.Title);
                Subject = ReplacePlaceholder(Subject, "Description", Document.Description);
                Subject = ReplacePlaceholder(Subject, "Category", Document.Category.Name);
                Subject = ReplacePlaceholder(Subject, "Author", Document.Author?.Mail ?? "");
                Body = ReplacePlaceholder(Body, "Url", Document.GetDocumentUrl());
                Body = ReplacePlaceholder(Body, "Url", Document.GetPath());
                Body = ReplacePlaceholder(Body, "Title", Document.Title);
                Body = ReplacePlaceholder(Body, "Description", Document.Description);
                Body = ReplacePlaceholder(Body, "Category", Document.Category.Name);
                Body = ReplacePlaceholder(Body, "Author", Document.Author?.Mail ?? "");
            }
        }

        private string ReplacePlaceholder(string input, string placeholder, string replacement)
        {
            return input.Replace("{" + placeholder + "}", replacement ?? "");
        }
    }
}
