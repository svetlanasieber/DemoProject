using DemoNunitTest.Models;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Text.Json;

namespace DemoNunitTest
{
    public class UnitTestsDemo
    {
        RestClient client;

        [SetUp]
        public void Setup()
        {
            var options = new RestClientOptions("https://api.github.com")
            {
                MaxTimeout = 3000,
                Authenticator = new HttpBasicAuthenticator("username", "token")
            };

            this.client = new RestClient(options);
        }

        [TearDown]
        public void Teardown()
        {
            this.client.Dispose();
        }

        [Test]
        public void Test_GitHubAPIRequest()
        {
            //var client = new RestClient("https://api.github.com");
            var request = new RestRequest("/repos/testnakov/test-nakov-repo/issues", Method.Get);
            //request.Timeout = 1000;
            var response = client.Get(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Test_GetAllIssuesFromARepo()
        {
            var request = new RestRequest("repos/testnakov/test-nakov-repo/issues");
            var response = client.Execute(request);
            var issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);

            Assert.That(issues.Count > 1);

            foreach (var issue in issues)
            {
                Assert.That(issue.id, Is.GreaterThan(0));
                Assert.That(issue.number, Is.GreaterThan(0));
                Assert.That(issue.title, Is.Not.Empty);

            }
        }

        private Issue CreateIssue(string title, string body)
        {
            var request = new RestRequest("repos/testnakov/test-nakov-repo/issues");
            request.AddBody(new { body, title });
            var response = client.Execute(request, Method.Post);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);
            return issue;
        }

        [Test]
        public void Test_CreateGitHubIssue()
        {
            string title = "This is a Demo Issue";
            string body = "QA Back-End Automation Course February 2024";
            var issue = CreateIssue(title, body);
            Assert.That(issue.id, Is.GreaterThan(0));
            Assert.That(issue.number, Is.GreaterThan(0));
            Assert.That(issue.title, Is.Not.Empty);
        }


        [Test]
        public void Test_EditIssue()
        {
            var request = new RestRequest("repos/testnakov/test-nakov-repo/issues/4946");
            request.AddJsonBody(new
            {
                title = "Changing the name of the issue that I created"
            }
            );

            var response = client.Execute(request, Method.Patch);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(issue.id, Is.GreaterThan(0), "Issue ID should be greater than 0.");
            Assert.That(response.Content, Is.Not.Empty, "The response content should not be empty.");
            Assert.That(issue.number, Is.GreaterThan(0), "Issue number should be greater than 0.");
            Assert.That(issue.title, Is.EqualTo("Changing the name of the issue that I created"), "The issue title should match the new title.");
        }
    }
}