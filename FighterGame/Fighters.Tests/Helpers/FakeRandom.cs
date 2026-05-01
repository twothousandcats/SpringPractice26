namespace Fighters.Tests.Helpers
{
    public class FakeRandom : Random
    {
        private readonly Queue<double> _values;

        public FakeRandom(params double[] values)
        {
            _values = new Queue<double>(values);
        }

        public override double NextDouble()
        {
            if (_values.Count == 0)
            {
                throw new InvalidOperationException("FakeRandom ran out of values");
            }

            return _values.Dequeue();
        }
    }
}
