/*
 * BlockChainParser - http://github.com/kria/BlockChainParser
 * 
 * Copyright (C) 2014 Kristian Adrup
 * 
 * This file is part of BlockChainParser.
 * 
 * BlockChainParser is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the
 * Free Software Foundation, either version 3 of the License, or (at your
 * option) any later version. See included file COPYING for details.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainParser
{
    public class BlockChain
    {
        private const uint magicId = 0xD9B4BEF9;
        // trueDiffOne == 0x00000000FFFF0000000000000000000000000000000000000000000000000000
        private static readonly double trueDiffOne = Double.Parse("26959535291011309493156476344723991336010898738574164086137773096960");
    
        /// <summary>
        /// Parse a Bitcoin-Qt blk*.dat file and generate a callback event on each block.
        /// </summary>
        /// <param name="path">Path of file to parse</param>
        /// <param name="callback">Callback method</param>
        /// <returns>number of blocks parsed</returns>
        public static int ParseBlockFile(string path, Action<string, double> callback) {
            int blockCount = 0;

            using (FileStream fs = File.OpenRead(path))
            {
                var rdr = new BinaryReader(fs);
                while (fs.Position < fs.Length)
                {
                    uint magic = rdr.ReadUInt32();
                    if (magic != magicId) throw new Exception("Expected magic, bailing file!");

                    uint headerLength = rdr.ReadUInt32();
                    uint versionNumber = rdr.ReadUInt32();

                    byte[] hashBuf = rdr.ReadBytes(32);
                    BigInteger prevHash = new BigInteger(hashBuf);

                    Array.Reverse(hashBuf); // to big endian from little
                    string hashstr = Utils.ToHexString(hashBuf);
                    double blockDiff = Math.Round(trueDiffOne / (double)prevHash);

                    callback(hashstr, blockDiff);

                    blockCount++;
                    fs.Seek(headerLength - 36, SeekOrigin.Current);
                }
            }
            return blockCount;
        }
    }

}
