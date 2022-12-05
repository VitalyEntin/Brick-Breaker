using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brick_Breaker.Models
{
    public class Paddle
    {
        private int _location;
        private int _width;
        private int _speed;
        private double _friction;

        public int Location => _location;
        public int Width => _width;
        public int Speed => _speed;
        public double Friction => _friction;

        public Paddle()
        {
            _location = GameSettings.ScreenWidth/2;
            _width = (int)(GameSettings.BaseWidth*1.5);
            _speed = GameSettings.BaseSpeed*2;
            _friction = GameSettings.PaddleFriction;
        }

        public void Move(int direction)
        {
            if(direction>0 && _location + _width/2 + direction*_speed > GameSettings.ScreenWidth)
                    _location = GameSettings.ScreenWidth-_width/2;
            else if(direction < 0 && _location - _width / 2 + direction * _speed <0)
                _location = Width/2;
            else
                _location += direction * _speed;
        }
        public void Enlarge()
        {
            if (_width < GameSettings.BaseWidth*3)
                _width = (int)(_width * 1.3);

        }
        public void Shrink()
        {
            if (_width > GameSettings.BaseWidth)
                _width = (int)(_width / 1.3);
        }
    }
}
