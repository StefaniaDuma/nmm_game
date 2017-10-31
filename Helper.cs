using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Threading;

namespace nmm_game
{
	class Helper
	{
		public static int[,] initialise_game()
		{
			// 0 is free cell, 8 is cell where no ball can be placed
			int[,] a = new int[9, 9] {{8,8,8,8,8,8,8,8,8}, 
									  {8,0,8,8,0,8,8,0,8}, 
									  {8,8,0,8,0,8,0,8,8}, 
									  {8,8,8,0,0,0,8,8,8},
									  {8,0,0,0,8,0,0,0,8}, 
									  {8,8,8,0,0,0,8,8,8},
									  {8,8,0,8,0,8,0,8,8},
									  {8,0,8,8,0,8,8,0,8}, 
									  {8,8,8,8,8,8,8,8,8}};
			return a;
		}
		public static int toIJ(int i, int j)
		{
			int index = 1;
			if (i == 4)
				if (j < 4) index = 9 + j; else index = 8 + j; 
			else
				if (i == 1) index = j / 2;
				else if (i == 2) index = i + 1 + j / 2;
				else if (i == 3) index = i + 1 + j;
				else if (i == 5) index = 13 + j;
				else if (i == 6) index = i * 3 + j / 2;
				else index = i * 3 + j / 3 + 1; 
			if (index == 0) index = 1;
			return index;
		}

		public static  void SetLanguageDictionary(Window w, string lang)
		{
			ResourceDictionary dict = new ResourceDictionary();
			switch (lang)
			{
				case "en":
					dict.Source = new Uri("..\\Resources\\Dictionary.xaml",
								  UriKind.Relative);
					break;
				case "de-DE":
					dict.Source = new Uri("..\\Resources\\Dictionary.de-DE.xaml",
									   UriKind.Relative);
					break;
				case "fr-FR":
					dict.Source = new Uri("..\\Resources\\Dictionary.fr-FR.xaml",
									   UriKind.Relative);
					break;
				default:
					dict.Source = new Uri("..\\Resources\\Dictionary.xaml",
									  UriKind.Relative);
					break;
			}
			w.Resources.MergedDictionaries.Add(dict);
		} 

		public static void printBoard(int[,] a)
		{
			Console.Out.WriteLine("The board is:");
			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
					Console.Out.Write(a[i, j] + " ");
				Console.Out.WriteLine();
			}
		}

	}
}
