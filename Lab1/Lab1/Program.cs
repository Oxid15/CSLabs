using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace Lab1
{
	class Employee
	{
		protected int ID;
		protected readonly DateTime birthday;                          // ? protected

		protected Employee()
		{
			ID = 0;
			birthday = DateTime.MinValue;
		}

		public virtual decimal avgFeeCompute() { return 0; }
	}

	class EmployeeFixedFee:Employee
	{
		private decimal fee;

		public EmployeeFixedFee()
		{
			fee = 0;
		}

		public EmployeeFixedFee(decimal _fee) { fee = _fee; }

		public override decimal avgFeeCompute() { return fee; }
	}

	class EmployeeHourlyFee:Employee
	{
		private decimal rate;

		public override decimal avgFeeCompute() { return (decimal)20.8 * 8 * rate; }
	}

	class Organization
	{
		private List<Employee> list;
	}

	class Program
	{
		static void Main(string[] args)
		{
			EmployeeFixedFee e = new EmployeeFixedFee(20);
			Console.WriteLine(e.avgFeeCompute());
			Console.ReadKey();
		}
	}
}
