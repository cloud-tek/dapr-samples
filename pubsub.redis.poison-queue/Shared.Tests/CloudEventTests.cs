using System;
using System.Text.Json;
using CloudNative.CloudEvents;
using FluentAssertions;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Shared.Tests
{
    public class CloudEventTests
    {
        [Fact]
        public void ShouldDeserialize()
        {
            var func = new Func<JsonDocument>(() => JsonDocument.Parse(Constants.PubSubCloudEvent));

            func.Should().NotThrow();

            var doc = func();

            doc.RootElement.TryGetProperty("topic", out var el1).Should().BeTrue();
            doc.RootElement.TryGetProperty("pubsubname", out var el2).Should().BeTrue();
            doc.RootElement.TryGetProperty("nonexistent", out var el3).Should().BeFalse();

            el1.ValueKind.Should().Be(JsonValueKind.String);
            el2.ValueKind.Should().Be(JsonValueKind.String);

            el1.ValueEquals("ordertopic").Should().BeTrue();
            el2.ValueEquals("pubsub").Should().BeTrue();
        }
    }
}
