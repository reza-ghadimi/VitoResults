
---

# 📘 VitoResults

**VitoResults** is a clean, expressive results-handling library for .NET, designed to simplify success/error flows and enrich your application's messaging. Inspired by the name *Vito*—derived from the Latin word *vita*, meaning “life”—this library brings clarity, vitality, and composability to your code.

---

## ✨ Features

- ✅ Chainable API for fluent result construction  
- ✅ Track successes, errors, and messages independently  
- ✅ Generic result types with value encapsulation  
- ✅ Configurable behavior:
  - Trim or preserve message whitespace
  - Ignore empty messages
  - Remove or allow duplicates  
- ✅ Compatible with multiple .NET versions: `.NET 5`, `.NET 6`, `.NET 7`, `.NET 8`, `.NET 9`

---

## 🚀 Quick Start

```csharp
var result = VitoResult.New
    .AddSuccess("Operation completed")
    .AddMessage("Details logged");

if (result.IsSuccess)
{
    Console.WriteLine("Success!");
}
```

---

## ✅ With Value Example

You can use `VitoResult<T>` to encapsulate a value alongside success or error messaging:

```csharp
var result = VitoResult<string>.New
    .WithValue("Hello, Reza!")
    .AddSuccess("Greeting generated");

if (result.HasValue)
{
    Console.WriteLine(result.Value); // Output: Hello, Reza!
}
```

If the result contains errors, the value is automatically discarded:

```csharp
var result = VitoResult<string>.New
    .AddError("Something went wrong")
    .WithValue("Should not persist");

Console.WriteLine(result.HasValue); // False
Console.WriteLine(result.Value);    // null
```

---

## 📦 Installation

```bash
dotnet add package VitoResults
```

Or via NuGet Package Manager:

```powershell
Install-Package VitoResults
```

---

## 🔄 Framework Compatibility

VitoResults supports the following target frameworks:

- `.NET 5.0`
- `.NET 6.0`
- `.NET 7.0`
- `.NET 8.0`
- `.NET 9.0`

Package dependencies are conditionally resolved to ensure compatibility across all supported versions. Whether you're maintaining legacy systems or building with the latest .NET, VitoResults integrates seamlessly.

---

## 🧪 Testing

VitoResults is fully tested with [xUnit](https://xunit.net) and [FluentAssertions](https://fluentassertions.com). See the [`VitoResultTests.cs`](https://github.com/reza-ghadimi/VitoResults/blob/main/tests/VitoResults.Tests/VitoResultTests.cs) file for examples.

---

## 📖 Philosophy

In Latin, *vita* means "life"—and **VitoResults** aims to bring life to your application's flow by making success and failure explicit, readable, and composable. Whether you're building APIs, services, or internal tools, VitoResults helps you express intent and outcome with precision.

---

## 👤 Author

**Reza Ghadimi**  
🔗 [LinkedIn](https://www.linkedin.com/in/rezaghadimi/)  
🐙 [GitHub Profile](https://github.com/reza-ghadimi)  
📦 [VitoResults Repository](https://github.com/reza-ghadimi/VitoResults)

---

## 📄 License

MIT License

---
