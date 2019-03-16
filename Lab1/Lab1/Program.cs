using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace Lab1
{
	abstract class Employee
	{
		public int ID { get; private set; }
		public string firstName { get; private set; }
		public string secondName { get; private set; }
		public string middleName { get; private set; }
		public decimal fee { get; protected set; }           //?
		public decimal rate { get; set; }
		public decimal bounty { get; set; }
		public readonly DateTime birthday;

		protected Employee()
		{
			ID = 0;
			firstName = null;
			secondName = null;
			middleName = null;
			fee = 0;
			rate = 0;
			bounty = 0;
			birthday = DateTime.MinValue;
		}

		protected Employee
			(
			int _ID,
			string _firstName,
			string _secondName,
			string _middleName,
			decimal _rate,
			decimal _bounty,
			DateTime _birthday
			)
		{
			ID = _ID;
			firstName = _firstName;
			secondName = _secondName;
			middleName = _middleName;
			rate = _rate;
			bounty = _bounty;
		}

		public abstract void avgFeeCompute();
	}

	class EmployeeFixedFee : Employee
	{
		public EmployeeFixedFee
			(
			int _ID,
			string _firstName,
			string _secondName,
			string _middleName,
			decimal _rate,
			decimal _bounty,
			DateTime _birthday
			) : base(_ID, _firstName, _secondName, _middleName, _rate, _bounty, _birthday)
		{
			fee = rate + bounty;
		}

		public override void avgFeeCompute() { fee = rate + bounty; }
	}

	class EmployeeHourlyFee : Employee
	{
		public EmployeeHourlyFee
			(
			int _ID,
			string _firstName,
			string _secondName,
			string _middleName,
			decimal _rate,
			decimal _bounty,
			DateTime _birthday
			) : base(_ID, _firstName, _secondName, _middleName, _rate, _bounty, _birthday)
		{
			fee = (decimal)20.8 * 8 * rate + bounty;
		}

		public override void avgFeeCompute()
		{
			fee = (decimal)20.8 * 8 * rate + bounty;
		}
	}

	class FeeComparer : IComparer<Employee>
	{
		public int Compare(Employee p, Employee q)
		{
			decimal pFee = p.fee;
			decimal qFee = q.fee;

			if (pFee != qFee)
				if (pFee > qFee)
					return -1;
				else
					return 1;
			else
			{
				return p.secondName.CompareTo(q.secondName);
			}
		}
	}

	class Organization
	{
		private List<Employee> list;
		public decimal avgFee { get; private set; }

		public Organization()
		{
			list = new List<Employee>();
		}

		public void add(Employee newEmployee)
		{
			list.Add(newEmployee);
			avgFeeCompute();
		}

		public void add(Employee[] newEmployee)
		{
			for (int i = 0; i < newEmployee.Length; i++)
			{
				list.Add(newEmployee[i]);
			}
			avgFeeCompute();
		}

		public void avgFeeCompute()
		{
			decimal sum = 0;
			for (int i = 0; i < list.Count; i++)
			{
				sum += list[i].fee;
			}

			avgFee = sum / list.Count;
		}

		public void sort()
		{
			FeeComparer c = new FeeComparer();
			list.Sort(c);
		}

		//void fileInput()
		//{

		//}

		//void fileOutput()
		//{

		//}
	}

	class Program
	{
		static void Main(string[] args)
		{
			EmployeeFixedFee e1 = new EmployeeFixedFee(0, null, null, null, 10, 0, DateTime.MinValue);
			EmployeeFixedFee e2 = new EmployeeFixedFee(1, null, null, null, 30, 0, DateTime.MinValue);
			EmployeeFixedFee e3 = new EmployeeFixedFee(2, null, "bbbbb", null, 20, 0, DateTime.MinValue);
			EmployeeFixedFee e4 = new EmployeeFixedFee(2, null, "aaaaa", null, 20, 0, DateTime.MinValue);

			Organization o = new Organization();
			o.add(e1);
			o.add(e2);
			o.add(e3);
			o.add(e4);

			o.sort();
			o.avgFeeCompute();
			Console.ReadKey();
		}

		//static void menu()
		//{

		//}
	}
}