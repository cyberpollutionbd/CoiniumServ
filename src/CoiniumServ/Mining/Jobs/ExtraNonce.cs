﻿#region License
// 
//     CoiniumServ - Crypto Currency Mining Pool Server Software
//     Copyright (C) 2013 - 2014, CoiniumServ Project - http://www.coinium.org
//     http://www.coiniumserv.com - https://github.com/CoiniumServ/CoiniumServ
// 
//     This software is dual-licensed: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
//    
//     For the terms of this license, see licenses/gpl_v3.txt.
// 
//     Alternatively, you can license this software under a commercial
//     license or white-label it as set out in licenses/commercial.txt.
// 
#endregion

using System;
using CoiniumServ.Utils.Extensions;

namespace CoiniumServ.Mining.Jobs
{
    /// <summary>
    /// Counter for extra nonce.
    /// Hex-encoded, per-connection unique string which will be used for coinbase serialization later. (http://mining.bitcoin.cz/stratum-mining)
    /// </summary>
    public class ExtraNonce:IExtraNonce
    {
        // TODO: use ioc for extranonce too

        private UInt32 _counter;

        /// <summary>
        /// ExtraNonce placeholder to be used with coinbase transactions.
        /// </summary>
        public byte[] ExtraNoncePlaceholder { get; private set; }

        /// <summary>
        /// The number of bytes that the miner users for its ExtraNonce2 counter 
        /// <remarks>Represents expected length of extranonce2 which will be generated by the miner. (http://mining.bitcoin.cz/stratum-mining)</remarks>
        /// </summary>
        public const int ExpectedExtraNonce2Size = 0x4;

        public ExtraNonce(UInt32 instanceId)
        {
            ExtraNoncePlaceholder = "f000000ff111111f".HexToByteArray();
            InitCounter(instanceId); // init. the extra nonce counter.
        }


        /// <summary>
        /// Inits ExtraNonce counter based on current instance Id.
        /// </summary>
        private void InitCounter(UInt32 instanceId)
        {
            _counter = instanceId << 27;  // init the ExtraNonce counter - last 5 most-significant bits represents instanceId, the rest is just an iterator of jobs.
        }

        /// <summary>
        /// Returns the next extranonce.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Last 5 most-significant bits represents instanceId, the rest is just an iterator of jobs.
        /// Basically allows us to run more-than-one pool-nodes within the same database.
        /// More: https://github.com/moopless/stratum-mining-litecoin/issues/23#issuecomment-22728564
        /// </remarks>
        public UInt32 Next()
        {
            return ++_counter; // return the next extra-nonce.
        }
    }
}
