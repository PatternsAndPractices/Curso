namespace PnP.Patterns.Structure.Decorator
{
    public abstract class Traceable : ILoad
    {
        private ILoad loadComponent;

        public Traceable(ILoad component)
        {
            loadComponent = component;
        }

        public virtual string Load(string fileName)
        {
            string dataFile = string.Empty;
            if (loadComponent != null)
            {
                dataFile = loadComponent.Load(fileName);
            }
            return dataFile;
        }
    }
}