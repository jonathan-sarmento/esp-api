using FluentAssertions;

namespace ESP.MQTT.WebApi.UnitTest;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var teste = 1;

        teste.Should().Be(1);
    }
}