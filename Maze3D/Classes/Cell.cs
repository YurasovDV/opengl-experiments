using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Maze3D.Classes
{
    [XmlType("cell")]
    public class Cell
    {
        [XmlArray(ElementName="lines")]
        public Line[] Lines { get; set; }
    }
}
