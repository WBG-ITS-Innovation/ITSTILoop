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
        public const string SAMPLE_FSP_3 = "SAMPLE_FSP_3";
        public const string SAMPLE_FSP_3_PARTIES = "SAMPLE_FSP_3_PARTIES";
        public const string USE_GAL = "USE_GAL";
    }

    public class EnvVarDefaultValues
    {
        public const string DB_CONNECTION = "PostgresComposeConnection";
        public const string SAMPLE_FSP_1 = "bankbethesda|BETHESDAAPIID|BETHESDAAPIKEY|0xE1A6dc4e3Bdb3A8384181a9F70aB76642762557b";
        public const string SAMPLE_FSP_1_PARTIES = "5551234567|5551234568";
        public const string SAMPLE_FSP_2 = "bankwheaton|WHEATONAPIID|WHEATONAPIKEY|0x34A816710f52a75F4430674a12E2Ad9De1092e86";
        public const string SAMPLE_FSP_2_PARTIES = "5551234569|5551234566|";
        public const string SAMPLE_FSP_3 = "bankprincegeorge|PRINCEGEORGEAPIID|PRINCEGEORGEAPIKEY|0x6a82bf493725771AD037DD0cf1ABa956e73C18ff";
        public const string SAMPLE_FSP_3_PARTIES = "5551234559|5551234556";
        public const string USE_GAL = "TRUE";
    }

    public static class EnvVars
    {
        public static string GetEnvironmentVariable(string name, string defaultValue)
    => Environment.GetEnvironmentVariable(name) is string v && v.Length > 0 ? v : defaultValue;
    }
}

