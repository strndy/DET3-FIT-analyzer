using System;
using System.IO;
using Det3FitAutoTune.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Det3FitAutoTune.Test2
{
    [TestClass]
    public class VeMapReadWriteTest
    {
        [TestMethod]
        public void TestReadWrite()
        {
            var veTableReader = new VeTableReader();
            var veTableBytes = File.ReadAllBytes(@"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\tables\VETable_real.bin");
            var veTable = veTableReader.ReadTable(veTableBytes);

            var reconstructedBytes = veTableReader.GetBytes(veTable);

            Assert.AreEqual(BitConverter.ToString(veTableBytes), BitConverter.ToString(reconstructedBytes));
            //Assert.AreEqual(veTableBytes, reconstructedBytes);
        }
    }
}
