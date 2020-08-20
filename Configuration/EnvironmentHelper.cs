using dotenv.net;
using System;
using System.IO;

namespace ECS.Framework.Configuration
{
    public class EnvironmentHelper
    {
        public static T GetValueFromEnv<T>(string keyName, bool throwException = true)
        {
            var value = GetEnvironmentVariable(keyName, throwException);

            if (string.IsNullOrWhiteSpace(value))
                return default;

            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static string GetEnvironmentVariable(string envName, bool throwException = true)
        {
            if (string.IsNullOrWhiteSpace(envName))
            {
                throw new ArgumentNullException(nameof(envName), $"o parâmetro {nameof(envName)} deve ser informado (não pode ser vazio ou nulo).");
            }

            var value = Environment.GetEnvironmentVariable(envName);

            if (string.IsNullOrWhiteSpace(value) && throwException)
            {
                throw new ArgumentNullException(nameof(envName), $"Não foi encontrado valor para env {envName}.");
            }

            return value;
        }

        public static string Init(string customDotEnv = "")
        {
            if (!string.IsNullOrEmpty(customDotEnv))
            {
                CustomOutput($"HOST: {Environment.MachineName} | ENV: *CUSTOM* | CONFIG: {customDotEnv}");
                DotEnv.Config(filePath: customDotEnv, throwOnError: true);
                return customDotEnv;
            }

            var env = EnvironmentHelper.GetValueFromEnv<string>("DOTENV_FILE", throwException: true);
            var path = $".config.{env}.env";
            var output = $"HOST: {Environment.MachineName} | ENV: {env} | CONFIG: {path}";

            if (env == "staging")
                StagingOutput(output);
            else if (env == "production")
                ProductionOutput(output);
            else
                throw new InvalidOperationException($"DOTENV_FILE={env} não suportado.");

            if (File.Exists(path))
                DotEnv.Config(filePath: path, throwOnError: true);

            return env;
        }

        private static void CustomOutput(string env)
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine(env);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void StagingOutput(string env)
        {
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;

            Console.WriteLine(env);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void ProductionOutput(string env)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine(env);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}