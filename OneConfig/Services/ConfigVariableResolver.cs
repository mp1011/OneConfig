using OneConfig.Models.Exceptions;
using OneConfig.Services.Interfaces;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OneConfig.Services
{
    public class ConfigVariableResolver
    {
        public string Resolve(IConfigurationProvider provider, string text)
        {
            bool somethingChanged = true;

            List<string> replacementHistory = new List<string>();
            
            replacementHistory.Add(text);

            while (somethingChanged)
            {               
                var replacement = Regex.Replace(text, @"#{[^}]+}", match =>
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
    }
}
