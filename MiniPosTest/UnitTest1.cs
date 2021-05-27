using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MiniPosTest
{
    [TestClass]
    public class UnitTestMPLine
    {
        [TestMethod]
        public void TestMPLineDefaultPropsName()
        { 
            Assert.AreEqual("", new MiniPos.MPLine().Name);
        }

        [TestMethod]
        public void TestMPLineDefaultPropsQty()
        {
            Assert.AreEqual("1", new MiniPos.MPLine().Qty);
        }

        [TestMethod]
        public void TestMPLineDefaultPropsPrc()
        {           
            Assert.AreEqual(0m.ToString(MiniPos.MPHelper.FMT_PRC), new MiniPos.MPLine().Prc);
        }

        [TestMethod]
        public void TestMPLineDefaultPropsDiscount()
        {
            Assert.AreEqual(0m.ToString(MiniPos.MPHelper.FMT_DISCOUNT), new MiniPos.MPLine().Discount);
        }

        [TestMethod]
        public void TestMPLineEvalPropsName()
        {            
            var line = new MiniPos.MPLine();
            line.Name = "Test";

            Assert.AreEqual("Test", line.Name);

            line.Name = "Кирилица";
            Assert.AreEqual("Кирилица", line.Name);
        }

        [TestMethod]
        public void TestMPLineEvalPropsQtyOk()
        {
            var line = new MiniPos.MPLine();
            line.Qty = "5";

            Assert.AreEqual("5", line.Qty);            
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestMPLineEvalPropsQtyFail()
        {
            var line = new MiniPos.MPLine();
            line.Qty = "not an int type value";
        }

        [TestMethod]
        public void TestMPLineEvalPropsPrcOk()
        {
            var line = new MiniPos.MPLine();
            line.Prc = "5";            
            Assert.AreEqual(5m.ToString(MiniPos.MPHelper.FMT_PRC), line.Prc);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestMPLineEvalPropsPrcFail()
        {
            var line = new MiniPos.MPLine();
            line.Prc = "not an int type value";
        }

        [TestMethod]
        public void TestMPLineEvalPropsDiscountOk()
        {
            var line = new MiniPos.MPLine();
            line.Discount = "5";            
            Assert.AreEqual(5m.ToString(MiniPos.MPHelper.FMT_DISCOUNT), line.Discount);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestMPLineEvalPropsDiscountFail()
        {
            var line = new MiniPos.MPLine();
            line.Discount = "not an int type value";
        }

        [TestMethod]
        public void TestMPLineEvalTotalNoDiscount()
        {
            var line = new MiniPos.MPLine();
            line.Qty = "5";
            line.Prc = 6m.ToString();
            Assert.AreEqual(30, line.Total);
            Assert.AreEqual(0, line.TotalDiscount);
            Assert.AreEqual(30, line.TotalTarget);
        }

        [TestMethod]
        public void TestMPLineEvalTotalAddDiscount()
        {
            var line = new MiniPos.MPLine();
            line.Qty = "5";
            line.Prc = 6m.ToString();
            line.Discount = 20.ToString();
            Assert.AreEqual(30, line.Total);
            Assert.AreEqual(6, line.TotalDiscount);
            Assert.AreEqual(24, line.TotalTarget);
        }
    }

    [TestClass]
    public class UnitTestMPDoc
    {
        [TestMethod]
        public void TestMPDocInit()
        {
            var doc = new MiniPos.MPDoc();            

            Assert.AreEqual(MiniPos.MPHelper.DEF_CLIENT_NAME, doc.Contractor);
            Assert.AreEqual(DateTime.Now.ToString(MiniPos.MPHelper.FMT_DOC_DATE), doc.DT);
            Assert.AreEqual(0, doc.Lines.Count);
            Assert.AreEqual(0, doc.Total);
            Assert.AreEqual(0, doc.TotalTarget);
            Assert.AreEqual(0, doc.TotalDiscount);
        }

        [TestMethod]
        public void TestMPDocLineAdd()
        {
            var doc = new MiniPos.MPDoc();
            doc.LineAdd();
            Assert.AreEqual(1, doc.Lines.Count);
        }

        [TestMethod]
        public void TestMPDoc1Line()
        {
            var doc = new MiniPos.MPDoc();
            var line = doc.LineAdd();            
            line.Prc = 0.12345.ToString();
            
            Assert.AreEqual(0.12m, doc.Total);
            Assert.AreEqual(0.12m, doc.TotalTarget);
            Assert.AreEqual(0, doc.TotalDiscount);
        }

        [TestMethod]
        public void TestMPDoc1LineDiscount()
        {
            var doc = new MiniPos.MPDoc();
            var line = doc.LineAdd();            
            line.Prc = 0.12345.ToString();
            line.Discount = 20.ToString();

            Assert.AreEqual(0.12m, doc.Total);
            Assert.AreEqual(0.1m, doc.TotalTarget);
            Assert.AreEqual(0.02m, doc.TotalDiscount);
        }

        [TestMethod]
        public void TestMPDoc3Lines()
        {
            var doc = new MiniPos.MPDoc();
            
            var line = doc.LineAdd();
            line.Qty = "1";
            line.Prc = 0.12345.ToString();

            line = doc.LineAdd();
            line.Qty = "2";
            line.Prc = 6.78901.ToString();
            // 13.57802

            line = doc.LineAdd();
            line.Qty = "3";
            line.Prc = 2.34567.ToString();
            //  7.03701

            Assert.AreEqual(20.74m, doc.Total);
            Assert.AreEqual(20.74m, doc.TotalTarget);
            Assert.AreEqual(0, doc.TotalDiscount);
        }

        [TestMethod]
        public void TestMPDoc3LinesDiscount()
        {
            var doc = new MiniPos.MPDoc();

            var line = doc.LineAdd();
            line.Qty = "1";
            line.Prc = 0.12345.ToString();
            line.Discount = "10";
            // 0.12345
            // 0,111105
            // 0.11110 -- banker's rounding

            line = doc.LineAdd();
            line.Qty = "2";
            line.Prc = 6.78901.ToString();
            line.Discount = "11";
            // 13.57802
            // 12,0844378
            // 12.08444

            line = doc.LineAdd();
            line.Qty = "3";
            line.Prc = 2.34567.ToString();
            line.Discount = "12";
            // 7.03701
            // 6,1925688
            // 6,19257

            Assert.AreEqual(20.74m, doc.Total);
            Assert.AreEqual(18.39m, doc.TotalTarget);
            Assert.AreEqual(2.35m, doc.TotalDiscount);
        }

        [TestMethod]
        public void TestMPDocLinesMixedDiscount()
        {
            var doc = new MiniPos.MPDoc();

            var line = doc.LineAdd();
            line.Qty = "1";
            line.Prc = 0.12345.ToString();
            line.Discount = "10";
            // 0.12345
            // 0,111105
            // 0.11110 -- banker's rounding

            line = doc.LineAdd();
            line.Qty = "2";
            line.Prc = 6.78901.ToString();            
            // 13.57802

            line = doc.LineAdd();
            line.Qty = "3";
            line.Prc = 2.34567.ToString();
            line.Discount = "12";
            // 7.03701
            // 6,1925688
            // 6,19257

            Assert.AreEqual(20.74m, doc.Total);
            Assert.AreEqual(19.88m, doc.TotalTarget);
            Assert.AreEqual(0.86m, doc.TotalDiscount);
        }
    }
}
