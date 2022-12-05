using Microsoft.Extensions.Options;
using VoteMovie.Infrastructure.Security;
using VoteMovie.Model.Config;
using VoteMovie.Repositories.Abstract;

namespace VoteMovie.Repositories
{
    public class EncryptionRepository : IEncryptionRepository
    {
        protected readonly IUnitOfWork unitOfWork;
        private readonly Encryption encryption;
        protected readonly AppSettings appSettings;

        public EncryptionRepository(IUnitOfWork UnitOfWork, IOptions<AppSettings> AppSettings)
        {
            unitOfWork = UnitOfWork;
            encryption = new Encryption();
            appSettings = AppSettings.Value;
        }

        public string CreateSalt()
        {
            return encryption.CreateSalt();
        }

        public string CreateSalt(string value)
        {
            return encryption.CreateSalt(new object[] { value, appSettings.Salt });
        }

        public string EncryptPassword(string password, string securityStamp)
        {
            return encryption.EncryptPassword(password, securityStamp);
        }

        public string HashSHA256(string value)
        {
            return encryption.HashSHA256(value);
        }
    }
}
