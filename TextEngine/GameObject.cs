using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{
    public abstract class GameObject
    {
        private GameObjectPrefab prefab;

        //Nick all properties from prefab
        #region Properties
        public Texture Texture
        {
            get => prefab.texture;
            set => prefab.texture = value;
        }

        public char Character
        {
            get => prefab.texture[0, 0].Character;
            set => prefab.texture = new(value);
        }

        public Vector2D Position
        {
            get => prefab.position;
            set => prefab.position = value;
        }

        public bool HasCollision
        {
            get => prefab.hasCollision;
            set => prefab.hasCollision = value;
        }

        public bool Invisible
        {
            get => prefab.Invisible;
            set => prefab.Invisible = value;
        }

        public Color Color
        {
            get => prefab.texture[0,0].Color;
            set => prefab.texture.SetColor(value);
        }

        public Color Highlight
        {
            get => prefab.texture[0, 0].Highlight;
            set => prefab.texture.SetHighlight(value);
        }

        public Scale Scale
        {
            get => prefab.scale;
            set => prefab.scale = value;
        }

        public uint RenderOrder
        {
            get => prefab.renderOrder;
            set => prefab.renderOrder = value;
        }
        #endregion

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
