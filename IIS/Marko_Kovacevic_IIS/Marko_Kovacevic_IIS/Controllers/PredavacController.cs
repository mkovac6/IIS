using Commons.Xml.Relaxng;
using Marko_Kovacevic_iis.Model;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Marko_Kovacevic_iis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredavacController : ControllerBase
    {
        [HttpPost("validate-xsd")]
        public bool ValidateXsd(XmlElement predavacArray)
        {
            try
            {
                bool f = false;
                XmlDocument documentXML = predavacArray.OwnerDocument;
                documentXML.AppendChild(predavacArray);

                XmlSchemaSet schemaSet = new XmlSchemaSet();
                schemaSet.Add("http://schemas.datacontract.org/2004/07/Marko_Kovacevic_iis.Model", Path.GetFullPath("predavac_schema.xsd"));
                XmlReader xmlReader = new XmlNodeReader(documentXML);
                XDocument document = XDocument.Load(xmlReader);

                document.Validate(schemaSet, (o, e) =>
                {
                    Console.WriteLine("{0}", e.Message);
                    f = true;
                });

                if (!f)
                {
                    DataContractSerializer deserialization = new DataContractSerializer(typeof(PredavacArray));
                    MemoryStream memoryStream = new MemoryStream();
                    document.Save(memoryStream);
                    memoryStream.Position = 0;

                    PredavacArray pred = (PredavacArray)deserialization.ReadObject(memoryStream);

                    foreach (var item in pred.PredavacList)
                    {
                        Startup.PredavacArray.PredavacList.Add(item);
                    }
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }

        [HttpPost("validate-rng")]
        public bool ValidateWithRng(XmlElement predavacArray)
        {
            XmlDocument docXml = predavacArray.OwnerDocument;
            docXml.AppendChild(predavacArray);
            var errors = false;
            XmlReader xml = new XmlNodeReader(docXml);
            XmlReader rng = XmlReader.Create(Path.GetFullPath("predavac_rng.rng"));
            using (var reader = new RelaxngValidatingReader(xml, rng))
            {
                reader.InvalidNodeFound += (source, message) =>
                {
                    Console.WriteLine("Error: " + message);
                    errors = true;
                    return true;
                };
                XDocument doc = XDocument.Load(reader);
            }

            if (errors)
            {
                return false;
            }
            else
            {
                DataContractSerializer deserijalizacija = new DataContractSerializer(typeof(PredavacArray));
                MemoryStream xmlStream = new MemoryStream();
                docXml.Save(xmlStream);
                xmlStream.Position = 0;
                PredavacArray predArray = (PredavacArray)deserijalizacija.ReadObject(xmlStream);

                foreach (var item in predArray.PredavacList)
                {
                    Startup.PredavacArray.PredavacList.Add(item);
                }
                return true;
            }

        }
    }
}
