using System;

namespace Firestorm.Stems.Roots.DataSource
{
    /// <summary>
    /// The start axis that can be used as an alternative to <see cref="Root"/>s.
    /// </summary>
    internal class Vase : IAxis
    {
        public IRestUser User { get; set; }

        public IStemConfiguration Configuration { get; set; }

        public event EventHandler OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke(this, EventArgs.Empty);
        }
    }
}