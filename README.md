# nk7-logger

Lightweight logging service for Unity built on ZLogger and Microsoft.Extensions.Logging, with allocation-free interpolated string handlers and Unity console/file sinks.

## Features

- `ILoggerService` interface with Debug/Information/Warning/Error, string and interpolated-string overloads.
- Interpolated string handlers backed by ZString, so disabled log levels skip formatting and allocations.
- `ZLoggerFactory` wires ZLogger to Unity console via `UnityDebugStream` and adds an optional file sink.
- Unity console messages get level prefixes (`[T]`, `[D]`, `[I]`, `[W]`, `[E]`, `[C]`) for quick scanning.
- `EmptyLoggerService` provides a no-op drop-in when logging is disabled.
- Editor utility ensures C# 10 (`Assets/csc.rsp`) so interpolated string handlers compile in Unity.

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
- [Inspector Tooling](#inspector-tooling)
- [Runtime API](#runtime-api)
- [Requirements](#requirements)

## Installation

### Unity Package Manager
1. Open Unity Package Manager (`Window → Package Manager`).
2. Click `+ → Add package from git URL…`.
3. Enter `https://github.com/s-elovikov/nk7-logger.git?path=src`.

Unity does not auto-update Git-based packages; update the hash manually when needed or use [UPM Git Extension](https://github.com/mob-sakai/UpmGitExtension).

### Manual Installation
Copy the `src` folder into your project and add `Nk7.Logger.asmdef` to the assembly.

## Quick Start

### 1. Ensure C# 10
- The editor script creates or updates `Assets/csc.rsp` with `-langversion:10` on first load.
- You can also run `Nk7 → Logger → Ensure C# 10 (csc.rsp)` from the Unity menu.

### 2. Create a Logger
- Use `ZLoggerFactory.GetLogger` to create a `Microsoft.Extensions.Logging.ILogger`.
- Provide a file path to enable file logging (directories are created automatically).

### 3. Wrap It
- Wrap the logger in `ZLoggerService` to use the `ILoggerService` convenience API.
- Swap to `EmptyLoggerService` when logging should be disabled.

### 4. Log Messages
- Use string overloads for simple messages.
- Use interpolated strings to avoid formatting cost when the log level is disabled.

## Lifecycle
- `ZLoggerFactory` builds an `ILogger` pipeline with a Unity stream sink and optional file sink.
- The Unity sink prefixes each entry (`[T]`, `[D]`, `[I]`, `[W]`, `[E]`, `[C]`) and routes it to `Debug.Log*`.
- Interpolated string handlers check `IsEnabled` before allocating ZString builders.

## Inspector Tooling
- On editor load, the package ensures `Assets/csc.rsp` contains `-langversion:10` for C# 10 features.
- The menu item `Nk7 → Logger → Ensure C# 10 (csc.rsp)` lets you re-apply the setting on demand.

## Runtime API

```csharp
using Microsoft.Extensions.Logging;
using Nk7.Logger;
using UnityEngine;

public sealed class LoggerExample : MonoBehaviour
{
    private ILoggerService _logger;

    private void Awake()
    {
        var logger = ZLoggerFactory.GetLogger(
            loggerFilePath: "Logs/app.log",
            categoryName: "Game",
            minimumLogLevel: LogLevel.Information);

        _logger = new ZLoggerService(logger);
    }

    private void Start()
    {
        var playerId = 42;

        _logger.Information($"Player {playerId} connected");
        _logger.Warning("Low memory warning");
        _logger.Error(new System.Exception("Boom"), $"Failed to load profile {playerId}");
    }
}
```

- `ILoggerService.IsEnabled(LogLevel)` lets you gate expensive work.
- String overloads call directly into `Microsoft.Extensions.Logging`.
- Interpolated string overloads skip formatting when the log level is disabled.

## Requirements

- Unity 6 (`6000.0+`)
- `org.nuget.zlogger` 2.5.10
- `org.nuget.zstring` 2.6.0
- C# 10 (enabled via `Assets/csc.rsp`)
