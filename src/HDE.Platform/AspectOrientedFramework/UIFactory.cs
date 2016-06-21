using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace HDE.Platform.AspectOrientedFramework
{
    public class UIFactory
    {
        private readonly Dictionary<Type, Type> _views = new Dictionary<Type, Type>();

        public void Register<TInterfaceType, TImplementationType>()
            where TImplementationType: class
        {
            Register(typeof(TInterfaceType), typeof(TImplementationType));
        }

        public void Register(Type key, Type type)
        {
            if (_views.ContainsKey(key))
            {
                throw new ArgumentException(string.Format("Type '{0}' was already registered!", key));
            }

            if (!type.IsClass)
            {
                throw new ArgumentException("Type must be class!");
            }

            if (!type.GetConstructors().Any(ctor=>ctor.GetParameters().Length==0 && ctor.IsPublic))
            {
                throw new ArgumentException("Type must have parameterless constructor public!");
            }

            _views.Add(key, type);
        }

        public Type Get(Type key)
        {
            if (!_views.ContainsKey(key))
            {
                throw new ArgumentException(string.Format("View '{0}' was not registered!", key));
            }

            return _views[key];
        }
    }
}
