using System;
using System.Linq.Expressions;
using Firestorm.Engine.Additives.Readers;
using Firestorm.Engine.Additives.Writers;
using Firestorm.Fluent.Fuel.Definitions;

namespace Firestorm.Fluent.Fuel.Builder
{
    internal class EngineFieldBuilder<TItem, TField> : IApiFieldBuilder<TItem, TField>
        where TItem : class
    {
        private readonly ApiFieldModel<TItem> _fieldModel;

        internal EngineFieldBuilder(ApiFieldModel<TItem> fieldModel)
        {
            _fieldModel = fieldModel;
        }

        public void AddExpression(Expression<Func<TItem, TField>> expression)
        {
            _fieldModel.Reader = new ExpressionFieldReader<TItem, TField>(expression);
            _fieldModel.Writer = new PropertyExpressionFieldWriter<TItem, TField>(expression);
        }

        public IApiFieldBuilder<TItem, TField> HasName(string fieldName)
        {
            _fieldModel.Name = fieldName;
            return this;
        }
    }
}