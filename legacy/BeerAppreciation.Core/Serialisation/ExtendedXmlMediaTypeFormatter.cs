namespace BeerAppreciation.Core.Serialisation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading.Tasks;

    /// <summary>
    /// Extends the build in XmlMediaTypeFormatter to support IQueryable and IList
    /// </summary>
    public class ExtendedXmlMediaTypeFormatter : XmlMediaTypeFormatter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedXmlMediaTypeFormatter"/> class.
        /// </summary>
        public ExtendedXmlMediaTypeFormatter()
        {
            // Use the XmlSerializer and not the default DataContractSerailizer
            this.UseXmlSerializer = true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether this <see cref="T:System.Net.Http.Formatting.XmlMediaTypeFormatter"/> can write objects of the specified type.
        /// </summary>
        /// <param name="type">The type of object that will be written.</param>
        /// <returns>
        /// true if objects of this type can be written, otherwise false.
        /// </returns>
        public override bool CanWriteType(Type type)
        {
            if (typeof(IQueryable).IsAssignableFrom(type))
            {
                // Does the queryable type have generic parameters
                Type[] generics = type.GetGenericArguments();

                if (generics.Any())
                {
                    return base.CanWriteType(generics[0]);
                }
            }

            return base.CanWriteType(type);
        }

        /// <summary>
        /// Called during serialization to write an object of the specified type to the specified writeStream.
        /// </summary>
        /// <param name="type">The type of object to write.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="writeStream">The <see cref="T:System.IO.Stream" /> to which to write.</param>
        /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> for the content being written.</param>
        /// <param name="transportContext">The <see cref="T:System.Net.TransportContext" />.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> that will write the value to the stream.
        /// </returns>
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, System.Net.TransportContext transportContext)
        {
            if (typeof(IQueryable).IsAssignableFrom(type))
            {
                // Does the queryable type have generic parameters
                Type[] generics = type.GetGenericArguments();

                if (generics.Any())
                {
                    // Convert the IQueryable to a list
                    Type genericListType = typeof(List<>).MakeGenericType(generics);
                    IList listInstance = (IList)Activator.CreateInstance(genericListType);

                    foreach (object item in (IEnumerable)value)
                    {
                        listInstance.Add(item);
                    }

                    return base.WriteToStreamAsync(genericListType, listInstance, writeStream, content, transportContext);
                }
            }

            return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
        }

        #endregion
    }
}
