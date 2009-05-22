using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DotNetMarche.Utils
{
   public static class GenericUtils
   {
      /// <summary>
      /// This to make it possible to use sequential guid with EF
      /// </summary>
      /// <param name="guid"></param>
      /// <returns></returns>
      [DllImport("rpcrt4.dll", SetLastError = true)]
      public static extern int UuidCreateSequential(out Guid guid);

      public static Guid CreateSequentialGuid()
      {
         Guid guid;
         UuidCreateSequential(out guid);
         return guid;
      }
   }
}
