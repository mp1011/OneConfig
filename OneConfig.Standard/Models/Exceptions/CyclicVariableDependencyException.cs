using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneConfig.Models.Exceptions
{
    public class CyclicVariableDependencyException : Exception
    {
        public CyclicVariableDependencyException(List<string> path) : base(GetMessage(path)) { }

        private static string GetMessage(List<string> path)
        {
            return $"Unable to resolve variable because it has a cyclic dependency. Resolution path: {String.Join(Environment.NewLine, path.ToArray())}";
        }
    }
}
