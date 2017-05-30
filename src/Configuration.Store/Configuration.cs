using System;

namespace Configuration.Store
{
    /// <summary>
    /// The configuration value
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// The sequence number of this configuration value in the specified version
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// The type of configuration value
        /// <see cref="ConfigurationDataType"/>
        /// </summary>
        public ConfigurationDataType Type { get; set; }

        /// <summary>
        /// The actual value, defined in the format specified by <seealso cref="Type"/>
        /// </summary>
        public string Data { get; set; }
    }
}