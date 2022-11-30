﻿// Accord Machine Learning Library
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © César Souza, 2009-2017
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
    using Accord.MachineLearning.Performance;
    using System;

    /// <summary>
    ///   Obsolete. Please refer to <see cref="SplitSetValidation{TModel, TInput}"/> instead.
    /// </summary>
    /// 
    [Obsolete("Please use SplitResult{TModel} instead.")]
    public class SplitSetResult<TModel> 
        where TModel : class
    {
        /// <summary>
        ///   Gets the CrossValidation{T}   
        ///   object used to generate this result.
        /// </summary>
        /// 
        public SplitSetValidation<TModel> Settings { get; private set; }

        /// <summary>
        ///   Gets the performance statistics for the training set.
        /// </summary>
        /// 
        public SplitSetStatistics<TModel> Training { get; private set; }

        /// <summary>
        ///   Gets the performance statistics for the validation set.
        /// </summary>
        /// 
        public SplitSetStatistics<TModel> Validation { get; private set; }

        /// <summary>
        ///   Gets or sets a tag for user-defined information.
        /// </summary>
        /// 
        public object Tag { get; set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="SplitSetValidation&lt;TModel&gt;"/> class.
        /// </summary>
        /// 
        /// <param name="owner">The <see cref="SplitSetValidation{TModel}"/> that is creating this result.</param>
        /// <param name="training">The training set statistics.</param>
        /// <param name="testing">The testing set statistics.</param>
        /// 
        public SplitSetResult(SplitSetValidation<TModel> owner,
            SplitSetStatistics<TModel> training, SplitSetStatistics<TModel> testing)
        {
            this.Settings = owner;
            this.Training = training;
            this.Validation = testing;
        }

    }
}
