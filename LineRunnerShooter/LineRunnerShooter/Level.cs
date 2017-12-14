﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRunnerShooter
{
    /*
     * The level class is the heart of the game, it build everything from an image file, pixel per pixel, well 1 pixel = 1 block, look to the switch case for wich color wich block is
     * Maybe I will add enemy placement, but it wil come after the game is in beta form (which it is not, it is alfa, i think, it is playable, but misses a lot and some code are a bit to big (to much for 1 method)
     * 
     */ 
     /*
    class Level
    {
        public int xDim = 12;
        private int yDim = 6;

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
                    //Console.WriteLine(colorCode);
                    switch (colorCode)
                    {
                        case "0 0":
                            {
                                blockArray[x, y] = new Block(0, new Vector2(x * 100, (y * 100)), new Vector2(100, 100));
                                if (B == 2)
                                {
                                    blockArray[x, y] = new Lava(2, new Vector2(x * 100, (y * 100)));
                                }
                                else if (B == 0)
                                {
                                    tileArray[x, y] = 1;
                                }
                                else if (B == 100)
                                {
                                    tileArray[x, y] = 2;
                                }
                                else if (B == 200)
                                {
                                    tileArray[x, y] = 3;
                                }
                                break;
                            }
                        case "0 255":
                            {
                                blockArray[x, y] = new BlockRed( new Vector2(x * 100, (y * 100)));
                                break;
                            }
                        case "72 0":
                            {
                                blockArray[x, y] = new BlockPurple( new Vector2(x * 100, (y * 100)));
                                break;
                            }
                        case "1 25":
                            {
                                blockArray[x, y] = new Target(1, new Vector2(x * 100, (y * 100)), 5);
                                break;
                            }
                        default:
                            {
                                //Just do nothing
                                break;
                            }
                        
                    }
                }
            }
            Random r = new Random();
            for (int x = 1; x < map.Width-1; x++)
            {
                for (int y = 1; y < map.Height-1; y++)
                {
                    if(tileArray[x,y] != 0)
                    {
                        if(tileArray[x+1, y] != 0 && tileArray[x-1, y] != 0 && tileArray[x, y-1] != 0) //Center
                        {
                            if(tileArray[x,y] == 1)
                            {
                                blockArray[x, y] = new Block(0, new Vector2(x * 100, (y * 100)), new Vector2(300 + (r.Next(0, 4) * 100), 0));
                            }
                            else
                            {
                                blockArray[x, y] = new Block(0, new Vector2(x * 100, (y * 100)), new Vector2(400, (100 * (tileArray[x, y] - 1))));
                            }
                        }
                        else if (tileArray[x + 1, y] != 0 && tileArray[x - 1, y] == 0 && tileArray[x, y - 1] != 0) //side Left
                        {
                            blockArray[x, y] = new Block(0, new Vector2(x * 100, (y * 100)), new Vector2(300, (100 * (tileArray[x, y] - 1))));
                        }
                        else if (tileArray[x + 1, y] == 0 && tileArray[x - 1, y] != 0 && tileArray[x, y - 1] != 0) //side Right
                        {
                            blockArray[x, y] = new Block(0, new Vector2(x * 100, (y * 100)), new Vector2(500, (100 * (tileArray[x, y] - 1))));
                        }
                        else if (tileArray[x + 1, y] != 0 && tileArray[x - 1, y] == 0 && tileArray[x, y - 1] == 0) //Top left
                        {
                            blockArray[x, y] = new Block(0, new Vector2(x * 100, (y * 100)), new Vector2(0, (100 * (tileArray[x, y] - 1))));
                        }
                        else if (tileArray[x + 1, y] != 0 && tileArray[x - 1, y] != 0 && tileArray[x, y - 1] == 0) //Top Middle
                        {
                            blockArray[x, y] = new Block(0, new Vector2(x * 100, (y * 100)), new Vector2(100, (100 * (tileArray[x, y] - 1))));
                        }
                        else if (tileArray[x + 1, y] == 0 && tileArray[x - 1, y] != 0 && tileArray[x, y - 1] == 0) //Top Right
                        {
                            blockArray[x, y] = new Block(0, new Vector2(x * 100, (y * 100)), new Vector2(200, (100 * (tileArray[x, y] - 1))));
                        }

                    }
                }
            }
        }


        

        public void Update(GameTime gameTime, User user)
        {
            foreach(Block b in blockArray)
            {
                if(b is BlockPurple)
                {
                    BlockPurple bp = b as BlockPurple;
                    bp.Update(gameTime, user.getFeetCollisionRect());
                    bp.getPosChange(user);
                }
                if (b is BlockRed)
                {
                    BlockRed br = b as BlockRed;
                    br.Update(gameTime);
                }
                if(b is Lava)
                {
                    Lava bl = b as Lava;
                    bl.Update(gameTime);
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
    */

    class LevelControl //handels everything
    {
        public int xDim = 12;
        private int yDim = 6;

        public int[,] tileArray;

        private Block[,] blockArray;

        public LevelControl()
        {
            xDim = 12;
            yDim = 6;
            blockArray = new Block[xDim, yDim];
        }

        public LevelControl(Texture2D map, List<Texture2D> texture, List<Orih> enemys)
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
                    //Console.WriteLine(colorCode);
                    switch (colorCode)
                    {
                        case "0 0":
                            {
                                blockArray[x, y] = new Block(0, new Vector2(x * 100, (y * 100)), new Vector2(100, 100));
                                if (B == 2)
                                {
                                    blockArray[x, y] = new Lava(2, new Vector2(x * 100, (y * 100)));
                                }
                                else if (B == 0)
                                {
                                    tileArray[x, y] = 1;
                                }
                                else if (B == 100)
                                {
                                    tileArray[x, y] = 2;
                                }
                                else if (B == 200)
                                {
                                    tileArray[x, y] = 3;
                                }
                                break;
                            }
                        case "0 255":
                            {
                                blockArray[x, y] = new BlockRed( new Vector2(x * 100, (y * 100)));
                                break;
                            }
                        case "72 0":
                            {
                                blockArray[x, y] = new BlockPurple( new Vector2(x * 100, (y * 100)));
                                break;
                            }
                        case "220 255":
                            {
                                blockArray[x, y] = new Target(2, new Vector2(x * 100, (y * 100)), 5);
                                break;
                            }
                        case "255 10":
                            {
                                enemys.Add(new Orih(General._afbeeldingEnemys[6], General._afbeeldingEnemys[7], new Rectangle(0, 0, 100, 200), new RobotMove(), General._afbeeldingEnemys[2], General._afbeeldingEnemys[3], new Vector2(x*100,y*100))); //ADDED some ducktape here, probably should remove the general and move it more down insede the class
                                break;
                            }
                        default:
                            {
                                //Do nothing
                                break;
                            }
                    }
                }
            }
            Random r = new Random();
            for (int x = 1; x < map.Width - 1; x++)
            {
                for (int y = 1; y < map.Height - 1; y++)
                {
                    if (tileArray[x, y] != 0)
                    {
                        if (tileArray[x + 1, y] != 0 && tileArray[x - 1, y] != 0 && tileArray[x, y - 1] != 0) //Center
                        {
                            if (tileArray[x, y] == 1)
                            {
                                blockArray[x, y] = new Block(0, new Vector2(x * 100, (y * 100)), new Vector2(300 + (r.Next(0, 4) * 100), 0));
                            }
                            else
                            {
                                blockArray[x, y] = new Block(0, new Vector2(x * 100, (y * 100)), new Vector2(400, (100 * (tileArray[x, y] - 1))));
                            }
                        }
                        else if (tileArray[x + 1, y] != 0 && tileArray[x - 1, y] == 0 && tileArray[x, y - 1] != 0) //side Left
                        {
                            blockArray[x, y] = new Block(0, new Vector2(x * 100, (y * 100)), new Vector2(300, (100 * (tileArray[x, y] - 1))));
                        }
                        else if (tileArray[x + 1, y] == 0 && tileArray[x - 1, y] != 0 && tileArray[x, y - 1] != 0) //side Right
                        {
                            blockArray[x, y] = new Block(0, new Vector2(x * 100, (y * 100)), new Vector2(500, (100 * (tileArray[x, y] - 1))));
                        }
                        else if (tileArray[x + 1, y] != 0 && tileArray[x - 1, y] == 0 && tileArray[x, y - 1] == 0) //Top left
                        {
                            blockArray[x, y] = new Block(0, new Vector2(x * 100, (y * 100)), new Vector2(0, (100 * (tileArray[x, y] - 1))));
                        }
                        else if (tileArray[x + 1, y] != 0 && tileArray[x - 1, y] != 0 && tileArray[x, y - 1] == 0) //Top Middle
                        {
                            blockArray[x, y] = new Block(0, new Vector2(x * 100, (y * 100)), new Vector2(100, (100 * (tileArray[x, y] - 1))));
                        }
                        else if (tileArray[x + 1, y] == 0 && tileArray[x - 1, y] != 0 && tileArray[x, y - 1] == 0) //Top Right
                        {
                            blockArray[x, y] = new Block(0, new Vector2(x * 100, (y * 100)), new Vector2(200, (100 * (tileArray[x, y] - 1))));
                        }

                    }
                }
            }
        }

        public void Update(GameTime gameTime, User user)
        {
            Hiro2 held = user as Hiro2;
            foreach (Block b in blockArray)
            {
                if(b != null)
                {
                    foreach (BulletBlueprint bullet in held.getBullets())
                    {
                        if (bullet.CollisionRect.Intersects(b.getCollisionRectagle()))
                        {
                            bullet.hitTarget();
                            if (b is Target)
                            {
                                (b as Target).hitTarget();
                            }
                        }
                    }
                    if (b is BlockPurple)
                    {
                        (b as BlockPurple).Update(gameTime, user.getFeetCollisionRect());
                        (b as BlockPurple).getPosChange(user);
                    }
                    if (b is BlockRed)
                    {
                        (b as BlockRed).Update(gameTime);
                    }
                    if (b is Lava)
                    {
                        (b as Lava).Update(gameTime);
                    }
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
                        int posY = ((y - sY) * 50) + 100 - startY % 50;
                        blockArray[x, y].Draw(spriteBatch);
                    }
                }
            }
        }

        public bool checkCollision(Rectangle user)
        {
            bool collision = false;
            bool col = false;
            Rectangle world;
            foreach (Block b in blockArray)
            {
                if (b != null)
                {
                    world = b.getCollisionRectagle();
                    collision = world.Intersects(user);
                    if (collision)
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
            for (int i = 0; i < blockArray.GetLength(0); i++)
            {
                for (int j = 0; j < blockArray.GetLength(1); j++)
                {
                    if (blockArray[i, j] != null)
                    {
                        if(!(blockArray[i,j] is Target))
                        {
                            blocks.Add(blockArray[i, j].getCollisionRectagle());
                        }
                        
                    }

                }
            }
            return blocks;
        }

        public int getPoints()
        {
            int score = 0;
            foreach(Block t in blockArray)
            {
                if(t is Target)
                {
                    score += (t as Target).getPoints();
                }
                
            }
            return score;
        }
    }
}
