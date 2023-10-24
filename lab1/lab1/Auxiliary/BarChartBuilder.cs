using ScottPlot;
using ScottPlot.Plottable;
using ScottPlot.Statistics;

namespace lab1.Auxiliary
{
    public class BarChartBuilder
    {
        public int BarsCount { get; set; } = 100;
        public int PlotWidth { get; set; } = 1000;
        public int PlotHeight { get; set; } = 1000;

        public void BuildBarChart(List<double> numbers, String name)
        {
            Histogram hist = new(numbers.Min(), numbers.Max(), BarsCount);
            hist.AddRange(numbers);
            Plot plt = new(PlotWidth, PlotHeight);
            BarPlot? bar = plt.AddBar(values: hist.Counts, positions: hist.Bins);
            bar.BarWidth = (numbers.Max() - numbers.Min()) / hist.BinCount;
            plt.SaveFig("BarCharts/" + name + ".jpg");
        }
    }
}
