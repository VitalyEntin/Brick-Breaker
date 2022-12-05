using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brick_Breaker.Models
{
    public class Brick
    {
        public enum BrickType { Regular, TwoHits };

        public int Row { get; set; }
        public int Col { get; set; }
        public BrickType Type { get; }
        public string ImageSource { get; }
        public string? CrackedImageSource { get; }
        public bool IsBroken { get; set; }
        public bool IsCracked { get; set; }
        public int X1 {  get { return Col * GameSettings.BaseWidth; } }
        public int X2 {  get {  return (Col + 1) * GameSettings.BaseWidth; } }
        public int Y1 {  get { return Row * GameSettings.BaseHeight+GameSettings.PaddleHeight; } }
        public int Y2 {  get { return (Row + 1) * GameSettings.BaseHeight + GameSettings.PaddleHeight; } }
        public Brick(int row, int col, BrickType type, string imageSource, string? crackedImageSource)
        {
            Row = row;
            Col = col;
            Type = type;
            IsBroken = false;
            IsCracked = false;
            ImageSource = imageSource;
            CrackedImageSource = crackedImageSource;
        }
    }
}
