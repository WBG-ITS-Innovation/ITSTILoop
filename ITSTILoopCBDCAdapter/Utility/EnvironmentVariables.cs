namespace ITSTILoopCBDCAdapter.Utility
{
    public class EnvironmentVariableNames
    {
        public const string CONNECTION_STRING = "CONNECTION_STRING";
        public const string ADMIN_USER = "ADMIN_USER";
        public const string ADMIN_PASSWORD = "ADMIN_PASSWORD";
        public const string REVIEWER_USER = "REVIEWER_USER";
        public const string REVIEWER_PASSWORD = "REVIEWER_PASSWORD";
        public const string MASTER_KEY = "MASTER_KEY";
        //BESU
        public const string BESU_CONTRACT_ADDRESS = "BESU_CONTRACT_ADDRESS";
        public const string BESU_CONTRACT_OWNER_KEY = "BESU_CONTRACT_OWNER_KEY";
        public const string BESU_CONTRACT_TRANSACTION_HASH = "BESU_CONTRACT_TRANSACTION_HASH";
        public const string BESU_RPC_ENDPOINT = "BESU_RPC_ENDPOINT";
        public const string BESU_NETWORK_ID = "BESU_NETWORK_ID";
    }



    public class EnvironmentVariableDefaultValues
    {
        //BESU DEFAULT VALUES
        public const string BESU_CONTRACT_ADDRESS_DEFAULT_VALUE = "0x2E68E12209cdea9374D2f35561bd5f803a63497f";
        public const string BESU_CONTRACT_TRANSACTION_HASH_DEFAULT_VALUE = "0xd7c86bd93d596d79045482bbf1117f43b401c9ca0dc8d50310a6332118093dea";
        public const string BESU_CONTRACT_OWNER_KEY_DEFAULT_VALUE = "0x5948fa37331e9c5a1ce790279c1bef0008488ab7b1d02324751353e862bca84a";
        public const string BESU_RPC_ENDPOINT_DEFAULT_VALUE = "http://nginx:eben11@20.127.87.76";
        public const string ADMIN_USER_DEFAULT_VALUE = "admin@sol";
        public const string ADMIN_PASSWORD_DEFAULT_VALUE = "testTEST11!@";
        public const string REVIEWER_USER_DEFAULT_VALUE = "reviewer@sol";
        public const string REVIEWER_PASSWORD_DEFAULT_VALUE = "ebe11";
        public const string CONNECTION_STRING_DEFAULT_VALUE = "PostgresConnection";
        public const string MASTER_KEY_DEFAULT_VALUE = "TZGL6947QRSMVWBZ6W65GWQNO2TOWMN8";
        public const string BESU_NETWORK_ID_DEFAULT_VALUE = "648529";
    }


    public static class EnvironmentVariables
    {
        public static string GetEnvironmentVariable(string name, string defaultValue)
    => Environment.GetEnvironmentVariable(name) is string v && v.Length > 0 ? v : defaultValue;
    }
}
