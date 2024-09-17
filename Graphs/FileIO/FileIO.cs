using System.Runtime.Serialization;
using System.Xml;

namespace GraphAlgorithmsAndVisualization.Graphs;

internal static class FileIO
{
    internal static void Serialize(string file, Graph graph)
    {
        try
        {
            using (var stream = new FileStream(file, FileMode.Create))
            {
                DataContractSerializer dcs = new(typeof(Graph));
                dcs.WriteObject(stream, graph);
            }
        }
        catch (Exception ex)
        {

        }
    }
    internal static Graph? Deserialize(string file)
    {
        Graph? graph = null;
        try
        {
            using (var stream = new FileStream(file, FileMode.Open))
            {
                XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
                DataContractSerializer dcs = new(typeof(Graph));
                graph = (Graph?)dcs.ReadObject(reader, true);
                reader.Close();
            }
            return graph;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}