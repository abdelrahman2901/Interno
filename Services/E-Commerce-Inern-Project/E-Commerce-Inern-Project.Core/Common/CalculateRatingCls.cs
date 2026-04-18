namespace E_Commerce_Inern_Project.Core.Common
{
    public static class CalculateRatingCls
    {
        public static double CalcuateTheRating(int TotalRates, double AvarageRating)
        {
            double R = AvarageRating;
            int V = TotalRates;
            int M = 20;
            double C = 3;

            double NewRating = (((V * R) + (M * C)) / (V + M));
            return NewRating;
        }
    }
}
