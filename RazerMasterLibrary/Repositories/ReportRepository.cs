using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace RazerMasterLibrary.Repositories
{
    public class ReportRepository : BaseRepository
    {

        public List<dynamic> GetProductReportByTime(DateTime? start, DateTime? end)
        {
            List<dynamic> productData;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                var parameters = new { startTime = start, endTime = end };
                string sql = @"SELECT p.ProductName AS 'Name'
                               ,SUM(od.Quantity) AS 'Sales'
                               ,SUM(od.UnitPrice * od.Quantity) AS 'SalesVolume'
                            FROM(SELECT
                                    *
                                FROM Orders

                                WHERE OrderTime > @startTime

                                AND OrderTime < @endTime) AS o
                            INNER JOIN OrderDetails AS od
                                ON o.OrderID = od.OrderID
                            RIGHT JOIN Products AS p
                                ON od.ProductID = p.ProductID
                            GROUP BY p.ProductName
                            ORDER BY SUM(od.Quantity* od.UnitPrice) DESC";
                productData = conn.Query(sql, parameters).ToList();
            }
            return productData;
        }
        public List<dynamic> GetCategroyReportByTime(DateTime? start, DateTime? end)
        {
            List<dynamic> categroyData;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                var parameters = new { startTime = start, endTime = end };
                string sql =
                    @"SELECT
	                ca.CategoryName AS 'Name'
                   ,SUM(od.Quantity) AS 'Sales'
                   ,SUM(od.Quantity*od.UnitPrice) AS 'SalesVolume'
                FROM (SELECT
		                *
	                FROM Orders
	                WHERE OrderTime > @startTime
	                AND OrderTime < @endTime) AS o
                INNER JOIN OrderDetails AS od
	                ON o.OrderID = od.OrderID
                INNER JOIN Products AS p
	                ON od.ProductID = p.ProductID
                RIGHT JOIN Categories AS ca
	                ON p.CategoryID = ca.CategoryID
                GROUP BY ca.CategoryName
                ORDER BY SUM(od.Quantity*od.UnitPrice) DESC";
                categroyData = conn.Query(sql, parameters).ToList();
            }
            return categroyData;
        }

        public string GetMemberReportByMonth()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"
                SELECT CAST(YEAR(OrderTime) AS VARCHAR(4)) + '-' +  CAST(MONTH(OrderTime) AS VARCHAR(2)) [Date],
                 COUNT(1) [NumberOfOrders],COUNT(DISTINCT UserID) [NumberOfCustomers], SUM(TotalPrice) [Sales]
                FROM Orders
                GROUP BY CAST(YEAR(OrderTime) AS VARCHAR(4)) + '-' +  CAST(MONTH(OrderTime) AS VARCHAR(2))
                ORDER BY 1";
                var member = conn.Query(sql).ToList();
                return JsonConvert.SerializeObject(member);
            }
        }

        public string GetMemberReportByYear()
        {
            //List<MemberViewModel> member;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"SELECT YEAR(OrderTime) [Date], 
                     COUNT(1) [NumberOfOrders],COUNT(DISTINCT UserID) [NumberOfCustomers], SUM(TotalPrice) [Sales]
                    FROM Orders
                    GROUP BY YEAR(OrderTime)
                    ORDER BY 1
                    ";
                var member = conn.Query(sql).ToList();
                return JsonConvert.SerializeObject(member);
            }
        }
    }
}
