using System;
using System.Diagnostics;
using System.Linq.Expressions;
using Firestorm.Engine.Additives.Readers;
using Firestorm.Engine.Additives.Writers;
using Firestorm.Fluent.Fuel.Models;

namespace Firestorm.Fluent.Fuel.Builder
{
    internal class EngineFieldBuilder<TItem, TField> : IApiFieldBuilder<TItem, TField>
        where TItem : class
    {
        private readonly ApiFieldModel<TItem> _fieldModel;
        private Expression<Func<TItem, TField>> _expression;

        internal EngineFieldBuilder(ApiFieldModel<TItem> fieldModel)
        {
            _fieldModel = fieldModel;
        }

        public void AddExpression(Expression<Func<TItem, TField>> expression)
        {
            Debug.Assert(_expression == null);

            _expression = expression;
            _fieldModel.Reader = new ExpressionFieldReader<TItem, TField>(_expression);
        }

        public IApiFieldBuilder<TItem, TField> HasName(string fieldName)
        {
            _fieldModel.Name = fieldName;
            return this;
        }

        public IApiFieldBuilder<TItem, TField> AllowWrite()
        {
            _fieldModel.Writer = new PropertyExpressionFieldWriter<TItem, TField>(_expression);
            return this;
        }
    }
}