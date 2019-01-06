using System;
using Firestorm.Fluent;

namespace Firestorm.Extensions.AspNetCore
{
    public class DelegateApiContext : IApiContext
    {
        private readonly Action<IApiBuilder> _onApiCreatingAction;

        public DelegateApiContext(Action<IApiBuilder> onApiCreatingAction)
        {
            _onApiCreatingAction = onApiCreatingAction;
        }

        public void CreateApi(IApiBuilder apiBuilder)
        {
            _onApiCreatingAction(apiBuilder);
        }
    }
}