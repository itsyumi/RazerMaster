using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data.SqlClient;

namespace RazerMasterLibrary.Repositories
{
    public class ChartRepository : BaseRepository
    {
        public int GetMemberCount()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"SELECT COUNT(ID) as Members from users";
                var result = conn.Query(sql).ToList();
                int count = result[0].Members;
                return count;

            }
        }
        public List<dynamic> GetCategoryTotalSale()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"SELECT C.CategoryName,ISNULL(T.Total,0) AS Total FROM Categories AS C
LEFT JOIN
(
SELECT distinct ca.CategoryName, sum(o.TotalPrice) as Total
                                FROM Orders O INNER JOIN OrderDetails as od ON o.OrderID = od.OrderID
                                INNER JOIN Products as p ON od.ProductID = p.ProductID
                                INNER JOIN Categories as ca ON p.CategoryID = ca.CategoryID
                                Group by ca.CategoryName
) AS T ON C.CategoryName=T.CategoryName";


                var result = conn.Query(sql).ToList();

                return result;
            }
        }

        public List<dynamic> Top5OrdersChart()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"SELECT Top 5 OrderID,CustomerName, OrderTime, TotalPrice, 
                               Status from Orders order by orderTime desc";

                var result = conn.Query(sql).ToList();

                return result;
            }
        }

        public List<dynamic> PurchaseANLSChart()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"SELECT COUNT(CASE WHEN BNUM = 1 THEN +1 END) AS FirstBuy, 
                               COUNT(CASE WHEN BNUM > 1 THEN + 1 END) AS ReBuy 
                               FROM(SELECT *, ROW_NUMBER() over(Partition BY UserID ORDER BY OrderTime ASC) as BNUM FROM Orders) as TA";

                var result = conn.Query(sql).ToList();
                return result;
            }
        }

        public ChartOneData PieCharted()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"SELECT 
                                CONVERT(varchar(7),o.OrderTime,120) AS Month,
                                count(o.OrderID) AS OrderCount
                                FROM Orders As o 
                                INNER JOIN OrderDetails as od ON o.OrderID = od.OrderID
                                INNER JOIN Products as p ON od.ProductID = p.ProductID 
                                where o.OrderTime between dateadd(mm,-4,getdate()) and getdate()
                                GROUP BY CONVERT(varchar(7),o.OrderTime,120)
                                ";

                var result = conn.Query(sql).ToList();

                List<string> month = new List<string>();
                List<dynamic> orderCount = new List<dynamic>();

                foreach (var item in result)
                {
                    orderCount.Add(item.OrderCount);
                    month.Add(item.Month);
                }

                ChartOneData chart = new ChartOneData
                {
                    Data = orderCount,
                    Date = month
                };

                return chart;
            }
        }

        public ChartTwoData LineCharted()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"SELECT
                                CONVERT(varchar(7),OrderTime,120) AS Month,
                                COUNT(CASE WHEN BNUM=1 THEN +1 END) as FirstBuy,
                                COUNT(CASE WHEN BNUM>1 THEN +1 END) AS ReBuy
                                FROM (
	                                SELECT *,
	                                ROW_NUMBER() over (Partition BY UserID ORDER BY OrderTime ASC) as BNUM
	                                FROM Orders
                                ) as TA
                                Where OrderTime between dateadd(mm,-4,getdate()) and getdate()
                                Group by CONVERT(varchar(7),OrderTime,120)";

                var result = conn.Query(sql).ToList();

                List<string> month = new List<string>();
                List<dynamic> firstBuy = new List<dynamic>();
                List<dynamic> ReBuy = new List<dynamic>();

                foreach (var item in result)
                {
                    month.Add(item.Month);
                    firstBuy.Add(item.FirstBuy);
                    ReBuy.Add(item.ReBuy);
                }

                ChartTwoData chart = new ChartTwoData
                {
                    Date = month,
                    FirstData = firstBuy,
                    SecondData = ReBuy
                };

                return chart;
            }
        }

        public ChartOneData PieMembered()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"Select count(username) as Member, CONVERT(varchar(7),ResigiterDate,120) AS Month from users
               GROUP BY CONVERT(varchar(7),ResigiterDate,120)";

                var result = conn.Query(sql).ToList();

                List<string> month = new List<string>();
                List<dynamic> memberIncrease = new List<dynamic>();
                foreach (var item in result)
                {
                    month.Add(item.Month);
                    memberIncrease.Add(item.Member);
                }

                ChartOneData chart = new ChartOneData
                {
                    Date = month,
                    Data = memberIncrease,
                };

                return chart;
            }
        }

        public ChartOneData LineCounted()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"DECLARE @startDate DATETIME, @endDate DATETIME
            SELECT @startDate = '2020-09-14', @endDate = '2020-9-20'
            ;WITH Calender AS (
                SELECT @startDate AS dt
                UNION ALL
                SELECT dt + 1 FROM Calender
                WHERE dt + 1 <= @endDate
            )

            select * 
            from (SELECT NameDay = DATENAME (WEEKDAY,dt) FROM Calender) AS A
            LEFT join (SELECT DATENAME(WEEKDAY,OrderTime) as Date, 
	            ISNULL(count(OrderID),0) AS OrderCount FROM Orders
	            where(DATEPART(wk, OrderTime) = DATEPART(wk, GETDATE())) AND (DATEPART(yy,  OrderTime) = DATEPART(yy, GETDATE()))
	            GROUP BY DATENAME(WEEKDAY,OrderTime)) AS B
            on (A.NameDay = B.Date)";

                var result = conn.Query(sql).ToList();

                List<string> weekDay = new List<string>();
                List<dynamic> ordercount = new List<dynamic>();
                foreach (var item in result)
                {
                    weekDay.Add(item.NameDay);
                    ordercount.Add(item.OrderCount);
                }

                ChartOneData chart = new ChartOneData
                {
                    Date = weekDay,
                    Data = ordercount,
                };

                return chart;


            }
        }

        public ChartOneData LineTotaled()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"SELECT 
                    CONVERT(varchar(7),o.OrderTime,120) AS Month,
                    SUM(od.UnitPrice * od.Quantity) AS Revenue
                    FROM Orders As o 
                    INNER JOIN OrderDetails as od ON o.OrderID = od.OrderID
                    INNER JOIN Products as p ON od.ProductID = p.ProductID 
                    INNER JOIN Categories as ca ON p.CategoryID = ca.CategoryID
                    where o.OrderTime between dateadd(mm,-3,getdate()) and getdate()
                    GROUP BY CONVERT(varchar(7),o.OrderTime,120)";

                var result = conn.Query(sql).ToList();

                List<string> month = new List<string>();
                List<dynamic> revenue = new List<dynamic>();

                foreach (var item in result)
                {
                    month.Add(item.Month);
                    revenue.Add(item.Revenue);
                }

                ChartOneData chart = new ChartOneData
                {
                    Date = month,
                    Data = revenue,
                };

                return chart;
            }
        }

        public List<dynamic> MonthTotaled()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"DECLARE @SQL VARCHAR(MAX), 
            @Columns VARCHAR(MAX)
            SELECT @Columns = COALESCE(@Columns + ',', '') + QUOTENAME(CategoryName)
            FROM Categories

            SET @Sql = ' 
                SELECT* FROM
                (
                    SELECT ca.CategoryName,
                    CONVERT(varchar(7),o.OrderTime,120) [Sales], 
                    od.Quantity* od.UnitPrice AS[Amount]
                    FROM Orders o
                    INNER JOIN OrderDetails as od ON o.OrderID = od.OrderID
                    INNER JOIN Products as p ON od.ProductID = p.ProductID
                    INNER JOIN Categories as ca ON p.CategoryID = ca.CategoryID
                    where o.OrderTime between dateadd(mm, -4, getdate()) and getdate()
                ) T
                PIVOT
                (
                    SUM([Amount])
                    FOR CategoryName IN(' + @Columns + ')
                ) p
                ORDER BY[Sales]' 
            EXEC(@Sql)";


                var result = conn.Query(sql).ToList();
                return result;
            }
        }
    }

    public class ChartOneData
    {
        public List<string> Date { get; set; }
        public List<dynamic> Data { get; set; }

    }

    public class ChartTwoData
    {
        public List<string> Date { get; set; }
        public List<dynamic> FirstData { get; set; }
        public List<dynamic> SecondData { get; set; }

    }
  

}
