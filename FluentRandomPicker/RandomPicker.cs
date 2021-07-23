﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentRandomPicker.Exceptions;
using FluentRandomPicker.Interfaces;
using FluentRandomPicker.Interfaces.General;
using FluentRandomPicker.Interfaces.Percentage;
using FluentRandomPicker.Interfaces.Weight;
using FluentRandomPicker.Picker;
using FluentRandomPicker.Random;

namespace FluentRandomPicker
{
    /// <summary>
    /// The main class.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    internal sealed class RandomPicker<T> : ISpecifyValueOrGenesisValuePriority<T>,
        ISpecifyValueOrValuePriorityOrPick<T>,
        ISpecifyValueOrValuePriority<T>,
        ISpecifyValuePrioritiesOrPick<T>,

        ISpecifyPercentageValue<T>,
        ISpecifyPercentageValueOrPick<T>,
        ISpecifyPercentageValueOrValuePercentageOrPick<T>,

        ISpecifyWeightValue<T>,
        ISpecifyWeightValueOrPick<T>,
        ISpecifyWeightValueOrValueWeightOrPick<T>
    {
        private readonly IRandomNumberGenerator _rng;

        private readonly List<T> _values = new List<T>();

        private readonly List<int> _priorities = new List<int>();

        private Priority _priority;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomPicker{T}"/> class.
        /// </summary>
        /// <param name="rng">The random number generator.</param>
        internal RandomPicker(IRandomNumberGenerator rng)
        {
            _rng = rng;
        }

        private void AddValue(T t)
        {
            _values.Add(t);
        }

        private void SetPriority(int numericPriority, Priority type)
        {
            if (numericPriority <= 0)
                throw new ArgumentException("Value priorities must be larger than 0.", nameof(numericPriority));

            _priorities.Add(numericPriority);
            _priority = type;
        }

        private void SetPriorities(IEnumerable<int> numericPriorities, Priority type)
        {
            if (numericPriorities.Count() != _values.Count)
                throw new NumberOfValuesDoesNotMatchNumberOfPrioritiesException();

            if (numericPriorities.Any(p => p <= 0))
                throw new ArgumentException("Value priorities must be larger than 0.", nameof(numericPriorities));

            _priorities.AddRange(numericPriorities);

            _priority = type;
        }

        // Adding initial value(s)

        /// <summary>
        /// Specifies the first value.
        /// </summary>
        /// <param name="t">The first value.</param>
        /// <returns>An <see cref="ICanHaveValuePrioritiesAndPick{T}"/> instance.</returns>
        public ISpecifyValueOrGenesisValuePriority<T> Value(T t)
        {
            AddValue(t);
            return this;
        }

        /// <summary>
        /// Specifies all value.
        /// </summary>
        /// <param name="ts">The values.</param>
        /// <returns>An <see cref="ICanHaveValuePrioritiesAndPick{T}"/> instance.</returns>
        public ISpecifyValuePrioritiesOrPick<T> Values(IEnumerable<T> ts)
        {
            if (ts.Count() <= 1)
                throw new NotEnoughValuesToPickException();

            foreach (var t in ts)
            {
                AddValue(t);
            }

            return this;
        }

        // Adding additional value(s)

        /// <inheritdoc/>
        public ISpecifyValueOrValuePriorityOrPick<T> AndValue(T value)
        {
            AddValue(value);
            return this;
        }

        /// <inheritdoc/>
        ISpecifyPercentageValueOrValuePercentageOrPick<T> ISpecifyAdditionalValue<T, ISpecifyPercentageValueOrValuePercentageOrPick<T>>.AndValue(T value)
        {
            AddValue(value);
            return this;
        }

        /// <inheritdoc/>
        ISpecifyWeightValueOrValueWeightOrPick<T> ISpecifyAdditionalValue<T, ISpecifyWeightValueOrValueWeightOrPick<T>>.AndValue(T value)
        {
            AddValue(value);
            return this;
        }

        // Specifying percentage(s)

        /// <inheritdoc/>
        public ISpecifyPercentageValue<T> WithPercentage(int p)
        {
            SetPriority(p, Priority.Percentage);
            return this;
        }

        /// <inheritdoc/>
        ISpecifyPercentageValueOrPick<T> ISpecifyPercentage<T, ISpecifyPercentageValueOrPick<T>>.WithPercentage(int p)
        {
            SetPriority(p, Priority.Percentage);
            return this;
        }

        /// <inheritdoc/>
        public IPick<T> WithPercentages(IEnumerable<int> ps)
        {
            SetPriorities(ps, Priority.Percentage);
            return this;
        }

        /// <inheritdoc/>
        public IPick<T> WithPercentages(params int[] ps)
        {
            SetPriorities(ps, Priority.Percentage);
            return this;
        }

        // Specifying weight(s)

        /// <inheritdoc/>
        public ISpecifyWeightValue<T> WithWeight(int w)
        {
            SetPriority(w, Priority.Weight);
            return this;
        }

        /// <inheritdoc/>
        ISpecifyWeightValueOrPick<T> ISpecifyWeight<T, ISpecifyWeightValueOrPick<T>>.WithWeight(int w)
        {
            SetPriority(w, Priority.Weight);
            return this;
        }

        /// <inheritdoc/>
        public IPick<T> WithWeights(IEnumerable<int> ws)
        {
            SetPriorities(ws, Priority.Weight);
            return this;
        }

        /// <inheritdoc/>
        public IPick<T> WithWeights(params int[] ws)
        {
            SetPriorities(ws, Priority.Weight);
            return this;
        }

        // Picking

        /// <inheritdoc/>
        public IEnumerable<T> Pick(int n)
        {
            return new DefaultPicker<T>(_rng, GeneratePairs(), n).Pick().Result;
        }

        /// <inheritdoc/>
        public T PickOne()
        {
            return new DefaultPicker<T>(_rng, GeneratePairs()).Pick().Result.First();
        }

        /// <inheritdoc/>
        public IEnumerable<T> PickDistinct(int n)
        {
            return new DistinctPicker<T>(_rng, GeneratePairs(), n).Pick().Result;
        }

        // More
        private ValuePriorityPairs<T> GeneratePairs()
        {
            var valuePriorityPairs = new ValuePriorityPairs<T>();
            valuePriorityPairs.Priority = _priority;
            var priorityEnumerator = _priorities.GetEnumerator();
            foreach (var value in _values)
            {
                priorityEnumerator.MoveNext();
                valuePriorityPairs.Add(new ValuePriorityPair<T>(value, priorityEnumerator.Current));
            }

            return valuePriorityPairs;
        }
    }
}
