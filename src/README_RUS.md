# nk7-logger

Лёгкий логгер для Unity на базе ZLogger и Microsoft.Extensions.Logging с интерполированными строками без лишних аллокаций и выводом в консоль Unity/файлы.

## Особенности

- Интерфейс `ILoggerService` с Debug/Information/Warning/Error и перегрузками для строк и интерполированных строк.
- Интерполированные строковые хэндлеры на ZString: форматирование и аллокации пропускаются, если уровень логирования выключен.
- `ZLoggerFactory` подключает ZLogger к Unity через `UnityDebugStream` и добавляет файловый вывод при необходимости.
- Сообщения в консоли Unity получают префиксы уровня (`[D]`, `[I]`, `[W]`, `[E]`).
- `EmptyLoggerService` — no-op реализация для отключённого логирования.
- Editor-утилита автоматически включает C# 10 через `Assets/csc.rsp`.

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
- [Инструменты инспектора](#инструменты-инспектора)
- [Runtime API](#runtime-api)
- [Требования](#требования)

## Установка

### Unity Package Manager
1. Откройте Unity Package Manager (`Window → Package Manager`).
2. Нажмите `+ → Add package from git URL…`.
3. Вставьте `https://github.com/s-elovikov/nk7-logger.git?path=src`.

Unity не обновляет git-пакеты автоматически — при необходимости меняйте хеш вручную или используйте [UPM Git Extension](https://github.com/mob-sakai/UpmGitExtension).

### Ручная установка
Скопируйте папку `src` в проект и добавьте `Nk7.Logger.asmdef` к сборке.

## Быстрый старт

### 1. Включите C# 10
- Скрипт редактора создаёт или обновляет `Assets/csc.rsp` с `-langversion:10` при первом запуске.
- Также можно выполнить `Nk7 → Logger → Ensure C# 10 (csc.rsp)` из меню Unity.

### 2. Создайте логгер
- Используйте `ZLoggerFactory.GetLogger`, чтобы получить `Microsoft.Extensions.Logging.ILogger`.
- Передайте путь к файлу, чтобы включить файловые логи (папки создаются автоматически).

### 3. Оберните его
- Оберните логгер в `ZLoggerService`, чтобы использовать удобный `ILoggerService`.
- Если логирование нужно отключить, используйте `EmptyLoggerService`.

### 4. Пишите логи
- Для простых сообщений используйте строковые перегрузки.
- Интерполированные строки экономят ресурсы при отключённом уровне логирования.

## Жизненный цикл
- `ZLoggerFactory` собирает пайплайн `ILogger` с Unity-стримом и опциональным файловым стримом.
- Unity-стрим добавляет префикс уровня (`[D]`, `[I]`, `[W]`, `[E]`) и отправляет сообщение в `Debug.Log*`.
- Интерполированные хэндлеры проверяют `IsEnabled` до создания ZString builder.

## Инструменты инспектора
- При запуске редактора пакет убеждается, что в `Assets/csc.rsp` есть `-langversion:10` для C# 10.
- Пункт меню `Nk7 → Logger → Ensure C# 10 (csc.rsp)` позволяет применить настройку вручную.

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

        _logger.Information($"Игрок {playerId} подключился");
        _logger.Warning("Предупреждение о низкой памяти");
        _logger.Error(new System.Exception("Boom"), $"Не удалось загрузить профиль {playerId}");
    }
}
```

- `ILoggerService.IsEnabled(LogLevel)` помогает отключать дорогостоящие вычисления.
- Строковые перегрузки вызывают `Microsoft.Extensions.Logging` напрямую.
- Интерполированные перегрузки пропускают форматирование при выключенном уровне.

## Требования

- Unity 6 (`6000.0+`)
- `org.nuget.zlogger` 2.5.10
- `org.nuget.zstring` 2.6.0
- C# 10 (включается через `Assets/csc.rsp`)
