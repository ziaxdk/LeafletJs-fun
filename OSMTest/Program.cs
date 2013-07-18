using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OsmSharp.Osm.Data.PBF.Raw.Processor;
using OsmSharp.Osm.Data.Raw.XML.OsmSource;
using OsmSharp.Osm.Xml;
using OsmSharp.Routing;
using OsmSharp.Routing.Route;
using OsmSharp.Tools.Xml.Sources;


namespace OSMTest
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run();

            Console.WriteLine("Done...");
            Console.ReadLine();
        }

        private void Run()
        {
            string xml = "path_to_your_osm_file.osm";

            //OsmDataSource osm = new OsmDataSource(@"C:\Users\keo\Desktop\Backup\LeafletJs\OSMTest\OSMmaps\map.osm");


            using (FileStream fs = new FileStream(@"C:\Users\keo\Desktop\Backup\LeafletJs\OSMTest\OSMmaps\denmark.osm.pbf", FileMode.Open))
            {
                OsmSharp.Osm.Data.PBF.Raw.Processor.PBFDataProcessorSource ss = new PBFDataProcessorSource(fs);
                var col = ss.PullToCollection();

            }
            // create the raw router.
            //OsmDataSource osm_data = new OsmDataSource(new Osm.Core.Xml.OsmDocument(new XmlFileSource(xml)));
            //Osm.Routing.Raw.Router raw_router = new Osm.Routing.Raw.Router(osm_data,
            //    new GraphInterpreterTime(osm_data, VehicleEnum.Car));

            //// resolve points
            //ResolvedPoint from = raw_router.Resolve(some_coordinate);
            //ResolvedPoint to = raw_router.ResolveAt(some_node2);

            //OsmSharpRoute route = raw_router.Calculate(from, to);
            //route.SaveAsGpx(new FileInfo("result.gpx"));
        }
    }
}
