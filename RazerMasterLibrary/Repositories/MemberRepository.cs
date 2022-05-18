using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data.SqlClient;
using RazerMasterLibrary.Models;

namespace RazerMasterLibrary.Repositories
{
    public class MemberRepository : BaseRepository
    {

        public IEnumerable<Users> GetAllMember()
        {
            string sql = "SELECT * FROM Users";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                var members = conn.Query<Users>(sql);
                return members;
            }
        }


        public Users MemberDetail(string id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT * FROM Users where Id=@id";
                var member = conn.QueryFirstOrDefault<Users>(sql, new { id = id });
                if (member.LockoutEndDateUtc.HasValue)
                {
                    member.LockoutEndDateUtc = member.LockoutEndDateUtc.Value.AddHours(8);
                }
                return member;
            }
        }

        public void UpdateMemberDetail(Users member)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                if (member.LockoutEndDateUtc.HasValue)
                {
                    member.LockoutEndDateUtc = member.LockoutEndDateUtc.Value.AddHours(-8);
                }
                string sql = @"update Users set NickName = @NickName, PhoneNumber= @PhoneNumber, Address= @Address ,LockoutEndDateUtc = @LockoutEndDateUtc ,LockoutEnabled = @LockoutEnabled where Id=@id";
                conn.Execute(sql, new
                {
                    member.NickName,
                    member.PhoneNumber,
                    member.Address,
                    member.LockoutEndDateUtc,
                    member.LockoutEnabled,
                    member.Id
                });
            }
        }
    }
}
