using System;
using System.IO;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Globalization;

namespace PLS_check
{
	class MainClass
	{

		public static void Main(string[] args)
		{
			Console.WriteLine(DateTime.Now);
			var currentDirectory = System.IO.Directory.GetCurrentDirectory();
			//string currentDirectory = "D:\\pls";

			Console.WriteLine(currentDirectory);


			string[] filelist = Directory.GetFiles(@currentDirectory, "*.ply");
			if (filelist.Length == 0)
			{
				Console.WriteLine("NO PLAYLISTS FOUND!!!");
				Console.ReadLine();

				return;
			}
			string RawRow;
			SortedSet<string> ply_rows = new SortedSet<string>();

			int i = 1;
			foreach (string filename in filelist)
			{
				FileStream ply_file = new FileStream(filename, FileMode.Open);
				StreamReader ply_reader = new StreamReader(ply_file, Encoding.GetEncoding("windows-1251"));



				while (ply_reader.EndOfStream != true)
				{
					RawRow = ply_reader.ReadLine();
					if (!RawRow.StartsWith("#"))
					{

						RawRow = RawRow.Remove(RawRow.IndexOf(";"));
						ply_rows.Add(RawRow);


					}
				}
				ply_reader.Close();
				Console.WriteLine(String.Format("№ {0}\t{1} - added", i++, filename));
			}
			Console.WriteLine(String.Format("total unique files = {0} ", ply_rows.Count));
			i = ply_rows.Count;
			foreach (string row in ply_rows)
			{
				if (File.Exists(row))
				{
					// ply_rows.Remove(row);
					i--;

				}

				else Console.WriteLine(String.Format("Missing - {0}", row));
			}

			Console.WriteLine(String.Format("total missign {0} files", i));
			ply_rows.Clear();

			string dateString;
			CultureInfo culture;
			DateTimeStyles styles;
			DateTime dateResult;
			int gooddaycount = 0;
			foreach (string rawname in filelist)
			{
				dateString = namedit(rawname);
				culture = CultureInfo.CurrentCulture;
				styles = DateTimeStyles.AssumeLocal;
				if (DateTime.TryParse(dateString, culture, styles, out dateResult))
				{
					// Console.WriteLine("{0} : {1} converted", dateResult.Date.Subtract(DateTime.Now.Date).Days, dateResult);

				}
				else
					Console.WriteLine("Wrong pls file name : {0}", dateString);
				if (gooddaycount == dateResult.Date.Subtract(DateTime.Now.Date).Days) gooddaycount++;

			}

			Console.WriteLine("sequental pls count starting from tomorrow : {0}", gooddaycount);
			//string text="0";
			Console.ReadLine();

		}



		public static string namedit(string plname)
		{
			//plname = plname.Remove(0, 7);
			plname = Path.GetFileName(plname);
			plname = plname.Remove(4, 1);
			plname = plname.Insert(4, ".");
			plname = plname.Remove(7, 1);
			plname = plname.Insert(7, ".");
			plname = plname.Remove(10, 1);
			plname = plname.Insert(10, " ");
			plname = plname.Remove(13, 1);
			plname = plname.Insert(13, ":");
			plname = plname.Remove(16, 1);
			plname = plname.Insert(16, ":");
			plname = plname.Remove(plname.IndexOf(".ply"));
			//Console.WriteLine("After changes inside :{0}",plname);
			return plname;
		}
	}
}
