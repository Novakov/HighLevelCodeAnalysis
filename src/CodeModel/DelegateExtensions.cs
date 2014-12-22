using System;

namespace CodeModel
{
    internal static class DelegateExtensions
    {
        public static void Call<TEventArgs>(this EventHandler<TEventArgs> @delegate, object @this, TEventArgs eventArgs)
        {
            if (@delegate != null)
            {
                @delegate(@this, eventArgs);
            }
        }
    }
}