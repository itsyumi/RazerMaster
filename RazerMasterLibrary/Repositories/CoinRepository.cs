using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using RazerMasterLibrary.Models;


namespace RazerMasterLibrary.Repositories
{
    public class CoinRepository : BaseRepository
    {
        public int InsertCasinoGameLog(Coins coin)
        {
            int affectedRow = 0;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"INSERT INTO Coins(ID, UserID, Quantity, EventTime,Status,ExpiredDate,EventType) VALUES (@ID, @UserID, @Quantity, @EventTime,@Status,@ExpiredDate,@EventType)";

                affectedRow = conn.Execute(sql, new { coin.ID, coin.UserID, coin.Quantity, coin.EventTime, coin.Status, coin.ExpiredDate, coin.EventType });
            }
            return affectedRow;
        }

        public int CheckPlayCasinoGameOrNot(string userID)
        {

            int affectedRow = 0;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"SELECT * FROM Coins WHERE DateDiff(dd,EventTime,getdate())=0 and UserID=@userID";
                var result = conn.Query(sql, new { UserID = userID });

                affectedRow = result.Count();
            }

            return affectedRow;
        }

        public decimal GetMemberCurrentCoin(string userID)
        {

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"SELECT SUM(
CASE  
	WHEN Status=1 THEN +Quantity
	WHEN Status=0 THEN -Quantity 
END) AS Total
FROM COINS WHERE UserID=@userID";
                var result = conn.QuerySingle(sql, new { UserID = userID });
                return Convert.ToDecimal(result.Total);

            }

        }

        public List<Coins> GetMemberCoinLog(string userID)
        {

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"SELECT * FROM Coins WHERE UserID=@userID";
                List<Coins> result = conn.Query<Coins>(sql, new { UserID = userID }).ToList();
                return result;
            }

        }
    }
}
