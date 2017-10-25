using System;
using System.Linq.Expressions;

namespace Firestorm.Stems
{
    public interface IStemField<TItem, TField>
    {
        Expression<Func<TItem, TField>> Expression { get; }

        void Set(TItem track, TField value);
    }
}