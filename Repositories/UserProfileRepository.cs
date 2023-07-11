using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Streamish.Models;
using Streamish.Utils;

namespace Streamish.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public List<UserProfile> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], Email, ImageUrl, DateCreated
                        FROM UserProfile";

                    using (var reader = cmd.ExecuteReader())
                    {
                        var profiles = new List<UserProfile>();
                        while (reader.Read())
                        {
                            var userProfile = new UserProfile()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                                ImageUrl = DbUtils.GetString(reader, "ImageUrl")
                            };
                            profiles.Add(userProfile);
                        }

                        return profiles;
                    }
                }
            }
        }

        public UserProfile GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], Email, ImageUrl, DateCreated
                        FROM UserProfile
                        WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var userProfile = new UserProfile()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                                ImageUrl = DbUtils.GetString(reader, "ImageUrl")
                        };
                            return userProfile;
                        }

                        return null;
                    }
                }
            }
        }

        public UserProfile GetUserByIdWithVideos(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT up.Id AS UserId, up.[Name], up.Email, up.ImageUrl, up.DateCreated AS UserCreated,
                                        v.Id AS VideoId, v.Title, v.[Description], v.[Url], v.DateCreated AS VideoCreated
                                        FROM UserProfile up
                                        LEFT JOIN Video v ON v.UserProfileId = up.Id
                                        WHERE up.Id = @id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        UserProfile user = null;
                        while (reader.Read())
                        {
                            if (user == null)
                            {
                                user = new UserProfile()
                                {
                                    Id = id,
                                    Name = DbUtils.GetString(reader, "Name"),
                                    Email = DbUtils.GetString(reader, "Email"),
                                    DateCreated = DbUtils.GetDateTime(reader, "UserCreated"),
                                    ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                                    Videos = new List<Video>()
                                };
                                
                            }
                            if (DbUtils.IsNotDbNull(reader, "VideoId"))
                            {
                                user.Videos.Add(new Video()
                                {
                                    Id = DbUtils.GetInt(reader, "VideoId"),
                                    Title = DbUtils.GetString(reader, "Title"),
                                    Description = DbUtils.GetString(reader, "Description"),
                                    Url = DbUtils.GetString(reader, "Url"),
                                    DateCreated = DbUtils.GetDateTime(reader, "VideoCreated"),
                                    UserProfileId = id
                                });
                            }
                        }
                        return user;
                    }
                }
            }
        }

        public void Add(UserProfile profile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO UserProfile ([Name], Email, ImageUrl, DateCreated)
                        OUTPUT INSERTED.ID
                        VALUES (@Name, @Email, @ImageUrl, @DateCreated)";

                    DbUtils.AddParameter(cmd, "@Name", profile.Name);
                    DbUtils.AddParameter(cmd, "@Email", profile.Email);
                    DbUtils.AddParameter(cmd, "@ImageUrl", profile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@DateCreated", profile.DateCreated);

                    profile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(UserProfile profile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE UserProfile
                        SET [Name] = @Name,
                            Email = @Email,
                            ImageUrl = @ImageUrl
                        WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@Name", profile.Name);
                    DbUtils.AddParameter(cmd, "@Email", profile.Email);
                    DbUtils.AddParameter(cmd, "@ImageUrl", profile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@Id", profile.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM UserProfile WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

