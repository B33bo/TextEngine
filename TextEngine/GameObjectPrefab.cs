using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{
    public struct GameObjectPrefab
    {
        public char Character;
        public Vector2D position;
        public bool hasCollision;
        public bool Invisible;

        public byte Color;
        public byte Highlight;

        public Scale scale;

        public GameObjectPrefab(char Character, Vector2D pos, bool hasCollision)
        {
            this.Character = Character;
            position = pos;
            this.hasCollision = hasCollision;
            Invisible = false;
            Color = 0;
            Highlight = 0;
            scale = new(1, 1);
        }

        public GameObjectPrefab(char Character, Vector2D pos, bool hasCollision, bool Invisible)
        {
            this.Character = Character;
            position = pos;
            this.hasCollision = hasCollision;
            this.Invisible = Invisible;
            Color = 0;
            Highlight = 0;
            scale = new(1, 1);
        }

        public GameObjectPrefab(char Character, Vector2D pos, bool hasCollision, bool Invisible, byte color, byte highlight)
        {
            this.Character = Character;
            position = pos;
            this.hasCollision = hasCollision;
            this.Invisible = Invisible;
            Color = color;
            Highlight = highlight;
            scale = new(1, 1);
        }
    }
}
