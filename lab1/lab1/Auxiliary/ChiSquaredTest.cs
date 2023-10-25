using lab1.Generators;
using MathNet.Numerics.Distributions;

namespace lab1.Auxiliary
{
    public static class ChiSquaredTest
    {
        public static int IntervalsCount { get; set; } = 100;
        public static int MinimalCount { get; set; } = 5;

        public static double Test(List<double> numbers, Generator generator, out double x2, out double tableX2)
        {
            double min = numbers.Min();
            double max = numbers.Max();
            double intervalsWidth = (max - min) / IntervalsCount;

            List<int> intervals = CreateIntervals(numbers, min, max, intervalsWidth);
            RemoveSmallIntervals(intervals, out List<(int bot, int up, int cnt)> MergedIntervals);
            x2 = ChiSquaredCalc(MergedIntervals, generator, min, intervalsWidth, numbers.Count);

            ChiSquared chiSquared = new(MergedIntervals.Count - 2);
            tableX2 = 0;
            for (double i = 0.01; i <= 1; i += 0.01)
            {
                tableX2 = chiSquared.InverseCumulativeDistribution(i);
                if (x2 < tableX2)
                    return 1 - i;
            }
            return 0;
        }

        private static List<int> CreateIntervals(List<double> numbers, double min, double max, double width)
        {
            int[] intervals = new int[IntervalsCount];
            foreach (double number in numbers)
            {
                int indx = (int)((number - min) / width);
                indx = number == max ? indx - 1 : indx;
                intervals[indx]++;
            }
            return intervals.ToList();
        }

        private static void RemoveSmallIntervals(List<int> intervals, out List<(int bot, int up, int cnt)> MergedIntervals)
        {
            MergedIntervals = new();
            int low = 0;
            int cnt = 0;
            for (int i = 0; i < intervals.Count; i++)
            {
                cnt += intervals[i];
                if (cnt < 5)
                    continue;
                MergedIntervals.Add((low, i + 1, cnt));
                low = i + 1;
                cnt = 0;
            }
            if (cnt != 0)
                MergedIntervals[^1] = (MergedIntervals[^1].bot, intervals.Count, MergedIntervals[^1].cnt + cnt);
        }

        private static double ChiSquaredCalc(List<(int bot, int up, int cnt)> MergedIntervals, Generator generator, double min, double intervalsWidth, int totalCount)
        {
            double x2 = 0;
            foreach (var interval in MergedIntervals)
            {
                double bot = min + intervalsWidth * interval.bot;
                double up = min + intervalsWidth * interval.up;
                double botDis = generator.GetFunctionValue(bot);
                double upDis = generator.GetFunctionValue(up);
                double exp = totalCount * (upDis - botDis);
                x2 += Math.Pow(interval.cnt - exp, 2) / exp;
            }
            return x2;
        }
    }
}
