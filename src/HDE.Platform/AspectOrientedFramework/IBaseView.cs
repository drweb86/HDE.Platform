using System;

namespace HDE.Platform.AspectOrientedFramework
{
    public interface IBaseView<in TContext>:IDisposable
    {
        void SetContext(TContext context);
        bool Process();
    }
}
