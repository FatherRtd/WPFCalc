using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace WpfApp1
{
	class MainViewModel : INotifyPropertyChanged
	{
		public ICommand GetButtonArgument { get; }
		public ICommand Calculate { get; }
		public ICommand Delete { get; }
		public ICommand Clear { get; }
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
			GetButtonArgument = new RelayCommand<string>(x => Expression += x);
			History = new ObservableCollection<string>();

			Calculate = new RelayCommand<string>(x =>
			{
				string str = Expression;
				try
				{
					Expression = Calculator.Parse(Expression.Replace(',','.'));
					str += "=" + Expression;
					History.Add(str);
					Calculator.SaveHistoryInDatabase(new DatabaseItem(str));
					Calculator.SaveHistoryInFile(DateTime.Now.ToString("d") + ", " + str);
				}
				catch
				{
					Expression = "Ошибка в выражении";
					IsError = true;
				}
			});

			Delete = new RelayCommand(() =>
			{
				if(IsError || String.IsNullOrEmpty(Expression))
					return;
				Expression = Expression.Remove(Expression.Length-1);
			});

			Clear = new RelayCommand(() =>
			{
				Expression = "";
				IsError = false;
			});

			ShowHistory = new RelayCommand(() => IsShowHistory = !IsShowHistory);


			foreach (var VARIABLE in Calculator.LoadHistoryFromDatabase())
			{
				History.Add(VARIABLE);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
