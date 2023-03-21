using SharpEnd.Server;

namespace SharpEnd.Resources
{
    public class View
    {
        public string Content { get; set; }
        
        public View()
        {
            Content = string.Empty;
        }

        public View(string fileName)
        {
            try { Content = File.ReadAllText(fileName); }
            catch (Exception e) { throw e; }
        }

        public static View Create(string fileName, string[] variables)
        {
            View view = new();
            try
            {
                string content = File.ReadAllText(WebServer.HTMLPath + "/" + fileName);
                view.Content = ParseContent(content, variables);
            }
            catch (Exception e) { throw e; }
            return view;
        }

        public static View Create(string name, string fileName)
        {
            View view = new();
            try
            {
                string content = File.ReadAllText(WebServer.HTMLPath + "/" + fileName);
                view.Content = content;
            }
            catch (Exception e) { throw e; }
            return view;
        }
        public override string ToString()
        {
            return Content;
        }

        private static string ParseContent(string content, string[] variables) 
        {
            string rawContent = content;
            for (int i = 0; i < variables.Length; i++)
            {
                string[] variableParts = variables[i].Split('=');
                rawContent = rawContent.Replace(
                    "{{" + variableParts[0] + "}}", 
                    variableParts[1]
                    );
            }
            return rawContent;
        }
    }
}
