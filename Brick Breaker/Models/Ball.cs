using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brick_Breaker.Models
{
    public class Ball
    {
        public enum BallType { Regular, Fire };

        private double _x;
        private double _y;
        private double _speed;
        private double _direction;
        private int _radius;
        private BallType _type;

        public int X => (int)_x;
        public int Y => (int)_y;
        public BallType Type => _type;
        public double Speed => _speed;
        public double Direction => _direction;
        public int Radius => _radius;
        public bool IsGlued { get; set; }
        public bool OutOfBounds { get; set; }

        public Ball(int x, int y)
        {
            _x = x;
            _y = y;
            _direction = Math.PI / 2;
            _speed = GameSettings.BaseSpeed*1.2;
            _radius = GameSettings.BaseHeight / 2;
            _type = BallType.Regular;
            IsGlued = true;
        }
        public void ChangeDirection(double da)
        {
            _direction += da;
            if (_direction >= Math.PI * 2) _direction -= Math.PI * 2;
            if (_direction < 0) _direction += Math.PI * 2;
            if (_direction < Math.PI / 8) _direction = Math.PI / 8;
            if (_direction < Math.PI && _direction > Math.PI * 7 / 8) _direction = Math.PI * 7 / 8;
        }
        public void Move()
        {
            _x += _speed * Math.Cos(_direction);
            _y += _speed * Math.Sin(_direction);

            if (_y + _radius < -30) OutOfBounds = true;
        }
        public void MoveTo(int x) => _x = x;

        public void SpeedUp(double amount)
        {
            _speed += amount;
        }

        public void MirrorDirectionAtPoint(double x, double y)
        {
            double directionVectorX = Math.Cos(_direction), directionVectorY = Math.Sin(_direction);
            double normalVectorX = (_x - x) / Math.Sqrt((_x - x) * (_x - x) + (_y - y) * (_y - y));
            double normalVectorY = (_y - y) / Math.Sqrt((_x - x) * (_x - x) + (_y - y) * (_y - y));
            double resultDirectionX = directionVectorX - 2 * (directionVectorX * normalVectorX + directionVectorY * normalVectorY) * normalVectorX;
            double resultDirectionY = directionVectorY - 2 * (directionVectorX * normalVectorX + directionVectorY * normalVectorY) * normalVectorY; ;

            double resultDirection = Math.Atan2(resultDirectionY, resultDirectionX);

            if (resultDirection >= 0)
                _direction = resultDirection;
            else _direction= resultDirection + 2 * Math.PI;
        }
        public void Enlarge()
        {
            if(_radius < GameSettings.BaseHeight)
                _radius = (int)(_radius * 1.3);
        }
        public void Shrink()
        {
            if (_radius > GameSettings.BaseHeight/4)
                _radius = (int)(_radius / 1.3);
        }
        public void ChangeType(BallType type) => _type = type;

        public void ChangeSpeed(double v) => _speed *= v;

    }
}
