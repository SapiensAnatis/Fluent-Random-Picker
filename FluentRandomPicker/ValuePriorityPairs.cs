﻿using System.Collections;
using System.Collections.Generic;

namespace FluentRandomPicker
{
    /// <summary>
    /// A collection of ValuePriorityPairs.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    internal class ValuePriorityPairs<T> : IEnumerable<ValuePriorityPair<T>>
    {
        private readonly List<ValuePriorityPair<T>> _pairs = new List<ValuePriorityPair<T>>();

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public Priority Priority { get; set; }

        /// <summary>
        /// Allows index access to the pairs.
        /// </summary>
        /// <param name="i">The index.</param>
        public ValuePriorityPair<T> this[int i]
        {
            get { return _pairs[i]; }
        }

        /// <inheritdoc/>
        public void Add(ValuePriorityPair<T> pair)
        {
            _pairs.Add(pair);
        }

        /// <inheritdoc/>
        public IEnumerator<ValuePriorityPair<T>> GetEnumerator()
        {
            return _pairs.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _pairs.GetEnumerator();
        }
    }
}
