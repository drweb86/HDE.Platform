namespace HDE.Platform.AspectOrientedFramework.View
{
    public interface IToolView
    {
        void ApplyChange(string subject, params object[] body);
    }
}
