using System.Collections.Generic;
using System.Data;
using System.IO;
using Dapper;

namespace WpfApp1
{
	class DatabaseItem
	{
		public string Expression { get; set; }

		public DatabaseItem(string expression)
		{
			Expression = expression;
		}
	}

	class Calculator
	{
		public static string Parse(string inputString)
		{
			DataTable dt = new DataTable();
			return dt.Compute(inputString, "").ToString();
		}

		public static void SaveHistoryInFile(string str)
		{
			var path = @"C:\Users\Евгений\Desktop\Новая папка\уник\трпо\Калькулятор\History.txt";
			File.AppendAllText(path, str + '\n');
		}

		public static void SaveHistoryInDatabase(DatabaseItem dbItem)
		{
			using (var connection = new System.Data.SQLite.SQLiteConnection("Data Source=C:\\Users\\Евгений\\Desktop\\Новая папка\\уник\\трпо\\SQLiteStudio\\CalculatorHistory.db"))
			{
				connection.Open();
				var result = connection.Query<DatabaseItem>($"insert into History (Expression) values ('{dbItem.Expression}')");
			}
		}

		public static List<string> LoadHistoryFromDatabase()
		{
			List<string> history = new List<string>();
			using (var connection = new System.Data.SQLite.SQLiteConnection("Data Source=C:\\Users\\Евгений\\Desktop\\Новая папка\\уник\\трпо\\SQLiteStudio\\CalculatorHistory.db"))
			{
				connection.Open();
				var result = connection.Query<DatabaseItem>("select * from History");
				foreach (var VARIABLE in result)
				{
					history.Add(VARIABLE.Expression);
				}
			}
			return history;
		}
	}
}
