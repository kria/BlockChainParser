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
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Diagnostics;
using CommandLine;

namespace BlockChainParser
{
    class Program
    {
        static void Main(string[] args)
        {
            int blockCount = 0;
            var opts = CommandLine.Parser.Default.ParseArguments<Options>(args);

            if (!opts.Errors.Any())
            {
                int blkindex = opts.Value.StartBlkIndex;
                string datadir = opts.Value.DataDir ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Bitcoin\blocks");
                System.Console.WriteLine("Scanning for block files in {0}...", datadir);

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                
                while (true)
                {
                    string blockfile = String.Format("blk{0:00000}.dat", blkindex);
                    string path = Path.Combine(datadir, blockfile);
                    if (!File.Exists(path)) break;

                    try
                    {
                        System.Console.WriteLine("Parsing {0}...", blockfile);
                        BlockChain.ParseBlockFile(path, (hash, diff) =>
                        {
                            if (opts.Value.Verbose)
                                System.Console.WriteLine(hash);
                            if (diff == opts.Value.BestShare)
                                System.Console.WriteLine("Matching block difficulty found:\n{0}, {1}", diff, hash);
                            blockCount++;
                        });
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                    }
                    blkindex++;
                }

                stopWatch.Stop();
                System.Console.WriteLine("Parsed {0} blocks in {1}.", blockCount, stopWatch.Elapsed.ToString());
            }
        }
    }

    class Options
    {
        [Option("bestshare", Required = true, HelpText = "The difficulty to find corresponding block hash for.")]
        public uint BestShare { get; set; }

        [Option("dir", Required = false, HelpText = "Directory containing blk*.dat files. The default Bitcoin-Qt dir will be used if not set.")]
        public string DataDir { get; set; }

        [Option("start", DefaultValue = 0, HelpText = "Which blk*.dat to start at.")]
        public int StartBlkIndex { get; set; }

        [Option("verbose", HelpText = "Display more info.")]
        public bool Verbose { get; set; }

    }
}
