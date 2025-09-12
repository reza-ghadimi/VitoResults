using System.Linq;
using System.Collections.Generic;

namespace VitoResults;

public class VitoResult
{
	public static VitoResult New
	{
		get
		{
			return new VitoResult();
		}
	}

	protected VitoResult() : base()
	{
		IsSuccess = true;

		_errors = [];
		_messages = [];
		_successes = [];
	}

	[System.Text.Json.Serialization.JsonInclude]
	public bool IsSuccess { get; private set; }

	[System.Text.Json.Serialization.JsonInclude]
	public bool IsFailure => !IsSuccess;

	private bool _shouldCleanMessages = true;
	public VitoResult EnableMessageCleaning(bool enable)
	{
		_shouldCleanMessages = enable;

		return this;
	}

	private bool _shouldIgnoreEmptyMessages = true;
	public VitoResult IgnoreEmptyMessages(bool ignoreEmptyMessages)
	{
		_shouldIgnoreEmptyMessages = ignoreEmptyMessages;

		return this;
	}

	private bool _shouldRemoveDuplicateMessages = true;
	public VitoResult RemoveDuplicateMessages(bool enable)
	{
		_shouldRemoveDuplicateMessages = enable;

		return this;
	}

	#region Errors
	private readonly List<string?> _errors;

	[System.Text.Json.Serialization.JsonInclude]
	public IReadOnlyList<string?> Errors
	{
		get
		{
			return _errors;
		}
		private set
		{
			WithErrors(errorMessages: value);
		}
	}
	#endregion /Errors

	#region Successes
	private readonly List<string?> _successes;

	[System.Text.Json.Serialization.JsonInclude]
	public IReadOnlyList<string?> Successes
	{
		get
		{
			return _successes;
		}
		private set
		{
			WithSuccesses(successMessages: value);
		}
	}
	#endregion /Successes

	#region Messages
	private readonly List<string?> _messages;

	[System.Text.Json.Serialization.JsonInclude]
	public IReadOnlyList<string?> Messages
	{
		get
		{
			return _messages;
		}
		private set
		{
			WithMessages(messages: value);
		}
	}
	#endregion /Messages

	#region AddSuccess()
	public VitoResult AddSuccess(string? success)
	{
		if (_shouldCleanMessages)
		{
			success = PrepareValue(value: success);
		}

		if (_shouldIgnoreEmptyMessages && string.IsNullOrWhiteSpace(value: success))
		{
			return this;
		}

		if (_shouldRemoveDuplicateMessages && _successes.Contains(item: success))
		{
			return this;
		}

		_successes.Add(item: success);

		return this;
	}

	public VitoResult WithSuccesses(IReadOnlyList<string?> successMessages)
	{
		if (successMessages.Any() == false)
		{
			return this;
		}

		foreach (var successMessage in successMessages)
		{
			AddSuccess(success: successMessage);
		}

		return this;
	}

	public VitoResult WithSuccesses(VitoResult result)
	{
		return WithSuccesses(result.Successes);
	}
	#endregion /AddSuccess()

	#region AddError()
	public VitoResult AddError(string? errorMessage)
	{
		IsSuccess = false;

		if (_shouldCleanMessages)
		{
			errorMessage = PrepareValue(value: errorMessage);
		}

		if (_shouldIgnoreEmptyMessages && string.IsNullOrWhiteSpace(value: errorMessage))
		{
			return this;
		}

		if (_shouldRemoveDuplicateMessages && _errors.Contains(item: errorMessage))
		{
			return this;
		}

		_errors.Add(item: errorMessage);

		return this;
	}

	public VitoResult WithErrors(IReadOnlyList<string?> errorMessages)
	{
		if (errorMessages.Any() == false)
		{
			return this;
		}

		foreach (var errorMessage in errorMessages)
		{
			AddError(errorMessage: errorMessage);
		}

		return this;
	}

