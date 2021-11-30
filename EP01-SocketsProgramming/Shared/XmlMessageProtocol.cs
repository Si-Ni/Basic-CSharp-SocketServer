using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Shared {

    public class XmlMessageProtocol : Protocol<XDocument> {
        protected override XDocument Decode(byte[] message) {
            var xmlData = Encoding.UTF8.GetString(message);
            var xmlReader = XmlReader.Create(new StringReader(xmlData), new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore });
            return XDocument.Load(xmlReader);
        }

        protected override byte[] EncodeBody<T>(T message) {
            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);
            var xmlSerializer = new XmlSerializer(typeof(T));
            xmlSerializer.Serialize(stringWriter, message);
            return Encoding.UTF8.GetBytes(stringBuilder.ToString());
        }
    }
}
