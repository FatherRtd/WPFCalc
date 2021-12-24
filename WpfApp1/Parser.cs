using System.Data;

namespace WpfApp1
{
	class Parser
	{
		public static string Parse(string inputString)
		{
			DataTable dt = new DataTable();
			return dt.Compute(inputString, "").ToString();
		}
	}
}
