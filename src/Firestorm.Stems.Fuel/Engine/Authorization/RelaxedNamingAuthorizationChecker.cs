using System.Linq;
using Firestorm.Engine;
using Firestorm.Engine.Fields;
using Firestorm.Stems.Naming;

namespace Firestorm.Stems.Fuel.Authorization
{
    internal class RelaxedNamingAuthorizationChecker<TItem> : IAuthorizationChecker<TItem>
        where TItem : class
    {
        private readonly IAuthorizationChecker<TItem> _authorizationChecker;
        private readonly NamingConventionSwitcher _conventionSwitcher;

        public RelaxedNamingAuthorizationChecker(IAuthorizationChecker<TItem> authorizationChecker, NamingConventionSwitcher conventionSwitcher)
        {
            _authorizationChecker = authorizationChecker;
            _conventionSwitcher = conventionSwitcher;
        }

        public IQueryable<TItem> ApplyFilter(IQueryable<TItem> items)
        {
            return _authorizationChecker.ApplyFilter(items);
        }

        public bool CanGetItem(IDeferredItem<TItem> item)
        {
            return _authorizationChecker.CanGetItem(item);
        }

        public bool CanAddItem()
        {
            return _authorizationChecker.CanAddItem();
        }

        public bool CanEditItem(IDeferredItem<TItem> item)
        {
            return _authorizationChecker.CanEditItem(item);
        }

        public bool CanDeleteItem(IDeferredItem<TItem> item)
        {
            return _authorizationChecker.CanDeleteItem(item);
        }

        public bool CanGetField(IDeferredItem<TItem> item, INamedField<TItem> field)
        {
            return _authorizationChecker.CanGetField(item, new RelaxedNamedField(field, _conventionSwitcher));
        }

        public bool CanEditField(IDeferredItem<TItem> item, INamedField<TItem> field)
        {
            return _authorizationChecker.CanEditField(item, new RelaxedNamedField(field, _conventionSwitcher));
        }

        private class RelaxedNamedField : INamedField<TItem>
        {
            private readonly INamedField<TItem> _field;
            private readonly NamingConventionSwitcher _conventionSwitcher;

            public RelaxedNamedField(INamedField<TItem> field, NamingConventionSwitcher conventionSwitcher)
            {
                _field = field;
                _conventionSwitcher = conventionSwitcher;
            }

            public string RequestedName => _field.Name;

            public string Name => _conventionSwitcher.ConvertRequestedToCoded(_field.Name);

            public IFieldReader<TItem> Reader => _field.Reader;

            public IFieldWriter<TItem> Writer => _field.Writer;
        }
    }
}