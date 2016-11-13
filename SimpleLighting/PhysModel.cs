using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLighting
{
    class PhysModel
    {
        List<SimpleModel> models = new List<SimpleModel>();

        public PhysModel()
        {
            SimpleModel model = CreateWall();
            models.Add(model);
        }

        public void Tick(long delta)
        {

        }

        public SimpleModel[] GetModelsForRender()
        {
            return models.ToArray();
        }


        private static SimpleModel CreateWall()
        {
            SimpleModel model = new SimpleModel();
            model.Vertices = new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(10, 0, 0),
                new Vector3(10, 10, 0),

                new Vector3(0, 0, 0),
                new Vector3(10, 10, 0),
                new Vector3(0, 10, 0),
            };

            model.Color = new Vector3[]
            {
                new Vector3(100, 0, 0),
                new Vector3(100, 0, 0),
                new Vector3(100, 0, 0),
                new Vector3(100, 0, 0),
                new Vector3(100, 0, 0),
                new Vector3(100, 0, 0),
            };

            model.Normals = new Vector3[]
            {
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                           
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 1),
            };
            return model;
        }

    }
}
