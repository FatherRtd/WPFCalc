using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace WpfApp1
{
	class MainViewModel : INotifyPropertyChanged
	{
		public ICommand GetButtonArgument { get; }
		public ICommand Calculate { get; }
		public ICommand Delete { get; }

		private string _expression;

		public string Expression
		{
			get => _expression;
			set
			{
				_expression = value;
				OnPropertyChanged(nameof(Expression));
			}
		}

		public MainViewModel()
		{
			GetButtonArgument = new RelayCommand<string>(x => Expression += x);
			Calculate = new RelayCommand(() => Expression = Expression);
			Delete = new RelayCommand(() => Expression = Expression!="" ? Expression.Remove(Expression.Length-1,1) : "");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
