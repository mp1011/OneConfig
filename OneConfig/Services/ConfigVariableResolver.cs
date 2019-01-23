using OneConfig.Models.Exceptions;
using OneConfig.Services.Interfaces;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OneConfig.Services
{
    public class ConfigVariableResolver
    {
        public const string VariableRegex = @"#{[^}]+}";

        public static string Resolve(IConfigurationProvider provider, string text)
        {
            if (text == null)
                return null;

            bool somethingChanged = true;

            List<string> replacementHistory = new List<string>();
            
            replacementHistory.Add(text);

            while (somethingChanged)
            {               
                var replacement = Regex.Replace(text, VariableRegex, match =>
                {
                    var key = match.Value.Replace("#{", "").Replace("}", "");
                    var resolvedValue = provider.GetValue(key);
                    if (resolvedValue != null) 
                        return resolvedValue.Text;
                    else
                        return match.Value;
                });

                if (text == replacement)
                {
                    somethingChanged = false;
                }
                else
                {
                    if (replacementHistory.Contains(replacement))
                    {
                        replacementHistory.Add(replacement);
                        throw new CyclicVariableDependencyException(replacementHistory);
                    }

                    replacementHistory.Add(replacement);
                }

                text = replacement;
            }

            return text;
        }

        public static bool HasUnresolvedVariables(string text)
        {
            return Regex.Matches(text, VariableRegex).Count > 0;
        }
    }
}
