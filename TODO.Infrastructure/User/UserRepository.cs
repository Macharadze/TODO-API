using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TODO.Application.Exceptions;
using TODO.Application.IUser;
using TODO.Application.Users.Request;
using TODO.Domain.Enums;
using TODO.Domain.TODO;
using TODO.Domain.Users;
using TODO.Infrastructure.Base;
using TODO.Persistence.Context;
namespace TODO.Infrastructure.Users
{
    public class UserRepository : BaseRepository<User>, IUserInterface
    {
        private readonly IConfiguration _configuration;

        public UserRepository(ApplicationDBcontext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
            _configuration = configuration;
        }

        public async Task Create(CancellationToken cancellationToken, UserLogin register)
        {
            if (Exists(cancellationToken, register.Username).Result)
                throw new UserAlreadyExistsException("");
            CreatePasswordHash(register.Password, out byte[] hash, out byte[] salt);

            var user = new User
            {
                Username = register.Username,
                PasswordHash = hash,
                PasswordSalt = salt,
                Status = Domain.Enums.Status.Created
            };
            await AddAsync(cancellationToken, user);
            await _context.SaveChangesAsync(true, cancellationToken);
        }

        public async Task<bool> Exists(CancellationToken cancellationToken, string username)
        {
            return await base.AnyAsync(cancellationToken, x => x.Username.ToLower().Equals(username.ToLower()));
        }

        public async Task<string> Login(CancellationToken cancellationToken, UserLogin login)
        {
            var targetUser = await base.Table.Where(i => i.Username.ToLower().Equals(login.Username)).FirstOrDefaultAsync();
            if (targetUser == null)
            {
                throw new UserDoesNotExistsException("");
            }
            if (!VerifyPasswordHash(login.Password, targetUser.PasswordSalt, targetUser.PasswordHash))
                throw new UserNotFoundException("");
            string token = CreateToken(targetUser);
            return token;
        }

        private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using (var hmac = new HMACSHA256())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] salt, byte[] hash)
        {
            using (var hmac = new HMACSHA256(salt))
            {
                var computehash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computehash.SequenceEqual(hash);
            }
        }
        private string CreateToken(User user)
        {

            List<Claim> claims = new List<Claim>
             {
               new Claim(ClaimTypes.Name, user.Username),
               new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),

             };
            if (user.Username == "MamukaKhazaradze")
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            else
                claims.Add(new Claim(ClaimTypes.Role, "Customer"));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
               _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        public async Task<User> GetCurrentUser(CancellationToken token)
        {
            var id = await GetAuthorizedId();
            var user = await base.GetAsync(token, i => i.Id.ToString().Equals(id));
            return user;

        }

        public async Task<List<ToDo>> GetAllToDos(CancellationToken token, Status status = 0)
        {
            var id = await GetAuthorizedId();
            var owner = await _dbSet.Include(i => i.ToDos).Where(i => i.Id.ToString().Equals(id)).FirstOrDefaultAsync();
            return owner.ToDos.Where(i => i.Status == status).ToList();
        }

    }
}
