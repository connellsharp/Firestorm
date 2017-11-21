using System;
using System.Linq.Expressions;
using Firestorm.Fluent.Fuel.Definitions;

namespace Firestorm.Fluent.Fuel.Builder
{
    public class EngineItemBuilder<TItem> : IApiItemBuilder<TItem>
        where TItem : class
    {
        private readonly ApiItemModel<TItem> _model;

        public EngineItemBuilder(ApiItemModel<TItem> model)
        {
            _model = model;
        }

        public IApiIdentifierBuilder<TItem, TIdentifier> Identifier<TIdentifier>(Expression<Func<TItem, TIdentifier>> expression)
        {
            var identifierModel = new ApiIdentifierModel<TItem>();

            var identifierBuilder = new ApiIdentifierBuilder<TItem, TIdentifier>(identifierModel);
            identifierBuilder.AddExpresion(expression);

            _model.Identifiers.Add(identifierModel.Name, identifierModel);
            return identifierBuilder;
        }

        public IApiFieldBuilder<TItem, TField> Field<TField>(Expression<Func<TItem, TField>> expression)
        {
            var fieldModel = new ApiFieldModel<TItem>();

            var fieldBuilder = new EngineFieldBuilder<TItem, TField>(fieldModel);
            fieldBuilder.AddExpression(expression);

            _model.Fields.Add(fieldModel.Name, fieldModel);
            return fieldBuilder;
        }
    }
}