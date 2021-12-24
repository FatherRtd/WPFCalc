using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Dapper;
using GalaSoft.MvvmLight.Command;

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

	class MainViewModel : INotifyPropertyChanged
	{
		public ICommand GetButtonArgument { get; }
		public ICommand Calculate { get; }
		public ICommand Delete { get; }
		public ICommand ShowHistory { get; }

		public ObservableCollection<string> History { get; set; }

		private string _expression;
		private bool _showHistory;
		private bool _isError;

		public string Expression
		{
			get => _expression;
			set
			{
				_expression = value;
				OnPropertyChanged(nameof(Expression));
			}
		}

		public bool IsShowHistory
		{
			get => _showHistory;
			set
			{
				_showHistory = value;
				OnPropertyChanged(nameof(IsShowHistory));
			}
		}

		public bool IsError
		{
			get { return _isError; }
			set
			{
				_isError = value;
				OnPropertyChanged(nameof(IsError));
			}
		}


		public MainViewModel()
		{
			IsShowHistory = false;
			GetButtonArgument = new RelayCommand<string>(AddArgument);//x => Expression += x
			History = new ObservableCollection<string>();

			Calculate = new RelayCommand<string>(x =>
			{
				string str = Expression;
				try
				{
					Expression = Parser.Parse(Expression.Replace(',','.'));
				}
				catch
				{
					Expression = "Ошибка в выражении";
					IsError = true;
				}
				str += "=" + Expression;
				History.Add(str);
				SaveHistoryInDatabase(new DatabaseItem(str));
				SaveHistoryInFile(DateTime.Now.ToString("d") + ", " + str);
			});

			Delete = new RelayCommand(() =>
			{
				Expression = "";
				IsError = false;
			});

			ShowHistory = new RelayCommand(() => IsShowHistory = !IsShowHistory);


			LoadHistoryFromDatabase();
		}

		private void AddArgument(string arg)
		{
			Expression += arg;
		}

		private void SaveHistoryInFile(string str)
		{
			var path = @"C:\Users\Евгений\Desktop\Новая папка\уник\трпо\Калькулятор\History.txt";
			File.AppendAllText(path, str+'\n');
		}

		private void SaveHistoryInDatabase(DatabaseItem dbItem)
		{
			using (var connection = new System.Data.SQLite.SQLiteConnection("Data Source=C:\\Users\\Евгений\\Desktop\\Новая папка\\уник\\трпо\\SQLiteStudio\\CalculatorHistory.db"))
			{
				connection.Open();
				var result = connection.Query<DatabaseItem>($"insert into History (Expression) values ('{dbItem.Expression}')");
			}
		}

		private void LoadHistoryFromDatabase()
		{
			using (var connection = new System.Data.SQLite.SQLiteConnection("Data Source=C:\\Users\\Евгений\\Desktop\\Новая папка\\уник\\трпо\\SQLiteStudio\\CalculatorHistory.db"))
			{
				connection.Open();
				var result = connection.Query<DatabaseItem>("select * from History");
				foreach (var VARIABLE in result)
				{
					History.Add(VARIABLE.Expression);
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
