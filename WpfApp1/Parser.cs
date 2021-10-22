using System;
using System.Collections.Generic;
using System.Text;

namespace WpfApp1
{
	class Parser
	{
		public string OutputString { get; set; }

		public string ParseExpression(string inputString)
		{
			inputString.Remove(0, 1);

			return OutputString;
		}

	}
}
