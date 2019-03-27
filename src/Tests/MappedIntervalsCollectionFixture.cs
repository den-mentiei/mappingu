using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Contract;
using NUnit.Framework;

namespace Tests
{
    [TestFixtureSource(typeof(CollectionFactories), nameof(CollectionFactories.Factories))]
    internal sealed class MappedIntervalsCollectionFixture
    {
        private readonly IStorageFactory _factory;

        private IMappedIntervalsCollection<Crate> _sut;

        public MappedIntervalsCollectionFixture(IStorageFactory factory)
        {
            _factory = factory;
        }

        [SetUp]
        public void SetUp()
        {
            _sut = _factory.Create<Crate>();
        }

        [Test]
        public void CreationFromData()
        {
            var input = IntervalGeneration.Sequence(0, 5, 5, 10, MakeDummy).ToArray();

            var sut = _factory.Create(input);

            CollectionAssert.AreEqual(sut, input);
        }

        [Test]
        public void CreationFromUnorderedDataThrows()
        {
            var input = new[] { MakeInterval(0, 100, 1), MakeInterval(10, 20, 3), MakeInterval(5, 20, 4) };

            Assert.Throws<ArgumentException>(() => _factory.Create(input));
        }

        [Test]
        public void PutOfUnorderedDataOrderThrows()
        {
            var input = new[] { MakeInterval(0, 100, 1), MakeInterval(10, 20, 3), MakeInterval(10, 20, 4) };

            Assert.Throws<ArgumentException>(() => _sut.Put(input));
        }

        [Test]
        public void NonOverwritingUpdate()
        {
            var input = IntervalGeneration.DashedSequence(0, 2, 10, MakeDummy).ToArray();

            _sut.Put(input);

            var output = _sut.ToArray();
            CollectionAssert.AreEqual(input, output);
        }

        [Test]
        public void TouchingUpdate()
        {
            const int count = 5;
            const int duration = 5;
            const int step = 5;

            var input = IntervalGeneration.Sequence(0, duration, step, count, MakeDummy).ToArray();
            _sut.Put(input);

            CollectionAssert.AreEqual(_sut, input);
        }

        [Test]
        public void OverwritingUpdate()
        {
            var input = IntervalGeneration.Sequence(0, 10, 5, 5, MakeDummy).ToArray();
            AddIntervals(input);

            var output = _sut.ToArray();
            var expectedOutput = IntervalGeneration.Sequence(0, 5, 5, input.Length - 1, i => input[i].Payload).Concat(input.Skip(input.Length - 1));
            CollectionAssert.AreEqual(expectedOutput, output);
        }

        [TestCaseSource(nameof(UpdateTestCases))]
        public void Universal(string tag, string initial, string update, string expected)
        {
            var initialIntervals = Parse(initial).ToArray();
            var addedIntervals = Parse(update).ToArray();
            var expectedIntervals = Parse(expected).ToArray();

            _sut.Put(initialIntervals);

            _sut.Put(addedIntervals);

            CollectionAssert.AreEqual(_sut, expectedIntervals);
        }

        private static IEnumerable<object[]> UpdateTestCases()
        {
            yield return new[]
            {
                "part-same-payload",
                "111  222  333",
                "1     2     3",
                "111  222  333",
            };

            yield return new[]
            {
                "part-other-payload",
                "111  222  333",
                "3     1     2",
                "311  212  332",
            };

            yield return new[]
            {
                "overlap-same-payload",
                "111     222     333",
                "1111   22222   3333",
                "1111   22222   3333",
            };

            yield return new[]
            {
                "overlap-other-payload",
                "111     222     333",
                "3333   11111   2222",
                "3333   11111   2222",
            };

            yield return new[]
            {
                "overlapping-part-same-payload",
                "111    22222    333",
                " 111    222    333 ",
                "1111   22222   3333",
            };

            yield return new[]
            {
                "overlapping-part-other-payload",
                "111    22222    333",
                " 333    111    222 ",
                "1333   21112   2223",
            };

            yield return new[]
            {
                "overlap-multiple-same",
                "111    222    333    444",
                "     2222222222222      ",
                "111  2222222222222   444",
            };

            yield return new[]
            {
                "overlap-part-multiple",
                "111    222    333    444",
                "     22222222222        ",
                "111  222222222223    444",
            };

            yield return new[]
            {
                "overlap-part-multiple",
                "111    222    333    444",
                "  222222222222222       ",
                "11222222222222222    444",
            };
        }

        [TestCase(0)]
        [TestCase(10)]
        public void SingleRemove(int margin)
        {
            var i = new MappedInterval<Crate>(0, 100, MakeDummy(0));
            _sut.Put(new [] { i });

            _sut.Delete(i.IntervalStart - margin, i.IntervalEnd + margin);

            Assert.That(_sut.Count, Is.Zero);
        }

