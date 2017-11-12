using System.Xml.Serialization;

namespace MazeTextured.Core.Models
{
    [XmlType("cell")]
    public class Cell
    {
        [XmlArray(ElementName="lines")]
        public Line[] Lines { get; set; }
    }
}
