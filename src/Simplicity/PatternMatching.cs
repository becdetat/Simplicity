using System;
using System.Collections.Generic;

namespace Simplicity
{
    public class IncompletePatternMatchException : Exception
    {
    }

    public static class PatternMatch
    {
        public static PatternMatchContext<TIn> Match<TIn>(this TIn value)
        {
            return new PatternMatchContext<TIn>(value);
        }

        public static PatternMatchContext Match()
        {
            return new PatternMatchContext();
        }

        public static PatternMatchOnDo<TIn, TOut> Match<TIn, TOut>()
        {
            return new PatternMatchOnDo<TIn, TOut>();
        }
    }

    public class PatternMatchWithoutInput<TOut>
    {
        private readonly IList<PatternMatchCase> _cases = new List<PatternMatchCase>();
        private Func<TOut> _elseCase;

        public PatternMatchWithoutInput<TOut> With(Func<bool> condition, Func<TOut> result)
        {
            _cases.Add(new PatternMatchCase
            {
                Condition = condition,
                Result = result
            });
            return this;
        }

        public PatternMatchWithoutInput<TOut> With(Func<bool> condition, TOut result)
        {
            return With(condition, () => result);
        }

        public static PatternMatchWithoutInput<TOut> Match()
        {
            return new PatternMatchWithoutInput<TOut>();
        }

        public PatternMatchWithoutInput<TOut> Else(Func<TOut> result)
        {
            if (_elseCase != null)
            {
                throw new InvalidOperationException("Cannot have multiple else cases");
            }

            _elseCase = result;

            return this;
        }

        public PatternMatchWithoutInput<TOut> Else(TOut result)
        {
            return Else(() => result);
        }

        public TOut Do()
        {
            if (_elseCase != null)
            {
                With(() => true, _elseCase);
            }

            foreach (var test in _cases)
            {
                if (test.Condition())
                {
                    return test.Result();
                }
            }

            throw new IncompletePatternMatchException();
        }

        public Func<TOut> ToFunc()
        {
            return Do;
        }

        private struct PatternMatchCase
        {
            public Func<bool> Condition;
            public Func<TOut> Result;
        }
    }

    public class PatternMatchOnDo<TIn, TOut>
    {
        private readonly IList<PatternMatchCase> _cases = new List<PatternMatchCase>();
        private Func<TIn, TOut> _elseCase;

        internal PatternMatchOnDo()
        {
        }

        public PatternMatchOnDo<TIn, TOut> With(Predicate<TIn> condition, Func<TIn, TOut> result)
        {
            _cases.Add(new PatternMatchCase
            {
                Condition = condition,
                Result = result
            });

            return this;
        }

        public PatternMatchOnDo<TIn, TOut> With(TIn value, Func<TIn, TOut> result)
        {
            return With(x => x.Equals(value), result);
        }

        public PatternMatchOnDo<TIn, TOut> With(Predicate<TIn> condition, TOut result)
        {
            return With(condition, x => result);
        }

        public PatternMatchOnDo<TIn, TOut> With(TIn value, TOut result)
        {
            return With(x => x.Equals(value), result);
        }

        public PatternMatchOnDo<TIn, TOut> Else(Func<TIn, TOut> result)
        {
            if (_elseCase != null)
            {
                throw new InvalidOperationException("Cannot have multiple else cases");
            }

            _elseCase = result;

            return this;
        }

        public PatternMatchOnDo<TIn, TOut> Else(TOut result)
        {
            return Else(x => result);
        }

        public TOut Do(TIn value)
        {
            if (_elseCase != null)
            {
                With(x => true, _elseCase);
            }

            foreach (var test in _cases)
            {
                if (test.Condition(value))
                {
                    return test.Result(value);
                }
            }

            throw new IncompletePatternMatchException();
        }

        public Func<TIn, TOut> ToFunc()
        {
            return Do;
        }

        private struct PatternMatchCase
        {
            public Predicate<TIn> Condition;
            public Func<TIn, TOut> Result;
        }
    }