        [Test]
        public void PartialRemove()
        {
            var i = new MappedInterval<Crate>(0, 1000, MakeDummy(0));
            _sut.Put(new []{ i });

            var gapFrom = 100;
            var gapTo = 400;

            _sut.Delete(gapFrom, gapTo);

            var output = _sut.ToArray();
            Assert.That(output.Length, Is.EqualTo(2));
            Assert.That(output[0].Payload.Value, Is.EqualTo(0));
            Assert.That(output[0].IntervalStart, Is.EqualTo(i.IntervalStart));
            Assert.That(output[0].IntervalEnd, Is.EqualTo(gapFrom));
            Assert.That(output[1].Payload.Value, Is.EqualTo(0));
            Assert.That(output[1].IntervalStart, Is.EqualTo(gapTo));
            Assert.That(output[1].IntervalEnd, Is.EqualTo(i.IntervalEnd));
        }

        [TestCaseSource(nameof(RemovalTestCases))]
        public void Removals(string tag, string initial, string target, string expected)
        {
            var initialIntervals = Parse(initial).ToArray();
            var toBeRemoved = Parse(target).ToArray();
            var expectedIntervals = Parse(expected).ToArray();

            _sut.Put(initialIntervals);

            foreach (var i in toBeRemoved)
            {
                _sut.Delete(i.IntervalStart, i.IntervalEnd);
            }

            CollectionAssert.AreEqual(_sut, expectedIntervals);
        }

        private static IEnumerable<object[]> RemovalTestCases()
        {
            yield return new[]
            {
                "nothing",
                "  11111   22222   ",
                "1       1        1",
                "  11111   22222   ",
            };

            yield return new[]
            {
                "part",
                "111   222   333",
                "1      2      3",
                " 11   2 2   33 ",
            };

            yield return new[]
            {
                "touching-overlapping-part",
                "111   222   333",
                "1111111        ",
                "       22   333",
            };

            yield return new[]
            {
                "whole",
                "111   222   333",
                "    1111111    ",
                "111         333",
            };

            yield return new[]
            {
                "overlapping-part",
                "111   222   333",
                "  11111        ",
                "11     22   333",
            };
        }

        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(0, 5)]
        [TestCase(0, 6)]
        public void EnumerationFrom(int intervalsToSkip, int margin)
        {
            const int count = 10;
            const int duration = 5;
            const int step = 5;

            var input = IntervalGeneration.Sequence(0, duration, step, count, MakeDummy).ToArray();
            _sut.Put(input);

            var from = (duration + step) * intervalsToSkip + margin;
            var later = input.Where(i => i.IntervalEnd >= from).ToArray();

            var part = CollectFrom(_sut, from).ToArray();
            Assert.That(part.Length, Is.EqualTo(later.Length));
            CollectionAssert.AreEqual(part, later);
        }

        private void AddIntervals(params MappedInterval<Crate>[] intervals)
        {
            var box = new MappedInterval<Crate>[1];
            foreach (var i in intervals)
            {
                box[0] = i;
                _sut.Put(box);
            }
        }

        private static IEnumerable<MappedInterval<Crate>> Parse(string description)
        {
            var now = 0L;
            var i = 0;
            long? start = null;

            int? current = null;
            while (i < description.Length)
            {
                var c = description[i];
                if (!current.HasValue && c != ' ')
                {
                    Debug.Assert(!start.HasValue);
                    current = c - '0';
                    start = now;
                }
                else if (current.HasValue)
                {
                    Debug.Assert(start.HasValue);
                    if ((c - '0') != current)
                    {
                        yield return new MappedInterval<Crate>(start.Value, now, MakeDummy(current.Value));
                        current = null;
                        start = null;
                        continue;
                    }
                }

                ++i;
                now += 10;
            }

            if (current.HasValue)
            {
                Debug.Assert(start.HasValue);
                yield return new MappedInterval<Crate>(start.Value, now, MakeDummy(current.Value));
            }
        }

        private static IEnumerable<MappedInterval<T>> CollectFrom<T>(IMappedIntervalsCollection<T> collection, long from)
        {
            using (var e = collection.GetEnumerator(from))
            {
                while (e.MoveNext())
                {
                    yield return e.Current;
                }
            }
        }

        private static MappedInterval<Crate> MakeInterval(long from, long to, int payload)
        {
            return new MappedInterval<Crate>(from, to, MakeDummy(payload));
        }

        private static Crate MakeDummy(int value)
        {
            return new Crate(value);
        }
    }
}