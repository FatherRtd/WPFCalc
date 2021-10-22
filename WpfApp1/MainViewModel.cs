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
		public ICommand MyCommand { get; }

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
			MyCommand = new RelayCommand<string>(s => Expression+="123");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
