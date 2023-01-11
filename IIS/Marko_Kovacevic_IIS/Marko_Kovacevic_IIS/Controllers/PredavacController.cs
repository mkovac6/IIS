using Marko_Kovacevic_iis.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
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
                schemaSet.Add("http://schemas.datacontract.org/2004/07/Marko_Kovacevic_iis1.Model", Path.GetFullPath("predavac_schema.xsd"));
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
    }
}
