using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using static Marko_Kovacevi_API.PredavacArray;
using ErgastApi.Client;
using ErgastApi.Ids;
using ErgastApi.Requests;
using ErgastApi.Responses;

namespace Marko_Kovacevi_API
{
    class Program
    {
        static string xsdValidation = "http://localhost:59255/api/Predavac/validate-xsd/";
        static string rngValidation = "http://localhost:59255/api/Predavac/validate-rng/";

        private static void PozivJava()
        {
            TcpClient tcpClient = new TcpClient("localhost", 1136);
            string path = "C:\\Users\\marko\\Desktop\\School\\IIS\\IIS\\Marko_Kovacevic_IIS\\Marko_Kovacevic_IIS\\PredavacSOAP.xml";
            byte[] bytes;
            bytes = Encoding.UTF8.GetBytes(path + "/n");

            NetworkStream networkStream = tcpClient.GetStream();
            networkStream.Write(bytes, 0, path.Length + 1);
            bytes = new byte[100];
            networkStream.Read(bytes, 0, 100);
            string odgovor = Encoding.UTF8.GetString(bytes);
            Console.WriteLine(odgovor);
        }
        static string Base64UrlEncode(string input)
        {
            var data = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(data).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

        private static async Task Formula1APIAsync()
        {

            var client = new ErgastClient();

            var request = new RaceResultsRequest
            {
                Season = Seasons.Current,
                Round = Rounds.Last,
                DriverId = Drivers.MaxVerstappen,

                Limit = 25,
                Offset = 0,
            };


            RaceResultsResponse response = await client.GetResponseAsync(request);

            var race = response.Races.First();

            Console.WriteLine("Showing up to date data for driver: " + Drivers.MaxVerstappen);
            Console.WriteLine("Race round: " + race.Round);
            Console.WriteLine("Name of the race: " + race.RaceName);
            Console.WriteLine("Name of the circuit: " + race.Circuit.CircuitName);

            var driver = race.Results[0];

            Console.WriteLine("Driver code: " + driver.Driver.Code);
            Console.WriteLine("Fastest lap number: " + driver.FastestLap.LapNumber);
            Console.WriteLine("Driver position at the end of the race: " + driver.Position);
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Pozdrav, koji app zelite pokrenuti: ");
                Console.WriteLine("1. XSD validator");
                Console.WriteLine("2. RNG validator");
                Console.WriteLine("3. Formula 1 API");
                Console.WriteLine("4. JAXB validator");
                Console.Write("Vas odabir: ");
                int odabir = 0;

                try
                {
                    odabir = int.Parse(Console.ReadLine());
                }
                catch
                {
                    continue;
                }

                switch (odabir)
                {
                    case 1:
                        PokreniZahtjev(xsdValidation);
                        break;
                    case 2:
                        PokreniZahtjev(rngValidation);
                        break;
                    case 3:
                        Formula1APIAsync();
                        break;
                    case 4:
                        PozivJava();
                        break;
                }
            }
        }

        private static void PokreniZahtjev(string path)
        {
            List<Predavac> predavacList = new List<Predavac>();
            Predavac predavac = new Predavac();

            string id;
            string type;
            string name;
            string placa;
            string nastavak = "";

            do
            {
                Console.WriteLine("Unesi id: ");
                id = Console.ReadLine();

                Console.WriteLine("Unesi type: ");
                type = Console.ReadLine();

                Console.WriteLine("Unesi ime: ");
                name = Console.ReadLine();

                Console.WriteLine("Unesi placu: ");
                placa = Console.ReadLine();
                double d = 0;
                if (!double.TryParse(placa, out d))
                {
                    Console.WriteLine("Greska...");
                    continue;
                }

                predavac.Id = id;
                predavac.Type = type;
                predavac.Name = name;
                predavac.Placa = double.Parse(placa);
                predavacList.Add(predavac);

                Console.WriteLine("Nastavi? (da | ne)");
                nastavak = Console.ReadLine();
            } while (nastavak == "da");

            PredavacArray predavacArray = new PredavacArray(predavacList);

            DataContractSerializer dataSerializer = new DataContractSerializer(typeof(PredavacArray));
            MemoryStream memoryStream = new MemoryStream();
            XmlWriter xmlWriter = XmlWriter.Create(memoryStream);
            dataSerializer.WriteObject(xmlWriter, predavacArray);

            xmlWriter.Close();

            var dataString = Encoding.UTF8.GetString(memoryStream.ToArray());
            byte[] nizBajtova = Encoding.UTF8.GetBytes(dataString);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
            request.Accept = "application/xml";
            request.Method = HttpMethod.Post.ToString();
            request.ContentType = "application/xml";
            Stream dataRequest = request.GetRequestStream();
            dataRequest.Write(nizBajtova, 0, nizBajtova.Length);
            dataRequest.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            DataContractSerializer deserialize = new DataContractSerializer(typeof(bool));
            bool success = (bool)deserialize.ReadObject(stream);

            if (success)
            {
                Console.WriteLine("Validacija uspjesna!");
            }
            else
            {
                Console.WriteLine("Validacija neuspjesna!");
            }
        }
    }
}