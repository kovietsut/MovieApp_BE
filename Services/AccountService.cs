using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VoteMovie.Entity;
using VoteMovie.Infrastructure.Security;
using VoteMovie.Infrastructure.Utils;
using VoteMovie.Model;
using VoteMovie.Model.Config;
using VoteMovie.Repositories.Abstract;
using VoteMovie.Services.Abstract;

namespace VoteMovie.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtConfiguration _jwtOptions;
        private readonly IEncryptionRepository _iEncryptionRepository;

        public AccountService(IUnitOfWork unitOfWork, IOptions<JwtConfiguration> jwtOptions,
            IEncryptionRepository iEncryptionRepository)
        {
            _unitOfWork = unitOfWork;
            _jwtOptions = jwtOptions.Value;
            _iEncryptionRepository = iEncryptionRepository;
        }

        public JsonResult SignUp(AccountModel model)
        {
            try
            {
                if (!Util.IsEmail(model.Email))
                {
                    return JsonUtil.Error("Incorrect format email");
                }

                model.Email = model.Email.ToLower();
                var checkEmail = _unitOfWork.RepositoryCRUD<User>().Any(x => x.Email == model.Email);
                if (checkEmail)
                {
                    return JsonUtil.Error("Email existed");
                }

                if (model.Password.Length < 8)
                {
                    return JsonUtil.Error("Password must be at least 8 characters");
                }

                User user = new()
                {
                    IsEnabled = true,
                    CreatedAt = DateTime.Now,
                    Email = model.Email.Trim(),
                    Username = model.Username.Trim(),
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                user.PasswordHash = _iEncryptionRepository.EncryptPassword(model.Password, user.SecurityStamp);
                _unitOfWork.RepositoryCRUD<User>().Insert(user);
                _unitOfWork.Commit();
                return JsonUtil.Success(user.Id, "Sign Up Account Success");
            }
            catch (Exception ex)
            {
                return JsonUtil.Error("Something went wrong");
            }
        }

        public async Task<dynamic> SignIn(LoginModel model)
        {
            try
            {
                var user = _unitOfWork.RepositoryCRUD<User>()
                    .GetSingleWithNoIsEnable(x => x.Email == model.Username.Trim() || model.Username.Trim() == x.Username);
                if (user == null)
                {
                    return JsonUtil.Error("Incorrect email or password");
                }

                if (model.Password.Length < 8)
                {
                    return JsonUtil.Error("Pass min length must be at least 8");
                }

                var checkPass = _iEncryptionRepository.EncryptPassword(model.Password, user.SecurityStamp);
                if (user.PasswordHash != checkPass)
                {
                    return JsonUtil.Error("Incorrect username or password");
                }
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.CHash, _iEncryptionRepository.HashSHA256(user.SecurityStamp)),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
            };

                // Create the JWT security token and encode it.
                var jwt = new JwtSecurityToken(
                    issuer: _jwtOptions.Issuer,
                    audience: _jwtOptions.Audience,
                    claims: claims,
                    notBefore: _jwtOptions.NotBefore,
                    expires: _jwtOptions.Expiration,
                    signingCredentials: _jwtOptions.SigningCredentials);

                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                // Serialize and return the response
                return JsonUtil.Success(new
                {
                    token = encodedJwt,
                    username = user.Username,
                });
            }
            catch (Exception ex)
            {
                return JsonUtil.Error("Something went wrong");
            }
        }
    }
}
