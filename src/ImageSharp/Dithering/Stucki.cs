﻿// <copyright file="Stucki.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Dithering
{
    /// <summary>
    /// Applies error diffusion based dithering using the Stucki image dithering algorithm.
    /// <see href="http://www.efg2.com/Lab/Library/ImageProcessing/DHALF.TXT"/>
    /// </summary>
    public class Stucki : ErrorDiffusion
    {
        /// <summary>
        /// The diffusion matrix
        /// </summary>
        private static readonly byte[,] StuckiMatrix =
            {
               { 0, 0, 0, 8, 4 },
               { 2, 4, 8, 4, 2 },
               { 1, 2, 4, 2, 1 }
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="Stucki"/> class.
        /// </summary>
        public Stucki()
            : base(StuckiMatrix, 4)
        {
        }
    }
}