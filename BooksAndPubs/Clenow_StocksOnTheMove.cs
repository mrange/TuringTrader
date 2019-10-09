﻿//==============================================================================
// Project:     TuringTrader, algorithms from books & publications
// Name:        Clenow_StocksOnTheMove
// Description: Strategy, as published in Andreas F. Clenow's book
//              'Stocks on the Move'.
//              http://www.followingthetrend.com/
// History:     2018xii14, FUB, created
//------------------------------------------------------------------------------
// Copyright:   (c) 2011-2019, Bertram Solutions LLC
//              https://www.bertram.solutions
// License:     This file is part of TuringTrader, an open-source backtesting
//              engine/ market simulator.
//              TuringTrader is free software: you can redistribute it and/or 
//              modify it under the terms of the GNU Affero General Public 
//              License as published by the Free Software Foundation, either 
//              version 3 of the License, or (at your option) any later version.
//              TuringTrader is distributed in the hope that it will be useful,
//              but WITHOUT ANY WARRANTY; without even the implied warranty of
//              MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//              GNU Affero General Public License for more details.
//              You should have received a copy of the GNU Affero General Public
//              License along with TuringTrader. If not, see 
//              https://www.gnu.org/licenses/agpl-3.0.
//==============================================================================

// USE_NORGATE_UNIVERSE
// defined: using survivorship-free universe through Norgate Data
// undefined: using fixed test univese
//#define USE_NORGATE_UNIVERSE

#region libraries
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TuringTrader.Indicators;
using TuringTrader.Simulator;
#endregion

namespace TuringTrader.BooksAndPubs
{
    #region TestUniverse - beware of survivorship bias!
    class TestUniverse : Universe
    {
        public override IEnumerable<string> Constituents => new List<string>()
        {
            "AAL",
            "AAPL",
            "ABBV",
            "ABT",
            //"ABX",
            "ACN",
            "ADBE",
            "ADI",
            "ADP",
            "ADSK",
            "AIG",
            "AKAM",
            "ALL",
            "ALXN",
            "AMAT",
            "AMGN",
            "AMZN",
            "APA",
            //"APC",
            "ATVI",
            "AVGO",
            "AXP",
            "BA",
            "BAC",
            "BBBY",
            "BHP",
            "BIDU",
            "BIIB",
            "BK",
            "BMY",
            "BP",
            "BRK.A",
            "BRK.B",
            "C",
            //"CA.x",
            "CAT",
            "CELG",
            "CERN",
            "CF",
            "CHK",
            "CHKP",
            "CHRW",
            "CHTR",
            "CL",
            "CLF",
            "CMCSA",
            "CMI",
            "COF",
            "COP",
            "COST",
            "CRM",
            "CSCO",
            "CTSH",
            "CTXS",
            "CVS",
            "CVX",
            "DE",
            "DIS",
            "DISCA",
            "DISCK",
            "DISH",
            "DLTR",
            //"DTV",
            "DVN",
            "EA",
            "EBAY",
            "EMR",
            "EOG",
            //"ESRX.X",
            "EXC",
            "EXPD",
            "F",
            "FAST",
            "FB",
            "FCX",
            "FDX",
            "FISV",
            //"TFCF.X",
            //"TFCFA.X",
            "GD",
            "GE",
            "GILD",
            "GLW",
            "GM",
            "GOOG",
            "GOOGL",
            "GRMN",
            "GS",
            "HAL",
            "HD",
            "HES",
            "HON",
            "HPQ",
            "HSIC",
            "IBM",
            "ILMN",
            "INTC",
            "INTU",
            "ISRG",
            "ITUB",
            "JCP",
            "JNJ",
            "JPM",
            "KLAC",
            "KMI",
            "KO",
            "LBTYA",
            "LBTYK",
            "LLY",
            "LMT",
            "LOW",
            "LRCX",
            "LVS",
            "M",
            "MA",
            "MAR",
            "MAT",
            "MCD",
            "MDLZ",
            "MDT",
            "MET",
            "MMM",
            "MNST",
            "MO",
            //"MON.X",
            "MOS",
            "MRK",
            "MS",
            "MSFT",
            "MU",
            "MYL",
            "NEM",
            "NFLX",
            "NKE",
            "NLY",
            "NOV",
            "NSC",
            "NTAP",
            "NVDA",
            "NWSA",
            "NXPI",
            "ORCL",
            "ORLY",
            "OXY",
            "PAYX",
            "PBR",
            "PCAR",
            "PEP",
            "PFE",
            "PG",
            "PM",
            "PRU",
            "QCOM",
            "REGN",
            "RIG",
            "ROST",
            "RTN",
            "SBAC",
            "SBUX",
            "SINA",
            "SIRI",
            "SLB",
            "SO",
            "SPG",
            "SRCL",
            "STX",
            "SYMC",
            "T",
            "TGT",
            "TRIP",
            "TSCO",
            "TSLA",
            //"TWX.X",
            "TXN",
            "UNH",
            "UNP",
            "UPS",
            "USB",
            "UTX",
            "V",
            "VALE",
            "VIAB",
            "VLO",
            "VOD",
            "VRSK",
            "VRTX",
            "VZ",
            "WBA",
            "WDC",
            "WFC",
            "WMT",
            "WYNN",
            "X",
            "XLNX",
            "XOM",
            //"ALTR.X",
            //"ANR.X",
            //"BHI.X",
            //"BRCM.X",
            //"CAM.X",
            //"CMCSK.X",
            //"COV.X",
            //"DELL.X",
            //"DD.X",
            //"DOW.X",
            //"EMC.X",
            //"EP.X",
            //"GMCR.X",
            //"LLTC.X",
            //"LMCA.X",
            //"LMCK.X",
            //"LVNTA.X",
            //"MHS.X",
            //"MJN.X",
            //"PCLN.X",
            //"POT.X",
            //"PSE.X",
            //"QVCA.X",
            //"SNDK.X",
            //"SPLS.X",
            //"STJ.X",
            //"TWC.X",
            //"VIP.X",
            //"WAG.X",
            //"WFM.X",
            //"WLP.X",
            //"WLT.X",
            //"YHOO.X",
        };
        public override bool IsConstituent(string nickname, DateTime timestamp)
        {
            return true;
        }
    }
    #endregion

