namespace REAC.Domain.Shared
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EntityNameAttribute : Attribute
    {
        public EntityNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

}