    public class PatternMatchOnValue<TIn, TOut>
    {
        private readonly IList<PatternMatchCase> _cases = new List<PatternMatchCase>();
        private readonly TIn _value;
        private Func<TIn, TOut> _elseCase;

        internal PatternMatchOnValue(TIn value)
        {
            _value = value;
        }

        public PatternMatchOnValue<TIn, TOut> With(Predicate<TIn> condition, Func<TIn, TOut> result)
        {
            _cases.Add(new PatternMatchCase
            {
                Condition = condition,
                Result = result
            });

            return this;
        }

        public PatternMatchOnValue<TIn, TOut> With(TIn value, Func<TIn, TOut> result)
        {
            return With(x => x.Equals(value), result);
        }

        public PatternMatchOnValue<TIn, TOut> With(Predicate<TIn> condition, TOut result)
        {
            return With(condition, x => result);
        }

        public PatternMatchOnValue<TIn, TOut> With(TIn value, TOut result)
        {
            return With(x => x.Equals(value), result);
        }

        public PatternMatchOnValue<TIn, TOut> Else(Func<TIn, TOut> result)
        {
            if (_elseCase != null)
            {
                throw new InvalidOperationException("Cannot have multiple else cases");
            }

            _elseCase = result;

            return this;
        }

        public PatternMatchOnValue<TIn, TOut> Else(TOut result)
        {
            return Else(x => result);
        }

        public TOut Do()
        {
            if (_elseCase != null)
            {
                With(x => true, _elseCase);
                _elseCase = null;
            }

            foreach (var test in _cases)
            {
                if (test.Condition(_value))
                {
                    return test.Result(_value);
                }
            }

            throw new IncompletePatternMatchException();
        }

        public static implicit operator TOut(PatternMatchOnValue<TIn, TOut> pattern)
        {
            return pattern.Do();
        }

        private struct PatternMatchCase
        {
            public Predicate<TIn> Condition;
            public Func<TIn, TOut> Result;
        }
    }

    public class PatternMatchContext
    {
        internal PatternMatchContext()
        {
        }

        public PatternMatchWithoutInput<TOut> WithOutputType<TOut>()
        {
            return new PatternMatchWithoutInput<TOut>();
        }

        public PatternMatchWithoutInput<TOut> With<TOut>(Func<bool> condition, TOut result)
        {
            return new PatternMatchWithoutInput<TOut>().With(condition, result);
        }

        public PatternMatchWithoutInput<TOut> With<TOut>(Func<bool> condition, Func<TOut> result)
        {
            return new PatternMatchWithoutInput<TOut>().With(condition, result);
        }

        public PatternMatchWithoutInput<TOut> With<TOut>(bool condition, TOut result)
        {
            return new PatternMatchWithoutInput<TOut>().With(() => condition, result);
        }
    }

    public class PatternMatchContext<TIn>
    {
        private readonly TIn _value;

        internal PatternMatchContext(TIn value)
        {
            _value = value;
        }

        public PatternMatchOnValue<TIn, TOut> WithOutputType<TOut>()
        {
            return new PatternMatchOnValue<TIn, TOut>(_value);
        }

        public PatternMatchOnValue<TIn, TOut> With<TOut>(Predicate<TIn> condition, TOut result)
        {
            return new PatternMatchOnValue<TIn, TOut>(_value)
                .With(condition, result);
        }

        public PatternMatchOnValue<TIn, TOut> With<TOut>(TIn value, TOut result)
        {
            return new PatternMatchOnValue<TIn, TOut>(_value).With(x => x.Equals(value), result);
        }

        public PatternMatchOnValue<TIn, TOut> With<TOut>(Predicate<TIn> condition, Func<TIn, TOut> result)
        {
            return new PatternMatchOnValue<TIn, TOut>(_value)
                .With(condition, result);
        }

        public PatternMatchOnValue<TIn, TOut> With<TOut>(TIn value, Func<TIn, TOut> result)
        {
            return new PatternMatchOnValue<TIn, TOut>(_value).With(x => x.Equals(value), result);
        }
    }
}