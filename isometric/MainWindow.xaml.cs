using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Drawing;
using Color = System.Drawing.Color;
using System.IO;
using Image = System.Windows.Controls.Image;

namespace isometric
{

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private static Vector vWorldsize = new Vector(14, 10);
		//using a 2x upscaled image. same issue happens without upscaling
		private Vector vTileSize = new Vector(80, 40);
		private Vector vOrigin = new Vector(5, 1);
		private Vector vSelected = new Vector();
		private BitmapImage spritesB;
		private int[] pWorld;
		private Image[] images = new Image[Convert.ToInt32(vWorldsize.X*vWorldsize.Y)];

		public MainWindow()
		{

			spritesB = new BitmapImage(new Uri("pack://application:,,,/isometric_demo2.png"));
			pWorld = new int[Convert.ToInt32(vWorldsize.X * vWorldsize.Y)];
			Array.Clear(pWorld, 0, pWorld.Length);
			InitializeComponent();

			StartUI();
			CompositionTarget.Rendering += CompositionTarget_Rendering;


		}

		private void StartUI()
		{

			canvas.Children.Clear();
			int counter = 0;
			for (int y = 0; y < vWorldsize.Y; y++)
			{
				for (int x = 0; x < vWorldsize.X; x++)
				{
					Vector vWorld = ToScreen(x, y);
					Image image = new Image();
					switch (pWorld[y * Convert.ToInt32(vWorldsize.X) + x])
					{
						
						case 0:

							image = (DrawPartialSprite(Convert.ToInt32(vWorld.X), Convert.ToInt32(vWorld.Y), 1 * Convert.ToInt32(vTileSize.X), 0, Convert.ToInt32(vTileSize.X), Convert.ToInt32(vTileSize.Y)));

							break;
						case 1:
							// Visible Tile
							image = (DrawPartialSprite(Convert.ToInt32(vWorld.X), Convert.ToInt32(vWorld.Y), 2 * Convert.ToInt32(vTileSize.X), 0, Convert.ToInt32(vTileSize.X), Convert.ToInt32(vTileSize.Y)));
							break;
						case 2:
							// Tree
							image = (DrawPartialSprite(Convert.ToInt32(vWorld.X), Convert.ToInt32(vWorld.Y) - Convert.ToInt32(vTileSize.Y), 0 * Convert.ToInt32(vTileSize.X), 1 * Convert.ToInt32(vTileSize.Y), Convert.ToInt32(vTileSize.X), Convert.ToInt32(vTileSize.Y*2)));
							break;
						case 3:
							// Spooky Tree
							image = (DrawPartialSprite(Convert.ToInt32(vWorld.X), Convert.ToInt32(vWorld.Y) - Convert.ToInt32(vTileSize.Y), 1 * Convert.ToInt32(vTileSize.X), 1 * Convert.ToInt32(vTileSize.Y), Convert.ToInt32(vTileSize.X), Convert.ToInt32(vTileSize.Y * 2)));
							break;
						case 4:
							// Beach
							image = (DrawPartialSprite(Convert.ToInt32(vWorld.X), Convert.ToInt32(vWorld.Y) - Convert.ToInt32(vTileSize.Y), 2 * Convert.ToInt32(vTileSize.X), 1 * Convert.ToInt32(vTileSize.Y), Convert.ToInt32(vTileSize.X), Convert.ToInt32(vTileSize.Y * 2)));
							break;
						case 5:
							// Water
							image = (DrawPartialSprite(Convert.ToInt32(vWorld.X), Convert.ToInt32(vWorld.Y) - Convert.ToInt32(vTileSize.Y), 3 * Convert.ToInt32(vTileSize.X), 1 * Convert.ToInt32(vTileSize.Y), Convert.ToInt32(vTileSize.X), Convert.ToInt32(vTileSize.Y * 2)));
							break;
					}
					canvas.Children.Add(image);
					images[counter] = image;
					counter++;
				}
			}
		}
		private void UpdateUI(int id,Vector v)
		{
			UIElement[] uIElements = new UIElement[canvas.Children.Count];
			int x = Convert.ToInt32(v.X);
			int y = Convert.ToInt32(v.Y);
			Vector vWorld = ToScreen(x, y);
			Image image = new Image();
			switch (pWorld[id])
			{

				case 0:

					image = (DrawPartialSprite(Convert.ToInt32(vWorld.X), Convert.ToInt32(vWorld.Y), 1 * Convert.ToInt32(vTileSize.X), 0, Convert.ToInt32(vTileSize.X), Convert.ToInt32(vTileSize.Y)));

					break;
				case 1:
					// Visible Tile
					image = (DrawPartialSprite(Convert.ToInt32(vWorld.X), Convert.ToInt32(vWorld.Y), 2 * Convert.ToInt32(vTileSize.X), 0, Convert.ToInt32(vTileSize.X), Convert.ToInt32(vTileSize.Y)));
					break;
				case 2:
					// Tree
					image = (DrawPartialSprite(Convert.ToInt32(vWorld.X), Convert.ToInt32(vWorld.Y) - Convert.ToInt32(vTileSize.Y), 0 * Convert.ToInt32(vTileSize.X), 1 * Convert.ToInt32(vTileSize.Y), Convert.ToInt32(vTileSize.X), Convert.ToInt32(vTileSize.Y * 2)));
					break;
				case 3:
					// Spooky Tree
					image = (DrawPartialSprite(Convert.ToInt32(vWorld.X), Convert.ToInt32(vWorld.Y) - Convert.ToInt32(vTileSize.Y), 1 * Convert.ToInt32(vTileSize.X), 1 * Convert.ToInt32(vTileSize.Y), Convert.ToInt32(vTileSize.X), Convert.ToInt32(vTileSize.Y * 2)));
					break;
				case 4:
					// Beach
					image = (DrawPartialSprite(Convert.ToInt32(vWorld.X), Convert.ToInt32(vWorld.Y) - Convert.ToInt32(vTileSize.Y), 2 * Convert.ToInt32(vTileSize.X), 1 * Convert.ToInt32(vTileSize.Y), Convert.ToInt32(vTileSize.X), Convert.ToInt32(vTileSize.Y * 2)));
					break;
				case 5:
					// Water
					image = (DrawPartialSprite(Convert.ToInt32(vWorld.X), Convert.ToInt32(vWorld.Y) - Convert.ToInt32(vTileSize.Y), 3 * Convert.ToInt32(vTileSize.X), 1 * Convert.ToInt32(vTileSize.Y), Convert.ToInt32(vTileSize.X), Convert.ToInt32(vTileSize.Y * 2)));
					break;

			}
			images[id] = image;
			canvas.Children.CopyTo(uIElements,0);
			for (int i = 0; i < uIElements.Length; i++)
			{
				if (i == id)
				{
					uIElements[id] = image;
				}
			}
			canvas.Children.Clear();
			foreach (var item in uIElements)
			{
				canvas.Children.Add(item);
			}
			
		}

