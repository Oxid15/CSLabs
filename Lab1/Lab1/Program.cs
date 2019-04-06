using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;

namespace Lab1
{
	[XmlInclude(typeof(EmployeeFixedFee))]
	[XmlInclude(typeof(EmployeeHourlyFee))]
	[DataContract]
	public abstract class Employee
	{
		[DataMember]
		public int ID { get; set; }
		[DataMember]
		public string firstName { get; set; }
		[DataMember]
		public string secondName { get; set; }
		[DataMember]
		public string middleName { get; set; }
		[DataMember]
		public decimal fee { get; set; }
		[DataMember]
		public decimal rate { get; set; }
		[DataMember]
		public decimal bounty { get; set; }
		[DataMember]
		public DateTime birthday;

		public Employee()
		{
			ID = 0;
			firstName = null;
			secondName = null;
			middleName = null;
			fee = 0;
			rate = 0;
			bounty = 0;
			birthday = new DateTime(2000, 1, 1);
		}

		public Employee
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
			birthday = _birthday;
		}

		public abstract void avgFeeCompute();

		public static string[] parseName(string name)
		{
			return name.Split(' ');
		}

		public static string[] parseBDay(string input)
		{
			return input.Split('.');
		}
	}

	[DataContract]
	public class EmployeeFixedFee : Employee
	{
		public EmployeeFixedFee() { return; }

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

	[DataContract]
	public class EmployeeHourlyFee : Employee
	{
		public EmployeeHourlyFee() { return; }

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

	[KnownType(typeof(Employee))]
	[KnownType(typeof(EmployeeFixedFee))]
	[KnownType(typeof(EmployeeHourlyFee))]
	[DataContract]
	public class Organization
	{
		[DataMember]
		public List<Employee> list { get; set; }
		[DataMember]
		public int numOfEmpl { get; set; }
		[DataMember]
		public decimal avgFee { get; set; }

		public Organization()
		{
			list = new List<Employee>();
			numOfEmpl = list.Count;
		}

		public int add(Employee newEmployee)
		{
			int size = list.Count;
			for (int i = 0; i < size; i++)
			{
				if (list[i].ID == newEmployee.ID)
					return 1;
			}
			list.Add(newEmployee);
			numOfEmpl++;
			avgFeeCompute();
			return 0;
		}

		public void add(Employee[] newEmployee)
		{
			for (int i = 0; i < newEmployee.Length; i++)
				this.add(newEmployee[i]);
			numOfEmpl += newEmployee.Length;
		}

		public void del(int ID)
		{
			int size = list.Count;
			for (int i = 0; i < size; i++)
			{
				if (list[i].ID == ID)
				{
					list.Remove(list[i]);
					break;
				}
			}
			numOfEmpl--;
		}

		public void avgFeeCompute()
		{
			decimal sum = 0;
			for (int i = 0; i < numOfEmpl; i++)
			{
				sum += list[i].fee;
			}
			avgFee = sum / numOfEmpl;
		}

		public void sort()
		{
			FeeComparer c = new FeeComparer();
			list.Sort(c);
		}

		public void jsonOutput(string fileName)
		{
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Organization));
			StreamWriter w = new StreamWriter(fileName);
			try { serializer.WriteObject(w.BaseStream, this); }
			catch(SerializationException e)
			{
				Console.WriteLine("Output Error");
				throw e;
			}
			w.Close();
		}

		public void jsonInput(string fileName)
		{
			try
			{
				StreamReader reader = new StreamReader(fileName);
				DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Organization));
				Organization newOrg = (Organization)serializer.ReadObject(reader.BaseStream);
				int size = newOrg.numOfEmpl;
				for (int i = 0; i < size; i++)
					add(newOrg.list[i]);
				avgFeeCompute();
				reader.Close();
			}
			catch(SerializationException e)
			{
				Console.WriteLine("Input Error");
				throw e;
			}
		}

		public void xmlOutput(string fileName)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Organization));
			TextWriter writer = new StreamWriter(fileName);
			try { serializer.Serialize(writer, this); }
			catch(SerializationException e)
			{
				Console.WriteLine("Output Error");
				throw e;
			}
			writer.Close();
		}

		public void xmlInput(string fileName)
		{
			try
			{
				TextReader reader = new StreamReader(fileName);
				XmlSerializer serializer = new XmlSerializer(typeof(Organization));
				Organization newOrg = (Organization)serializer.Deserialize(reader);
				reader.Close();

				int size = newOrg.numOfEmpl;
				for (int i = 0; i < size; i++)
					add(newOrg.list[i]);
				avgFeeCompute();
			}
			catch (SerializationException e)
			{
				Console.WriteLine("Input Error");
				throw e;
			}
		}
	}

	abstract class Menu
	{
		public static void employeeInfoOut(Employee e)
		{
			Console.Write(e.ID);
			Console.Write(" ");
			Console.Write(e.secondName);
			Console.Write(" ");
			Console.Write(e.firstName);
			Console.Write(" ");
			Console.Write(e.middleName);
			Console.Write(" ");
			Console.Write(e.fee);
			Console.Write("\n");
		}

		private static int consoleGetInt(string message)
		{
			Console.WriteLine(message);
			string strID = Console.ReadLine();
			try
			{
				return int.Parse(strID);
			}
			catch (FormatException e)
			{
				Console.WriteLine("\nFormat Error\n");
				return 0;
				throw e;
			}
		}

		private static string[] consoleGetStrName()
		{
			Console.WriteLine("Enter the second first and middle names of the employee \nin following order\n");
			string strName = Console.ReadLine();
			try
			{
				return Employee.parseName(strName);
			}
			catch(FormatException e)
			{
				Console.WriteLine("\nFormat Error\n");
				throw e;
			}
		}

		private static decimal consoleGetDecimal(string message)
		{
			Console.WriteLine(message);
			string strRate = Console.ReadLine();
			try
			{
				return decimal.Parse(strRate);
			}
			catch(FormatException e)
			{
				Console.WriteLine("\nFormat Error\n");
				throw e;
			}
		}

		private static DateTime consoleGetDateTime(string message)
		{
			Console.WriteLine(message);
			string[] bDay = Employee.parseBDay(Console.ReadLine());
			try
			{
				return new DateTime(int.Parse(bDay[2]), int.Parse(bDay[1]), int.Parse(bDay[0]));
			}
			catch(FormatException e)
			{
				Console.WriteLine("\nFormat Error\n");
				throw e;
			}
		}

		private static bool consoleGetBoolean(string message, string trueValue, string falseValue)
		{
			Console.WriteLine(message);
			string inputType = Console.ReadLine();

			if (inputType == trueValue)
				return false;
			else if (inputType == falseValue)
				return true;

			return false;
		}

		public static void addEmployee(Organization org)
		{
			string[] name = consoleGetStrName();
			if (consoleGetBoolean("Enter the type of the employee\n \"fixed\" or\n \"hourly\"\n", "", ""))
			{
				EmployeeHourlyFee newEmployee =
					new EmployeeHourlyFee(
						consoleGetInt("Enter the ID of the new employee\n"),
						name[1],//first
						name[0],//second
						name[2],//middle
						consoleGetDecimal("Enter the rate of the employee"),
						consoleGetDecimal("Enter the bounty of the employee, if there isn't - enter \"0\""),
						consoleGetDateTime("Enter the birthday of the employee\nDD.MM.YYYY\n")
						);

				Console.WriteLine("Proceed the adding?\n\n");
				employeeInfoOut(newEmployee);
				if (consoleGetBoolean("Enter \"yes\" or \"no\"...", "yes", "no"))
				{
					org.add(newEmployee);
					return;
				}
				else return;
			}
			else
			{
				EmployeeFixedFee newEmployee = 
					new EmployeeFixedFee(
						consoleGetInt("Enter the ID of the new employee\n"),
						name[1],//first
						name[0],//second
						name[2],//middle
						consoleGetDecimal("Enter the rate of the employee"),
						consoleGetDecimal("Enter the bounty of the employee, if there isn't - enter \"0\""),
						consoleGetDateTime("Enter the birthday of the employee\nDD.MM.YYYY\n")
						);

				Console.WriteLine("Proceed the adding?\n\n");
				employeeInfoOut(newEmployee);
				if (consoleGetBoolean("Enter \"yes\" or \"no\"...", "yes", "no"))
				{
					org.add(newEmployee);
					return;
				}
				else return;
			}
		}

		public static void info(Organization org)
		{
			Console.Write("\n");
			for (int i = 0; i < org.numOfEmpl; i++)
			{
				employeeInfoOut(org.list[i]);
			}
			Console.Write("\n");
		}

		public static void topFive(Organization org)
		{
			Console.Write("\n");
			for (int i = 0; i < org.numOfEmpl && i < 5; i++)
			{
				employeeInfoOut(org.list[i]);
			}
			Console.Write("\n");
		}

		public static void bottomThree(Organization org)
		{
			List<Employee> lastThree = new List<Employee>();
			for (int i = (org.numOfEmpl - 1); i >= org.numOfEmpl - 3 && i >= 0; i--)
			{
				lastThree.Add(org.list[i]);
			}

			FeeComparer comparer = new FeeComparer();
			lastThree.Sort(comparer);

			Console.Write("\n");
			for (int i = 0; i < 3; i++)
			{
				employeeInfoOut(lastThree[i]);
			}
			Console.Write("\n");
		}

		public static void run(Organization org)
		{
			while (true)
			{
				Console.WriteLine("Menu:");
				Console.Write
					(
						"1 - Organization Info\n" +
						"2 - Top Five\n" +
						"3 - Last Three\n" +
						"4 - json output\n" +
						"5 - xml output\n" +
						"6 - json input\n" +
						"7 - xml input\n" +
						"8 - add employee\n" +
						"9 - clear console\n" +
						"0 - exit\n\n"
					);
				ConsoleKeyInfo key = Console.ReadKey();
				if (key.Key == ConsoleKey.D1)
				{
					info(org);
					Console.WriteLine("Press any key...\n");
					Console.ReadKey();
					continue;
				}
				if (key.Key == ConsoleKey.D2)
				{
					topFive(org);
					Console.WriteLine("Press any key...\n");
					Console.ReadKey();
					continue;
				}
				if (key.Key == ConsoleKey.D3)
				{
					bottomThree(org);
					Console.WriteLine("Press any key...\n");
					Console.ReadKey();
					continue;
				}
				if (key.Key == ConsoleKey.D4)
				{
					Console.WriteLine("Type name of .json file");
					string fileName = Console.ReadLine();

					org.jsonOutput(fileName);
					Console.WriteLine("Press any key...\n");
					Console.ReadKey();
					continue;
				}
				if (key.Key == ConsoleKey.D5)
				{
					Console.WriteLine("Type name of .xml file");
					string fileName = Console.ReadLine();

					org.xmlOutput(fileName);
					Console.WriteLine("Press any key...\n");
					Console.ReadKey();
					continue;
				}
				if (key.Key == ConsoleKey.D6)
				{
					Console.WriteLine("Type name of .json file");
					string fileName = Console.ReadLine();

					org.jsonInput(fileName);
					Console.WriteLine("Press any key...\n");
					Console.ReadKey();
					continue;
				}
				if (key.Key == ConsoleKey.D7)
				{
					Console.WriteLine("Type name of .xml file");
					string fileName = Console.ReadLine();

					org.xmlInput(fileName);
					Console.WriteLine("Press any key...\n");
					Console.ReadKey();
					continue;
				}
				if (key.Key == ConsoleKey.D8)
				{
					addEmployee(org);
					Console.WriteLine("Press any key...\n");
					Console.ReadKey();
					continue;
				}
				if (key.Key == ConsoleKey.D9)
				{
					Console.Clear();
					continue;
				}
				if (key.Key == ConsoleKey.D0)
					return;
			}
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Organization o = new Organization();
			Menu.run(o);
		}
		static void test()
		{
			EmployeeFixedFee e1 = new EmployeeFixedFee(0, "FirstName1", "SecondName1", "MiddleName1", 10, 0, new DateTime(2000, 1, 1));
			EmployeeFixedFee e2 = new EmployeeFixedFee(1, "FirstName2", "SecondName2", "MiddleName2", 30, 0, new DateTime(2000, 1, 1));
			EmployeeFixedFee e3 = new EmployeeFixedFee(2, "FirstName3", "SecondName3", "MiddleName3", 20, 0, new DateTime(2000, 1, 1));
			EmployeeFixedFee e4 = new EmployeeFixedFee(4, "FirstName4", "SecondName4", "MiddleName4", 5, 0, new DateTime(2000, 1, 1));
			EmployeeHourlyFee e5 = new EmployeeHourlyFee(5, "FirstName5", "SecondName5", "MiddleName5", 2, 20, new DateTime(2000, 1, 1));
			EmployeeFixedFee e6 = new EmployeeFixedFee(6, "FirstName6", "SecondName6", "MiddleName6", 10, 50, new DateTime(2000, 1, 1));
			EmployeeHourlyFee e7 = new EmployeeHourlyFee(7, "FirstName7", "SecondName7", "MiddleName7", 2, 0, new DateTime(2000, 1, 1));
			EmployeeFixedFee e8 = new EmployeeFixedFee(8, "FirstName8", "SecondName8", "MiddleName8", 8, 0, new DateTime(2000, 1, 1));

			Organization o = new Organization();

			o.add(e1);
			o.add(e2);
			o.add(e3);
			o.add(e4);
			o.add(e5);
			o.add(e6);
			o.add(e7);
			o.add(e8);

			o.sort();
			Menu.run(o);
		}
	}
}