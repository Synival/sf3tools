using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Utils {
    /// <summary>
    /// Utility functions for managing resources
    /// TODO: This should eventually be a "ResourceManager" singleton
    /// </summary>
    public static class Resources {
        /// <summary>
        /// Returns a dictionary of all ScenarioType values with a corresponding Dictionary of resource values
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static Dictionary<ScenarioType, Dictionary<int, string>> GetValueNameDictionaryForAllScenariosFromXML(string resourceName) {
            // TODO: maybe use reflection over ScenarioType?
            return new Dictionary<ScenarioType, Dictionary<int, string>>() {
                { ScenarioType.Scenario1, GetValueNameDictionaryFromXML(ResourceFileForScenario(ScenarioType.Scenario1, resourceName)) },
                { ScenarioType.Scenario2, GetValueNameDictionaryFromXML(ResourceFileForScenario(ScenarioType.Scenario2, resourceName)) },
                { ScenarioType.Scenario3, GetValueNameDictionaryFromXML(ResourceFileForScenario(ScenarioType.Scenario3, resourceName)) },
                { ScenarioType.PremiumDisk, GetValueNameDictionaryFromXML(ResourceFileForScenario(ScenarioType.PremiumDisk, resourceName)) }
            };
        }

        /// <summary>
        /// Returns the relative path of a resource file that is different depending on the scenario.
        /// </summary>
        /// <param name="scenario">The scenario for the resource.</param>
        /// <param name="resourceName">The name of the resource file without its path.</param>
        /// <returns>A string with a relative path with file for the resource.</returns>
        /// <exception cref="ArgumentException">Thrown if the scenario is invalid.</exception>
        public static string ResourceFileForScenario(ScenarioType scenario, string resourceName) {
            switch (scenario) {
                case ScenarioType.Scenario1:
                    return "Resources/S1/" + resourceName;
                case ScenarioType.Scenario2:
                    return "Resources/S2/" + resourceName;
                case ScenarioType.Scenario3:
                    return "Resources/S3/" + resourceName;
                case ScenarioType.PremiumDisk:
                    return "Resources/PD/" + resourceName;
                default:
                    throw new ArgumentException(MethodBase.GetCurrentMethod().Name + ": Handled scenario for " + nameof(scenario));
            }
        }

        /// <summary>
        /// Creates a standard XML reader for resource streams.
        /// </summary>
        /// <param name="stream">Stream from which to read the XML resource</param>
        /// <returns>A new XmlReader.</returns>
        public static XmlReader MakeXmlReader(Stream stream) {
            var settings = new XmlReaderSettings {
                IgnoreComments = true,
                IgnoreWhitespace = true
            };
            return XmlReader.Create(stream, settings);
        }
    }
}
