using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{
    public abstract class GameObject
    {
        public Texture texture;

        public char Character
        {
            get => texture[0, 0].Character;
            set => texture = new(value, Color, Highlight, Formatting);
        }

        public Vector2D Position;

        public bool HasCollision;

        public bool Invisible;

        public Color Color
        {
            get => texture[0,0].Color;
            set => texture.SetColor(value);
        }

        public Color Highlight
        {
            get => texture[0, 0].Highlight;
            set => texture.SetHighlight(value);
        }

        public TextFormatting Formatting
        {
            get => texture[0, 0].Formatting;
            set => texture.SetFormatting(value);
        }

        public Scale Scale;

        public uint RenderOrder;

        public GameObject()
        {
            //Default values
            Character = '?';
            Scale = new(1, 1);
        }

        /// <summary>
        /// The move function will move an object in any direction
        /// It has the bonus of handling physics so if there's a block in the way
        /// it won't move
        /// </summary>
        /// <param name="movementVector">Movement</param>
        public void Move(Vector2D movementVector)
        {
            Vector2D newPos = Position + movementVector;

            for (int i = 0; i < Game.GameObjects.Count; i++)
            {
                try
                {
                    if (Game.GameObjects[i] == this)
                        continue;

                    Vector2D OtherPos = Game.GameObjects[i].Position;
                    Vector2D OtherPosBottomRight = OtherPos + Game.GameObjects[i].Scale;

                    if (!Scale.IntersectsWith((newPos, newPos + Scale), (OtherPos, OtherPosBottomRight)))
                        continue;

                    //Object in the way

                    Game.GameObjects[i].OnCollision(this, Game.GameObjects[i].Position - Position);

                    //OnCollision() could remove the object
                    if (Game.GameObjects.Count <= i)
                        continue;

                    OnCollision(Game.GameObjects[i], Position - Game.GameObjects[i].Position);

                    //OnCollision() could remove the object
                    if (Game.GameObjects.Count <= i)
                        continue;

                    if (Game.GameObjects[i].HasCollision && HasCollision)
                        //Both objects have collision
                        return;
                }
                catch (NullReferenceException)
                {
                    continue;
                }
            }

            Position += movementVector;
        }

        public void Destroy()
        {
            Game.GameObjects.Remove(this);
        }

        public abstract void OnCollision(GameObject collision, Vector2D displacement);

        public abstract void Update();

        public abstract void KeyPress(ConsoleKey key);
    }
}
