using DemoNunitTest.Models;
using Newtonsoft.Json;
using RestSharp;

namespace DemoNunitTest
{
    public class ZippopotamusTest
    {
        [TestCase("BG", "1000", "Sofija")]
        [TestCase("BG", "5000", "Veliko Turnovo")]
        [TestCase("CA", "M5S", "Toronto")]
        [TestCase("GB", "B1", "Birmingham")]
        [TestCase("DE", "01067", "Dresden")]
        public void TestZippopotamus(
    string countryCode, string zipCode,
    string expectedPlace)
        {
            // Arrange
            var restClient = new RestClient("https://api.zippopotam.us");
            var httpRequest = new RestRequest(countryCode + "/" + zipCode, Method.Get);

            // Act
            RestResponse httpResponse = restClient.Execute(httpRequest);
            var location = JsonConvert.DeserializeObject<Location>(httpResponse.Content);


            // Assert
            StringAssert.Contains(expectedPlace, location.Places[0].PlaceName);
        }
    }
}
