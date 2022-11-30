using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.Extensions.Configuration.Json;
using System.Data.SQLite;

namespace SignalRHub
{
	/// <summary>
	/// 本类为Sqlite数据库帮助类
	/// </summary>
	public class SqliteHelper
	{
		private static string connectionString = "Data Source=D:\\SignalR\\SignalRWeb\\bin\\Debug\\net6.0\\data.db;";


		/// <summary>
		/// 适合增删改操作，返回影响条数
		/// </summary>
		/// <param name="sql">SQL</param>
		/// <param name="parameters">参数</param>
		/// <returns></returns>
		public static int ExecuteNonQuery(string sql, params SqliteParameter[] parameters)
		{
			using (SqliteConnection conn = new SqliteConnection(connectionString))
			{
				using (SqliteCommand comm = conn.CreateCommand())
				{
					try
					{
						conn.Open();
						comm.CommandText = sql;
						comm.Parameters.AddRange(parameters);
						return comm.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						throw new Exception(ex.Message);
					}
					finally
					{
						if (conn != null && conn.State != ConnectionState.Closed)
							conn.Close();
					}

				}
			}
		}

		/// <summary>
		/// 查询操作，返回查询结果中的第一行第一列的值
		/// </summary>
		/// <param name="sql">SQL</param>
		/// <param name="parameters">参数</param>
		/// <returns></returns>
		public static object ExecuteScalar(string sql, params SqliteParameter[] parameters)
		{
			using (SqliteConnection conn = new SqliteConnection(connectionString))
			{
				using (SqliteCommand comm = conn.CreateCommand())
				{
					try
					{
						conn.Open();
						comm.CommandText = sql;
						comm.Parameters.AddRange(parameters);
						return comm.ExecuteScalar();
					}
					catch (Exception ex)
					{
						throw new Exception(ex.Message);
					}
					finally
					{
						if (conn != null && conn.State != ConnectionState.Closed)
							conn.Close();
					}
				}
			}
		}


		/// <summary>
		/// 执行ExecuteReader
		/// </summary>
		/// <param name="sqlText">SQL</param>
		/// <param name="parameters">参数</param>
		/// <returns></returns>
		public static SqliteDataReader ExecuteReader(string sql, params SqliteParameter[] parameters)
		{
			SqliteConnection conn = null;
			try
			{
				//SqlDataReader要求，它读取数据的时候有，它独占它的SqlConnection对象，而且SqlConnection必须是Open状态
				conn = new SqliteConnection(connectionString);//不要释放连接，因为后面还需要连接打开状态
				SqliteCommand cmd = conn.CreateCommand();
				conn.Open();
				cmd.CommandText = sql;
				cmd.Parameters.AddRange(parameters);
				//CommandBehavior.CloseConnection当SqlDataReader释放的时候，顺便把SqlConnection对象也释放掉
				return cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (Exception ex)
			{
				conn.Close();
			}
			return null;
		}


		/// <summary>
		/// Adapter调整，查询操作，返回DataTable
		/// </summary>
		/// <param name="sql">SQL</param>
		/// <param name="parameters">参数</param>
		/// <returns></returns>
		public static DataTable ExecuteDataTable(string sql, params SqliteParameter[] parameters)
		{
			using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, connectionString))
			{
				DataTable dt = new DataTable();
				adapter.SelectCommand.Parameters.AddRange(parameters);
				adapter.Fill(dt);
				return dt;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sql">sql语句</param>
		/// <param name="param">参数</param>
		/// <returns></returns>
		public List<U_Info> GetList(string sql, List<SqliteParameter> param =null)
		{
			List<U_Info> result = new List<U_Info>();
			List<SqliteParameter> parameters = new List<SqliteParameter>();
			using var connection = new SqliteConnection(connectionString);
			connection.Open();
			var command = connection.CreateCommand();
			command.CommandText = sql;
			if (param!=null && param.Count>0)
				command.Parameters.AddRange(parameters);
			//command.Parameters.AddWithValue("$id", id);
			using var reader = command.ExecuteReader();
			result = SqliteDataReaderToList<U_Info>.GetList(reader);
			return result;
		}
	}
	public class SqliteDataReaderToList<T> where T : class
	{
		public static List<T> GetList(SqliteDataReader reader)
		{
			List<T> list = new List<T>();
			Type type = typeof(T);
			PropertyInfo[] p = type.GetProperties(); //得到该T类中的所有公共属性

			while (reader.Read())
			{
				var model = Activator.CreateInstance<T>();
				foreach (var item in p)
				{
					if (item == null)
					{
						continue;
					}
					item.SetValue(model, reader[item.Name], null);
				}
				list.Add(model);
			}
			reader.Close();
			return list;
		}
	}
}
