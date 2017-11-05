using System.Xml.Serialization;

namespace SimpleShadows.Core.Models
{
    [XmlType("cell")]
    public class Cell
    {
        [XmlArray(ElementName="lines")]
        public Line[] Lines { get; set; }
    }
}
