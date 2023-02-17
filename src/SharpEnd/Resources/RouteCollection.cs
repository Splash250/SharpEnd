using SharpEnd.Packet;

namespace SharpEnd.Resources
{
    public class RouteCollection
    {
        public List<Route> Routes { get; private set; }
        public static RouteCollection Empty
        {
            get
            {
                return new RouteCollection();
            }
        }
        public RouteCollection()
        {
            Routes = new List<Route>();
        }
        public void Add(RequestMethod method, string path, Route.ControllerDelegate controller)
        {
            Routes.Add(new Route(method, path, controller));
        }
        public void Remove(string path)
        {
            Routes.RemoveAll(x => x.Path == path);
        }
        public void Remove(Route route)
        {
            Routes.Remove(route);
        }
        public Route GetRoute(string path, RequestMethod requestMethod)
        {
            return Routes.FirstOrDefault(x => x.Path == path && x.RequestMethod == requestMethod);
        }

    }
}
