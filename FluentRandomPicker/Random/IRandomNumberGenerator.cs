﻿namespace FluentRandomPicker.Random
{
    /// <summary>
    /// Generates random numbers.
    /// </summary>
    public interface IRandomNumberGenerator
    {
        /// <summary>
        /// Generates a random int32 value.
        /// </summary>
        /// <returns>The random double value.</returns>
        int NextInt();

        /// <summary>
        /// Generates a random int32 value smaller than n.
        /// </summary>
        /// <param name="n">The exclusive upper limit of the random int.</param>
        /// <returns>The random double value.</returns>
        int NextInt(int n);

        /// <summary>
        /// Generates a random int32 value smaller than n.
        /// </summary>
        /// <param name="min">The lower limit of the random int (inclusive).</param>
        /// <param name="max">The upper limit of the random int (exclusive).</param>
        /// <returns>The random double value.</returns>
        int NextInt(int min, int max);

        /// <summary>
        /// Generates a random double value larger or equal to 0.0 and smaller than 1.0.
        /// </summary>
        /// <returns>The random double value.</returns>
        double NextDouble();
    }
}
