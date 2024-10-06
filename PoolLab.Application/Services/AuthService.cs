using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public AuthService(IAccountService accountService, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<string> LoginAsync(LoginDTO loginData)
        {
            var acc = await _accountService.GetAccountByEmailAndPasswordAsync(loginData.Email, loginData.Password);
            if(acc == null)
            {
                return null;
            }
            return CreateToken(acc);           
        }

        public async Task<string> LoginStaffAsync(LoginAccDTO loginAccDTO)
        {
            var acc = await _accountService.GetLoginAcc(loginAccDTO);
            if(acc == null)
            {
                return null;
            }
            return CreateToken2(acc);
        }

        public async Task<string> RegisterAsync(RegisterDTO registerData)
        {
            try
            {
                var acc = await _accountService.AddNewUser(registerData);
                return acc;
            }catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string CreateToken(AccountLoginDTO account)
        {
            var nowUtc = DateTime.UtcNow;
            var expirationDuration = TimeSpan.FromMinutes(60);
            var expirationUtc = nowUtc.Add(expirationDuration);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,
                        _configuration.GetSection("JwtSecurityToken:Subject").Value),
                new Claim(JwtRegisteredClaimNames.Jti,
                                  Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                                  EpochTime.GetIntDate(nowUtc).ToString(),
                                  ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Exp,
                                  EpochTime.GetIntDate(expirationUtc).ToString(),
                                  ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Iss,
                                  _configuration.GetSection("JwtSecurityToken:Issuer").Value),
                new Claim(JwtRegisteredClaimNames.Aud,
                                  _configuration.GetSection("JwtSecurityToken:Audience").Value),
                new Claim(ClaimTypes.Role, account.Role.Name),
                new Claim("AccountId", account.Id.ToString()),
                new Claim("AccountStatus", account.Status),
                new Claim("Username", account.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                (_configuration.GetSection("JwtSecurityToken:Key").Value));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                        issuer: _configuration.GetSection("JwtSecurityToken:Issuer").Value,
                        audience: _configuration.GetSection("JwtSecurityToken:Audience").Value,
                        claims: claims,
                        expires: expirationUtc,
                        signingCredentials: signIn);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        private string CreateToken2(GetLoginAccDTO account)
        {
            var nowUtc = DateTime.UtcNow;
            var expirationDuration = TimeSpan.FromMinutes(60);
            var expirationUtc = nowUtc.Add(expirationDuration);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,
                        _configuration.GetSection("JwtSecurityToken:Subject").Value),
                new Claim(JwtRegisteredClaimNames.Jti,
                                  Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                                  EpochTime.GetIntDate(nowUtc).ToString(),
                                  ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Exp,
                                  EpochTime.GetIntDate(expirationUtc).ToString(),
                                  ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Iss,
                                  _configuration.GetSection("JwtSecurityToken:Issuer").Value),
                new Claim(JwtRegisteredClaimNames.Aud,
                                  _configuration.GetSection("JwtSecurityToken:Audience").Value),
                new Claim(ClaimTypes.Role, account.Role.Name),
                new Claim("AccountId", account.Id.ToString()),
                new Claim("AccountStatus", account.Status),
                new Claim("StoreId", account.StoreId.ToString()),
                new Claim("Username", account.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                (_configuration.GetSection("JwtSecurityToken:Key").Value));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                        issuer: _configuration.GetSection("JwtSecurityToken:Issuer").Value,
                        audience: _configuration.GetSection("JwtSecurityToken:Audience").Value,
                        claims: claims,
                        expires: expirationUtc,
                        signingCredentials: signIn);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
