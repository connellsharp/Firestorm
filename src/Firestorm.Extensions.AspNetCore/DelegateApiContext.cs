using System;
using Firestorm.Fluent;

namespace Firestorm.Extensions.AspNetCore
{
    public class DelegateApiContext : ApiContext
    {
        private readonly Action<IApiBuilder> _onApiCreatingAction;

        public DelegateApiContext(Action<IApiBuilder> onApiCreatingAction)
        {
            _onApiCreatingAction = onApiCreatingAction;
        }

        protected override void OnApiCreating(IApiBuilder apiBuilder)
        {
            _onApiCreatingAction(apiBuilder);
            base.OnApiCreating(apiBuilder);
        }
    }
}