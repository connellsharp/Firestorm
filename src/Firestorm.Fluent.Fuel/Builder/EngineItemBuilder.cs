using System;
using System.Linq;
using System.Linq.Expressions;
using Firestorm.Fluent.Fuel.Models;

namespace Firestorm.Fluent.Fuel.Builder
{
    internal class EngineItemBuilder<TItem> : IApiItemBuilder<TItem>
        where TItem : class, new()
    {
        private readonly ApiItemModel<TItem> _model;

        internal EngineItemBuilder(ApiItemModel<TItem> model)
        {
            _model = model;
        }

        public string RootName
        {
            get { return _model.RootName; }
            set { _model.RootName = value; }
        }

        public IApiIdentifierBuilder<TItem, TIdentifier> Identifier<TIdentifier>(Expression<Func<TItem, TIdentifier>> expression)
        {
            var identifierModel = new ApiIdentifierModel<TItem>();

            var identifierBuilder = new ApiIdentifierBuilder<TItem, TIdentifier>(identifierModel);
            identifierBuilder.AddExpression(expression);

            _model.Identifiers.Add(identifierModel);

            return identifierBuilder;
        }

        public IApiFieldBuilder<TItem, TField> Field<TField>(Expression<Func<TItem, TField>> expression)
        {
            string originalName = ExpressionNameHelper.GetFullPropertyName(expression);

            var fieldModel = _model.Fields.FirstOrDefault(f => f.OriginalName == originalName)
                             ?? CreateNewFieldModel(originalName);

            var fieldBuilder = new EngineFieldBuilder<TItem, TField>(fieldModel, expression);

            if (fieldModel.Reader == null) // only models created above
                fieldBuilder.AllowRead();

            return fieldBuilder;
        }

        private ApiFieldModel<TItem> CreateNewFieldModel(string originalName)
        {
            var fieldModel = new ApiFieldModel<TItem>
            {
                OriginalName = originalName,
                Name = originalName
            };

            _model.Fields.Add(fieldModel);

            return fieldModel;
        }

        public IApiItemBuilder<TItem> OnCreating(Action<TItem> action)
        {
            _model.OnCreating = action;
            return this;
        }
    }
}