    public class Clenov_StocksOnTheMove : Algorithm
    {
        public override string Name => "Stocks on the Move";


        #region inputs
        [OptimizerParam(63, 252, 21)]
        public int MOM_PERIOD = 90;

        [OptimizerParam(110, 125, 5)]
        public int MAX_MOVE = 115;

        [OptimizerParam(63, 252, 21)]
        public int INSTR_FLT = 100;

        [OptimizerParam(5, 25, 5)]
        public int ATR_PERIOD = 20;

        [OptimizerParam(63, 252, 21)]
        public int INDEX_FLT = 200;

        [OptimizerParam(5, 50, 5)]
        public int TOP_PCNT = 20;

        public int RISK_BPS = 10;
        #endregion
        #region private data
        private readonly string BENCHMARK = "$SPX";
        private readonly double INITIAL_FUNDS = 100000;
#if USE_NORGATE_UNIVERSE
        // this is the proper way of doing things
        private Universe UNIVERSE = Universe.New("$SPX");
#else
        // this if for testing only
        private Universe UNIVERSE = new TestUniverse();
#endif
        private Plotter _plotter = new Plotter();
        #endregion

        #region public override void Run()
        public override void Run()
        {
            //---------- initialization

            // set simulation time frame
#if false
            // matching Clenow's charts
            WarmupStartTime = DateTime.Parse("01/01/1998", CultureInfo.InvariantCulture);
            StartTime = DateTime.Parse("01/01/1999", CultureInfo.InvariantCulture);
            EndTime = DateTime.Parse("12/31/2014", CultureInfo.InvariantCulture);
#else
            WarmupStartTime = DateTime.Parse("01/01/2006", CultureInfo.InvariantCulture);
            StartTime = DateTime.Parse("01/01/2007", CultureInfo.InvariantCulture);
            EndTime = DateTime.Now.Date - TimeSpan.FromDays(5);
#endif

            // set account value
            Deposit(INITIAL_FUNDS);
            CommissionPerShare = 0.015; // not sure if Clenow is considering commissions

            // add instruments
            AddDataSource(BENCHMARK);
            AddDataSources(UNIVERSE.Constituents);

            //---------- simulation

            Instrument benchmark = null;

            // loop through all bars
            foreach (DateTime simTime in SimTimes)
            {
                benchmark = benchmark ?? FindInstrument(BENCHMARK);

                // calculate indicators
                // store to list, to make sure indicators are evaluated
                // exactly once per bar
                // do this on all available instruments, to make sure we
                // have valid data available when instruments become
                // constituents of our universe
                var instrumentIndicators = Instruments
                    .Select(i => new
                    {
                        instrument = i,
                        regression = i.Close.LogRegression(MOM_PERIOD),
                        maxMove = i.Close.LogReturn().AbsValue().Highest(MOM_PERIOD),
                        avg100 = i.Close.SMA(INSTR_FLT),
                        atr20 = i.AverageTrueRange(ATR_PERIOD),
                    })
                    .ToList();

                // index filter: only buy any shares, while S&P-500 is trading above its 200-day moving average
                bool allowNewEntries = FindInstrument(BENCHMARK).Close[0] > FindInstrument(BENCHMARK).Close.SMA(INDEX_FLT)[0];

                // trade once per week
                // this is a slight simplification from Clenow's suggestion to adjust positions
                // every week, and adjust position sizes only every other week
                if (SimTime[0].DayOfWeek <= DayOfWeek.Wednesday && NextSimTime.DayOfWeek > DayOfWeek.Wednesday)
                {
                    // disqualify instruments, if
                    //    - not a constituent of the universe
                    //    - trading below 100-day moving average
                    //    - negative momentum
                    //    - maximum move > 15%
                    var availableInstruments = instrumentIndicators
                        .Where(e => UNIVERSE.IsConstituent(e.instrument.Nickname, SimTime[0])
                            && e.instrument.Close[0] > e.avg100[0]
                            && e.regression.Slope[0] > 0.0
                            && e.maxMove[0] < Math.Log(MAX_MOVE / 100.0))
                        .ToList();

                    // 1) rank instruments by volatility-adjusted momentum
                    // 2) determine position size with risk equal to 10-basis points
                    //    of NAV per instrument, based on 20-day ATR
                    var rankedInstruments = availableInstruments
                        .OrderByDescending(e => e.regression.Slope[0] * e.regression.R2[0])
                        .Select(e => new
                        {
                            instrument = e.instrument,
                            positionSize = RISK_BPS / 10000.0 / e.atr20[0] * e.instrument.Close[0],
                        })
                        .ToList();

                    // select the top-ranking 20% from our filtered list
                    var topInstruments = rankedInstruments
                        .Take((int)Math.Round(TOP_PCNT / 100.0 * rankedInstruments.Count))
                        .ToList();

                    // assign equity, until we run out of cash
                    var instrumentRelativeEquity = Enumerable.Range(0, topInstruments.Count)
                        .ToDictionary(
                            i => topInstruments[i].instrument,
                            i => Math.Min(
                                topInstruments[i].positionSize,
                                Math.Max(0.0, 1.0 - topInstruments.Take(i).Sum(r => r.positionSize))));

                    // loop through all instruments and submit trades
                    foreach (Instrument instrument in Instruments)
                    {
                        int currentShares = instrument.Position;
                        int targetShares = instrumentRelativeEquity.ContainsKey(instrument)
                            ? (int)Math.Round(NetAssetValue[0] * instrumentRelativeEquity[instrument] / instrument.Close[0])
                            : 0;

                        if (!allowNewEntries)
                            targetShares = Math.Min(targetShares, currentShares);

                        instrument.Trade(targetShares - currentShares);
                    }

                    string message = instrumentRelativeEquity
                        .Where(i => i.Value != 0.0)
                        .Aggregate(string.Format("{0:MM/dd/yyyy}: ", SimTime[0]),
                            (prev, next) => prev + string.Format("{0}={1:P2} ", next.Key.Symbol, next.Value));
                    if (!IsOptimizing)
                        Output.WriteLine(message);
                }

                // create plots on Sheet 1
                if (TradingDays > 0)
                {
                    _plotter.SelectChart(Name, "date");
                    _plotter.SetX(SimTime[0]);
                    _plotter.Plot(Name, NetAssetValue[0]);
                    _plotter.Plot(benchmark.Name, benchmark.Close[0]);
                }
            }

            //----- post processing

            // print position log, grouped as LIFO
            var tradeLog = LogAnalysis
                .GroupPositions(Log, true)
                .OrderBy(i => i.Entry.BarOfExecution.Time);

            _plotter.SelectChart("Strategy Positions", "entry date");
            foreach (var trade in tradeLog)
            {
                _plotter.SetX(trade.Entry.BarOfExecution.Time.Date);
                _plotter.Plot("exit date", trade.Exit.BarOfExecution.Time.Date);
                _plotter.Plot("Symbol", trade.Symbol);
                _plotter.Plot("Quantity", trade.Quantity);
                _plotter.Plot("% Profit", trade.Exit.FillPrice / trade.Entry.FillPrice - 1.0);
                _plotter.Plot("Exit", trade.Exit.OrderTicket.Comment ?? "");
                //_plotter.Plot("$ Profit", trade.Quantity * (trade.Exit.FillPrice - trade.Entry.FillPrice));
            }
        }
        #endregion
        #region public override void Report()
        public override void Report()
        {
            _plotter.OpenWith("SimpleReport");
        }
        #endregion
    }
}

//==============================================================================
// end of file