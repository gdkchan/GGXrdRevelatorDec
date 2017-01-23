using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GGXrdRevelator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("GGXrdRevelator v0.1 by gdkchan");
            Console.ResetColor();

            Console.WriteLine("Guilty Gear XRD -REVELATOR- UPK decrypter");

            Console.WriteLine(string.Empty);

            uint OutVal = 0;

            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "-revel": OutVal = 0x72642a6f; break;
                    case "-sign": OutVal = 0x43415046; break;
                }
            }

            if (args.Length < 2 || OutVal == 0)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Usage:");
                Console.ResetColor();

                Console.WriteLine("GGXrdRevelator (-revel|-sign) file.upk");
                Console.WriteLine("-revel  Decrypts GG Xrd REVELATOR files");
                Console.WriteLine("-sign  Decrypts GG Xrd SIGN files");

                Console.WriteLine(string.Empty);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Example:");
                Console.ResetColor();

                Console.WriteLine("GGXrdRevelator -revel ELP_VOICE_JPN_A_SF.upk");

                Console.WriteLine(string.Empty);

                Console.WriteLine("The output file name is the input name with .dec extension");

                return;
            }

            string FileName = args[1];
            string Name = Path.GetFileName(FileName).ToUpper();

            uint Seed = 0;

            foreach (char Chr in Name)
            {
                Seed *= 137;
                Seed += Chr;
            }

            MersenneTwister MT = new MersenneTwister(Seed);

            using (FileStream Input = new FileStream(FileName, FileMode.Open))
            {
                using (FileStream Output = new FileStream(FileName + ".dec", FileMode.Create))
                {
                    BinaryReader Reader = new BinaryReader(Input);
                    BinaryWriter Writer = new BinaryWriter(Output);

                    while (Input.Position + 4 <= Input.Length)
                    {
                        Writer.Write(OutVal ^= Reader.ReadUInt32() ^ MT.GenRandomNumber());
                    }
                }
            }

            Console.WriteLine("Finished!");
        }
    }
}
