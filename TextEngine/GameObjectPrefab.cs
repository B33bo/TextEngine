using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{
    public struct GameObjectPrefab
    {
        public Texture texture;
        public bool Invisible;      //Will not be rendered

        public Vector2D position;   //The position
        public Scale scale;         //The Scale

        public bool hasCollision;   //If this is true, it cannot interact with other collision objects

        public GameObjectPrefab(Texture texture, Vector2D pos, bool hasCollision)
        {
            this.texture = texture;
            position = pos;
            this.hasCollision = hasCollision;
            Invisible = false;
            scale = new(1, 1);
        }

        public GameObjectPrefab(Texture texture, Vector2D pos, bool hasCollision, bool Invisible)
        {
            this.texture = texture;
            position = pos;
            this.hasCollision = hasCollision;
            this.Invisible = Invisible;
            scale = new(1, 1);
        }
    }
}
