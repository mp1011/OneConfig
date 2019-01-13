# OneConfig

OneConfig is a simple library for managing application configuration. 

# Usage

With OneConfig, the only configuration you need in your app.config or web.config is a reference to a location in which your configuration can be found.
```
<add key="OneConfig_Source" value="(see below)"/>
```
The value may be any one of the following:

* The string "this file" which uses the app.config or web.config itself.
* The string "windows environment" which reads values from the Windows Environment
* The string "command line" which reads values from the command line in the form /key:value or /key:"value"
* A sql connection string, which assumes your configuration is in a table called ConfigurationSettings with "Name" and "Value" columns
* A path to an xml file, followed by an xpath expression indicating the node that contains configuration settings. Configuration entries themselves are assume to be add tags in with a key and value attribute.

To access configuration values at runtime, use the static OneConfig class.
* OneConfig.GetValue("Key") - returns the configuration value with the given key
* OneConfig.GetValueSource("Key") - returns what source provides the given configuration value
* OneConfig.SetValue("Key", "Value") - adds or changes a configuration at runtime
* OneConfig.ResetToDefault("Key") - undoes SetValue, reverting the configuration to what it was at app startup
* OneConfig.AddReader(reader) - allows you to inject a class at runtime that can provide configuration values

# Using Multiple Configuration Sources

You can add additional Source elements, suffixed with increasing numbers.
```
    <add key="OneConfig_Source" value="this file"/>
    <add key="OneConfig_Source2" value="Override\configuration.xml//settings"/>
```
You may use as many sources as you need. For any given configuration key, the highest numbered source that contains that configuration is the one that will be used.

# Configuration Variables
The value for any configuration key may refer to other configuration values by using the #{ } syntax

Example:
```
<add key="FullName" value="#{First Name" #{Last Name"} />
<add key="First Name" value="Jane" />
<add key="Last Name" value="Doe"} />
```
The other variables need not come from the same configuration source. Variables that cannot be resolved are left as is. Any variables with a circular dependency will result in a CyclicVariableDependencyException

# Changing Configuration at Runtime

```