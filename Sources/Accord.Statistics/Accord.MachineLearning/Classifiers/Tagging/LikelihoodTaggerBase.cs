﻿// Accord Statistics Library
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © César Souza, 2009-2016
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

namespace Accord.MachineLearning
{
    using Accord.Math;
    using Accord.Statistics;
    using System;

    /// <summary>
    ///   Base implementation for generative observation sequence taggers. A sequence
    ///   tagger can predict the class label of each individual observation in a 
    ///   input sequence vector.
    /// </summary>
    /// 
    /// <typeparam name="TInput">The data type for the input data. Default is double[].</typeparam>
    /// 
    [Serializable]
    public abstract class LikelihoodTaggerBase<TInput> :
        ScoreTaggerBase<TInput>,
        ILikelihoodTagger<TInput>
    {



        /// <summary>
        /// Predicts a the probability that the sequence vector
        /// has been generated by this log-likelihood tagger.
        /// </summary>
        /// 
        public abstract double[] LogLikelihood(TInput[][] sequences, double[] result);

        /// <summary>
        /// Predicts a the probability that the sequence vector
        /// has been generated by this log-likelihood tagger.
        /// </summary>
        /// 
        public abstract double[] LogLikelihood(TInput[][] sequences, ref int[][] decision, double[] result);



        /// <summary>
        /// Predicts a the log-likelihood for each of the observations in
        /// the sequence vector assuming each of the possible states in the
        /// tagger model.
        /// </summary>
        /// 
        public abstract double[][] LogLikelihoods(TInput[] sequence, double[][] result);

        /// <summary>
        /// Predicts a the log-likelihood for each of the observations in
        /// the sequence vector assuming each of the possible states in the
        /// tagger model.
        /// </summary>
        /// 
        public abstract double[][] LogLikelihoods(TInput[] sequence, ref int[] decision, double[][] result);



        /// <summary>
        /// Predicts a the probability that the sequence vector
        /// has been generated by this log-likelihood tagger.
        /// </summary>
        /// 
        public double Probability(TInput[] sequence)
        {
            return Probability(new[] { sequence })[0];
        }

        /// <summary>
        /// Predicts a the probability that the sequence vector
        /// has been generated by this log-likelihood tagger.
        /// </summary>
        /// 
        public double Probability(TInput[] sequence, ref int[] decision)
        {
            var d = new[] { decision };
            return Probability(new[] { sequence }, ref d)[0];
        }

        /// <summary>
        /// Predicts a the probability that the sequence vector
        /// has been generated by this log-likelihood tagger.
        /// </summary>
        /// 
        public double[] Probability(TInput[][] sequences)
        {
            return Probability(sequences, new double[sequences.Length]);
        }

        /// <summary>
        /// Predicts a the probability that the sequence vector
        /// has been generated by this log-likelihood tagger.
        /// </summary>
        /// 
        public double[] Probability(TInput[][] sequences, ref int[][] decision)
        {
            return Probability(sequences, ref decision, new double[sequences.Length]);
        }

        /// <summary>
        /// Predicts a the probability that the sequence vector
        /// has been generated by this log-likelihood tagger.
        /// </summary>
        /// 
        public virtual double[] Probability(TInput[][] sequences, double[] result)
        {
            LogLikelihood(sequences, result);
            return Elementwise.Exp(result, result: result);
        }

        /// <summary>
        /// Predicts a the probability that the sequence vector
        /// has been generated by this log-likelihood tagger.
        /// </summary>
        /// 
        public virtual double[] Probability(TInput[][] sequences, ref int[][] decision, double[] result)
        {
            LogLikelihood(sequences, ref decision, result);
            return Elementwise.Exp(result, result: result);
        }

        /// <summary>
        /// Predicts a the probability that the sequence vector
        /// has been generated by this log-likelihood tagger.
        /// </summary>
        /// 
        public double LogLikelihood(TInput[] sequence)
        {
            return LogLikelihood(new[] { sequence })[0];
        }

        /// <summary>
        /// Predicts a the probability that the sequence vector
        /// has been generated by this log-likelihood tagger.
        /// </summary>
        /// 
        public double LogLikelihood(TInput[] sequence, ref int[] decision)
        {
            var d = new[] { decision };
            return LogLikelihood(new[] { sequence }, ref d)[0];
        }

        /// <summary>
        /// Predicts a the probability that the sequence vector
        /// has been generated by this log-likelihood tagger.
        /// </summary>
        /// 
        public double[] LogLikelihood(TInput[][] sequences)
        {
            return LogLikelihood(sequences, new double[sequences.Length]);
        }

        /// <summary>
        /// Predicts a the probability that the sequence vector
        /// has been generated by this log-likelihood tagger.
        /// </summary>
        /// 
        public double[] LogLikelihood(TInput[][] sequences, ref int[][] decision)
        {
            return LogLikelihood(sequences, ref decision, new double[sequences.Length]);
        }


        /// <summary>
        /// Predicts a the probabilities for each of the observations in
        /// the sequence vector assuming each of the possible states in the
        /// tagger model.
        /// </summary>
        /// 
        public double[][] Probabilities(TInput[] sequence)
        {
            return Probabilities(sequence, create(sequence));
        }

        /// <summary>
        /// Predicts a the probabilities for each of the observations in
        /// the sequence vector assuming each of the possible states in the
        /// tagger model.
        /// </summary>
        /// 
        public virtual double[][] Probabilities(TInput[] sequence, double[][] result)
        {
            Probabilities(sequence, result);
            return Elementwise.Exp(result, result: result);
        }

        /// <summary>
        /// Predicts a the log-likelihood for each of the observations in
        /// the sequence vector assuming each of the possible states in the
        /// tagger model.
        /// </summary>
        /// 
        public double[][] Probabilities(TInput[] sequence, ref int[] decision)
        {
            return Probabilities(sequence, ref decision, create(sequence));
        }

        /// <summary>
        /// Predicts a the log-likelihood for each of the observations in
        /// the sequence vector assuming each of the possible states in the
        /// tagger model.
        /// </summary>
        /// 
        public virtual double[][] Probabilities(TInput[] sequence, ref int[] decision, double[][] result)
        {
            Probabilities(sequence, ref decision, result);
            return Elementwise.Exp(result, result: result);
        }

        /// <summary>
        /// Predicts a the log-likelihood for each of the observations in
        /// the sequence vector assuming each of the possible states in the
        /// tagger model.
        /// </summary>
        /// 
        public double[][] LogLikelihoods(TInput[] sequence)
        {
            return LogLikelihoods(sequence, create(sequence));
        }

        /// <summary>
        /// Predicts a the log-likelihood for each of the observations in
        /// the sequence vector assuming each of the possible states in the
        /// tagger model.
        /// </summary>
        /// 
        public double[][] LogLikelihoods(TInput[] sequence, ref int[] decision)
        {
            return LogLikelihoods(sequence, ref decision, create(sequence));
        }

        /// <summary>
        /// Predicts a the log-likelihood for each of the observations in
        /// the sequence vector assuming each of the possible states in the
        /// tagger model.
        /// </summary>
        /// 
        public double[][][] Probabilities(TInput[][] sequences)
        {
            return Probabilities(sequences, create(sequences));
        }

        /// <summary>
        /// Predicts a the log-likelihood for each of the observations in
        /// the sequence vector assuming each of the possible states in the
        /// tagger model.
        /// </summary>
        /// 
        public virtual double[][][] Probabilities(TInput[][] sequences, double[][][] result)
        {
            LogLikelihoods(sequences, result);
            for (int i = 0; i < result.Length; i++)
                Elementwise.Exp(result[i], result: result[i]);
            return result;
        }

        /// <summary>
        /// Predicts a the probabilities for each of the observations in
        /// the sequence vector assuming each of the possible states in the
        /// tagger model.
        /// </summary>
        /// 
        public double[][][] Probabilities(TInput[][] sequences, ref int[][] decision)
        {
            return Probabilities(sequences, ref decision, create(sequences));
        }

        /// <summary>
        /// Predicts a the probabilities for each of the observations in
        /// the sequence vector assuming each of the possible states in the
        /// tagger model.
        /// </summary>
        /// 
        public virtual double[][][] Probabilities(TInput[][] sequences, ref int[][] decision, double[][][] result)
        {
            LogLikelihoods(sequences, ref decision, result);
            for (int i = 0; i < result.Length; i++)
                Elementwise.Exp(result[i], result: result[i]);
            return result;
        }
        
        
        /// <summary>
        /// Predicts a the log-likelihood for each of the observations in
        /// the sequence vector assuming each of the possible states in the
        /// tagger model.
        /// </summary>
        /// 
        public double[][][] LogLikelihoods(TInput[][] sequences)
        {
            return LogLikelihoods(sequences, create(sequences));
        }

        /// <summary>
        /// Predicts a the log-likelihood for each of the observations in
        /// the sequence vector assuming each of the possible states in the
        /// tagger model.
        /// </summary>
        /// 
        public virtual double[][][] LogLikelihoods(TInput[][] sequences, double[][][] result)
        {
            for (int i = 0; i < sequences.Length; i++)
                LogLikelihoods(sequences[i], result[i]);
            return result;
        }

        /// <summary>
        /// Predicts a the log-likelihood for each of the observations in
        /// the sequence vector assuming each of the possible states in the
        /// tagger model.
        /// </summary>
        /// 
        public virtual double[][][] LogLikelihoods(TInput[][] sequences, ref int[][] decision, double[][][] result)
        {
            for (int i = 0; i < sequences.Length; i++)
                LogLikelihoods(sequences[i], ref decision[i], result[i]);
            return result;
        }

        /// <summary>
        /// Predicts a the log-likelihood for each of the observations in
        /// the sequence vector assuming each of the possible states in the
        /// tagger model.
        /// </summary>
        /// 
        public double[][][] LogLikelihoods(TInput[][] sequences, ref int[][] decision)
        {
            return LogLikelihoods(sequences, ref decision, create(sequences));
        }




        /// <summary>
        /// Computes numerical scores measuring the association between
        /// each of the given <paramref name="sequences" /> vectors and each
        /// possible class.
        /// </summary>
        /// 
        public override double[][][] Scores(TInput[][] sequences, double[][][] result)
        {
            return LogLikelihoods(sequences, result);
        }

        /// <summary>
        /// Computes numerical scores measuring the association between
        /// each of the given <paramref name="sequences" /> vectors and each
        /// possible class.
        /// </summary>
        /// 
        public override double[][][] Scores(TInput[][] sequences, ref int[][] decision, double[][][] result)
        {
            return LogLikelihoods(sequences, ref decision, result);
        }
    }
}
