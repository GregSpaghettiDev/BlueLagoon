using System.IO;

namespace Common.StaticFiles
{
    public static class HtmlReader
    {
        public static string ReadHtmlTemplate(string templatePath, string[] htmlParams)
        {
            string htmlTemplate = null;

            using (StreamReader sourceReader = File.OpenText(templatePath))
            {
                htmlTemplate = sourceReader.ReadToEnd();
            }

            return string.Format(htmlTemplate, htmlParams);
        }
    }
}
