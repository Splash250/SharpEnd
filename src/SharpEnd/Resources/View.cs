using SharpEnd.Server;

namespace SharpEnd.Resources
{
    public class View
    {
        public string Name { get; set; }
        public string Content { get; set; }
        
        public View()
        {
            Name = string.Empty;
            Content = string.Empty;
        }

        public View(string name, string fileName)
        {
            Name = name;
            try { Content = File.ReadAllText(fileName); }
            catch (Exception e) { throw e; }
        }

        public static View Create(string name, string fileName, string[] variables)
        {
            View view = new() { Name = name };
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
            View view = new() { Name = name };
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
