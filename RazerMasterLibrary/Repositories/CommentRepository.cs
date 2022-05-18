using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data.SqlClient;



namespace RazerMasterLibrary.Repositories
{
    public class CommentRepository : BaseRepository
    {
        public List<dynamic> GetCommentListByProduct(int? productID)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"SELECT * FROM ProductComments AS PC
INNER JOIN Users AS U ON U.Id = PC.UserID
WHERE PC.ProductID = @ProductID and PC.Status=1 Order By CreateDate DESC";
                var commentList = conn.Query(sql, new { ProductID = new[] { productID } }).ToList();
                return commentList;
            }
        }

        public List<dynamic> GetRatingScoreByProduct(int? productID)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"DECLARE @startnum INT=1
DECLARE @endnum INT=5
;
WITH gen AS (
    SELECT @startnum AS Score
    UNION ALL
    SELECT Score+1 FROM gen WHERE Score+1<=@endnum
)
SELECT gen.Score,ISNULL(Count,0) AS Count FROM gen
LEFT JOIN(
SELECT PC.Score,COUNT(*) AS Count FROM ProductComments AS PC
INNER JOIN Users AS U ON U.Id = PC.UserID
WHERE PC.ProductID =@ProductID and PC.Status=1
GROUP BY PC.Score
) AS T ON T.Score=GEN.Score
ORDER BY gen.Score DESC
option (maxrecursion 100)";
                var commentList = conn.Query(sql, new { ProductID = new[] { productID } }).ToList();
                return commentList;
            }
        }

        public List<dynamic> GetAllComment()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"SELECT PC.ID,U.Email,Content,CreateDate,ProductName,P.Picture,Score,PC.Status FROM ProductComments AS PC
INNER JOIN Users AS U ON U.Id = PC.UserID
INNER JOIN Products AS P ON P.ProductID=PC.ProductID
Order By CreateDate DESC";
                var commentList = conn.Query(sql).ToList();
                return commentList;
            }
        }
    }
}
