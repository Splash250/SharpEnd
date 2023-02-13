using SharpEnd.Packet;

namespace SharpEnd.Resources
{
    public class Routes
    {
        public List<Route> RouteCollection { get; private set; }
        public static Routes Empty
        {
            get
            {
                return new Routes();
            }
        }
        public Routes()
        {
            RouteCollection = new List<Route>();
        }
        public void Add(RequestMethod Method, string path, Route.ControllerDelegate controller)
        {
            RouteCollection.Add(new Route(Method, path, controller));
        }
        public void Remove(string path)
        {
            RouteCollection.RemoveAll(x => x.Path == path);
        }
        public void Remove(Route route)
        {
            RouteCollection.Remove(route);
        }
        public Route GetRoute(string path, RequestMethod requestMethod)
        {
            return RouteCollection.FirstOrDefault(x => x.Path == path && x.RequestMethod == requestMethod);
        }

    }
}
