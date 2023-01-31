using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;
using static SOAPIIS.PredavacArray;

namespace SOAPIIS
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        PredavacArray predArray = new PredavacArray();
        List<Predavac> predavacList = new List<Predavac>();
        Predavac pred1 = new Predavac("1", "Visi predavac", "Marko", 8000.145);
        Predavac pred2 = new Predavac("2", "Nositelj kolegija", "Ana", 10000.177);
        Predavac pred3 = new Predavac("3", "Predavac", "Marta", 14784.4154);
        [WebMethod]

        public List<Predavac> EntitySearch(string word)
        {
            predavacList.Add(pred1);
            predavacList.Add(pred2);
            predavacList.Add(pred3);
            predArray.PredavacList = predavacList;

            DataContractSerializer dcs = new DataContractSerializer(typeof(PredavacArray));
            string path = @"C:\Users\marko\Desktop\School\IIS\IIS\Marko_Kovacevic_IIS\Marko_Kovacevic_IIS\PredavacSOAP.xml";
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                dcs.WriteObject(fs, predArray);
                fs.Close();
            }

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(path);
            XmlNode root = xmldoc.DocumentElement;

            XmlNamespaceManager manager = new XmlNamespaceManager(xmldoc.NameTable);
            manager.AddNamespace("ns", "http://schemas.datacontract.org/2004/07/Marko_Kovacevic_iis1.Model");

            XmlNodeList list = root.SelectNodes("/ns:PredavacArray/ns:PredavacList/ns:Predavac[ns:Type='" + word + "']", manager);

            List<Predavac> result = new List<Predavac>();

            for (int i = 0; i < list.Count; i++)
            {
                var currentNodeXml = "<Predavac manager=\"http://schemas.datacontract.org/2004/07/Marko_Kovacevic_iis1.Model\">" + list[i].InnerXml + "</Predavac>";
                Stream ms = new MemoryStream(Encoding.UTF8.GetBytes(currentNodeXml));

                DataContractSerializer ds = new DataContractSerializer(typeof(Predavac));
                result.Add((Predavac)ds.ReadObject(ms));
            }

            return result;
        }
    }
}
