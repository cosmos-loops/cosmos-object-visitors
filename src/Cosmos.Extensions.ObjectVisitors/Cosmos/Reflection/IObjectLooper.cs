namespace Cosmos.Reflection
{
    public interface IObjectLooper
    {
        IObjectVisitor BackToVisitor();

        void Fire();

        IObjectVisitor FireAndBack();
    }
    
    public interface IObjectLooper<T>
    {
        IObjectVisitor<T> BackToVisitor();

        void Fire();

        IObjectVisitor<T> FireAndBack();
    }
}