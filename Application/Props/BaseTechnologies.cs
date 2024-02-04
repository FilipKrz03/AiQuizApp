using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Props
{
    public static class BaseTechnologies
    {
        public static IEnumerable<string> Get()
        {
            yield return "C#";
            yield return "Java";
            yield return "JavaScript";
            yield return "Python";
            yield return "PHP";
            yield return "TypeScript";
            yield return "C++";
            yield return "React";
            yield return "Kotlin";
            yield return "SQL";
        }
    }
}
