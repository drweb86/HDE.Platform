using System;
using HDE.Platform.Logging;

namespace HDE.Platform.AspectOrientedFramework
{
    public abstract class BaseController<TModel>: IDisposable
        where TModel: class, new()
    {
        #region Properties

        public TModel Model { get; private set; }
        protected UIFactory UiFactory { get; private set; }
        public ILog Log { get; private set; }

        #endregion

        #region Construcotors

        protected BaseController()
        {
            Log = CreateOpenLog();
            Model = new TModel();
            UiFactory = new UIFactory();
        }

        #endregion

        public TView CreateCustomView<TView>()
            where TView : IBaseView<BaseController<TModel>>, new()
        {
            var result = new TView();
            result.SetContext(this);
            return result;
        }

        public IView CreateView<IView>()
            where IView : IBaseView<BaseController<TModel>>
        {
            var type = UiFactory.Get(typeof(IView));
            var result = (IBaseView<BaseController<TModel>>)Activator.CreateInstance(type);
            result.SetContext(this);
            return (IView)result;
        }

        public virtual void Dispose()
        {
            CloseLog(true);
        }

        protected abstract ILog CreateOpenLog();

        protected virtual void CloseLog(bool disposing)
        {
            if (Log != null && Log.IsOpened)
            {
                Log.Close();
                Log = null;
            }
        }
    }
}
