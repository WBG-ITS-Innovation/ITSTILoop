using System;
namespace ITSTILoop.Helpers
{
	public class Constants
	{
		public Constants()
		{
		}
	}

    public class EnvVarNames
    {
        public const string DB_CONNECTION = "DB_CONNECTION";
        public const string SAMPLE_FSP_1 = "SAMPLE_FSP_1";
        public const string SAMPLE_FSP_1_PARTIES = "SAMPLE_FSP_1_PARTIES";
        public const string SAMPLE_FSP_2 = "SAMPLE_FSP_2";
        public const string SAMPLE_FSP_2_PARTIES = "SAMPLE_FSP_2_PARTIES";
    }

    public class EnvVarDefaultValues
    {
        public const string DB_CONNECTION = "PostgresComposeConnection";
        public const string SAMPLE_FSP_1 = "bankbethesda|BETHESDAAPIID|BETHESDAAPIKEY";
        public const string SAMPLE_FSP_1_PARTIES = "5551234567|5551234568";
        public const string SAMPLE_FSP_2 = "bankwheaton|WHEATONAPIID|WHEATONAPIKEY";
        public const string SAMPLE_FSP_2_PARTIES = "5551234569|5551234566";
    }

    public static class EnvVars
    {
        public static string GetEnvironmentVariable(string name, string defaultValue)
    => Environment.GetEnvironmentVariable(name) is string v && v.Length > 0 ? v : defaultValue;
    }
}

