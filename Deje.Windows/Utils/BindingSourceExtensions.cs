using System.Windows.Forms;

namespace Deje.Windows.Utils
{
    public static class BindingSourceExtensions
    {
         public static void MoveTo(this BindingSource bindingSource, object item)
         {
             var i = bindingSource.IndexOf(item);
             bindingSource.Position = i;
         }
    }
}