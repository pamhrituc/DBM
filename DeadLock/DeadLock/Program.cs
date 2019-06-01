using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeadLock
{
    class Program
    {
        static void firstRun2()
        {
            firstRun(1);
        }
        static void firstRun(int no)
        {
            string c = "Data Source = DESKTOP-U7E7QTV\\SQLEXPRESS;" + "Initial Catalog = MCU; Integrated Security = true";
            SqlConnection conn = new SqlConnection(c);
            conn.Open();
            try
            {
                SqlTransaction tran = conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand("deadlock1", conn, tran);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.ExecuteNonQuery();
                tran.Commit();
                Console.WriteLine("first transaction committed");
            }
            catch(SqlException e)
            {
                //chosen as deadlock victim
                if (e.Number == 1205)
                {
                    if (no > 0)
                    {
                        Console.WriteLine("First transaction has been chosen as a deadlock victim; please restart");
                        firstRun(--no);
                    }
                    else
                    {
                        Console.WriteLine("First transaction has been aborted");
                    }
                }
            }
        }
        static void secondRun2()
        {
            secondRun(1);
        }
        static void secondRun(int no)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-U7E7QTV\\SQLEXPRESS; Initial Catalog = MCU; Integrated Security = true");
            conn.Open();
            try
            {
                SqlTransaction tran = conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand("deadlock2", conn, tran);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.ExecuteNonQuery();
                tran.Commit();
                Console.WriteLine("second transaction committed");
            }
            catch (SqlException e)
            {
                //chosen as deadlock victim
                if (e.Number == 1205)
                {
                    if (no > 0)
                    {
                        Console.WriteLine("second transaction has been chosen as a deadlock victim; please restart");
                        secondRun(--no);
                    }
                    else
                    {
                        Console.WriteLine("Second transaction has been aborted");
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            Thread t1 = new Thread(new ThreadStart(firstRun2));
            Thread t2 = new Thread(new ThreadStart(secondRun2));
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
            Console.Write("done");
            Console.ReadKey();
        }
    }
}
