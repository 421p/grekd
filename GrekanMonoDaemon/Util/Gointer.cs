using System;
using System.Linq;

namespace GrekanMonoDaemon.Util
{
    public class Gointer
    {
        private readonly long _max;
        private long _current;
        private int[] _sequence;

        public long Index
        {
            get
            {
                if (_current == _max)
                {
                    _current = 0;
                    _sequence = CreateSequence(_max);
                }
                
                var temp = _current;
                _current++;
                
                return _sequence.ElementAt((int) temp);
            }
        }
        
        public Gointer(long max)
        {
            _sequence = CreateSequence(max);

            _max = max;
            _current = 0;
        }

        private static int[] CreateSequence(long max)
        {
            var rand = new Random();
            return Enumerable.Range(0, (int) max).OrderBy(x => rand.Next()).ToArray();
        }
    }
}