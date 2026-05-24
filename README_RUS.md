# nk7-logger

Лёгкий логгер для Unity на базе `Microsoft.Extensions.Logging`, ZLogger и ZString с интерполированными строками без сборки сообщений при выключенном уровне и выводом в консоль Unity/файлы.

## Особенности

- Фасад `ILoggerService` для уровней `Trace`, `Debug`, `Information`, `Warning`, `Error` и `Critical`.
- Перегрузки для сообщения, исключения, исключения + сообщения и интерполированной строки на каждом уровне.
- Интерполированные строковые обработчики на ZString: сообщение не собирается, если уровень логирования выключен.
- `LoggerFactory` подключает ZLogger к Unity через `UnityDebugStream` и добавляет файловый вывод при необходимости.
- Вывод в Unity направляется по уровню: Trace/Debug/Information в `Debug.Log`, Warning в `Debug.LogWarning`, Error/Critical в `Debug.LogError`.
- `EmptyLoggerService` — no-op реализация для отключённого логирования.
- Пакет включает `Runtime/csc.rsp` с `-langversion:10` для обработчиков интерполированных строк.

## Содержание
- [Установка](#установка)
  - [Unity Package Manager](#unity-package-manager)
  - [Ручная установка](#ручная-установка)
- [Быстрый старт](#быстрый-старт)
  - [1. Включите C# 10](#1-включите-c-10)
  - [2. Создайте логгер](#2-создайте-логгер)
  - [3. Оберните его](#3-оберните-его)
  - [4. Пишите логи](#4-пишите-логи)
- [Жизненный цикл](#жизненный-цикл)
- [Вывод в консоль Unity](#вывод-в-консоль-unity)
- [Runtime API](#runtime-api)
- [Требования](#требования)

## Установка

### Unity Package Manager
1. Откройте Unity Package Manager (`Window -> Package Manager`).
2. Нажмите `+ -> Add package from git URL...`.
3. Вставьте `https://github.com/s-elovikov/nk7-logger.git?path=src`.

Unity не обновляет git-пакеты автоматически — при необходимости меняйте хеш вручную или используйте [UPM Git Extension](https://github.com/mob-sakai/UpmGitExtension).

### Ручная установка
Скопируйте папку `src` в проект и добавьте ссылку на asmdef `Nk7.Logger`.

## Быстрый старт

### 1. Включите C# 10
- C# 10 настраивается пакетом через `Runtime/csc.rsp` (`-langversion:10`).
- Если в проекте используются свои response-файлы компилятора, сохраните версию языка не ниже 10.

### 2. Создайте логгер
Используйте `LoggerFactory.GetLogger`, чтобы получить `Microsoft.Extensions.Logging.ILogger`.

```csharp
using Microsoft.Extensions.Logging;
using Nk7.Logger;

ILogger logger = LoggerFactory.GetLogger(
    minimumLogLevel: LogLevel.Information,
    loggerFilePath: "Logs/app.log");
```

`loggerFilePath` опционален. Если путь передан, недостающие директории создаются автоматически.

### 3. Оберните его
Оберните логгер в `LoggerService`, чтобы использовать удобный API `ILoggerService`.

```csharp
ILoggerService loggerService = new LoggerService(logger);
```

Если логирование нужно отключить, используйте `EmptyLoggerService`.

```csharp
ILoggerService loggerService = new EmptyLoggerService();
```

### 4. Пишите логи
Для простых сообщений используйте строковые перегрузки, а для пропуска форматирования на выключенных уровнях — интерполированные строки.

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

        _logger.Information($"Игрок {playerId} подключился");
        _logger.Warning("Предупреждение о низкой памяти");
        _logger.Error(new System.Exception("Boom"), $"Не удалось загрузить профиль {playerId}");
    }
}
```

## Жизненный цикл
- `LoggerFactory` собирает пайплайн `ILogger` с Unity-стримом и опциональным файловым выводом.
- Unity-стрим использует внутренний префикс уровня и направляет сообщение в подходящий метод `Debug.Log*`.
- Файловый вывод использует plain-text форматирование ZLogger, если передан `loggerFilePath`.
- Интерполированные строковые обработчики проверяют `IsEnabled` до создания ZString builder.
- `LoggerService` проверяет включённый уровень перед передачей строковых перегрузок и исключений в `Microsoft.Extensions.Logging`.

## Вывод в консоль Unity

| Уровень | Метод Unity |
| --- | --- |
| `Trace` | `Debug.Log` |
| `Debug` | `Debug.Log` |
| `Information` | `Debug.Log` |
| `Warning` | `Debug.LogWarning` |
| `Error` | `Debug.LogError` |
| `Critical` | `Debug.LogError` |

Внутренний префикс удаляется перед отправкой сообщения в консоль Unity.

## Runtime API

```csharp
public static class LoggerFactory
{
    public static ILogger GetLogger(
        LogLevel minimumLogLevel = LogLevel.Error,
        string loggerFilePath = null);
}
```

- Возвращает `Microsoft.Extensions.Logging.ILogger` с категорией `Application`.
- По умолчанию использует `LogLevel.Error`.
- При передаче `LogLevel.None` используется запасной уровень `LogLevel.Warning`.

```csharp
public sealed class LoggerService : ILoggerService
{
    public LoggerService(ILogger logger);
}
```

`ILoggerService` предоставляет:

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

Для тех же шести уровней есть перегрузки с интерполированными строками:

```csharp
_logger.Debug($"Position: {transform.position}");
_logger.Error(exception, $"Не удалось загрузить предмет {itemId}");
```

## Требования

- Unity-проект с поддержкой C# 10
- `org.nuget.zlogger` 2.5.10
- `org.nuget.zstring` 2.6.0
- UPM package name: `com.nk7.logger`
