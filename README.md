# nk7-logger

Lightweight logging service for Unity built on `Microsoft.Extensions.Logging`, ZLogger and ZString, with allocation-conscious interpolated string handlers and Unity console/file sinks.

## Features

- `ILoggerService` facade for `Trace`, `Debug`, `Information`, `Warning`, `Error`, and `Critical`.
- Message, exception, exception + message, and interpolated-string overloads for every log level.
- Interpolated string handlers backed by ZString, so disabled log levels skip message building.
- `LoggerFactory` wires ZLogger to Unity console through `UnityDebugStream` and adds an optional file sink.
- Unity console output is routed by level: Trace/Debug/Information to `Debug.Log`, Warning to `Debug.LogWarning`, Error/Critical to `Debug.LogError`.
- `EmptyLoggerService` provides a no-op drop-in when logging is disabled.
- Package includes `Runtime/csc.rsp` with `-langversion:10` for interpolated string handlers.

## Table of Contents
- [Installation](#installation)
  - [Unity Package Manager](#unity-package-manager)
  - [Manual Installation](#manual-installation)
- [Quick Start](#quick-start)
  - [1. Ensure C# 10](#1-ensure-c-10)
  - [2. Create a Logger](#2-create-a-logger)
  - [3. Wrap It](#3-wrap-it)
  - [4. Log Messages](#4-log-messages)
- [Lifecycle](#lifecycle)
- [Console Output](#console-output)
- [Runtime API](#runtime-api)
- [Requirements](#requirements)

## Installation

### Unity Package Manager
1. Open Unity Package Manager (`Window -> Package Manager`).
2. Click `+ -> Add package from git URL...`.
3. Enter `https://github.com/s-elovikov/nk7-logger.git?path=src`.

Unity does not auto-update Git-based packages; update the hash manually when needed or use [UPM Git Extension](https://github.com/mob-sakai/UpmGitExtension).

### Manual Installation
Copy the `src` folder into your project and reference the `Nk7.Logger` asmdef.

## Quick Start

### 1. Ensure C# 10
- C# 10 is configured by the package through `Runtime/csc.rsp` (`-langversion:10`).
- Keep language version 10+ if you override compiler response files in your project.

### 2. Create a Logger
Use `LoggerFactory.GetLogger` to create a `Microsoft.Extensions.Logging.ILogger`.

```csharp
using Microsoft.Extensions.Logging;
using Nk7.Logger;

ILogger logger = LoggerFactory.GetLogger(
    minimumLogLevel: LogLevel.Information,
    loggerFilePath: "Logs/app.log");
```

`loggerFilePath` is optional. When it is provided, missing directories are created automatically.

### 3. Wrap It
Wrap the logger in `LoggerService` to use the `ILoggerService` convenience API.

```csharp
ILoggerService loggerService = new LoggerService(logger);
```

Use `EmptyLoggerService` as a drop-in replacement when logging should be disabled.

```csharp
ILoggerService loggerService = new EmptyLoggerService();
```

### 4. Log Messages
Use string overloads for simple messages and interpolated strings when formatting should be skipped for disabled levels.

```csharp
using UnityEngine;

public sealed class LoggerExample : MonoBehaviour
{
    private ILoggerService _logger;

    private void Awake()
    {
        ILogger logger = LoggerFactory.GetLogger(
            minimumLogLevel: LogLevel.Information,
            loggerFilePath: "Logs/app.log");

        _logger = new LoggerService(logger);
    }

    private void Start()
    {
        int playerId = 42;

        _logger.Information($"Player {playerId} connected");
        _logger.Warning("Low memory warning");
        _logger.Error(new System.Exception("Boom"), $"Failed to load profile {playerId}");
    }
}
```

## Lifecycle
- `LoggerFactory` builds an `ILogger` pipeline with a Unity stream sink and optional file sink.
- The Unity stream uses an internal level prefix and routes each message to the matching `Debug.Log*` method.
- File output uses ZLogger plain-text formatting when `loggerFilePath` is provided.
- Interpolated string handlers check `IsEnabled` before creating a ZString builder.
- `LoggerService` checks the enabled level before forwarding string and exception overloads to `Microsoft.Extensions.Logging`.

## Console Output

| Log level | Unity method |
| --- | --- |
| `Trace` | `Debug.Log` |
| `Debug` | `Debug.Log` |
| `Information` | `Debug.Log` |
| `Warning` | `Debug.LogWarning` |
| `Error` | `Debug.LogError` |
| `Critical` | `Debug.LogError` |

The internal prefix is stripped before the message is sent to the Unity console.

## Runtime API

```csharp
public static class LoggerFactory
{
    public static ILogger GetLogger(
        LogLevel minimumLogLevel = LogLevel.Error,
        string loggerFilePath = null);
}
```

- Returns a `Microsoft.Extensions.Logging.ILogger` with category `Application`.
- Defaults to `LogLevel.Error` when `minimumLogLevel` is not specified.
- Passing `LogLevel.None` falls back to `LogLevel.Warning`.

```csharp
public sealed class LoggerService : ILoggerService
{
    public LoggerService(ILogger logger);
}
```

`ILoggerService` exposes:

```csharp
bool IsEnabled(LogLevel logLevel);

void Trace(string message);
void Debug(string message);
void Information(string message);
void Warning(string message);
void Error(string message);
void Critical(string message);

void Trace(Exception exception);
void Debug(Exception exception);
void Information(Exception exception);
void Warning(Exception exception);
void Error(Exception exception);
void Critical(Exception exception);

void Trace(Exception exception, string message);
void Debug(Exception exception, string message);
void Information(Exception exception, string message);
void Warning(Exception exception, string message);
void Error(Exception exception, string message);
void Critical(Exception exception, string message);
```

The same six levels also support interpolated-string overloads:

```csharp
_logger.Debug($"Position: {transform.position}");
_logger.Error(exception, $"Failed to load item {itemId}");
```

## Requirements

- Unity project with C# 10 support
- `org.nuget.zlogger` 2.5.10
- `org.nuget.zstring` 2.6.0
- UPM package name: `com.nk7.logger`
