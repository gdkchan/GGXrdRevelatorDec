namespace GGXrdRevelator
{
    class MersenneTwister
    {
        private int Index;
        private uint[] MT;

        public MersenneTwister(uint Seed)
        {
            MT = new uint[624];

            MT[0] = Seed;

            for (int i = 1; i < MT.Length; i++)
            {
                uint Prev = MT[i - 1];

                MT[i] = (uint)(0x6c078965 * (Prev ^ (Prev >> 30)) + i);
            }

            Index = MT.Length;
        }

        private void Twist()
        {
            for (int i = 0; i < MT.Length; i++)
            {
                uint Value = (MT[i] & 0x80000000) + (MT[(i + 1) % MT.Length] & 0x7fffffff);

                MT[i] = MT[(i + 397) % MT.Length] ^ (Value >> 1);

                if ((Value & 1) != 0) MT[i] ^= 0x9908b0df;
            }

            Index = 0;
        }

        public uint GenRandomNumber()
        {
            if (Index >= MT.Length) Twist();

            uint Value = MT[Index++];

            Value ^= Value >> 11;
            Value ^= (Value << 7) & 0x9d2c5680;
            Value ^= (Value << 15) & 0xefc60000;
            Value ^= Value >> 18;

            return Value;
        }
    }
}
