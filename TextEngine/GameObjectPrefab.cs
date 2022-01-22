using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{
    public struct GameObjectPrefab
    {
        //public char Character;      //The look of the object
        public Texture texture;
        public bool Invisible;      //Will not be rendered

        public byte Color;          //The color
        public byte Highlight;      //The highlight

        public Vector2D position;   //The position
        public Scale scale;         //The Scale

        public bool hasCollision;   //If this is true, it cannot interact with other collision objects

        public GameObjectPrefab(Texture texture, Vector2D pos, bool hasCollision)
        {
            this.texture = texture;
            position = pos;
            this.hasCollision = hasCollision;
            Invisible = false;
            Color = 0;
            Highlight = 0;
            scale = new(1, 1);
        }

        public GameObjectPrefab(Texture texture, Vector2D pos, bool hasCollision, bool Invisible)
        {
            this.texture = texture;
            position = pos;
            this.hasCollision = hasCollision;
            this.Invisible = Invisible;
            Color = 0;
            Highlight = 0;
            scale = new(1, 1);
        }

        public GameObjectPrefab(Texture texture, Vector2D pos, bool hasCollision, bool Invisible, byte color, byte highlight)
        {
            this.texture = texture;
            position = pos;
            this.hasCollision = hasCollision;
            this.Invisible = Invisible;
            Color = color;
            Highlight = highlight;
            scale = new(1, 1);
        }
    }
}
