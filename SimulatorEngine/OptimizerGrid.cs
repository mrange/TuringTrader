﻿//==============================================================================
// Project:     Trading Simulator
// Name:        OptimizerGrid
// Description: exhaustive parameter optimizer
// History:     2018ix20, FUB, created
//------------------------------------------------------------------------------
// Copyright:   (c) 2017-2018, Bertram Solutions LLC
//              http://www.bertram.solutions
// License:     this code is licensed under GPL-3.0-or-later
//==============================================================================

#region libraries
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace FUB_TradingSim
{

    /// <summary>
    /// class to run exhaustive optimization
    /// </summary>
    public class OptimizerGrid
    {
        #region internal data
        private Algorithm _masterInstance;
        private MTJobQueue _jobQueue = new MTJobQueue();
        private readonly object _optimizerLock = new object();
        private int _numIterationsTotal;
        private int _numIterationsCompleted;
        private double _maxFitness = -1e10;
        #endregion

        #region public static int NumIterations(Algorithm algo)
        public static int NumIterations(Algorithm algo)
        {
            // figure out total number of iterations
            int numIterationsTotal = 1;
            foreach (OptimizerParam parameter in algo.OptimizerParams.Values)
            {
                int iterationsThisLevel = 0;
                if (parameter.IsEnabled)
                {
                    for (int i = parameter.Start; i <= parameter.End; i += parameter.Step)
                        iterationsThisLevel++;
                }
                else
                {
                    iterationsThisLevel = 1;
                }

                numIterationsTotal *= iterationsThisLevel;
            }

            return numIterationsTotal;
        }
        #endregion

        #region private void RunIteration(bool firstRun = true)
        private Algorithm RunIteration(bool firstRun = true)
        {
            // create algorithm instance to run
            Type algoType = _masterInstance.GetType();
            Algorithm instanceToRun = (Algorithm)Activator.CreateInstance(algoType);

            // apply optimizer values to new instance
            foreach (OptimizerParam parameter in _masterInstance.OptimizerParams.Values)
                instanceToRun.OptimizerParams[parameter.Name].Value = parameter.Value;

            if (firstRun)
            {
                // mark this as an optimizer run
                instanceToRun.IsOptimizing = true;

                // create result entry
                OptimizerResult result = new OptimizerResult();
                foreach (OptimizerParam parameter in _masterInstance.OptimizerParams.Values)
                    result.Parameters[parameter.Name] = parameter.Value;
                result.Fitness = null;
                Results.Add(result);

                // run algorithm with these values
                _jobQueue.QueueJob(() =>
                {
                    instanceToRun.Run();
                    result.NetAssetValue = instanceToRun.NetAssetValue[0];
                    result.MaxDrawdown = instanceToRun.NetAssetValueMaxDrawdown;
                    result.Fitness = instanceToRun.FitnessValue;
                    instanceToRun = null;
                    lock (_optimizerLock)
                    {
                        _numIterationsCompleted++;
                        _maxFitness = Math.Max(_maxFitness, (double)result.Fitness);
                        Output.WriteLine("GridOptimizer: {0} of {1} iterations completed, max fitness = {2}",
                            _numIterationsCompleted, _numIterationsTotal, _maxFitness);

                    }
                });
            }
            else
            {
                // this is for re-runs
                instanceToRun.Run();
            }

            return instanceToRun;
        }
        #endregion
        #region private void IterateLevel(int level)
        private void IterateLevel(int level)
        {
            OptimizerParam parameter = _masterInstance.OptimizerParams.Values
                    .Skip(level)
                    .FirstOrDefault();

            if (parameter != default(OptimizerParam))
            {
                if (parameter.IsEnabled)
                {
                    for (int value = parameter.Start; value <= parameter.End; value += parameter.Step)
                    {
                        parameter.Value = value;
                        IterateLevel(level + 1);
                    }
                }
                else
                {
                    IterateLevel(level + 1);
                }
            }
            else
            {
                RunIteration();
            }
        }
        #endregion

        #region public OptimizerExhaustive(Algorithm algorithm)
        public OptimizerGrid(Algorithm algorithm)
        {
            _masterInstance = algorithm;
        }
        #endregion
        #region public void Run()
        public void Run()
        {
            // create new results list
            Results = new List<OptimizerResult>();

            // figure out total number of iterations
            _numIterationsCompleted = 0;
            _numIterationsTotal = NumIterations(_masterInstance);
            Output.WriteLine("GridOptimizer: total of {0} iterations", _numIterationsTotal);

            // create and queue iterations
            IterateLevel(0);

            // wait for completion
            _jobQueue.WaitForCompletion();
        }
        #endregion

        public List<OptimizerResult> Results;
        #region public void ResultsToExcel(string excelPath)
        public void ResultsToExcel(string excelPath)
        {
            Logger logger = new Logger();

            logger.SelectPlot("Optimizer Results", "iteration");

            for (int i = 0; i < Results.Count; i++)
            {
                OptimizerResult result = Results[i];

                logger.SetX(i);
                logger.Log("NetAssetValue", (result.NetAssetValue != null) ? string.Format("{0}", result.NetAssetValue) : "");
                logger.Log("Fitness", (result.Fitness != null) ? string.Format("{0}", result.Fitness) : "");

                foreach (var parameter in result.Parameters)
                    logger.Log(parameter.Key, parameter.Value);
            }

            logger.OpenWithExcel(excelPath);
        }
        #endregion
        #region public void SetParametersFromResult(OptimizerResult result)
        public void SetParametersFromResult(OptimizerResult result)
        {
            foreach (var parameter in result.Parameters)
                _masterInstance.OptimizerParams[parameter.Key].Value = parameter.Value;
        }
        #endregion
    }
}

//==============================================================================
// end of file