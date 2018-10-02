using System.Data.Entity;
using Stock.DAL.TransferObjects;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Stock.DAL.Infrastructure
{
    public class EFDbContext : DbContext
    {

        private static EFDbContext _instance;
        public DbSet<QuotationDto> Quotations { get; set; }
        public DbSet<TrendlineDto> Trendlines { get; set; }
        public DbSet<ExtremumDto> Extrema { get; set; }
        public DbSet<AnalysisInfoDto> AnalysisInfos { get; set; }
        public DbSet<TrendHitDto> TrendHits { get; set; }
        public DbSet<TrendBreakDto> TrendBreaks { get; set; }
        public DbSet<TrendRangeDto> TrendRanges { get; set; }
        public DbSet<ExtremumGroupDto> ExtremumGroups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuotationDto>().ToTable("ViewQuotes");
            modelBuilder.Entity<TrendlineDto>().ToTable("trendlines");
            modelBuilder.Entity<ExtremumDto>().ToTable("extrema");
            modelBuilder.Entity<AnalysisInfoDto>().ToTable("ViewDataInfo");
            modelBuilder.Entity<TrendHitDto>().ToTable("GetVisibleTrendHits");
            modelBuilder.Entity<TrendBreakDto>().ToTable("GetVisibleTrendBreaks");
            modelBuilder.Entity<TrendRangeDto>().ToTable("GetVisibleTrendRanges");
            modelBuilder.Entity<ExtremumGroupDto>().ToTable("extremumGroups");
        }

        public EFDbContext()
        {
            Database.Initialize(false);
        }

        public static EFDbContext GetInstance()
        {
            return _instance ?? (_instance = new EFDbContext());
        }

    }
}