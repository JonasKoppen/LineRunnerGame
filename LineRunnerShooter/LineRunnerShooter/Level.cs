using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
{
    class Level : ICollide
    {
        public int xDim = 12;
        private int yDim = 6;
        private Rectangle endCollsion = new Rectangle(500, 0, 10, 500);

        public int[,] tileArray;

        private Block[,] blockArray;

        public Level(){
            xDim = 12;
            yDim = 6;
            blockArray = new Block[xDim, yDim];
            }

        public Level(Texture2D map, List<Texture2D> texture)
        {   //gebruik Texture 2D colors array als level editor: https://stackoverflow.com/questions/10127871/how-can-i-read-image-pixels-values-as-rgb-into-2d-array // http://www.riemers.net/eng/Tutorials/XNA/Csharp/Series2D/Texture_to_Colors.php
            xDim = map.Width;
            yDim = map.Height;
            tileArray = new int[xDim, yDim];
            blockArray = new Block[xDim, yDim];
            Color[] colors1D = new Color[map.Width * map.Height];
            map.GetData(colors1D);
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    tileArray[x, y] = 0;
                    String colorCode = colors1D[x + y * map.Width].R.ToString() + " "
                        + colors1D[x + y * map.Width].G.ToString() + "";
                    //+ colors1D[x + y * map.Width].B.ToString() + ""; // IDEE: gebruik blauw als extra informatie om mee te geven aan de builder (offset data) Alfa is ook bruikbaar
                    int B = colors1D[x + y * map.Width].B;
                    int alfa = colors1D[x + y * map.Width].A;
                    Console.WriteLine(colorCode);
                    switch (colorCode)
                    {
                        case "255 255":
                            {
                                // white pixel
                                break;
                            }
                        case "0 0":
                            {
                                blockArray[x, y] = new Block(texture[B], new Vector2(x * 100, (y * 50)),new Vector2(100,50));
                                if (B == 2)
                                {
                                    blockArray[x, y] = new Lava(texture[B], new Vector2(x * 100, (y * 50)));
                                }
                                else
                                {
                                    tileArray[x, y] = 1;

                                }
                                break;
                            }
                        case "0 255":
                            {
                                blockArray[x, y] = new BlockRed(texture[1], new Vector2(x * 100, (y * 50)));
                                break;
                            }
                        case "72 0":
                            {
                                blockArray[x, y] = new BlockPurple(texture[1], new Vector2(x * 100, (y * 50)));
                                break;
                            }
                        case "100 0":
                            {
                                blockArray[x, y] = new MovingPlatform(texture[0], new Vector2(x * 100, (y * 50)), B, 255 - alfa);
                                break;
                            }
                    }
                }
            }

            for (int x = 1; x < map.Width-1; x++)
            {
                for (int y = 1; y < map.Height-1; y++)
                {
                    if(tileArray[x,y] == 1)
                    {
                        if(tileArray[x+1, y] == 1 && tileArray[x-1, y] == 1 && tileArray[x, y+1] == 1 && tileArray[x, y-1] == 1) //Middle
                        {
                            blockArray[x, y] = new Block(texture[0], new Vector2(x * 100, (y * 50)), new Vector2(100,50));
                        }
                        else if (tileArray[x + 1, y] == 0 && tileArray[x - 1, y] == 1 && tileArray[x, y + 1] == 1 && tileArray[x, y - 1] == 1) //Right
                        {
                            blockArray[x, y] = new Block(texture[0], new Vector2(x * 100, (y * 50)), new Vector2(200, 50));
                        }
                        else if (tileArray[x + 1, y] == 1 && tileArray[x - 1, y] == 0 && tileArray[x, y + 1] == 1 && tileArray[x, y - 1] == 1) // left
                        {
                            blockArray[x, y] = new Block(texture[0], new Vector2(x * 100, (y * 50)), new Vector2(0, 50));
                        }
                        else if (tileArray[x + 1, y] == 1 && tileArray[x - 1, y] == 1 && tileArray[x, y + 1] == 1 && tileArray[x, y - 1] == 0) //UP
                        {
                            blockArray[x, y] = new Block(texture[0], new Vector2(x * 100, (y * 50)), new Vector2(100, 0));
                        }
                        else if (tileArray[x + 1, y] == 1 && tileArray[x - 1, y] == 0 && tileArray[x, y + 1] == 1 && tileArray[x, y - 1] == 0) //UP LEFT
                        {
                            blockArray[x, y] = new Block(texture[0], new Vector2(x * 100, (y * 50)), new Vector2(0, 0));
                        }
                        else if (tileArray[x + 1, y] == 0 && tileArray[x - 1, y] == 1 && tileArray[x, y + 1] == 1 && tileArray[x, y - 1] == 0) //UP RIGht
                        {
                            blockArray[x, y] = new Block(texture[0], new Vector2(x * 100, (y * 50)), new Vector2(200, 0));
                        }
                        else
                        {   
                            blockArray[x, y] = new Block(texture[0], new Vector2(x * 100, (y * 50)), new Vector2(100, 50));   
                        }
                    }
                }
            }
        }


        public void CreateWorld(Texture2D texture,Texture2D texture2)
        {
            for(int x = 0; x < xDim; x++)
            {
                
                for(int y = 0; y <yDim; y++) //Switch Case HERE
                {
                    if(tileArray[x,y] == 1)
                    {
                        blockArray[x, y] = new Block(texture, new Vector2(x * 100, (y * 50)));
                    }
                    if (tileArray[x, y] == 2)
                    {
                        blockArray[x, y] = new BlockRed(texture2, new Vector2(x * 100, (y * 50)));
                    }
                    if (tileArray[x, y] == 3)
                    {
                        blockArray[x, y] = new BlockPurple(texture2, new Vector2(x * 100, (y * 50)));
                    }
                }
            }
        }

        public void Update(GameTime gameTime, Rectangle player)
        {
            foreach(Block b in blockArray)
            {
                if(b is BlockPurple)
                {
                    BlockPurple bp = b as BlockPurple;
                    bp.Update(gameTime, player);
                }
                if (b is BlockRed)
                {
                    BlockRed br = b as BlockRed;
                    br.Update(gameTime);
                }
                if(b is MovingPlatform)
                {
                    MovingPlatform mp = b as MovingPlatform;
                    mp.Update(gameTime,player,5);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, int startX, int startY)
        {
            int sX = startX / 50;
            int sY = startY / 50;
            for (int x = sX; x < xDim; x++)
            {
                for (int y = sY; y < yDim; y++)
                {
                    if (blockArray[x, y] != null)
                    {
                        int posX = ((x - sX) * 50) - startX % 50;
                        int posY = ((y - sY) * 50 )+ 100 - startY%50;
                        blockArray[x, y].Draw(spriteBatch);
                    }
                }
            }
        }

        public Rectangle getCollisionRectagle()
        {
            throw new NotImplementedException();
        }

        public bool checkCollision(Rectangle user)
        {
            bool collision = false;
            bool col = false;
            Rectangle world;
            foreach(Block b in blockArray)
            {
                if(b != null)
                {
                    world = b.getCollisionRectagle();
                    collision = world.Intersects(user);
                    if(collision)
                    {
                        col = true;
                    }
                }
                
            }
            return col;
        }

        public List<Rectangle> getRectangles() //geeft de lijst met collisionRectangle terug rond positie x, dit is mss voor later
        {
            List<Rectangle> blocks = new List<Rectangle>();
            //if (x < 3) { x = 3; }
            //if (x < blockArray.GetLength(0)) { x = blockArray.GetLength(0) - 4; }
            for(int i = 0; i < blockArray.GetLength(0); i++)
            {
                for(int j = 0; j < blockArray.GetLength(1); j++)
                {
                    if(blockArray[i,j] != null)
                    {
                        blocks.Add(blockArray[i, j].getCollisionRectagle());
                    }
                    
                }
            }
            return blocks;
        }
    }
}
