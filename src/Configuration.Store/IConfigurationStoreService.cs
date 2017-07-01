using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Configuration.Store
{
    /// <summary>
    /// The contract used to load and save configuration values
    /// {key, version} (type)
    /// [
    ///    {envTag*}(value, sequence)
    /// ]
    /// </summary>
    public interface IConfigurationStoreService
    {
        /// <summary>
        /// Gets all configuration keys available, including their type and latest version
        /// </summary>
        /// <returns>All available configuration keys</returns>
        Task<IEnumerable<ConfigurationKey>> GetConfigurationKeys();

        /// <summary>
        /// Gets a configuration value for all speficied filters;
        /// </summary>
        /// <param name="key">The configuration key being requested</param>
        /// <param name="version">The configuration version required</param>
        /// <param name="environmentTag">The environment for which the configuration will be used</param>
        /// <param name="currentSequence">The sequence number currently held by the client application (it should only be specified if the client doesn't wants to receive the configuration value when no updates exist)</param>
        /// <returns>NULL when no configuration value has been found</returns>
        /// <returns>Complete Configuration when value was found and either no currentSequence has been passed or a higher sequence than currentSequence is available</returns>
        /// <returns>Small Configuration (just Sequence) when currentSequence was passed and no update happened on this version of the key</returns>
        Task<Configuration> GetConfiguration(
            string key,
            Version version,
            string environmentTag,
            int? currentSequence);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="version"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        Task AddConfiguration(
            string key,
            Version version,
            ConfigurationDataType dataType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        Task RemoveConfiguration(
            string key,
            Version version);

        /// <summary>
        /// Adds a value keyed by the collection of tags to the configuration
        /// </summary>
        /// <param name="key"></param>
        /// <param name="version"></param>
        /// <param name="tags"></param>
        /// <param name="value"></param>
        /// <remarks>If any subset of tags is a match to some other collection of tags, the operation fails</remarks>
        Task<Guid> AddValueToConfiguration(
            string key,
            Version version,
            IEnumerable<string> tags,
            string value);

        /// <summary>
        /// Updated a value keyed by the collection of tags to the configuration. It increments the sequence
        /// </summary>
        /// <param name="key"></param>
        /// <param name="version"></param>
        /// <param name="valueId"></param>
        /// <param name="tags"></param>
        /// <param name="value"></param>
        Task UpdateValueOnConfiguration(
            string key,
            Version version,
            Guid valueId,
            IEnumerable<string> tags,
            string value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="version"></param>
        /// <param name="valueId"></param>
        /// <returns></returns>
        Task RemoveValueFromConfiguration(
            string key,
            Version version,
            Guid valueId);
    }
}
