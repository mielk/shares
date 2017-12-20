namespace Stock.Domain.Entities
{
    public class Price
    {
        public int Id { get; set; }
        public Extremum PeakByClose { get; set; }
        public Extremum PeakByHigh { get; set; }
        public Extremum TroughByClose { get; set; }
        public Extremum TroughByLow { get; set; }

        public void SetExtremum(Extremum extremum)
        {
            switch (extremum.ExtremumType)
            {
                case 1: PeakByClose = extremum; break;
                case 2: PeakByHigh = extremum; break;
                case 3: TroughByClose = extremum; break;
                case 4: TroughByLow = extremum; break;
            }
        }

    }

}