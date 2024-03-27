namespace facebook_api.Service.Hash
{
    public interface IPasswordHashing
    {
        string GenerateHash(string password);
        bool VerifyHash(string newPassword, string hashedPassword);
    }
}
