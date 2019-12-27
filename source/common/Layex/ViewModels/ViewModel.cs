using System;
using Caliburn.Micro;

namespace Layex.ViewModels
{
    public abstract class ViewModel : Screen, IViewModel
    {
        private bool _isStatic;

        public new IItemsViewModel Parent
        {
            get { return base.Parent as IItemsViewModel; }
        }

        public bool IsStatic
        {
            get { return _isStatic; }
            set
            {
                _isStatic = value;
                NotifyOfPropertyChange(() => IsStatic);
            }
        }

        public event EventHandler Disposed;

        public virtual void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public static string GetCodeName<T>() where T : IViewModel
        {
            return GetCodeName(typeof(T));
        }

        public static string GetCodeName(Type type)
        {
            string codeName = string.Empty;
            if (type != null)
            {
                ViewModelAttribute attribute = ViewModelAttribute.GetAttribute(type);
                if (attribute != null)
                {
                    codeName = attribute.CodeName;
                }
            }
            return codeName;
        }
    }
}
