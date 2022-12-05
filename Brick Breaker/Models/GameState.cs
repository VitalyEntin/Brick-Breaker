using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Brick_Breaker.Models
{
    public class GameState
    {
        private Paddle _paddle;
        private Ball _ball;
        private GameLevel _level;
        private int _lives;
        private Key? _pressedKey;
        private bool _PressedN;
        private int _streak;
        private Stopwatch FireBallTimer = new Stopwatch();

        public Paddle Paddle => _paddle;
        public Ball Ball => _ball;
        public GameLevel Level => _level;
        public List<DropItem> DropItems;
        public int PaddleDirection { get; set; }
        public Key? PressedKey => _pressedKey;
        public int Lives => _lives;
        public bool IsGameOver
        {
            get { return _lives > 0 ? false : true; }
        }
        public bool IsLevelOver
        {
            get
            {
                bool isLevelOver = true;
                foreach (Brick brick in _level.Bricks)
                    if (!brick.IsBroken) isLevelOver = false;
                if (_PressedN) isLevelOver = true;
                return isLevelOver;
            }
        }
        public int Score { get; private set; }

        public GameState(GameLevel level)
        {
            _paddle = new Paddle();
            _ball = new Ball(_paddle.Location, GameSettings.BaseHeight / 2 + GameSettings.PaddleHeight);
            _level = level;
            DropItems = new List<DropItem>();
            _lives = GameSettings.LivesAtStart;
            _pressedKey = null;

        }
        public bool BallXWall()
        {
            if (_ball.X + _ball.Radius > GameSettings.ScreenWidth)
            {
                _ball.ChangeDirection(Math.PI - 2 * _ball.Direction);
                return true; 
            }
            if (_ball.X - _ball.Radius < 0)
            {
                _ball.ChangeDirection(Math.PI - 2 * _ball.Direction);
                return true;
            }
            if (_ball.Y + _ball.Radius > GameSettings.ScreenHeight && _ball.Direction < Math.PI)
            {
                _ball.ChangeDirection(-2 * _ball.Direction);
                return true;
            }
            return false;
        }
        public bool BallXPaddle()
        {
            int closestX = Math.Clamp(_ball.X, _paddle.Location - _paddle.Width / 2, _paddle.Location + _paddle.Width / 2);
            int closestY = Math.Clamp(_ball.Y, 0, GameSettings.PaddleHeight);

            if ((closestX - _ball.X) * (closestX - _ball.X) + (closestY - _ball.Y) * (closestY - _ball.Y) < _ball.Radius * _ball.Radius && _ball.Direction > Math.PI)
            {
                _ball.MirrorDirectionAtPoint(closestX, closestY);
                _ball.ChangeDirection((_paddle.Location - closestX) * (Math.PI / 2) / _paddle.Width);
                _streak = 0;
                return true;
            }
            return false;
        }
        public void RestartBall()
        {
            _ball.OutOfBounds = false;
            _ball.IsGlued = true;
            _ball = new Ball(_paddle.Location, GameSettings.BaseHeight / 2 + GameSettings.PaddleHeight);
            _lives--;
        }
        public void CheckFireBall()
        {
            if(FireBallTimer.ElapsedMilliseconds>5000)
            {
                FireBallTimer.Stop();
                _ball.ChangeType(Ball.BallType.Regular);
            }
        }

        public bool BallXBricks()
        {
            foreach (Brick brick in _level.Bricks)
            {
                if (!brick.IsBroken)
                {
                    int closestX = Math.Clamp(_ball.X, brick.X1, brick.X2);
                    int closestY = Math.Clamp(_ball.Y, brick.Y1, brick.Y2);

                    if ((closestX - _ball.X) * (closestX - _ball.X) + (closestY - _ball.Y) * (closestY - _ball.Y) < _ball.Radius * _ball.Radius)
                    {
                        if(!(_ball.Type==Ball.BallType.Fire)) _ball.MirrorDirectionAtPoint(closestX, closestY);
                        _ball.SpeedUp(_level.SpeedUpRate);
                        _streak++;
                        Score += _streak * 10;

                        if (brick.Type == Brick.BrickType.TwoHits && !brick.IsCracked)
                            brick.IsCracked = true;
                        else if (brick.Type == Brick.BrickType.Regular || (brick.Type == Brick.BrickType.TwoHits && brick.IsCracked))
                        {
                            brick.IsBroken = true;
                            Random rand = new Random();
                            int random = rand.Next(2); // should be decided by level difficulty
                            if (random == 0)
                            {
                                random = rand.Next(8);
                                var item = new DropItem(closestX, closestY, (DropItem.DropItemType)random);
                                DropItems.Add(item);

                            }
                        }
                        return true;
                    }
                }
            }
            return false;
        }
        public bool ItemXPaddle()
        {
            var item = new DropItem();
            bool itemRewarded = false;

            foreach (DropItem dropItem in DropItems)
            {
                if (dropItem.Y - 15 < GameSettings.PaddleHeight && dropItem.Y + 15 > 0
                        && dropItem.X - 15 < _paddle.Location + _paddle.Width / 2 && dropItem.X + 15 > _paddle.Location - _paddle.Width / 2)
                {
                    RewardDropItem(dropItem);
                    Score += 10;
                    item = dropItem;
                    itemRewarded = true;
                    break;
                }
            }
            DropItems.Remove(item);

            return itemRewarded;
        }
        private void RewardDropItem(DropItem dropItem)
        {
            switch (dropItem.Type)
            {
                case DropItem.DropItemType.LargeBall:
                    _ball.Enlarge();
                    break;
                case DropItem.DropItemType.SmallBall:
                    _ball.Shrink();
                    break;
                case DropItem.DropItemType.FireBall:
                    _ball.ChangeType(Ball.BallType.Fire);
                    FireBallTimer.Restart();
                    break;
                case DropItem.DropItemType.SlowBall:
                    _ball.ChangeSpeed(0.8);
                    break;
                case DropItem.DropItemType.FastBall:
                    _ball.ChangeSpeed(1.3);
                    break;
                case DropItem.DropItemType.LargePaddle:
                    _paddle.Enlarge();
                    break;
                case DropItem.DropItemType.SmallPaddle:
                    _paddle.Shrink();
                    break;
                case DropItem.DropItemType.BonusPoints:
                    Score += 100;
                    break;
            }
        }
        public void ItemXFloor()
        {
            List<DropItem> itemsToRemove = new List<DropItem>();

            foreach (DropItem dropItem in DropItems)
                if (dropItem.Y < 0)
                    itemsToRemove.Add(dropItem);

            foreach (DropItem dropItem in itemsToRemove)
                DropItems.Remove(dropItem);
        }
        public void MoveAllItems()
        {
            foreach (DropItem dropItem in DropItems)
                dropItem.Y -= GameSettings.BaseSpeed;
        }
        public void KeyPressed(Key key)
        {
            _pressedKey = key;

            if (key == Key.Space) _ball.IsGlued = false;
        }
        public void KeyReleased()
        {
            _pressedKey = null;
        }

        public void StartLevel(GameLevel gameLevel)
        {
            _paddle = new Paddle();
            _ball = new Ball(_paddle.Location, GameSettings.BaseHeight / 2 + GameSettings.PaddleHeight);
            _level = gameLevel;
            _lives = GameSettings.LivesAtStart;
            _pressedKey = null;
            _PressedN = false;
        }
        public void SkipLevel() => _PressedN = true;

    }
}
