namespace ITSTILoopLibrary.Utility
{
    public class EnvironmentVariableNames
    {
        public const string CONNECTION_STRING = "CONNECTION_STRING";
        public const string ADMIN_USER = "ADMIN_USER";
        public const string ADMIN_PASSWORD = "ADMIN_PASSWORD";
        public const string REVIEWER_USER = "REVIEWER_USER";
        public const string REVIEWER_PASSWORD = "REVIEWER_PASSWORD";
        public const string MASTER_KEY = "MASTER_KEY";
        
        public const string CBDC_TRANSFER_CONTRACT_ADDRESS = "CBDC_TRANSFER_CONTRACT_ADDRESS";
        public const string CBDC_TRANSFER_CONTRACT_OWNER_KEY = "CBDC_TRANSFER_CONTRACT_OWNER_KEY";        
        public const string CBDC_RPC_ENDPOINT = "CBDC_RPC_ENDPOINT";
        public const string CBDC_NETWORK_ID = "CBDC_NETWORK_ID";

        public const string BESU_CONTRACT_TRANSACTION_HASH = "BESU_CONTRACT_TRANSACTION_HASH";
        public const string IS_LOOP_PARTICIPANT = "IS_LOOP_PARTICIPANT";

        public const string GLOBAL_ADDRESS_LOOKUP_PARTIES = "GLOBAL_ADDRESS_LOOKUP_PARTIES";
        
        public const string GLOBAL_ADDRESS_LOOKUP_URL = "GLOBAL_ADDRESS_LOOKUP_URL";
    }



    public class EnvironmentVariableDefaultValues
    {
        //BESU DEFAULT VALUES
        public const string CBDC_TRANSFER_CONTRACT_ADDRESS_DEFAULT_VALUE = "0xa88F7F14d97304f9B694d962E84fCFfe01E729D5";                        
        public const string CBDC_TRANSFER_CONTRACT_OWNER_KEY_DEFAULT_VALUE = "0xb5cc3ddbd4dacedd91eea5994c6cbbcd02b9b8644f89ef4576ce9bad2ae104be";
        public const string CBDC_RPC_ENDPOINT_DEFAULT_VALUE = "https://cbdcuser:testTEST11@cbdcethereum.azurewebsites.net";
        public const string CBDC_NETWORK_ID_DEFAULT_VALUE = "1492";

        public const string BESU_CONTRACT_TRANSACTION_HASH_DEFAULT_VALUE = "0xd7c86bd93d596d79045482bbf1117f43b401c9ca0dc8d50310a6332118093dea";
        public const string ADMIN_USER_DEFAULT_VALUE = "admin@sol";
        public const string ADMIN_PASSWORD_DEFAULT_VALUE = "testTEST11!@";
        public const string REVIEWER_USER_DEFAULT_VALUE = "reviewer@sol";
        public const string REVIEWER_PASSWORD_DEFAULT_VALUE = "ebe11";
        public const string CONNECTION_STRING_DEFAULT_VALUE = "PostgresConnection";
        public const string MASTER_KEY_DEFAULT_VALUE = "TZGL6947QRSMVWBZ6W65GWQNO2TOWMN8";
        
        public const string IS_LOOP_PARTICIPANT_DEFAULT_VALUE = "true";
        public const string GLOBAL_ADDRESS_LOOKUP_URL_DEFAULT_VALUE = "http://globalpartylookup/";
    }


    public static class EnvironmentVariables
    {
        public static string GetEnvironmentVariable(string name, string defaultValue)
    => Environment.GetEnvironmentVariable(name) is string v && v.Length > 0 ? v : defaultValue;
    }
}
