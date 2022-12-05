using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brick_Breaker.Models
{
    public static class GameSettings
    {
        public static int BaseWidth { get; } = 90;
        public static int BaseHeight { get; } = 30;
        public static int PaddleHeight { get; } = 10;
        public static int Rows { get; } = 22;
        public static int Columns { get; } = 9;
        public static int BaseSpeed { get; } = 1;
        public static int LivesAtStart { get; } = 3;

        public static int ScreenWidth = BaseWidth * Columns;
        public static int ScreenHeight = BaseHeight * Rows + PaddleHeight;

        public static int MaxPaddleDX = BaseWidth / 4;
        public static double PaddleFriction = 0;

    }
}
