using NUnit.Framework;
using Roadkill.Core.Configuration;
using Roadkill.Core.Text.Parsers.Images;
using Roadkill.Tests.Unit.StubsAndMocks;

namespace Roadkill.Tests.Unit.Text.Parsers.Images
{
	public class ImageTagProviderTests
	{
		private ApplicationSettings _applicationSettings;

		[SetUp]
		public void Setup()
		{
			var container = new MocksAndStubsContainer();
			_applicationSettings = container.ApplicationSettings;
		}

		[Test]
		public void absolute_paths_should_be_prefixed_with_attachmentpath()
		{
			// Arrange
			var resolver = new UrlResolverMock();
			resolver.AbsolutePathSuffix = "BlahBlah";

			var provider = new ImageTagProvider(_applicationSettings);
			provider.UrlResolver = resolver;

			HtmlImageTag htmlImageTag = new HtmlImageTag("/DSC001.jpg", "/DSC001.jpg", "alt", "title");
			string x = "";

			// Act
			HtmlImageTag actualTag = provider.Parse(htmlImageTag);

			// Assert
			Assert.That(actualTag.Src, Is.EqualTo("/Attachments/DSC001.jpgBlahBlah"));
		}

		[Test]
		[TestCase("http://www.example.com/img.jpg")]
		[TestCase("https://www.foo.com/img.jpg")]
		public void should_ignore_urls_starting_with_http_and_https(string imageUrl)
		{
			// Arrange
			var resolver = new UrlResolverMock();
			resolver.AbsolutePathSuffix = "BlahBlah";

			var provider = new ImageTagProvider(_applicationSettings);
			provider.UrlResolver = resolver;

			HtmlImageTag htmlImageTag = new HtmlImageTag(imageUrl, imageUrl, "alt", "title");
			string x = "";

			// Act
			HtmlImageTag actualTag = provider.Parse(htmlImageTag);

			// Assert
			Assert.That(actualTag.Src, Is.EqualTo(imageUrl));
		}
	}
}