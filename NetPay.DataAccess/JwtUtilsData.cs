namespace NetPay.DataAccess
{

        public interface IJwtUtilsData
        {
            string ValidateJwtToken(string token);
            string GenerateToken();
        }
}