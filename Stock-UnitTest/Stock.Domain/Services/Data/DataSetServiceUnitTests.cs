using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.DAL.Repositories;
using Stock.DAL.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Stock.Domain.Entities;
using Stock.Domain.Services;
using Stock.Domain.Enums;
using Stock.Utils;
using Stock.Core;
using Stock_UnitTest.Helpers;

namespace Stock_UnitTest.Stock.Domain.Services
{
    [TestClass]
    public class DataSetServiceUnitTests
    {

        private UTFactory utf = new UTFactory();


        #region GET_DATA_SETS

        [TestMethod]
        public void GetDataSets_ReturnsEmptyCollection_IfThereIsNoQuotationForGivenDateRange()
        {

            //Arrange
            IEnumerable<AnalysisType> analysisTypes = new AnalysisType[] { AnalysisType.Quotations, AnalysisType.Prices };
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(UTDefaulter.DEFAULT_ASSET_ID, UTDefaulter.DEFAULT_TIMEFRAME_ID) { AnalysisTypes = analysisTypes };
            Mock<IQuotationRepository> quotationRepository = new Mock<IQuotationRepository>();
            Mock<IPriceRepository> priceRepository = new Mock<IPriceRepository>();
            IEnumerable<PriceDto> priceDtos = utf.getDefaultPriceDtosCollection();
            quotationRepository.Setup(q => q.GetQuotations(queryDef)).Returns(new List<QuotationDto>());
            priceRepository.Setup(q => q.GetPrices(queryDef)).Returns(priceDtos);
            
            //Act
            IDataSetService service = new DataSetService();
            service.InjectQuotationRepository(quotationRepository.Object);
            service.InjectPriceRepository(priceRepository.Object);
            var dataSets = service.GetDataSets(queryDef);

            //Assert
            var isEmpty = (dataSets.Count() == 0);
            Assert.IsTrue(isEmpty);

        }

