namespace BeerAppreciation.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Extensions methods for serialisation
    /// </summary>
    public static class SerialisationExtensions
    {
        #region Fields and Constants

        /// <summary>
        /// A static dictionary of serializers keyed on the type of the serializer.
        /// </summary>
        private static readonly Dictionary<Type, DataContractSerializer> Serialisers = new Dictionary<Type, DataContractSerializer>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Serialises the object to Xml using data contract serialiser
        /// </summary>
        /// <param name="obj">The object to serialise.</param>
        /// <returns>The serialised object as Xml</returns>
        public static string SerialiseToXml(this object obj)
        {
            // Get a datacontract serialiser, either from static dictionary or create new
            DataContractSerializer serialiser = obj.GetType().GetDataContractSerialiser();

            // Serialise the string to
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true, IndentChars = "\t" };

            using (XmlWriter writer = XmlWriter.Create(stringBuilder, settings))
            {
                serialiser.WriteObject(writer, obj);
                writer.Flush();

                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Deserialises the specified stream containing xml in data contract format.
        /// </summary>
        /// <typeparam name="T">The type to deserialise into</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns>The de-serialised type</returns>
        public static T DeserialiseXml<T>(this Stream stream)
        {
            // Get a datacontract serialiser, either from static dictionary or create new
            DataContractSerializer serialiser = typeof(T).GetDataContractSerialiser();

            return (T)serialiser.ReadObject(stream);
        }

        /// <summary>
        /// Gets a data contract serialiser of the specified type.
        /// </summary>
        /// <param name="type">The type of the object to serialise.</param>
        /// <returns>
        /// The DataContractSerializer for the requested type
        /// </returns>
        public static DataContractSerializer GetDataContractSerialiser(this Type type)
        {
            // Get a datacontract serialiser, either from static dictionary or create new
            DataContractSerializer serialiser;

            if (Serialisers.ContainsKey(type))
            {
                serialiser = Serialisers[type];
            }
            else
            {
                serialiser = new DataContractSerializer(type);
                Serialisers.Add(type, serialiser);
            }

            return serialiser;
        }

        #endregion
    }
}
