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

        public GameObjectPrefab(char Character, Vector2D pos, bool hasCollision)
        {
            this.Character = Character;
            position = pos;
            this.hasCollision = hasCollision;
            Invisible = false;
        }

        public GameObjectPrefab(char Character, Vector2D pos, bool hasCollision, bool Invisible)
        {
            this.Character = Character;
            position = pos;
            this.hasCollision = hasCollision;
            this.Invisible = Invisible;
        }
    }
}