		private System.Windows.Controls.Image DrawPartialSprite(int locx, int locy, int px, int py, int width, int height)
		{
			CroppedBitmap cb = new CroppedBitmap(
							spritesB,
							new Int32Rect(1 * px, py, width, height));
			System.Windows.Controls.Image cbi = new System.Windows.Controls.Image
			{
				Width = cb.PixelWidth,
				Height = cb.PixelHeight,
				Source = cb,
			};
			RenderOptions.SetBitmapScalingMode(cbi, BitmapScalingMode.NearestNeighbor);
			Canvas.SetTop(cbi, locy);
			Canvas.SetLeft(cbi, locx);
			return cbi;
		}
		private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
		{
			using (MemoryStream outStream = new MemoryStream())
			{
				BitmapEncoder enc = new BmpBitmapEncoder();
				enc.Frames.Add(BitmapFrame.Create(bitmapImage));
				enc.Save(outStream);
				System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

				return new Bitmap(bitmap);
			}
		}

		System.Windows.Point GetMousePos()
		{
			return Mouse.GetPosition(canvas);
		}
		Vector ToScreen(int x, int y)
		{
			double a = (vOrigin.X * vTileSize.X) + (x - y) * (vTileSize.X / 2);
			double b = (vOrigin.Y * vTileSize.Y) + (x + y) * (vTileSize.Y / 2);
			return new Vector(a, b);
		}
		System.Windows.Controls.Image remove;
		//game loop
		private void CompositionTarget_Rendering(object sender, EventArgs e)
		{

			Vector vMouse = new Vector(GetMousePos().X, GetMousePos().Y);
			Vector vCell = new Vector(Math.Floor(vMouse.X / vTileSize.X), Math.Floor(vMouse.Y / vTileSize.Y));

			Vector vOffset = new Vector(vMouse.X % vTileSize.X, vMouse.Y % vTileSize.Y);


			vSelected = new Vector();
			vSelected.X = (vCell.Y - vOrigin.Y) + (vCell.X - vOrigin.X);
			vSelected.Y = (vCell.Y - vOrigin.Y) - (vCell.X - vOrigin.X);
			Color col = Color.White;
			if (vOffset.X > 0 && vOffset.Y > 0)
			{

				col = BitmapImage2Bitmap(spritesB).GetPixel(Convert.ToInt32(3 * vTileSize.X + vOffset.X), Convert.ToInt32(vOffset.Y));
				if (col == Color.FromArgb(255, 0, 0))
				{
					vSelected.X += -1;
					vSelected.Y += 0;
				}
				if (col == Color.FromArgb(0, 0, 255))
				{
					vSelected.X += 0;
					vSelected.Y += -1;
				}
				if (col == Color.FromArgb(0, 255, 0))
				{
					vSelected.X += 0;
					vSelected.Y += +1;
				}
				if (col == Color.FromArgb(255, 255, 0))
				{
					vSelected.X += +1;
					vSelected.Y += 0;
				}

			}





			Vector vSelectedWorld = ToScreen(Convert.ToInt32(vSelected.X), Convert.ToInt32(vSelected.Y));
			canvas.Children.Remove(remove);
			System.Windows.Controls.Image img = DrawPartialSprite(Convert.ToInt32(vSelectedWorld.X), Convert.ToInt32(vSelectedWorld.Y), 0, 0, Convert.ToInt32(vTileSize.X), Convert.ToInt32(vTileSize.Y));
			canvas.Children.Add(img);

			remove = img;

			info.Text = $"Mouse \t: {vMouse.X} {vMouse.Y}\nCell \t: {vCell.X} {vCell.Y}\nSelected\t:{vSelected.X} {vSelected.Y}";

		}

		private void clickevent(object sender, MouseButtonEventArgs e)
		{
			if (vSelected.X >= 0 && vSelected.X < vWorldsize.X && vSelected.Y >= 0 && vSelected.Y < vWorldsize.Y)
			{
				if (pWorld[Convert.ToInt32(vSelected.Y * vWorldsize.X + vSelected.X)] == 5)
				{
					pWorld[Convert.ToInt32(vSelected.Y * vWorldsize.X + vSelected.X)] = 0;
				}
				else
				{
					pWorld[Convert.ToInt32(vSelected.Y * vWorldsize.X + vSelected.X)]++;
				}
				UpdateUI(Convert.ToInt32(vSelected.Y * vWorldsize.X + vSelected.X), vSelected);
			}


			


		}
	}
}
