# BlockChainParser

BlockChainParser is a .NET application that can parse the [Bitcoin-Qt][0] blk*.dat files that contain the Bitcoin block chain. The application is very rudimentary and right now the only real use case is finding a block when you know the specific block difficulty.

I wrote the parser because my Bitcoin miner had found a block, and I wanted to know which block. *Found Blocks=1* in the [cgminer][1] stats indicates that a block has been found by the miner. To figure out which block was found, we can use the *Best Share* value, which represents the difficulty of this session's best share. If that value is greater than the network difficulty at the time, a block was found. By going through the block chain, converting block hashes to the corresponding difficulty and comparing it to the best share value, we can find the block we mined (unless it was orphaned and never reached our client).

Use the summary command of the [cgminer API][2] to get the exact best share value.

[0]: https://bitcoin.org/en/download
[1]: https://github.com/ckolivas/cgminer
[2]: https://github.com/ckolivas/cgminer/blob/master/API-README

## Usage

```
--bestshare    Required. The difficulty to find corresponding block hash for.

--dir          Directory containing blk*.dat files. The default Bitcoin-Qt
               dir will be used if not set.

--start        (Default: 0) Which blk*.dat to start at.

--verbose      Display more info.

--help         Display this help screen.
```

### Example

```
>BlockChainParser.exe --bestshare 3338771124
...
Parsing blk00109.dat...
Parsing blk00110.dat...
Matching block difficulty found:
3338771124, 0000000000000001494fad43392fa22f71704228f05e7acaeb3b6a722dc74870
```

## License

Copyright (C) 2014 Kristian Adrup

BlockChainParser is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. See included file [COPYING][1] for details.

[3]: https://github.com/kria/BlockChainParser/blob/master/COPYING

## Acknowledgements

* I got the data format of the files from [this post][2] by John Ratcliff.
* This application uses [Command Line Parser Library][3] by Giacomo Stelluti licensed under the MIT License.

[4]: http://codesuppository.blogspot.se/2014/01/how-to-parse-bitcoin-blockchain.html
[5]: https://github.com/gsscoder/commandline


