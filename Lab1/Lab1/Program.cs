using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace Lab1
{
	abstract class Employee
	{
		private int ID;
		private string firstName;
		private string secondName;
		private string middleName;
		protected decimal fee;                                    
		protected decimal rate;
		protected decimal bounty;
		private readonly DateTime birthday; 

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

	class EmployeeFixedFee:Employee
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

	class EmployeeHourlyFee:Employee
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
			) : base(_ID, _firstName, _secondName, _middleName,_rate,_bounty, _birthday) 
		{
			fee = (decimal)20.8 * 8 * rate + bounty;
		}

		public override void avgFeeCompute()
		{
			fee = (decimal)20.8 * 8 * rate + bounty;
		}
	}

	//class FeeComparer : IComparer<Employee>
	//{
	//	public int Compare(Employee p, Employee q)
	//	{
	//		decimal pFee = p.avgFeeCompute();
	//		decimal qFee = q.avgFeeCompute();

	//		if (pFee != qFee)
	//			if (pFee > qFee)
	//				return -1;
	//			else
	//				return 1;
	//		else
	//		{
	//			return 0;
	//		}
	//	}
	//}

	class Organization
	{
		private List<Employee> list;
		private decimal avgFee;
		public Organization()
		{
			list = new List<Employee>();
		}

		public void add(Employee newEmployee)
		{	
			list.Add(newEmployee);
		}

		public void add(Employee[] newEmployee)
		{
			for (int i = 0; i < newEmployee.Length;i++)
			{
				list.Add(newEmployee[i]);
			}
		}

		//void fileInput()
		//{

		//}

		public void sort()
		{
			//FeeComparer c = new FeeComparer();
			//list.Sort(c);
		}

		//void fileOutput()
		//{

		//}
	}

	class Program
	{
		static void Main(string[] args)
		{
			EmployeeFixedFee e1 = new EmployeeFixedFee(0, null, null, null, 10, 5, DateTime.MinValue);
			EmployeeFixedFee e2 = new EmployeeFixedFee(1, null, null, null, 30, 15, DateTime.MinValue);
			EmployeeFixedFee e3 = new EmployeeFixedFee(2, null, null, null, 20, 5, DateTime.MinValue);

			Organization o = new Organization();
			o.add(e1);
			o.add(e2);
			o.add(e3);

			Console.ReadKey();
		}
	}
}