using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO.Compression;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Globalization;

namespace Clima
{
    class Program
    {
        static void Main(string[] args)
        {
            GetRequest("https://smn.cna.gob.mx/webservices/index.php?method=1");
            Console.ReadLine();
            Console.ReadKey();
        }

        async static void GetRequest(String url)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url)) //obtener una variable con la info del url
            using (var content = await response.Content.ReadAsStreamAsync()) //obtener la info del archivo
            using (var descomprimido = new GZipStream(content, CompressionMode.Decompress)) //descomprimir el archivo
            {
                if (response.IsSuccessStatusCode)
                {
                    StreamReader reader = new StreamReader(descomprimido);
                    String data = reader.ReadLine();
                    var listInfo = JsonConvert.DeserializeObject<List<Ciudad>>(data);
                    foreach (var info in listInfo)
                    {
                        if(info.CityId.Equals("MXTS2043"))
                        {
                            Console.WriteLine("ID: " + info.CityId + "\nNombre: " + info.Name + "\nAbreviatura: " + info.StateAbbr + "\nNumero de día: " + info.DayNumber + "\nFechaUTC: " + DateTime.ParseExact(info.ValidDateUtc.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture).ToLongDateString()
                                + "\nFecha Local: " + DateTime.ParseExact(info.LocalValidDate.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture).ToLongDateString() + "\nTemperatura máxima: " + info.HiTempC + "\nTemperatura mínima " + info.LowTempC + "\nDescripción del día: " + info.PhraseDay + "\nDescripción de la noche: " + info.PhraseNight
                                + "\nDescripción del cielo: " + info.SkyText + "\nProbabilidad de precipitación: " + info.ProbabilityOfPrecip + "\nHumedad relativa: " + info.RelativeHumidity + "\nVelocidad del viento en millas: " + info.WindSpeedMph
                                + "\nVelocidad del viento en kilometros: " + info.WindSpeedKm + "\nDirección del viento: " + info.WindDirection + "\nDirección del viento en puntos cardinales: " + info.WindDirectionCardinal + "\nCobertura de nubes: " + info.CloudCoverage
                                + "\nÍndice ultravioleta: " + info.UvIndex + "\nDescripción ultravioleta: " + info.UvDescription + "\nCódigo del ícono del día: " + info.IconCode + "\nCódigo del ícono de la noche: " + info.IconCodeNight + "\nDescripción del cielo por la noche: " + info.SkyTextNight
                                + "\nLatitud: " + info.Latitude + "\nLongitud: " + info.Longitude + "\n");
                        }
                    }
                }
            }
        }
    }
}
