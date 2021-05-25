using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect4.Interfaces;

namespace Connect4
{

    public class Player : IPlayer
    {
        public double Time { get; set; } = Board.startTime;
    }
}
