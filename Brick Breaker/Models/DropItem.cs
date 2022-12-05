using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brick_Breaker.Models
{
    public class DropItem
    {
        public enum DropItemType { LargeBall, SmallBall, FireBall, SlowBall, FastBall, LargePaddle, SmallPaddle, BonusPoints };
        public string ImageSource 
        { 
            get
            {
                string[] imageSource =
                {
                    "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/DropItemLargeBall.jpg",
                    "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/DropItemSmallBall.jpg",
                    "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/DropItemFireBall.jpg",
                    "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/DropItemSpeedDown.jpg",
                    "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/DropItemSpeedUp.jpg",
                    "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/DropItemLargePaddle.jpg",
                    "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/DropItemSmallPaddle.jpg",
                    "C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/DropItemScore.jpg"
                };

                return imageSource[(int)Type];
            }
        }
        public int X {get; set;}
        public int Y {get; set;}
        public DropItemType Type { get; set;}

        public DropItem()
        {
        }
        public DropItem(int x, int y, DropItemType type)
        {
            X = x;
            Y = y;
            Type = type;
        }
    }
}
