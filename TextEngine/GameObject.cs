﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{
    public abstract class GameObject
    {
        private GameObjectPrefab prefab;

        #region Properties
        public char Character
        {
            get => prefab.Character;
            set => prefab.Character = value;
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

        public byte Color
        {
            get => prefab.Color;
            set => prefab.Color = value;
        }

        public byte Highlight
        {
            get => prefab.Highlight;
            set => prefab.Highlight = value;
        }

        public Scale Scale
        {
            get => prefab.scale;
            set => prefab.scale = value;
        }
        #endregion

        public GameObject()
        {
            Character = '?';
            Scale = new(1, 1);
        }

        public GameObject(char Char)
        {
            Character = Char;
        }

        public GameObject(char Char, Vector2D pos)
        {
            Character = Char;
            Position = pos;
        }

        public GameObject(char Char, Vector2D pos, bool HasCollision)
        {
            Character = Char;
            Position = pos;
            this.HasCollision = HasCollision;
        }

        public GameObject(GameObjectPrefab prefab)
        {
            Character = prefab.Character;
            Position = prefab.position;
            HasCollision = prefab.hasCollision;
        }

        public void Move(Vector2D movementVector)
        {
            Vector2D newPos = Position + movementVector;

            for (int i = 0; i < Game.gameObjects.Count; i++)
            {
                if (Game.gameObjects[i] == this)
                    continue;

                Vector2D OtherPos = Game.gameObjects[i].Position;
                Vector2D OtherPosBottomRight = OtherPos + Game.gameObjects[i].Scale;

                if (!Scale.IntersectsWith((newPos, newPos + Scale), (OtherPos, OtherPosBottomRight)))
                    continue;

                //Object in the way

                Game.gameObjects[i].OnCollision(this, Game.gameObjects[i].Position - Position);

                //OnCollision() could remove the object
                if (Game.gameObjects.Count <= i)
                    continue;

                OnCollision(Game.gameObjects[i], Position - Game.gameObjects[i].Position);

                //OnCollision() could remove the object
                if (Game.gameObjects.Count <= i)
                    continue;

                if (Game.gameObjects[i].HasCollision && HasCollision)
                    //Both objects have collision
                    return;
            }

            Position += movementVector;
        }

        public void Destroy()
        {
            Game.gameObjects.Remove(this);
        }

        public abstract void OnCollision(GameObject collision, Vector2D displacement);

        public abstract void Update();

        public abstract void KeyPress(ConsoleKey key);
    }
}
