using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock.Domain.Entities;
using Stock.Domain.Enums;
using Stock.DAL.TransferObjects;

namespace Stock_UnitTest.Stock.Domain
{
    [TestClass]
    public class HelperMethodsUnitTests
    {


        #region GET_ANALYSIS_INFO

        [Ignore]
        [TestMethod]
        public void GetAnalysisInfo_WszystkieTesty()
        {
            //jeżeli podany zestaw jest pusty - ...
            //jeżeli jest tylko jeden element - skopiowane są jego własciwości
            //prawidłowo zwraca najniższy element indeksu
            //prawidłowo zwraca najwcześniejszą datę
            //prawidłowo zwraca ostatni indeks
            //prawidłowo zwraca najpóźniejszą datę
            Assert.Fail("Not implemented yet");
        }


        #endregion GET_ANALYSIS_INFO


    }
}
