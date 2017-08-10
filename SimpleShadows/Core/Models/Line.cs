using System.Xml.Serialization;

namespace SimpleShadows.Core.Models
{
    [XmlType(TypeName="line")]
    public class Line
    {
        [XmlElement(ElementName="name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "x0")]
        public float X0 { get; set; }

        [XmlElement(ElementName = "x1")]
        public float X1 { get; set; }

        [XmlElement(ElementName = "y0")]
        public float Y0 { get; set; }

        [XmlElement(ElementName = "y1")]
        public float Y1 { get; set; }
    }
}
