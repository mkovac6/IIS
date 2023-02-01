using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Marko_Kovacevi_API
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/Marko_Kovacevic_iis1.Model")]
    public class PredavacArray
    {
        [DataMember(Order = 0)]
        public List<Predavac> PredavacList { get; set; }

        public PredavacArray()
        {

        }

        public PredavacArray(List<Predavac> predavacList)
        {
            PredavacList = predavacList;
        }

        [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/Marko_Kovacevic_iis1.Model")]
        public class Predavac
        {
            public Predavac(string id, string type, string name, double placa)
            {
                Id = id;
                Type = type;
                Name = name;
                Placa = placa;
            }
            public Predavac()
            {

            }
            [DataMember(Order = 0)]
            public string Id { get; set; }
            [DataMember(Order = 1)]
            public string Type { get; set; }
            [DataMember(Order = 2)]
            public string Name { get; set; }
            [DataMember(Order = 3)]
            public double Placa { get; set; }
        }
    }
}
