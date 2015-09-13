﻿using System;
using System.Net;
using CSharpTradeOffers;
using Moq;
using NUnit.Framework;

namespace CSharpTradeOffers_Tests
{
    [TestFixture]
    public sealed class WebTests
    {
        [SetUp]
        public void BeforeEachTest()
        {
            _mockSteamStream = new Mock<ISteamStream>();
            _mockSteamStream.Setup(x => x.ReadToEnd()).Returns(SteamStreamResponse);

            _mockResponse = new Mock<IResponse>();
            _mockResponse.Setup(x => x.GetResponseStream()).Returns(_mockSteamStream.Object);

            _mockRequestHandler = new Mock<IRequestHandler<IResponse>>();

            _mockRequestHandler.Setup(x => x.HandleRequest(Url, Method, null, null, true, "")).Returns(_mockResponse.Object);

            _web = new Web(_mockRequestHandler.Object);
        }

        private const string Url = "url";
        private const string Method = "method";
        private const string SteamStreamResponse = "Response From Steam";
        private Mock<IRequestHandler<IResponse>> _mockRequestHandler;
        private Mock<ISteamStream> _mockSteamStream;
        private Mock<IResponse> _mockResponse;
        private Web _web;

        [Test]
        public void FetchStream_ShouldFetchStream()
        {
            ISteamStream steamStream = _web.FetchStream(Url, Method);

            Assert.AreEqual(steamStream, _mockSteamStream.Object);
        }

        [Test]
        public void RetryFetch_ReturnsNull_WhenExceedingRetryCount()
        {
            _mockRequestHandler.Setup(x => x.HandleRequest(Url, Method, null, null, true, "")).Throws<WebException>();

            string response = _web.RetryFetch(new TimeSpan(1), 2, Url, Method);
            Assert.IsNull(response);
        }

        [Test]
        public void RetryFetch_ReturnsSteamStreamResponse_WhenNotExceedingRetryCount()
        {
            string response = _web.RetryFetch(new TimeSpan(1), 2, Url, Method);
            Assert.AreEqual(response, SteamStreamResponse);
        }

        [Test]
        public void RetryFetchStream_ReturnsNull_WhenExceedingRetryCount()
        {
            _mockRequestHandler.Setup(x => x.HandleRequest(Url, Method, null, null, true, "")).Throws<WebException>();

            ISteamStream steamStream = _web.RetryFetchStream(new TimeSpan(1), 2, Url, Method);

            Assert.IsNull(steamStream);
        }

        [Test]
        public void RetryFetchStream_ReturnsSteamStream_WhenNotExceedingRetryCount()
        {
            ISteamStream steamStream = _web.RetryFetchStream(new TimeSpan(1), 2, Url, Method);

            Assert.AreEqual(steamStream, _mockSteamStream.Object);
        }

        [Test]
        public void WebRequest_Fetch_ReturnsResponseStream()
        {
            string responseStream = _web.Fetch(Url, Method);

            Assert.AreEqual(responseStream, SteamStreamResponse);
        }

        [Test]
        public void WebRequest_ShouldReturnResponse()
        {
            IResponse response = _web.Request(Url, Method);

            Assert.AreEqual(response, _mockResponse.Object);
        }
    }
}