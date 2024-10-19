using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MINIMALAPI.Domain.Entities;

namespace Teste.Domain.Entities
{
    [TestClass]
    public class VeiculosTest
    {
        [TestMethod]
        public void TestGetSetVeuculosProperties()
        {
            //Arrange
            var veiculo = new Veiculo();
            
            //Act
            veiculo.Id = 1;
            veiculo.Marca = "Fiat";
            veiculo.Nome = "Uno";
            veiculo.Ano = 2021;

            //Assert
            Assert.AreEqual(1, veiculo.Id);
            Assert.AreEqual("Fiat", veiculo.Marca);
            Assert.AreEqual("Uno", veiculo.Nome);
            Assert.AreEqual(2021, veiculo.Ano);
        }
    }
}