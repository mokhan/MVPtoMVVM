using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace MVPtoMVVM.mvvm.viewmodels
{
    public class Observable<T> : INotifyPropertyChanged where T : INotifyPropertyChanged
    {
        public void update(params Expression<Func<T, object>>[] properties)
        {
            foreach (var property in properties)
            {
                PropertyChanged(null, new PropertyChangedEventArgs(GetPropertyNameFrom(property)));
            }
        }

        string GetPropertyNameFrom(Expression<Func<T, object>> property)
        {
            if (property.Body.NodeType == ExpressionType.Convert)
                return (((UnaryExpression) property.Body).Operand as MemberExpression).Member.Name;
            if (property.Body.NodeType == ExpressionType.MemberAccess)
                return (property.Body as MemberExpression).Member.Name;
            return "";
        }

        public event PropertyChangedEventHandler PropertyChanged = (o, e) => { };
    }
}