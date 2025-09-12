using Xunit;
using VitoResults;
using FluentAssertions;
using System.Linq;

namespace FluentResults.Tests;

public class VitoResultTests
{
	[Fact]
	public void AddSuccess_ShouldAddAndTrimMessage()
	{
		var result = VitoResult.New
			.AddSuccess("  Success message  ");

		result.Successes.Should().ContainSingle()
			.Which.Should().Be("Success message");
	}

	[Fact]
	public void AddError_ShouldMarkAsFailedAndCleanMessage()
	{
		var result = VitoResult.New
			.AddError("  Some error  ");

		result.IsSuccess.Should().BeFalse();
		result.Errors.Should().ContainSingle()
			.Which.Should().Be("Some error");
	}

	[Fact]
	public void AddMessage_ShouldIgnoreEmpty_WhenConfigured()
	{
		var result = VitoResult.New
			.IgnoreEmptyMessages(true)
			.AddMessage(" ")
			.AddMessage(null)
			.AddMessage("Valid message");

		result.Messages.Should().ContainSingle()
			.Which.Should().Be("Valid message");
	}

	[Fact]
	public void AddMessage_ShouldAllowDuplicates_WhenDisabled()
	{
		var result = VitoResult.New
			.RemoveDuplicateMessages(false)
			.AddMessage("duplicate")
			.AddMessage("duplicate");

		result.Messages.Count.Should().Be(2);
		result.Messages.All(m => m == "duplicate").Should().BeTrue();
	}

	[Fact]
	public void Merge_ShouldCombineErrorsSuccessesAndMessages()
	{
		var r1 = VitoResult.New
			.AddSuccess("S1")
			.AddError("E1")
			.AddMessage("M1");

		var r2 = VitoResult.New
			.AddSuccess("S2")
			.AddError("E2")
			.AddMessage("M2");

		r1.Merge(r2);

		r1.Successes.Should().Contain(new[] { "S1", "S2" });
		r1.Errors.Should().Contain(new[] { "E1", "E2" });
		r1.Messages.Should().Contain(new[] { "M1", "M2" });
		r1.IsSuccess.Should().BeFalse();
	}

	[Fact]
	public void GenericResult_ShouldStoreValue_WhenSuccess()
	{
		var result = VitoResult<string>.New
			.WithValue("Hello")
			.AddSuccess("OK");

		result.HasValue.Should().BeTrue();
		result.Value.Should().Be("Hello");
	}

	[Fact]
	public void GenericResult_ShouldNotStoreValue_WhenFailed()
	{
		var result = VitoResult<string>.New
			.AddError("Failed")
			.WithValue("Hello");

		result.HasValue.Should().BeFalse();
		result.Value.Should().BeNull();
	}

	[Fact]
	public void EnableMessageCleaning_ShouldTrimSpaces()
	{
		var result = VitoResult.New
			.EnableMessageCleaning(true)
			.AddMessage("   Test   ");

		result.Messages.Should().ContainSingle().Which.Should().Be("Test");
	}

	[Fact]
	public void DisableMessageCleaning_ShouldKeepSpaces()
	{
		var result = VitoResult.New
			.EnableMessageCleaning(false)
			.AddMessage("   Test   ");

		result.Messages.Should().ContainSingle().Which.Should().Be("   Test   ");
	}

	[Fact]
	public void EnableMessageCleaning_ShouldTrim_WhenEnabled()
	{
		var result = VitoResult<string>.New
			.EnableMessageCleaning(true)
			.AddMessage("   test   ");

		result.Messages.Should().ContainSingle().Which.Should().Be("test");
	}

	[Fact]
	public void EnableMessageCleaning_ShouldNotTrim_WhenDisabled()
	{
		var result = VitoResult<string>.New
			.EnableMessageCleaning(false)
			.AddMessage("   test   ");

		result.Messages.Should().ContainSingle().Which.Should().Be("   test   ");
	}

	[Fact]
	public void IgnoreEmptyMessages_ShouldSkipEmpty_WhenEnabled()
	{
		var result = VitoResult<string>.New
			.IgnoreEmptyMessages(true)
			.AddMessage(" ")
			.AddMessage(null)
			.AddMessage("valid");

		result.Messages.Should().ContainSingle().Which.Should().Be("valid");
	}

	[Fact]
	public void IgnoreEmptyMessages_ShouldKeepEmpty_WhenDisabled()
	{
		var result = VitoResult<string>.New
			.IgnoreEmptyMessages(false)
			.RemoveDuplicateMessages(false)
			.AddMessage(" ")
			.AddMessage(null)
			.AddMessage("valid");

		result.Messages.Count.Should().Be(3);
	}

	[Fact]
	public void RemoveDuplicateMessages_ShouldRemoveDuplicates_WhenEnabled()
	{
		var result = VitoResult<string>.New
			.RemoveDuplicateMessages(true)
			.AddMessage("dup")
			.AddMessage("dup");

		result.Messages.Count.Should().Be(1);
	}

	[Fact]
	public void RemoveDuplicateMessages_ShouldAllowDuplicates_WhenDisabled()
	{
		var result = VitoResult<string>.New
			.RemoveDuplicateMessages(false)
			.AddMessage("dup")
			.AddMessage("dup");

		result.Messages.Count.Should().Be(2);
	}
}
