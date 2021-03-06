﻿using Joker.Logging.Models;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using System;

namespace Joker.Logging
{
    public class ILoggerBuilderExtensions
    {
        public static Serilog.ILogger CreateLoggerElasticsearch(Action<ElkOptions> optionBuilder)
        {
            var elkOptions = new ElkOptions();
            optionBuilder.Invoke(elkOptions);

            var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("ApplicationName", elkOptions.AppName)
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elkOptions.Url))
            {
                AutoRegisterTemplate = true,
                IndexFormat = elkOptions.IndexFormat,
                CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true)
            });

            return logger.CreateLogger();
        }

        public static Serilog.ILogger CreateLoggerConsole(Action<LoggerOptions> optionBuilder)
        {
            var loggerOptions = new LoggerOptions();
            optionBuilder.Invoke(loggerOptions);

            var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("ApplicationName", loggerOptions.AppName)
            .WriteTo.Console();

            return logger.CreateLogger();
        }
    }
}
