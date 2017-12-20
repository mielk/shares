namespace Stock.DAL.Repositories
{
    public class RepositoryFactory
    {
        private static readonly IQuotationRepository quotationRepository;
        private static readonly ITrendlineRepository trendlineRepository;

        static RepositoryFactory()
        {
            quotationRepository = new EFQuotationRepository();
            trendlineRepository = new EFTrendlineRepository();
        }


        public static IQuotationRepository GetQuotationRepository()
        {
            return quotationRepository;
        }

        public static ITrendlineRepository GetTrendlineRepository()
        {
            return trendlineRepository;
        }

    }
}

