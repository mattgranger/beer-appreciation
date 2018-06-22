using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;

namespace BeerAppreciation.Core.Authorisation
{
    /// <summary>
    /// Use AES key based encryption to Encrypt/Decrypt oAuth bearer tokens
    /// </summary>
    /// <seealso cref="Microsoft.Owin.Security.ISecureDataFormat{AuthenticationTicket}" />
    public class SecureTokenFormatter : ISecureDataFormat<AuthenticationTicket>
    {
        /// <summary>
        /// The serialiser that will serialise/de-serialise a binary representation of an oAuth AuthenticationTicket
        /// </summary>
        private readonly TicketSerializer serialiser;

        /// <summary>
        /// An instance of an IDataProtector that will encrypt the serialised representation of an AuthenticationTicket
        /// </summary>
        private readonly IDataProtector protector;

        /// <summary>
        /// An instance of an encode that will encode the encrypted authentication ticket into a string
        /// </summary>
        private readonly ITextEncoder encoder;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureTokenFormatter"/> class.
        /// </summary>
        /// <param name="key">The encryption key, this should be stored in the config of both the Encryptor and Decryptor</param>
        public SecureTokenFormatter(string key)
        {
            this.serialiser = new TicketSerializer();
            this.protector = new AesDataProtectorProvider(key);
            this.encoder = TextEncodings.Base64Url;
        }

        /// <summary>
        /// Protects the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>An encoded string representing the encrypted AuthenticationTicket</returns>
        public string Protect(AuthenticationTicket ticket)
        {
            byte[] ticketData = this.serialiser.Serialize(ticket);
            byte[] protectedData = this.protector.Protect(ticketData);

            return this.encoder.Encode(protectedData);
        }

        /// <summary>
        /// Unprotects the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The AuthenticationTicket represented by the encrypted text passed in</returns>
        public AuthenticationTicket Unprotect(string text)
        {
            byte[] protectedData = this.encoder.Decode(text);
            byte[] ticketData = this.protector.Unprotect(protectedData);

            return this.serialiser.Deserialize(ticketData);
        }
    }
}