        [TestMethod]
        public void GetDataSets_ReturnsCollectionWithQuotationsOnly_IfGivenAnalysisTypesParameterIsInvalid()
        {

            //Arrange:QueryDef
            IEnumerable<AnalysisType> analysisTypes = new AnalysisType[] { };
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(UTDefaulter.DEFAULT_ASSET_ID, UTDefaulter.DEFAULT_TIMEFRAME_ID) { AnalysisTypes = analysisTypes };

            //Arrange:Quotations
            Mock<IQuotationRepository> quotationRepository = new Mock<IQuotationRepository>();
            IEnumerable<QuotationDto> quotationDtos = utf.getDefaultQuotationDtosCollection();
            quotationRepository.Setup(q => q.GetQuotations(queryDef)).Returns(quotationDtos);

            //Arrange:Prices
            Mock<IPriceRepository> priceRepository = new Mock<IPriceRepository>();
            IEnumerable<PriceDto> priceDtos = utf.getDefaultPriceDtosCollection();
            priceRepository.Setup(p => p.GetPrices(queryDef)).Returns(priceDtos);

            //Act
            IDataSetService service = new DataSetService();
            service.InjectQuotationRepository(quotationRepository.Object);
            service.InjectPriceRepository(priceRepository.Object);
            var dataSets = service.GetDataSets(queryDef);

            //Assert
            DataSet ds1 = utf.getDataSet(1);
            ds1.SetQuotation(utf.getQuotation(ds1));
            DataSet ds2 = utf.getDataSet(2);
            ds2.SetQuotation(utf.getQuotation(ds2));
            DataSet ds3 = utf.getDataSet(3);
            ds3.SetQuotation(utf.getQuotation(ds3));
            DataSet ds4 = utf.getDataSet(4);
            ds4.SetQuotation(utf.getQuotation(ds4));

            IEnumerable<DataSet> expectedDataSets = new DataSet[] { ds1, ds2, ds3, ds4 };
            var areEqual = expectedDataSets.HasEqualItems(dataSets);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetDataSets_ReturnsCollectionWithAllGivenAnalysisTypes()
        {

            //Arrange:QueryDef
            IEnumerable<AnalysisType> analysisTypes = new AnalysisType[] { AnalysisType.Prices };
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(UTDefaulter.DEFAULT_ASSET_ID, UTDefaulter.DEFAULT_TIMEFRAME_ID) { AnalysisTypes = analysisTypes };
            
            //Arrange:Quotations
            Mock<IQuotationRepository> quotationRepository = new Mock<IQuotationRepository>();
            IEnumerable<QuotationDto> quotationDtos = utf.getDefaultQuotationDtosCollection();
            quotationRepository.Setup(q => q.GetQuotations(queryDef)).Returns(quotationDtos);

            //Arrange:Prices
            Mock<IPriceRepository> priceRepository = new Mock<IPriceRepository>();
            IEnumerable<PriceDto> priceDtos = new PriceDto[] { utf.getPriceDto(1), utf.getPriceDto(2), utf.getPriceDto(3), utf.getPriceDto(4) }; 
            priceRepository.Setup(p => p.GetPrices(queryDef)).Returns(priceDtos);

            //Act
            IDataSetService service = new DataSetService();
            service.InjectQuotationRepository(quotationRepository.Object);
            service.InjectPriceRepository(priceRepository.Object);
            var dataSets = service.GetDataSets(queryDef);

            //Assert
            DataSet ds1 = utf.getDataSet(1);
            ds1.SetQuotation(utf.getQuotation(ds1)).SetPrice(utf.getPrice(ds1));
            DataSet ds2 = utf.getDataSet(2);
            ds2.SetQuotation(utf.getQuotation(ds2)).SetPrice(utf.getPrice(ds2));
            DataSet ds3 = utf.getDataSet(3);
            ds3.SetQuotation(utf.getQuotation(ds3)).SetPrice(utf.getPrice(ds3));
            DataSet ds4 = utf.getDataSet(4);
            ds4.SetQuotation(utf.getQuotation(ds4)).SetPrice(utf.getPrice(ds4));
            IEnumerable<DataSet> expectedDataSets = new DataSet[] { ds1, ds2, ds3, ds4 };
            var areEqual = expectedDataSets.HasEqualItems(dataSets);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetDataSets_ReturnsCollectionWithoutSomeAnalysisType_IfThereIsNoDataForThisType()
        {

            //Arrange:QueryDef
            IEnumerable<AnalysisType> analysisTypes = new AnalysisType[] { AnalysisType.Prices };
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(UTDefaulter.DEFAULT_ASSET_ID, UTDefaulter.DEFAULT_TIMEFRAME_ID) { AnalysisTypes = analysisTypes };

            //Arrange:Quotations
            Mock<IQuotationRepository> quotationRepository = new Mock<IQuotationRepository>();
            IEnumerable<QuotationDto> quotationDtos = new QuotationDto[] { utf.getQuotationDto(2), utf.getQuotationDto(3), utf.getQuotationDto(4) };
            quotationRepository.Setup(q => q.GetQuotations(queryDef)).Returns(quotationDtos);

            //Arrange:Prices
            Mock<IPriceRepository> priceRepository = new Mock<IPriceRepository>();
            IEnumerable<PriceDto> priceDtos = new PriceDto[] { utf.getPriceDto(3), utf.getPriceDto(4), utf.getPriceDto(5), utf.getPriceDto(6) }; 
            priceRepository.Setup(p => p.GetPrices(queryDef)).Returns(priceDtos);

            //Act
            IDataSetService service = new DataSetService();
            service.InjectQuotationRepository(quotationRepository.Object);
            service.InjectPriceRepository(priceRepository.Object);
            var dataSets = service.GetDataSets(queryDef);

            //Assert
            DataSet ds2 = utf.getDataSet(2);
            ds2.SetQuotation(utf.getQuotation(ds2));
            DataSet ds3 = utf.getDataSet(3);
            ds3.SetQuotation(utf.getQuotation(ds3)).SetPrice(utf.getPrice(ds3));
            DataSet ds4 = utf.getDataSet(4);
            ds4.SetQuotation(utf.getQuotation(ds4)).SetPrice(utf.getPrice(ds4));
            IEnumerable<DataSet> expectedDataSets = new DataSet[] { ds2, ds3, ds4 };
            var areEqual = expectedDataSets.HasEqualItems(dataSets);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetDataSets_ReturnsCollectionWithExistingItems()
        {

            //Arrange:QueryDef
            IEnumerable<AnalysisType> analysisTypes = new AnalysisType[] { AnalysisType.Prices };
            DateTime startDate = new DateTime(2016, 1, 15, 22, 30, 0);
            DateTime endDate = new DateTime(2016, 1, 15, 22, 50, 0);
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1) { AnalysisTypes = analysisTypes, StartDate = startDate, EndDate = endDate };

            //Arrange:Quotations
            Mock<IQuotationRepository> quotationRepository = new Mock<IQuotationRepository>();
            IEnumerable<QuotationDto> quotationDtos = new QuotationDto[] { utf.getQuotationDto(1), utf.getQuotationDto(2), utf.getQuotationDto(3), utf.getQuotationDto(4) };
            quotationRepository.Setup(q => q.GetQuotations(queryDef)).Returns(quotationDtos);

            //Arrange:Prices
            Mock<IPriceRepository> priceRepository = new Mock<IPriceRepository>();
            IEnumerable<PriceDto> priceDtos = new PriceDto[] { utf.getPriceDto(1), utf.getPriceDto(2), utf.getPriceDto(3), utf.getPriceDto(4) };
            priceRepository.Setup(p => p.GetPrices(queryDef)).Returns(priceDtos);

            //Act
            IDataSetService service = new DataSetService();
            service.InjectQuotationRepository(quotationRepository.Object);
            service.InjectPriceRepository(priceRepository.Object);
            var dataSets = service.GetDataSets(queryDef);
            DataSet baseDataSet = dataSets.SingleOrDefault(ds => ds.IndexNumber == 1);

            queryDef.StartDate = new DateTime(2016, 1, 15, 22, 25, 0);
            dataSets = service.GetDataSets(queryDef);
            DataSet comparedDataSet = dataSets.SingleOrDefault(ds => ds.IndexNumber == 1);

            //Assert
            var areTheSameObject = (baseDataSet == comparedDataSet);
            Assert.IsTrue(areTheSameObject);

        }

        [TestMethod]
        public void GetDataSets_UseAlwaysExistingInstancesOfObjects_EvenIfThereIsAnotherOneInRepository()
        {

            //Arrange:QueryDef
            IEnumerable<AnalysisType> analysisTypes = new AnalysisType[] { AnalysisType.Prices };
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(1, 1) { AnalysisTypes = analysisTypes };

            //Arrange:Quotations
            Mock<IQuotationRepository> quotationRepository = new Mock<IQuotationRepository>();
            IEnumerable<QuotationDto> quotationDtos = new QuotationDto[] { utf.getQuotationDto(1), utf.getQuotationDto(2), utf.getQuotationDto(3), utf.getQuotationDto(4) };
            quotationRepository.Setup(q => q.GetQuotations(queryDef)).Returns(quotationDtos);

            //Arrange:Prices
            Mock<IPriceRepository> priceRepository = new Mock<IPriceRepository>();
            IEnumerable<PriceDto> priceDtos = new PriceDto[] { utf.getPriceDto(1), utf.getPriceDto(2), utf.getPriceDto(3), utf.getPriceDto(4) };
            priceRepository.Setup(p => p.GetPrices(queryDef)).Returns(priceDtos);

            //Act
            IDataSetService service = new DataSetService();
            service.InjectQuotationRepository(quotationRepository.Object);
            service.InjectPriceRepository(priceRepository.Object);
            var dataSets = service.GetDataSets(queryDef);
            DataSet baseDataSet = dataSets.SingleOrDefault(ds => ds.IndexNumber == 2);

            Quotation stubQuotation = utf.getQuotation(2);
            stubQuotation.Open = stubQuotation.Open + 3;
            IEnumerable<Quotation> quotations = new Quotation[] { utf.getQuotation(1), stubQuotation, utf.getQuotation(3), utf.getQuotation(4) };
            quotationRepository.Setup(q => q.GetQuotations(queryDef)).Returns(quotationDtos);

            service.InjectQuotationRepository(quotationRepository.Object);
            dataSets = service.GetDataSets(queryDef);
            DataSet comparedDataSet = dataSets.SingleOrDefault(ds => ds.IndexNumber == 2);

            //Assert
            var areTheSameObject = (baseDataSet.GetQuotation() == comparedDataSet.GetQuotation());
            Assert.IsTrue(areTheSameObject);

        }

        [TestMethod]
        public void GetDataSets_DoesntOverrideExistingObjectsWithNulls_IfTheyAreNotInSpecificRepositories()
        {

            //Arrange:QueryDef
            IEnumerable<AnalysisType> analysisTypes = new AnalysisType[] { AnalysisType.Prices };
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(UTDefaulter.DEFAULT_ASSET_ID, UTDefaulter.DEFAULT_TIMEFRAME_ID) { AnalysisTypes = analysisTypes };

            //Arrange:Quotations
            Mock<IQuotationRepository> quotationRepository = new Mock<IQuotationRepository>();
            IEnumerable<QuotationDto> quotationDtos = new QuotationDto[] { utf.getQuotationDto(1), utf.getQuotationDto(2), utf.getQuotationDto(3), utf.getQuotationDto(4) };
            quotationRepository.Setup(q => q.GetQuotations(queryDef)).Returns(quotationDtos);

            //Arrange:Prices
            Mock<IPriceRepository> priceRepository = new Mock<IPriceRepository>();
            IEnumerable<PriceDto> priceDtos = new PriceDto[] { utf.getPriceDto(1), utf.getPriceDto(2), utf.getPriceDto(3), utf.getPriceDto(4) };
            priceRepository.Setup(p => p.GetPrices(queryDef)).Returns(priceDtos);

            //Act
            IDataSetService service = new DataSetService();
            service.InjectQuotationRepository(quotationRepository.Object);
            service.InjectPriceRepository(priceRepository.Object);
            var dataSets = service.GetDataSets(queryDef);
            if (dataSets == null) throw new Exception("Collection should not be null");

            DataSet baseDataSet = dataSets.SingleOrDefault(ds => ds.IndexNumber == 2);
            if (baseDataSet == null) throw new Exception("Base data set shouldn't be null");

            Price basePrice = (baseDataSet == null ? null : baseDataSet.GetPrice());
            if (basePrice == null) throw new Exception("Base price shouldn't be null");
            

            priceRepository.Setup(q => q.GetPrices(queryDef)).Returns(new PriceDto[] { });
            service.InjectPriceRepository(priceRepository.Object);
            dataSets = service.GetDataSets(queryDef);
            DataSet comparedDataSet = dataSets.SingleOrDefault(ds => ds.IndexNumber == 2);
            Price comparedPrice = (comparedDataSet == null ? null : comparedDataSet.GetPrice());

            //Assert
            var areTheSameObject = (basePrice == comparedPrice);
            Assert.IsTrue(areTheSameObject);

        }

        #endregion GET_DATA_SETS



        #region GET_DATA_SETS_WITH_INITIAL_COLLECTION

        [TestMethod]
        public void GetDataSetsWithInitialCollection_ReturnsNullArray_IfNullArrayWasPassedAndThereWereNoDataToBeAppended()
        {

            //Arrange
            IEnumerable<AnalysisType> analysisTypes = new AnalysisType[] { AnalysisType.Quotations, AnalysisType.Prices };
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(UTDefaulter.DEFAULT_ASSET_ID, UTDefaulter.DEFAULT_TIMEFRAME_ID) { AnalysisTypes = analysisTypes };
            Mock<IQuotationRepository> quotationRepository = new Mock<IQuotationRepository>();
            Mock<IPriceRepository> priceRepository = new Mock<IPriceRepository>();
            IEnumerable<PriceDto> priceDtos = utf.getDefaultPriceDtosCollection();
            quotationRepository.Setup(q => q.GetQuotations(queryDef)).Returns(new List<QuotationDto>());
            priceRepository.Setup(p => p.GetPrices(queryDef)).Returns(priceDtos);

            //Act
            DataSet[] previous = new DataSet[] { };
            IDataSetService service = new DataSetService();
            service.InjectQuotationRepository(quotationRepository.Object);
            service.InjectPriceRepository(priceRepository.Object);
            var dataSets = service.GetDataSets(queryDef, previous).ToArray();

            //Assert
            var isEmpty = (dataSets.Length == 0 || (dataSets.Length == 1 && dataSets[0] == null));
            Assert.IsTrue(isEmpty);

        }

        [TestMethod]
        public void GetDataSetsWithInitialCollection_ReturnsArrayWithoutChanges_IfNonEmptyArrayWasPassedAndThereWereNoDataToBeAppended()
        {

            //Arrange
            IEnumerable<AnalysisType> analysisTypes = new AnalysisType[] { AnalysisType.Quotations, AnalysisType.Prices };
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(UTDefaulter.DEFAULT_ASSET_ID, UTDefaulter.DEFAULT_TIMEFRAME_ID) { AnalysisTypes = analysisTypes };
            Mock<IQuotationRepository> quotationRepository = new Mock<IQuotationRepository>();
            Mock<IPriceRepository> priceRepository = new Mock<IPriceRepository>();
            IEnumerable<PriceDto> priceDtos = utf.getDefaultPriceDtosCollection();
            quotationRepository.Setup(q => q.GetQuotations(queryDef)).Returns(new List<QuotationDto>());
            priceRepository.Setup(p => p.GetPrices(queryDef)).Returns(priceDtos);

            //Act
            DataSet[] baseDataSets = new DataSet[11];
            baseDataSets[7] = utf.getDataSet(7);
            baseDataSets[8] = utf.getDataSet(8);
            baseDataSets[9] = utf.getDataSet(9);
            baseDataSets[10] = utf.getDataSet(10);
            IDataSetService service = new DataSetService();
            service.InjectQuotationRepository(quotationRepository.Object);
            service.InjectPriceRepository(priceRepository.Object);
            var dataSets = service.GetDataSets(queryDef, baseDataSets).ToArray();

            //Assert
            var areEqual = baseDataSets.HasEqualItemsInTheSameOrder(dataSets);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetDataSetsWithInitialCollection_ReturnsProperlyFilledAndIndexedArray_IfNullArrayWasPassedAndSomeDataWereAppended()
        {

            //Arrange
            IEnumerable<AnalysisType> analysisTypes = new AnalysisType[] { AnalysisType.Quotations, AnalysisType.Prices };
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(UTDefaulter.DEFAULT_ASSET_ID, UTDefaulter.DEFAULT_TIMEFRAME_ID) { AnalysisTypes = analysisTypes };
            Mock<IQuotationRepository> quotationRepository = new Mock<IQuotationRepository>();
            Mock<IPriceRepository> priceRepository = new Mock<IPriceRepository>();

            //Act
            IDataSetService service = new DataSetService();
            IEnumerable<QuotationDto> quotationDtos = new QuotationDto[] { utf.getQuotationDto(101), utf.getQuotationDto(102), utf.getQuotationDto(103) };
            quotationRepository.Setup(q => q.GetQuotations(queryDef)).Returns(quotationDtos);
            IEnumerable<PriceDto> priceDtos = new PriceDto[] { utf.getPriceDto(101), utf.getPriceDto(102), utf.getPriceDto(103) };
            priceRepository.Setup(q => q.GetPrices(queryDef)).Returns(priceDtos);
            service.InjectQuotationRepository(quotationRepository.Object);
            service.InjectPriceRepository(priceRepository.Object);
            DataSet[] result = service.GetDataSets(queryDef, new List<DataSet>()).ToArray();

            //Assert
            DataSet[] expectedDataSets = new DataSet[104];
            DataSet ds101 = utf.getDataSet(101);
            ds101.SetQuotation(utf.getQuotation(ds101)).SetPrice(utf.getPrice(ds101));
            DataSet ds102 = utf.getDataSet(102);
            ds102.SetQuotation(utf.getQuotation(ds102)).SetPrice(utf.getPrice(ds102));
            DataSet ds103 = utf.getDataSet(103);
            ds103.SetQuotation(utf.getQuotation(ds103)).SetPrice(utf.getPrice(ds103));
            expectedDataSets[101] = ds101;
            expectedDataSets[102] = ds102;
            expectedDataSets[103] = ds103;
            var areEqual = expectedDataSets.HasEqualItemsInTheSameOrder(result);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void GetDataSetsWithInitialCollection_ReturnsProperlyFilledArray_IfNonEmptyArrayWasPassedAndSomeDataWereAppended()
        {

            //Arrange.
            IEnumerable<AnalysisType> analysisTypes = new AnalysisType[] { AnalysisType.Quotations, AnalysisType.Prices };
            AnalysisDataQueryDefinition queryDef = new AnalysisDataQueryDefinition(UTDefaulter.DEFAULT_ASSET_ID, UTDefaulter.DEFAULT_TIMEFRAME_ID) { AnalysisTypes = analysisTypes };

            //Quotation repository.
            Mock<IQuotationRepository> quotationRepository = new Mock<IQuotationRepository>();
            IEnumerable<QuotationDto> quotationDtos = new QuotationDto[] { utf.getQuotationDto(1), utf.getQuotationDto(2), utf.getQuotationDto(3) };
            quotationRepository.Setup(q => q.GetQuotations(queryDef)).Returns(quotationDtos);

            //Price repository.
            Mock<IPriceRepository> priceRepository = new Mock<IPriceRepository>();
            IEnumerable<PriceDto> priceDtos = new PriceDto[] { utf.getPriceDto(1), utf.getPriceDto(2), utf.getPriceDto(3) };
            priceRepository.Setup(q => q.GetPrices(queryDef)).Returns(priceDtos);

            //Act
            IDataSetService service = new DataSetService();
            service.InjectQuotationRepository(quotationRepository.Object);
            service.InjectPriceRepository(priceRepository.Object);
            var dataSets = service.GetDataSets(queryDef);

            IEnumerable<QuotationDto> quotationDtos2 = new QuotationDto[] { utf.getQuotationDto(101), utf.getQuotationDto(102), utf.getQuotationDto(103) };
            quotationRepository.Setup(q => q.GetQuotations(queryDef)).Returns(quotationDtos2);

            IEnumerable<PriceDto> priceDtos2 = new PriceDto[] { utf.getPriceDto(101), utf.getPriceDto(102), utf.getPriceDto(103) };
            priceRepository.Setup(q => q.GetPrices(queryDef)).Returns(priceDtos2);
            service.InjectQuotationRepository(quotationRepository.Object);
            service.InjectPriceRepository(priceRepository.Object);
            DataSet[] result = service.GetDataSets(queryDef, dataSets).ToArray();

            //Assert
            DataSet[] expectedDataSets = new DataSet[104];

            DataSet ds1 = utf.getDataSet(1);
            ds1.SetQuotation(utf.getQuotation(ds1)).SetPrice(utf.getPrice(ds1));
            DataSet ds2 = utf.getDataSet(2);
            ds2.SetQuotation(utf.getQuotation(ds2)).SetPrice(utf.getPrice(ds2)); ;
            DataSet ds3 = utf.getDataSet(3);
            ds3.SetQuotation(utf.getQuotation(ds3)).SetPrice(utf.getPrice(ds3));
            DataSet ds101 = utf.getDataSet(101);
            ds101.SetQuotation(utf.getQuotation(ds101)).SetPrice(utf.getPrice(ds101));
            DataSet ds102 = utf.getDataSet(102);
            ds102.SetQuotation(utf.getQuotation(ds102)).SetPrice(utf.getPrice(ds102));
            DataSet ds103 = utf.getDataSet(103);
            ds103.SetQuotation(utf.getQuotation(ds103)).SetPrice(utf.getPrice(ds103));

            expectedDataSets[1] = ds1;
            expectedDataSets[2] = ds2;
            expectedDataSets[3] = ds3;
            expectedDataSets[101] = ds101;
            expectedDataSets[102] = ds102;
            expectedDataSets[103] = ds103;
            var areEqual = expectedDataSets.HasEqualItemsInTheSameOrder(result);
            Assert.IsTrue(areEqual);

        }

        #endregion GET_DATA_SETS_WITH_INITIAL_COLLECTION



        #region UPDATE_DATA_SETS

        [TestMethod]
        public void UpdateDataSets_OnlyQuotationsFlaggedAsUpdatedAreSendToRepository()
        {

            //Arrange
            QuotationUpdateTester quotationRepository = new QuotationUpdateTester();
            PriceUpdateTester priceRepository = new PriceUpdateTester();

            List<DataSet> dataSets = new List<DataSet>();
            DataSet ds1 = utf.getDataSet(1);
            Quotation q1 = utf.getQuotation(ds1);
            Price p1 = utf.getPrice(ds1);

            DataSet ds2 = utf.getDataSet(2);
            Quotation q2 = utf.getQuotation(ds2);
            Price p2 = utf.getPrice(ds2);

            DataSet ds3 = utf.getDataSet(3);
            Quotation q3 = utf.getQuotation(ds3);
            Price p3 = utf.getPrice(ds3);

            DataSet ds4 = utf.getDataSet(4);
            Quotation q4 = utf.getQuotation(ds4);
            Price p4 = utf.getPrice(ds4);

            dataSets.AddRange(new DataSet[] { ds1, ds2, ds3, ds4 });

            //Act
            q1.Updated = true;
            q3.Updated = true;
            q4.New = true;

            IDataSetService service = new DataSetService();
            service.InjectQuotationRepository(quotationRepository);
            service.InjectPriceRepository(priceRepository);
            service.UpdateDataSets(dataSets);

            //Assert
            IEnumerable<QuotationDto> passedDtos = quotationRepository.GetQuotationDtosPassedForUpdate();
            List<QuotationDto> expectedDtos = new List<QuotationDto>();
            expectedDtos.AddRange(new QuotationDto[] { q1.ToDto(), q3.ToDto(), q4.ToDto() });
            bool areEqual = expectedDtos.HasEqualItems(passedDtos);
            Assert.IsTrue(areEqual);

        }


        [TestMethod]
        public void UpdatePrices_OnlyItemsFlaggedAsUpdatedAreSendToRepository()
        {

            //Arrange
            QuotationUpdateTester quotationRepository = new QuotationUpdateTester();
            PriceUpdateTester priceRepository = new PriceUpdateTester();

            List<DataSet> dataSets = new List<DataSet>();
            DataSet ds1 = utf.getDataSet(1);
            Quotation q1 = utf.getQuotation(ds1);
            Price p1 = utf.getPrice(ds1);

            DataSet ds2 = utf.getDataSet(2);
            Quotation q2 = utf.getQuotation(ds2);
            Price p2 = utf.getPrice(ds2);

            DataSet ds3 = utf.getDataSet(3);
            Quotation q3 = utf.getQuotation(ds3);
            Price p3 = utf.getPrice(ds3);

            DataSet ds4 = utf.getDataSet(4);
            Quotation q4 = utf.getQuotation(ds4);
            Price p4 = utf.getPrice(ds4);

            dataSets.AddRange(new DataSet[] { ds1, ds2, ds3, ds4 });

            //Act
            p3.Updated = true;
            p4.New = true;


            IDataSetService service = new DataSetService();
            service.InjectQuotationRepository(quotationRepository);
            service.InjectPriceRepository(priceRepository);
            service.UpdateDataSets(dataSets);

            //Assert
            IEnumerable<PriceDto> priceDtos = priceRepository.GetPriceDtosPassedForUpdate();
            List<PriceDto> expectedPriceDtos = new List<PriceDto>();
            expectedPriceDtos.AddRange(new PriceDto[] { p3.ToDto(), p4.ToDto() });
            bool areEqual = expectedPriceDtos.HasEqualItems(priceDtos);
            Assert.IsTrue(areEqual);

        }

        [TestMethod]
        public void UpdatePrices_OnlyExtremaFlaggedAsUpdatedAreSendToRepository()
        {

            //Arrange
            QuotationUpdateTester quotationRepository = new QuotationUpdateTester();
            PriceUpdateTester priceRepository = new PriceUpdateTester();

            List<DataSet> dataSets = new List<DataSet>();
            DataSet ds1 = utf.getDataSet(1);
            Quotation q1 = utf.getQuotation(ds1);
            Price p1 = utf.getPrice(ds1);

            DataSet ds2 = utf.getDataSet(2);
            Quotation q2 = utf.getQuotation(ds2);
            Price p2 = utf.getPrice(ds2);
            Extremum ex2 = new Extremum(p2, ExtremumType.PeakByClose);

            DataSet ds3 = utf.getDataSet(3);
            Quotation q3 = utf.getQuotation(ds3);
            Price p3 = utf.getPrice(ds3);
            Extremum ex3 = new Extremum(p3, ExtremumType.PeakByHigh);

            DataSet ds4 = utf.getDataSet(4);
            Quotation q4 = utf.getQuotation(ds4);
            Price p4 = utf.getPrice(ds4);

            dataSets.AddRange(new DataSet[] { ds1, ds2, ds3, ds4 });

            //Act
            p2.Updated = true;
            p3.Updated = true;
            ex3.Updated = true;
            p4.New = true;


            IDataSetService service = new DataSetService();
            service.InjectQuotationRepository(quotationRepository);
            service.InjectPriceRepository(priceRepository);
            service.UpdateDataSets(dataSets);

            //Assert
            IEnumerable<ExtremumDto> extremumDtos = priceRepository.GetExtremumDtosPassedForUpdate();
            List<ExtremumDto> expectedExtremumDtos = new List<ExtremumDto>();
            expectedExtremumDtos.AddRange(new ExtremumDto[] { ex3.ToDto() });
            bool areEqual = expectedExtremumDtos.HasEqualItems(extremumDtos);
            Assert.IsTrue(areEqual);

        }


        private class QuotationUpdateTester : IQuotationRepository
        {
            List<QuotationDto> quotationDtos = new List<QuotationDto>();

            public void UpdateQuotations(IEnumerable<QuotationDto> quotations)
            {
                quotationDtos.AddRange(quotations);
            }
            
            public IEnumerable<QuotationDto> GetQuotationDtosPassedForUpdate()
            {
                return quotationDtos;
            }

            public IEnumerable<QuotationDto> GetQuotations(AnalysisDataQueryDefinition queryDef)
            {
                return null;
            }
        }

        private class PriceUpdateTester : IPriceRepository
        {
            List<PriceDto> priceDtos = new List<PriceDto>();
            List<ExtremumDto> extremumDtos = new List<ExtremumDto>();

            public void UpdatePrices(IEnumerable<PriceDto> prices)
            {
                priceDtos.AddRange(prices);
            }

            public void UpdateExtrema(IEnumerable<ExtremumDto> extrema)
            {
                extremumDtos.AddRange(extrema);
            }

            public IEnumerable<PriceDto> GetPriceDtosPassedForUpdate()
            {
                return priceDtos;
            }

            public IEnumerable<ExtremumDto> GetExtremumDtosPassedForUpdate()
            {
                return extremumDtos;
            }

            public IEnumerable<PriceDto> GetPrices(AnalysisDataQueryDefinition queryDef)
            {
                return null;
            }

            public IEnumerable<ExtremumDto> GetExtrema(AnalysisDataQueryDefinition queryDef)
            {
                return null;
            }

        }


        #endregion UPDATE_DATA_SETS



    }
}


