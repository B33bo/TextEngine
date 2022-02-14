using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{
    public class GameObject
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
                    GameObject other = Game.GameObjects[i];

                    if (other == this)
                        continue;
                    if (other is null)
                        continue;

                    Vector2D OtherPos = other.Position;
                    Vector2D OtherPosBottomRight = OtherPos + other.Scale;

                    bool CurrentlyIntersectsWith = Scale.IntersectsWith((Position, Position + Scale), (OtherPos, OtherPosBottomRight));
                    bool WillIntersectWith = Scale.IntersectsWith((newPos, newPos + Scale), (OtherPos, OtherPosBottomRight));

                    if (CurrentlyIntersectsWith && !WillIntersectWith)
                    {
                        other.ExitCollision(this, other.Position - Position);
                        if (this is null) //all blue :o
                            continue;
                        ExitCollision(other, Position - other.Position);
                    }

                    if (!WillIntersectWith)
                        continue;

                    if (CurrentlyIntersectsWith)
                        continue;

                    //Object in the way

                    other.OnCollision(this, other.Position - Position);

                    //OnCollision() could remove the object
                    if (other is null)
                        continue;

                    OnCollision(other, Position - other.Position);

                    //OnCollision() could remove the object
                    if (other is null)
                        continue;

                    if (other.HasCollision && HasCollision)
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

        public virtual void OnCollision(GameObject collision, Vector2D displacement) { }

        public virtual void ExitCollision(GameObject collision, Vector2D displacement) { }

        public virtual void Update() { }

        public virtual void KeyPress(ConsoleKey key) { }
    }
}
