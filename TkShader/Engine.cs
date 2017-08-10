using System;
using System.Drawing;
using OpenTK;

namespace ShaderOnForm
{
    public class Engine
    {
        protected AbstractVehicle vehicle;
        public HeightMap Map { get; protected set; }
        public RectangleF MapRectangle { get; set; }
        public GraphicsManager gr { get; set; }
        public AbstractVehicle Vehicle
        {
            get
            {
                return vehicle;
            }
            set
            {
                value.PointInField = this.PointInField;
                vehicle = value;
            }
        }

        public KeyHandler KeyHandler { get; set; }

        public Engine(GraphicsManager graph, int size = Constants.MAP_SIZE)
        {
            //Size = size;
            Map = MapGenerator.Generate(size);
            graph.Map = Map;
            MapRectangle = new RectangleF(0f, 0f, (float)(size * Constants.CELL_SIZE), (float)(size * Constants.CELL_SIZE));
            gr = graph;
            graph.Engine = this;
        }

        public virtual void Tick()
        {
            if (KeyHandler == null)
            {
                throw new NullReferenceException("keyHandler == null");
            }
            KeyHandler.CheckKeys();

            Vector3[] normals;
            var points = Map.GetPoints(out normals);
            gr.RenderCall(points, normals);
        }

        public virtual void ProcessKey(InputSignal signal)
        {
            Vehicle.Move(signal);
        }

        /// <summary>
        /// в этом класе не используется
        /// </summary>
        /// <param name="signal"></param>
        protected virtual void UpdateAcceleration(InputSignal signal)
        {
            
        }

        public bool PointInField(float x, float z)
        {
            return MapRectangle.Contains((int)x, (int)z);
        }
    }
}
