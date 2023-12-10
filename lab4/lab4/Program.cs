
namespace lab4
{
    public class Programm
    {
        public static void Main(string[] args)
        {
            Model simple = ModelCreator.CreateModel(1000);
            //simple.IsPrintingResults = true;
            simple.Simulate(1000);
            simple.PrintWorkingTime();

            Model lines = ModelCreator.CreateModelWithLines(250, 4);
            //lines.IsPrintingResults = true;
            lines.Simulate(1000);
            lines.PrintWorkingTime();
        }
    }
}