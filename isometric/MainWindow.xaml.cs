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

namespace isometric
{ 
	
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Vector vWorldsize = new Vector(14, 10);
		//using a 2x upscaled image. same issue happens without upscaling
		private Vector vTileSize = new Vector(80, 40);
		private Vector vOrigin = new Vector(5, 1);
		private BitmapImage spritesB;
		private int[] pWorld;
		
		public MainWindow()
		{

			spritesB = new BitmapImage(new Uri("pack://application:,,,/isometric_demo2.png"));
			pWorld = new int[Convert.ToInt32(vWorldsize.X * vWorldsize.Y)];
			Array.Clear(pWorld, 0, pWorld.Length);
			InitializeComponent();

			Updateui(0);
			CompositionTarget.Rendering += CompositionTarget_Rendering;


		}

		private void Updateui(int test)
		{
			
			canvas.Children.Clear();

			for (int y = 0; y < vWorldsize.Y; y++)
			{
				for (int x = 0; x < vWorldsize.X; x++)
				{
					Vector vWorld = ToScreen(x, y);
					switch (pWorld[y * Convert.ToInt32(vWorldsize.X) + x])
					{
						case 0:
							CroppedBitmap cb = new CroppedBitmap(
							spritesB,
							new Int32Rect(test * Convert.ToInt32(vTileSize.X), 0, Convert.ToInt32(vTileSize.X), Convert.ToInt32(vTileSize.Y)));
							Image cbi = new Image
							{
								Width = cb.PixelWidth,
								Height = cb.PixelHeight,
								Source = cb,
							};
							RenderOptions.SetBitmapScalingMode(cbi, BitmapScalingMode.NearestNeighbor);
							Canvas.SetTop(cbi, vWorld.Y);
							Canvas.SetLeft(cbi, vWorld.X);
							canvas.Children.Add(cbi);

							break;
						default:
							Console.WriteLine("Default case");
							break;
					}
				}
			}
		}

		Point GetMousePos()
		{
			return Mouse.GetPosition(canvas);
		}
		private Vector ToScreen(int x,int y)
		{
			double a = (vOrigin.X * vTileSize.X) + (x - y) * (vTileSize.X / 2);
			double b = (vOrigin.Y * vTileSize.Y) + (x + y) * (vTileSize.Y / 2);
			return new Vector(a, b);
		}

		//game loop
		private void CompositionTarget_Rendering(object sender, EventArgs e)
		{
			Vector vMouse = new Vector(GetMousePos().X, GetMousePos().Y);
			info.Text = $"mouse position is: {vMouse.X} {vMouse.Y}";
			
		}

		int hey = 0;
		private void clickevent(object sender, MouseButtonEventArgs e)
		{
			hey++;
			if (hey == 4)
			{
				hey = 0;
			}
				Updateui(hey);
			
			
		}
	}
}