	public VitoResult WithErrors(VitoResult result)
	{
		return WithErrors(result.Errors);
	}
	#endregion /AddError()

	#region AddMessage()
	public VitoResult AddMessage(string? message)
	{
		if (_shouldCleanMessages)
		{
			message = PrepareValue(value: message);
		}

		if (_shouldIgnoreEmptyMessages && string.IsNullOrWhiteSpace(value: message))
		{
			return this;
		}

		if (_shouldRemoveDuplicateMessages && _messages.Contains(item: message))
		{
			return this;
		}

		_messages.Add(item: message);

		return this;
	}

	public VitoResult WithMessages(IReadOnlyList<string?> messages)
	{
		foreach (var message in messages)
		{
			AddMessage(message: message);
		}

		return this;
	}

	public VitoResult WithMessages(VitoResult result)
	{
		return WithMessages(result.Messages);
	}
	#endregion /AddMessage()

	public VitoResult Merge(VitoResult other)
	{
		WithErrors(other.Errors);
		WithSuccesses(other.Successes);
		WithMessages(other.Messages);

		return this;
	}

	private string? PrepareValue(string? value)
	{
		value = value?.Trim();

		if (string.IsNullOrWhiteSpace(value))
		{
			return null;
		}

		while (value.Contains("  "))
		{
			value = value.Replace("  ", " ");
		}

		return value;
	}
}

public class VitoResult<T> : VitoResult
{
	new public static VitoResult<T> New
	{
		get
		{
			return new VitoResult<T>();
		}
	}

	protected VitoResult() : base()
	{
	}

	[System.Text.Json.Serialization.JsonInclude]
	public bool HasValue
	{
		get
		{
			if (Value == null)
			{
				return false;
			}

			return true;
		}
	}


	[System.Text.Json.Serialization.JsonInclude]
	public T Value { get; private set; }

	public new VitoResult<T> EnableMessageCleaning(bool enable)
	{
		base.EnableMessageCleaning(enable);
		return this;
	}

	public new VitoResult<T> IgnoreEmptyMessages(bool ignoreEmptyMessages)
	{
		base.IgnoreEmptyMessages(ignoreEmptyMessages);
		return this;
	}

	public new VitoResult<T> RemoveDuplicateMessages(bool enable)
	{
		base.RemoveDuplicateMessages(enable);
		return this;
	}

	public new VitoResult<T> AddMessage(string? message)
	{
		base.AddMessage(message);
		return this;
	}

	public new VitoResult<T> AddSuccess(string? success)
	{
		base.AddSuccess(success);
		return this;
	}

	public new VitoResult<T> AddError(string? error)
	{
		base.AddError(error);
		return this;
	}

	public new VitoResult<T> WithMessages(IReadOnlyList<string?> messages)
	{
		base.WithMessages(messages);
		return this;
	}

	public new VitoResult<T> WithMessages(VitoResult result)
	{
		base.WithMessages(result);
		return this;
	}

	public new VitoResult<T> WithSuccesses(IReadOnlyList<string?> successes)
	{
		base.WithSuccesses(successes);

		return this;
	}

	public new VitoResult<T> WithSuccesses(VitoResult result)
	{
		base.WithSuccesses(result);
		return this;
	}

	public new VitoResult<T> WithErrors(IReadOnlyList<string?> errors)
	{
		base.WithErrors(errors);
		return this;
	}

	public new VitoResult<T> WithErrors(VitoResult result)
	{
		base.WithErrors(result);
		return this;
	}


	public VitoResult<T> WithValue(T value)
	{
		if (IsSuccess)
		{
			Value = value;
		}

		return this;
	}

	public new VitoResult<T> Merge(VitoResult other)
	{
		base.Merge(other);

		return this;
	}

	public VitoResult<T> Merge(VitoResult<T> other)
	{
		base.Merge(other);

		if (other.HasValue)
		{
			WithValue(other.Value);
		}

		return this;
	}
}
