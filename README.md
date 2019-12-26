# OneConfig

OneConfig is a simple library for managing application configuration. 

# Usage

With OneConfig, the only configuration you need in your app.config or web.config is a reference to a location in which your configuration can be found.
```
<add key="OneConfig_Source" value="(see below)"/>
```
The value may be any one of the following:

* The string "this file" which uses the app.config or web.config itself (.net framework only)
* The string appsettings.json" which reads values from an appsettings.json file in the application directory
* The string "windows environment" which reads values from the Windows Environment
* The string "command line" which reads values from the command line in the form /key:value or /key:"value"
* A sql connection string, which assumes your configuration is in a table called ConfigurationSettings with "Name" and "Value" columns
* A path to an xml file, followed by an xpath expression indicating the node that contains configuration settings. Configuration entries themselves are assume to be add tags in with a key and value attribute.

If you do not supply any sources, OneConfig defaults to using the app.config or appsettings.json file, the windows environment, and the command line.

To access configuration values at runtime, use the static AppConfig class.
* AppConfig.GetValue("Key") - returns the configuration value with the given key
* AppConfig.GetValueSource("Key") - returns what source provides the given configuration value
* AppConfig.SetValue("Key", "Value") - adds or changes a configuration at runtime
* AppConfig.ResetToDefault("Key") - undoes SetValue, reverting the configuration to what it was at app startup
* AppConfig.AddReader(reader) - allows you to inject a class at runtime that can provide configuration values

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
<add key="FullName" value="#{First Name} #{Last Name"} />
<add key="First Name" value="Jane" />
<add key="Last Name" value="Doe" />
```
The other variables need not come from the same configuration source. Variables that cannot be resolved are left as is. Any variables with a circular dependency will result in a CyclicVariableDependencyException

# Sources can have Variables
Your configuration sources can have variables in them provided by other sources. You can even leave some variables
unresolved and set them at runtime. Such unresolved sources as "inactive" until all their variables have values.

Example:
```
    <add key="OneConfig_Source" value="this file"/>
    <add key="OneConfig_Source2" value="Data Source=#{ConfigServer};Initial Catalog=MyDatabase;Integrated Security=True"/>
    <add key="ConfigServer" value="localhost\SQLExpress" />
```

# In-Memory Caching
Once read, configuration values are cached in memory to avoid unneeded hits to the disk or database. However if you set a variable at runtime using AppConfig.SetValue(), or add a new reader using AppConfig.AddReader(), all such caches are "reset" and configuration values will be read again.

# Diagnostics

* The static collection AppConfig.ReaderLoadErrors contains any exceptions encountered while trying to read from any of the configuration sources.
* If your configuration variables have any cyclic dependencies, trying to access their value will throw a CyclicVariableDependencyException
* You can find which source provided a value using AppConfig.GetValueSource(). This not only tells you which reader provided the value, but tells you all of overridden values from other readers.

