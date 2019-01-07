using System;

namespace Firestorm.Fluent
{
    internal class DelegateApiContext : IApiContext
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