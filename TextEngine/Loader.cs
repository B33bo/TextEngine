using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine
{
    public static class SceneLoader
    {
        public static void LoadFromText(string[] file, Dictionary<char, Type> gameObjects)
        {
            for (int i = 0; i < file.Length; i++)
            {
                for (int j = 0; j < file[i].Length; j++)
                {
                    if (!gameObjects.TryGetValue(file[i][j], out Type gameObjectType))
                        continue;

                    GameObject gm = (GameObject)Activator.CreateInstance(gameObjectType);
                    gm.Position = new(j, i);
                    Game.AddObject(gm);
                }
            }
        }
    }
}
