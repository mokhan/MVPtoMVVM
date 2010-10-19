using System;
using System.ComponentModel;
using System.Linq.Expressions;
using MVPtoMVVM.utility;

namespace MVPtoMVVM.mvvm.viewmodels
{
    public class Observable<T> : INotifyPropertyChanged where T : INotifyPropertyChanged
    {
        public void update(params Expression<Func<T, object>>[] properties)
        {
            properties.each(x => { PropertyChanged(null, new PropertyChangedEventArgs(x.pick_property().Name)); });
        }

        public event PropertyChangedEventHandler PropertyChanged = (o, e) => { };
    }
}