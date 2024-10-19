using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MINIMALAPI.Domain.Entities;

namespace Teste.Domain.Entities
{
    [TestClass]
    public class AdministratorTest
    {
        [TestMethod]
        public void TestGetSetProperties()
        {
            //Arrange
            var adm = new Administrator();
            
            //Act
            adm.Id = 1;
            adm.Email = "teste@taste.com";
            adm.Senha = "teste";
            adm.Perfil = "Adm";
            
            //Assert
            Assert.AreEqual(1, adm.Id);
            Assert.AreEqual("teste@taste.com", adm.Email);
            Assert.AreEqual("teste", adm.Senha);
            Assert.AreEqual("Adm", adm.Perfil);
        }
    }
}