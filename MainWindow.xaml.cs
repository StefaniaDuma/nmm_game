﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Runtime.InteropServices;

namespace nmm_game
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// the stones are visually represented by Ellipses
		Ellipse[] ellipseV = new Ellipse[25];
		Random rand = new Random();
		int[] tops = new int[25];
		int[] lefts = new int[25] { 55, 55, 255, 455, 121, 255, 389, 189, 255, 319, 55, 121, 189, 319, 389, 455, 189, 255, 319, 121, 255, 389, 55, 255, 455 };
		private Ellipse selectedNodeEllipse;
		private Ellipse previousEllipse = null;
		int ballPlayer1 = 9, ballPlayer2 = 9, nrBlueMills = 0, removedRedStone = 0;
		int[,] a;
		int countMoves = 0;
		bool firstMoveBlue = true;
		int oldBlueMills = 0;
		static string blueColor = "#FFAFA9E1", yellowColor = "#FFD9C768";
		bool secondPhase = false, secondClick = false, thirdPhaseBlue = false, thirdPhaseRed = false;
		SolidColorBrush fillEmptySpot = (SolidColorBrush)new BrushConverter().ConvertFromString(yellowColor);
		bool redMill = false, removedBlueStone = false, blueMill = false, breakMill = false;
		List<Mill> bMills = new List<Mill>();
		List<Mill> rMills = new List<Mill>();
		string lang = "en";
		int newBlueMills = 0;
		// flag icons are freely available at www.freeflagicons.com

		public MainWindow()
		{
			InitializeComponent();
			this.SetResourceReference(Window.TitleProperty, "windowTitle");
			canvas1.Visibility = System.Windows.Visibility.Visible;
			warning.SetResourceReference(TextBox.TextProperty, "intro");
			warning.Visibility = System.Windows.Visibility.Visible;
			winnerLabel.Visibility = System.Windows.Visibility.Hidden;
			playAgainButton.Visibility = System.Windows.Visibility.Hidden;
			playAgainButton.SetResourceReference(Button.ContentProperty, "playAgain");  
			cancelButton.Visibility = System.Windows.Visibility.Hidden;
			cancelButton.SetResourceReference(Button.ContentProperty, "cancel");
			langLabel.SetResourceReference(Label.ContentProperty, "language");  
			a = Helper.initialise_game();
			draw_empty_board();
			Helper.SetLanguageDictionary(this, lang);

		}

		private void draw_empty_board()
		{
			// board size = (400, 400)
			int step = 66, // =400/6
				top = 55,
				left = 55,
				sizeEmptySpot = 15;
			for (int i = 0; i < 25; i++)
			{
				if (i < 4) tops[i] = top;
				else
					if (i > 3 && i < 7) tops[i] = top + step;
					else
						if (i > 6 && i < 10) tops[i] = top + step * 2;
						else
							if (i > 9 && i < 16) tops[i] = top + step * 3;
							else
								if (i > 15 && i < 19) tops[i] = top + step * 4;
								else
									if (i > 18 && i < 22) tops[i] = top + step * 5;
									else
										tops[i] = top + step * 6;

			}
			// add 24 places where pieces (elipses) can be placed on

			for (int i = 1; i < 25; i++)
			{
				ellipseV[i] = new Ellipse();
				ellipseV[i].Fill = fillEmptySpot;
				ellipseV[i].Height = ellipseV[i].Width = sizeEmptySpot;
				ellipseV[i].MouseDown += ellipse_MouseDown;
				ellipseV[i].Tag = i;
				canvas1.Children.Add(ellipseV[i]);
				Canvas.SetTop(ellipseV[i], tops[i]);
				Canvas.SetLeft(ellipseV[i], lefts[i]);
			}
		}


		private void ellipse_MouseDown(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
			selectedNodeEllipse = (Ellipse)sender;
			Cell currentCell = new Cell((int)selectedNodeEllipse.Tag);
			checkMill(2, selectedNodeEllipse);
			// if blue reached phase 3 having only 3 blue stones left, move to form mills or block red mills
			if ((Cell.count(a, 2) == 3) && (secondPhase))
			{
				thirdPhaseBlue = true;
			}
			if ((Cell.count(a, 1) == 3) && (secondPhase))
			{
				thirdPhaseRed = true;
			}
			// blue player lost
			if (secondPhase && ((Cell.count(a, 2) < 3) || (!Mill.playerCanMove(a, 2))))
			{
				gameOver("redWon");
			}
			else
				// red player lost
				if (secondPhase && ((Cell.count(a, 1) < 3) || (!Mill.playerCanMove(a, 1))))
				{
					gameOver("blueWon");
				}
				else
					// if the game reached 70 moves (sum of both players moves) and no player won, it is a draw
					if (countMoves == 70)
					{
						gameOver("draw");
					}
					else
			// if red formed mill and user clicked on blue stone then remove the blue stone
			if (redMill && (a[currentCell.i, currentCell.j] == 2))
			{			
				// remove blue stone that is not inside a mill or if all the blue stones are inside mills, allow to remove from mill
				if ((!blueMill) || (blueMill && Mill.StonesOnlyInMills(a,2)))
				{
					drawEllipse(selectedNodeEllipse, 2, yellowColor, 15, false);
					redMill = false;
					removedBlueStone = true;
					moveBlue();
					a[currentCell.i, currentCell.j] = 0;
					warning.SetResourceReference(TextBox.TextProperty, "blueStoneRemoved");
					warning.Visibility = System.Windows.Visibility.Visible;
				   
				}
				else
				{
					warning.SetResourceReference(TextBox.TextProperty, "stoneNotInMill");     
					warning.Visibility = System.Windows.Visibility.Visible;
				}
			}
			else
				

				if (!redMill)
				{
					removedBlueStone = false;
					if (secondPhase && (secondClick == false))
					{
						if (a[currentCell.i, currentCell.j] == 1)
						{
							secondClick = true;
							previousEllipse = selectedNodeEllipse;
							selectedNodeEllipse.Stroke = Brushes.Yellow;
							selectedNodeEllipse.StrokeThickness = 2;
						}
					}
					else
						if (secondPhase && secondClick)
						{
							Cell previousCell = new Cell((int)previousEllipse.Tag);
							if (currentCell.Equals(previousCell))
							{
								selectedNodeEllipse.Stroke = Brushes.Transparent;
								warning.Text = "";
								warning.Visibility = System.Windows.Visibility.Hidden;
								secondClick = false;
							}
							else
								if ((a[currentCell.i, currentCell.j] == 0 && Cell.neighbour(a,previousCell, currentCell) && (!thirdPhaseRed))
									|| (a[currentCell.i, currentCell.j] == 0 && thirdPhaseRed))
								{
									a[previousCell.i, previousCell.j] = 0;
									warning.Text = "";
									warning.Visibility = System.Windows.Visibility.Hidden;
									drawEllipse(previousEllipse, 1, yellowColor, 15, false);
									moveRed(selectedNodeEllipse);
									secondClick = false;									
								}
								else 
									if (a[currentCell.i, currentCell.j] != 0 || (!Cell.neighbour(a,previousCell, currentCell)) 
										&& (!currentCell.Equals(previousCell)) && (!thirdPhaseRed))
								{
									warning.SetResourceReference(TextBox.TextProperty, "invalidMove");
									warning.Visibility = System.Windows.Visibility.Visible;
								   // previousEllipse = selectedNodeEllipse;
								}

						}
					if (!secondPhase)
					{
						moveRed(selectedNodeEllipse);
					}
				
				}

		}

		private void gameOver(string winner)
		{
			winnerLabel.Visibility = System.Windows.Visibility.Visible;
			winnerLabel.SetResourceReference(Label.ContentProperty, winner);
			foreach (Ellipse el in ellipseV)
			{
				canvas1.Children.Remove(el);
			}
			playAgainButton.Visibility = System.Windows.Visibility.Visible;
			cancelButton.Visibility = System.Windows.Visibility.Visible;
			warning.Text = "";
			warning.Visibility = System.Windows.Visibility.Hidden;
		}

		
		private void moveRed(Ellipse e)
		{
			if (Cell.count(a, 1) == 3)
			{
				warning.SetResourceReference(TextBox.TextProperty, "red3Phase");
				warning.Visibility = System.Windows.Visibility.Visible;
			}
			Cell c = new Cell((int)e.Tag);
			if (a[c.i, c.j] != 2)
			{
				if (ballPlayer1 == 1)
					secondPhase = true;
				if (!secondPhase)
				{
					ballPlayer1 -= 1;
				}
				drawEllipse(e, 1, "#FFD86E59", 35, false);
				countMoves++;
				a[c.i, c.j] = 1;
				warning.Text = "";
				warning.Visibility = System.Windows.Visibility.Hidden;
				checkMill(1, e);
				if (!redMill || (thirdPhaseRed))
				{
					moveBlue();
					countMoves++;
				}
			}
					
			 
			
		}


		private void moveBlue()
		{		
			// the blue oponent moves too fast, so a delay is added in order for the human to spot clearly what move has been made
			var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
			timer.Start();
			timer.Tick += (sender, args) =>
			{
				timer.Stop();
				warning.Text = "";
				warning.Visibility = System.Windows.Visibility.Hidden;
				int move = 0;
				// blue opponent's first move is random
				int i = 1, j = 2, index = 1;
				if (firstMoveBlue)
				{
					while (a[i, j] != 0)
					{
						index = rand.Next(1, 25);
						Cell c = new Cell(index);
						i = c.i;
						j = c.j;
					}
					a[i, j] = 2;
					firstMoveBlue = false;
					drawEllipse(ellipseV[index], 2, blueColor, 35, false);
					move = index;
					ballPlayer2 -= 1;
				}
				else
					if (thirdPhaseBlue)
					{
						// check if red will soon create a mill
						List<Cell> incompleteRedMill = Mill.checkDangerMill(a, 1);
						// check if blue will create soon a mill 
						List<Cell> incompleteBlueMill = Mill.checkDangerMill(a, 2);
						List<Mill> blueMills = Mill.getMills(a, 2);
						Cell moved = Mill.getBlueStoneThirdPhase(a);
						if (incompleteBlueMill.Count != 0)
						{
							a[moved.i, moved.j] = 0;
							drawEllipse(ellipseV[Helper.toIJ(moved.i, moved.j)], 2, yellowColor, 15, false);
							int ii = incompleteBlueMill[0].i;
							int jj = incompleteBlueMill[0].j;
							drawEllipse(ellipseV[Helper.toIJ(ii,jj)], 2, blueColor, 35, false);
							a[ii, jj] = 2;
						}
						else
							if (incompleteRedMill.Count != 0)
							{
								a[moved.i, moved.j] = 0;
								drawEllipse(ellipseV[Helper.toIJ(moved.i, moved.j)], 2, yellowColor, 15, false);
								int ii = incompleteRedMill[0].i;
								int jj = incompleteRedMill[0].j;
								drawEllipse(ellipseV[Helper.toIJ(ii, jj)], 2, blueColor, 35, false);
								a[ii, jj] = 2;
							}
							else
								if (blueMills.Count != 0)
								{
									Cell x = new Cell(blueMills[0].start);
									for (int k = 1; k < 25; k++)
									{
										Cell n = new Cell(k);
										if (a[n.i, n.j] == 0)
										{
											a[n.i, n.j] = 2;
											a[x.i, x.j] = 0;
											drawEllipse(ellipseV[Helper.toIJ(x.i, x.j)], 2, yellowColor, 15, false);
											drawEllipse(ellipseV[Helper.toIJ(n.i, n.j)], 2, blueColor, 35, false);
										}
									}
								}
								else
								{
									int ct = 0;
									Cell first = new Cell(0);
									Cell second = new Cell(0);
									for (int k = 1; k < 25; k++)
									{
										Cell x = new Cell(k);
										if ((a[x.i, x.j] == 2) && (ct==0))
										{
											ct++;
											first = x;
										} 
										else
											if ((a[x.i, x.j] == 2) && (ct == 1))
											{
												second = x;
												a[first.i, first.j] = 0;
												drawEllipse(ellipseV[Helper.toIJ(first.i, first.j)], 2, yellowColor, 15, false);
												for (int n =1; n < 25; n++)
												{
													Cell n_c = new Cell(n);
													if (Cell.neighbour(a, n_c, second) && (a[n_c.i, n_c.j] == 0))
													{
														a[n_c.i, n_c.j] = 2;
														drawEllipse(ellipseV[Helper.toIJ(n_c.i, n_c.j)], 2, blueColor, 35, false);
														break;
													}
												}
											}
									}
								}
					} else
					// if the game is in phase 1 check whether red will soon form a mill (two neighbour red stones)
					// and whether blue will form soon a mill;
					// completing blue mill has preference over blocking formation of red mill because after 
					// completing a blue mill, it will remove a red stone that is inside a dangerous/ incomplete red mill)
					if (ballPlayer2 > 0)
					{
						// check if red will soon create a mill
						List<Cell> incompleteRedMill = Mill.checkDangerMill(a, 1);
						// check if blue will create soon a mill 
						List<Cell> incompleteBlueMill = Mill.checkDangerMill(a, 2);
						// try to build blue mill
						Cell aimBlueMill = Mill.goMill(a);    
						// if no danger of red mill was detected and blue will not create soon a mill
						if (incompleteRedMill.Count == 0 && incompleteBlueMill.Count == 0)
						{
							if (aimBlueMill != null)
							{
								a[aimBlueMill.i, aimBlueMill.j] = 2;
								drawEllipse(ellipseV[Helper.toIJ(aimBlueMill.i, aimBlueMill.j)], 2, blueColor, 35, false);
								move = Helper.toIJ(aimBlueMill.i, aimBlueMill.j);
							}
							else
							{
								i = 1; j = 2; index = 1;
								while (a[i, j] != 0)
								{
									index = rand.Next(1, 25);
									Cell c = new Cell(index);
									i = c.i;
									j = c.j;
								}
								a[i, j] = 2;
								drawEllipse(ellipseV[index], 2, blueColor, 35, false);
								move = index;
							}
								
						}
						else
							if ((incompleteRedMill.Count != 0) && (incompleteBlueMill.Count == 0))
							{
								// danger for red to create mill and blue will not create soon a mill
								// => move blue stone to block red mill
								a[incompleteRedMill[0].i, incompleteRedMill[0].j] = 2;
								drawEllipse(ellipseV[Helper.toIJ(incompleteRedMill[0].i, incompleteRedMill[0].j)], 2, blueColor, 35, false);
								move = Helper.toIJ(incompleteRedMill[0].i, incompleteRedMill[0].j);
								checkMill(2, ellipseV[Helper.toIJ(incompleteRedMill[0].i, incompleteRedMill[0].j)]);
							}
							else
								if ((incompleteRedMill.Count != 0) && (incompleteBlueMill.Count != 0))
								{
									// danger for red to create mill and blue will create soon a mill
									// => complete blue mill
									a[incompleteBlueMill[0].i, incompleteBlueMill[0].j] = 2;
									drawEllipse(ellipseV[Helper.toIJ(incompleteBlueMill[0].i, incompleteBlueMill[0].j)], 2, blueColor, 35, false);
									move = Helper.toIJ(incompleteBlueMill[0].i, incompleteBlueMill[0].j);
								}
								else
								{
									// no danger for red to create mill and blue will create soon a mill
									a[incompleteBlueMill[0].i, incompleteBlueMill[0].j] = 2;
									drawEllipse(ellipseV[Helper.toIJ(incompleteBlueMill[0].i, incompleteBlueMill[0].j)], 2, blueColor, 35, false);
									move = Helper.toIJ(incompleteBlueMill[0].i, incompleteBlueMill[0].j);
								}
						ballPlayer2 -= 1;
					}
					else
						if (secondPhase)
						{
							List<Cell> blueDangerousMills = Mill.checkDangerMill(a, 2);
							List<Cell> redDangerousMills = Mill.checkDangerMill(a, 1);
							List<Mill> blueMills = Mill.getMills(a, 2);
							bool blueMoved = false;
							// search through all the blue mills to get a free neighbour position where to move
							// one of the blue stones from inside the mill (strategy for removing red stones => next move will close back the mill)
							if (!blueMoved && (!breakMill))
							{
								HashSet<int> blueStones = new HashSet<int>();
								foreach (Mill m in blueMills)
								{
									blueStones.Add(m.start);
									blueStones.Add(m.middle);
									blueStones.Add(m.stop);
								}
								if (blueStones.Count != 0)
								{
									foreach (int stone in blueStones)
									{
										if (!blueMoved)
										{
											for (int k = 1; k < 25; k++)
											{
												Cell n_k = new Cell(k);
												Cell n_s = new Cell(stone);
												if (Cell.neighbour(a, n_k, n_s) && (a[n_k.i, n_k.j] == 0))
												{
													drawEllipse(ellipseV[Helper.toIJ(n_k.i, n_k.j)], 2, blueColor, 35, false);
													move = Helper.toIJ(n_k.i, n_k.j);
													drawEllipse(ellipseV[Helper.toIJ(n_s.i, n_s.j)], 2, yellowColor, 15, false);
													a[n_k.i, n_k.j] = 2;
													a[n_s.i, n_s.j] = 0;
													blueMoved = true;
													breakMill = true;
													break;
												}
											}
										}
										else
											break;


									}
								}
							}
							// try to find an incomplete blue mill and if a blue stone is in the neighbourhood, move it to complete the mill
							if (!blueMoved && breakMill)
							{
								HashSet<int> StonesInMills = new HashSet<int>();
								foreach (Mill m in blueMills)
								{
									StonesInMills.Add(m.start);
									StonesInMills.Add(m.middle);
									StonesInMills.Add(m.stop);
								}

								foreach (Cell blue in blueDangerousMills)
								{
									for (int k = 1; k < 25; k++)
									{
										if (!blueMoved)
										{
											Cell n = new Cell(k);
											if (Cell.neighbour(a, n, blue) && (a[n.i, n.j] == 2) && (!StonesInMills.Contains(k)))
											{
												drawEllipse(ellipseV[Helper.toIJ(blue.i, blue.j)], 2, blueColor, 35, false);
												move = Helper.toIJ(blue.i, blue.j);
												drawEllipse(ellipseV[Helper.toIJ(n.i, n.j)], 2, yellowColor, 15, false);
												a[n.i, n.j] = 0;
												a[blue.i, blue.j] = 2;
												blueMoved = true;
												breakMill = false;
												break; // found a good position to move the blue stone, so stop searching
											}
										}
										else
											break; // break from the outer loop after finding a good postion to move

									}
								}
							}
							// if the precedent case didn't result in a position where to move the blue stone
							// then search through all incomplete red mills for a position that has in 
							// the neighbourhood a blue stone and if one is found, move blue stone to block forming a red mill
							if (!blueMoved)
								foreach (Cell red in redDangerousMills)
								{
									for (int k = 1; k < 25; k++)
									{
										if (!blueMoved)
										{
											Cell n = new Cell(k);
											if (Cell.neighbour(a, n, red) && (a[n.i, n.j] == 2))
											{
												drawEllipse(ellipseV[Helper.toIJ(red.i, red.j)], 2, blueColor, 35, false);
												move = Helper.toIJ(red.i, red.j);
												drawEllipse(ellipseV[Helper.toIJ(n.i, n.j)], 2, yellowColor, 15, false);
												a[n.i, n.j] = 0;
												a[red.i, red.j] = 2;
												blueMoved = true;
												break;
											}
										}
										else
											break;
									}
								}
						
							while (!blueMoved)
							{
								index = rand.Next(1, 25); 
								 
								for (int k = 1; k < 25; k++)
								{
									Cell n_k = new Cell(k);
									Cell c = new Cell(index);
									if ((a[c.i, c.j] == 2) && (Cell.neighbour(a, n_k, c)) && (a[n_k.i, n_k.j] == 0))
									{
										a[n_k.i, n_k.j] = 2;
										a[c.i, c.j] = 0;
										drawEllipse(ellipseV[k], 2, blueColor, 35, false);
										drawEllipse(ellipseV[index], 2, yellowColor, 15, false);
										move = index;
										blueMoved = true;
									}
								}
								   
								
							}
											 
						}				
						   
				Helper.printBoard(a);
				blueMill = false;
				checkMill(2, ellipseV[move]);
				oldBlueMills = bMills.Count;
				bMills = Mill.getMills(a, 2);
				newBlueMills = bMills.Count;
				if (oldBlueMills - newBlueMills == 1)
					removedRedStone--;  // blue moved so that one mill is broken 
				if (newBlueMills != removedRedStone)
				{
					Cell red = Mill.removeRedStone(a);
					warning.SetResourceReference(TextBox.TextProperty, "redStoneRemoved");
					warning.Visibility = System.Windows.Visibility.Visible;
					a[red.i, red.j] = 0;
					Helper.printBoard(a);
					drawEllipse(ellipseV[Helper.toIJ(red.i, red.j)], 1, yellowColor, 15, false);
					removedRedStone++;
				}
				
			};
			
					
		}

		private void checkMill(int player, [Optional] Ellipse e)
		{
			int ellipse = (int)e.Tag;
			blueMill = false;
			if (!removedBlueStone)
			{
				for (int ii = 1; ii < 25; ii++)
				{
					for (int jj = 1; jj < 25; jj++)
					{
						for (int k = 1; k < 25; k++)
						{
							Cell x = new Cell(ii);
							Cell y = new Cell(jj);
							Cell z = new Cell(k);
							if (ii != jj && jj != k && ii != k && (a[x.i, x.j] == a[y.i, y.j])
								&& (a[x.i, x.j] == a[z.i, z.j]) && (a[x.i, x.j] == player)
								&& (Cell.neighbour(a, x, y)) && (Cell.neighbour(a, y, z))
								&& Cell.consecutive(x, y, z)
								&& ((ellipse == ii) || (ellipse == jj) || (ellipse == k)))
							{
								if (player == 1)
								{
									// red formed a mill; clicks on a blue ellipse (check its blue, not in a mill)
									// yes --> delete blue ellipse; not --> warning, wait til user click on good ellipse
									warning.SetResourceReference(TextBox.TextProperty, "stoneNotInMill");
									warning.Visibility = System.Windows.Visibility.Visible;
									redMill = true;
								}


								else if (player == 2)
								{
									blueMill = true;
									nrBlueMills++;
								}
							}
						}
					}

				}
			}

		}



		private void drawEllipse(Ellipse e, int player, string color, int size, bool stroke)
		{
			double newTop, newLeft;
			SolidColorBrush fillBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(color);
			e.Fill = fillBrush;
			e.Height = size;
			e.Width = size;
			if (!stroke) e.Stroke = null;
			if (size == 15)
			{
				newTop = Canvas.GetTop(e) + 10;
				newLeft = Canvas.GetLeft(e) + 10;
			}
			else
			{
				newTop = Canvas.GetTop(e) - 10;
				newLeft = Canvas.GetLeft(e) - 10;
			}

			Canvas.SetTop(e, newTop);
			Canvas.SetLeft(e, newLeft);

		}

		private void playAgainButton_Click(object sender, RoutedEventArgs e)
		{
			InitializeComponent();
			winnerLabel.Visibility = System.Windows.Visibility.Hidden;
			winnerLabel.Content = "";
			foreach (Ellipse el in ellipseV)
			{
				canvas1.Children.Remove(el);
			}
			a = Helper.initialise_game();
			ellipseV = new Ellipse[25];
			draw_empty_board();
			ballPlayer1 = 9; ballPlayer2 = 9; 
			nrBlueMills = 0; removedRedStone = 0;			
			firstMoveBlue = true;
			secondPhase = false;
			secondClick = false;
			redMill = false; removedBlueStone = false; blueMill = false;
			bMills = new List<Mill>();
			rMills = new List<Mill>();
			thirdPhaseBlue = false;
			thirdPhaseRed = false;
			oldBlueMills = 0;		
			playAgainButton.Visibility = System.Windows.Visibility.Hidden;
			cancelButton.Visibility = System.Windows.Visibility.Hidden;
			warning.SetResourceReference(TextBox.TextProperty, "intro");
			warning.Visibility = System.Windows.Visibility.Visible;
			newBlueMills = 0;
			countMoves = 0;
		}

		private void cancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void ukButton_Click(object sender, RoutedEventArgs e)
		{
			lang = "en";
			Helper.SetLanguageDictionary(this, lang);
		}

		private void deButton_Click(object sender, RoutedEventArgs e)
		{
			lang = "de-DE";
			Helper.SetLanguageDictionary(this, lang);
		}

		private void frButton_Click(object sender, RoutedEventArgs e)
		{
			lang = "fr-FR";
			Helper.SetLanguageDictionary(this, lang);
		}





		
	}


}