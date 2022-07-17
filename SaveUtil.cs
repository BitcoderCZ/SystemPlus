using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemPlus
{
    public static class SaveUtil
    {
        public static string CreateValue((string name, string value) obj)
            => $"{{v {obj.name}={obj.value} }}";

        public static string CreateObject(params (string name, string value)[] values)
        {
            if (values.Length <= 0)
                return "{o }";
            else if (values.Length == 1)
                return $"{{o {CreateValue(values[0])} }}";
            else
            {
                string retS = "{o ";
                for (int i = 0; i < values.Length - 1; i++)
                    retS += $"{CreateValue(values[i])}, ";

                int index = values.Length - 1;

                retS += $"{CreateValue(values[index])} }}";

                return retS;
            }
        }

        public static string CreateArray(bool newLines, params string[] values)
        {
            if (values.Length <= 0)
                return "{a }";
            else if (values.Length == 1)
                return $"{{a {values[0]} }}";
            else
            {
                char insert = newLines ? '\n' : '\0';
                string retS = "{a " + insert;
                for (int i = 0; i < values.Length - 1; i++)
                    retS += $"{values[i]}, {insert}";

                int index = values.Length - 1;

                retS += $"{values[index]} {insert}}}";

                return retS;
            }
        }
    }
}
