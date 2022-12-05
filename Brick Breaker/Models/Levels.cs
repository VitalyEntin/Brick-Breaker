using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brick_Breaker.Models
{
    public class GameLevel
    {
        public List<Brick> Bricks = new List<Brick>();
        public double SpeedUpRate;
        public int ID;

        public GameLevel(List<Brick> bricks, double speedUpRate, int id)
        {
            Bricks = bricks;
            SpeedUpRate = speedUpRate;
            ID = id;
        }
    }

    public class GameLevelFactory
    {
        public List<GameLevel> Levels = new List<GameLevel>();

        public GameLevelFactory()
        {
            AddLevel1();
            AddLevel2();
        }
        public void AddLevel1()
        {
            List<Brick> bricks = new List<Brick>();
            string imageSource = "";

            for (int i = GameSettings.Rows - 1; i > 15; i--)
            {
                for (int j = 0; j < GameSettings.Columns; j++)
                {
                    if (i % 3 == 0) imageSource = "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/OrangeBrick.jpg";
                    if (i % 3 == 1) imageSource = "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/YellowBrick.jpg";
                    if (i % 3 == 2) imageSource = "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/RedBrick.jpg";
                    bricks.Add(new Brick(i, j, Brick.BrickType.Regular, imageSource, null));
                }
            }

            Levels.Add(new GameLevel(bricks, 0.01, 1));
        }
        public void AddLevel2()
        { 
            var bricks = new List<Brick>();
            string imageSource = "";
            string crackedImageSource = "";
            Brick.BrickType brickType = Brick.BrickType.Regular;

            for (int i = 0; i <6; i++)
            {
                for (int j = GameSettings.Columns - (i+1); j >= i; j--)
                {
                    if (i % 3 == 0)
                    {
                        imageSource = "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/OrangeBrick.jpg";
                        crackedImageSource = "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/OrangeBrickCracked.jpg";
                        brickType = Brick.BrickType.Regular;
                    }
                    if (i % 3 == 1)
                    {
                        imageSource = "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/YellowBrick.jpg";
                        crackedImageSource = "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/YellowBrickCracked.jpg";
                        brickType = Brick.BrickType.Regular;
                    }
                    if (i % 3 == 2)
                    {
                        imageSource = "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/RedBrick.jpg";
                        crackedImageSource = "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/RedBrickCracked.jpg";
                        brickType= Brick.BrickType.TwoHits;
                    }
                    bricks.Add(new Brick(GameSettings.Rows- 5+i,  j, brickType, imageSource, crackedImageSource));
                }
            }
            for (int i = 7; i <= 11; i++)
            {
                for (int j = i-6; j < GameSettings.Columns - i+6; j++)
                {
                    if (i % 3 == 0)
                    {
                        imageSource = "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/OrangeBrick.jpg";
                        crackedImageSource = "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/OrangeBrickCracked.jpg";
                        brickType = Brick.BrickType.Regular;
                    }
                    if (i % 3 == 1)
                    {
                        imageSource = "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/YellowBrick.jpg";
                        crackedImageSource = "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/YellowBrickCracked.jpg";
                        brickType = Brick.BrickType.Regular;
                    }
                    if (i % 3 == 2)
                    {
                        imageSource = "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/RedBrick.jpg";
                        crackedImageSource = "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/RedBrickCracked.jpg";
                        brickType = Brick.BrickType.TwoHits;
                    }
                    bricks.Add(new Brick(GameSettings.Rows - i+1, j, brickType, imageSource, crackedImageSource));
                }
            }

            Levels.Add(new GameLevel(bricks, 0.01, 2));
        }
    }
}
