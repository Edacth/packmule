using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace packmule
{
    public struct Vector2
    {
        public float posX;
        public float posY;

        public static Vector2 Empty { get => new Vector2( 0, 0 ); }

        public Vector2(int _posX, int _posY)
        {
            posX = _posX;
            posY = _posY;
        }

        public override string ToString()
        {
            return posX + " " + posY;
        }
    }
}
