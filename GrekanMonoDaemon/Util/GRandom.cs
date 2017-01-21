using System;
using System.Collections.Generic;

namespace GrekanMonoDaemon.Util
{
    public class GRandom : Random
    {
        private readonly Queue<int> _stash;

        public GRandom() : base((int) DateTime.Now.ToBinary())
        {
            _stash = new Queue<int>(10);
        }

        public int Next(long minValue, long maxValue)
        {
            return Next((int) minValue, (int) maxValue);
        }

        public new int Next(int minValue, int maxValue)
        {
            if (_stash.Count == 10)
            {
                _stash.Dequeue();
            }

            int value;

            do
            {
                value = base.Next(minValue, maxValue);
            } while (_stash.Contains(value));

            _stash.Enqueue(value);

            return value;
        }
    }
}