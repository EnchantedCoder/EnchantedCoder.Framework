using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Havit.Drawing
{
	/// <summary>
	/// T��da s metodami pro rychlou pr�ci s obr�zky.
	/// </summary>
	public static class BitmapExt
	{
		#region RotateFlip
		/// <summary>
		/// Metoda rotuje a/nebo p�ekl�p� obr�zek.
		/// </summary>
		/// <param name="sourceFilename">Zdrojov� obr�zek.</param>
		/// <param name="destinationFilename">C�lov� obr�zek.</param>
		/// <param name="rotateFlipType"><see cref="System.Drawing.RotateFlipType"/> ur�uj�c� sm�r oto�en� a/nebo p�eklopen�.</param>
		public static void RotateFlip(string sourceFilename, string destinationFilename, RotateFlipType rotateFlipType)
		{
			if (String.Compare(sourceFilename, destinationFilename, true) == 0)
			{
				// pro stejn� zdroj a c�l mus�me pou��t modifikovanou verzi
				RotateFlip(sourceFilename, rotateFlipType);
			}
			else
			{
				// pro r�zn� n�zvy soubor� je rychlej�� toto
				using (Image image = Image.FromFile(sourceFilename))
				{
					image.RotateFlip(rotateFlipType);
					image.Save(destinationFilename);
				}
			}
		}

		/// <summary>
		/// Metoda rotuje a/nebo p�ekl�p� obr�zek.
		/// </summary>
		/// <remarks>
		/// Tato varianta implementace obch�z� nemo�nost z�pisu do otev�en�ho obr�zku jeho vykop�rov�n�m,
		/// uzav�en�m a ulo�en�m. Je proto o malinko pomalej�� ne� vytvo�en� c�lov�ho obr�zku do vedlej��ho souboru.
		/// </remarks>
		/// <param name="filename">obr�zek</param>
		/// <param name="rotateFlipType"><see cref="System.Drawing.RotateFlipType"/> ur�uj�c� sm�r oto�en� a/nebo p�eklopen�.</param>
		public static void RotateFlip(string filename, RotateFlipType rotateFlipType)
		{
			Bitmap imageCopy;
			using (Image image = Image.FromFile(filename))
			{
				// zkop�rujeme obr�zek do nov� bitmapy
				imageCopy = new Bitmap(image);
			}
			imageCopy.RotateFlip(rotateFlipType);
			imageCopy.Save(filename);
			imageCopy.Dispose();
		}
		#endregion

		#region Resize
		/// <summary>
		/// Zm�n� rozm�ry obr�zku.
		/// </summary>
		/// <remarks>
		/// Jako vedlej�� efekt lze zm�nit i form�t obr�zku, pokud zvol�me jinou c�lovou p��ponu.
		/// </remarks>
		/// <param name="sourceFilename">zdrojov� obr�zek (n�zev souboru v�etn� cesty)</param>
		/// <param name="destinationFilename">c�lov� obr�zek (n�zev souboru v�etn� cesty)</param>
		/// <param name="width">c�lov� ���ka</param>
		/// <param name="height">c�lov� v��ka</param>
		/// <param name="resizeMode"><see cref="ResizeMode"/> mo�nost ur�uj�c� re�im</param>
		/// <param name="quality">kvalita p�evodu a v�sledn�ho obr�zku 0-100</param>
		/// <returns><see cref="Size"/> s rozm�ry v�sledn�ho obr�zku</returns>
		public static Size Resize(
			string sourceFilename,
			string destinationFilename,
			int width,
			int height,
			ResizeMode resizeMode,
			int quality
		)
		{
			Bitmap originalBitmap;
			using (Image image = Image.FromFile(sourceFilename))
			{
				// na�teme si obr�zek do bitmapy, abychom mohli zav��t soubor
				originalBitmap = new Bitmap(image);
			}

			Size destinationSize = new Size(width, height);
			switch (resizeMode)
			{
				case ResizeMode.PreserveAspectRatioFitBox:
					if ((originalBitmap.Width / originalBitmap.Height) >= (width / height))
					{
						destinationSize.Height = (int)Math.Round(originalBitmap.Height * (width * 1.0 / originalBitmap.Width));
					}
					else
					{
						destinationSize.Width = (int)Math.Round(originalBitmap.Width * (height * 1.0 / originalBitmap.Height));
					}
					break;

				case ResizeMode.PreserveAspectRatioFitBoxReduceOnly:
					if ((originalBitmap.Width > width) || (originalBitmap.Height > height))
					{
						if ((originalBitmap.Width * 1.0 / originalBitmap.Height) >= (width * 1.0 / height))
						{
							destinationSize.Height = (int)Math.Round(originalBitmap.Height * (width * 1.0 / originalBitmap.Width));
						}
						else
						{
							destinationSize.Width = (int)Math.Round(originalBitmap.Width * (height * 1.0 / originalBitmap.Height));
						}
					}
					else
					{
						destinationSize.Width = originalBitmap.Width;
						destinationSize.Height = originalBitmap.Height;

						if (String.Compare(Path.GetFullPath(destinationFilename), Path.GetFullPath(sourceFilename), true) == 0)
						{
							// soubor je v po��dku
							return destinationSize;
						}
						else if (String.Compare(Path.GetExtension(sourceFilename), Path.GetExtension(destinationFilename), true) == 0)
						{
							// zrychluj�c� zkratka - nic se nem�n�, typ souboru stejn�, tak�e jenom zkop�rujem
							File.Copy(sourceFilename, destinationFilename);
							return destinationSize;
						}
					}
					break;

				case ResizeMode.AdjustToBox:
					// nastaven� vyhovuje
					break;

				default:
					throw new ArgumentException("Nerozpoznan� ResizeMode", "resizeMode");
			}

			using (Bitmap destinationBitmap = new Bitmap(destinationSize.Width, destinationSize.Height))
			{
				using (Graphics destinationBitmapGraphics = Graphics.FromImage(destinationBitmap))
				{
					if (quality >= 75)
					{
						destinationBitmapGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
						destinationBitmapGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
						destinationBitmapGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
						destinationBitmapGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
					}
					
					destinationBitmapGraphics.DrawImage(originalBitmap, 0, 0, destinationSize.Width, destinationSize.Height);
				}

				bool saved = false;
				if ( (String.Compare(Path.GetExtension(destinationFilename), ".jpg", true) == 0)
					|| (String.Compare(Path.GetExtension(destinationFilename), ".jpeg", true) == 0) )
				{
					ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
					ImageCodecInfo jpegEncoder = null;
					for (int i = 0; i < encoders.Length; i++)
					{
						if (encoders[i].FormatID.Equals(ImageFormat.Jpeg.Guid))
						{
							jpegEncoder = encoders[i];
							break;
						}
					}
					if (jpegEncoder != null)
					{
						EncoderParameters p = new EncoderParameters(1);
						p.Param[0] = new EncoderParameter(Encoder.Quality, quality);
						destinationBitmap.Save(destinationFilename, jpegEncoder, p);
						saved = true;
					}
				}

				if (!saved)
				{
					destinationBitmap.Save(destinationFilename);
				}
			}

			return destinationSize;
		}

		/// <summary>
		/// Zm�n� rozm�ry obr�zku.
		/// </summary>
		/// <remarks>
		/// Jako vedlej�� efekt lze zm�nit i form�t obr�zku, pokud zvol�me jinou c�lovou p��ponu.
		/// </remarks>
		/// <param name="filename">obr�zek (n�zev souboru v�etn� cesty)</param>
		/// <param name="width">c�lov� ���ka</param>
		/// <param name="height">c�lov� v��ka</param>
		/// <param name="resizeMode"><see cref="ResizeMode"/> mo�nost ur�uj�c� re�im</param>
		/// <param name="quality">kvalita p�evodu a v�sledn�ho obr�zku 0-100</param>
		/// <returns><see cref="Size"/> s rozm�ry v�sledn�ho obr�zku</returns>
		public static Size Resize(string filename, int width, int height, ResizeMode resizeMode, int quality)
		{
			return Resize(filename, filename, width, height, resizeMode, quality);
		}

		/// <summary>
		/// Zm�n� rozm�ry obr�zku.
		/// Pozor, pou��v� default-quality 75.
		/// </summary>
		/// <param name="sourceFilename">zdrojov� obr�zek (n�zev souboru v�etn� cesty)</param>
		/// <param name="destinationFilename">c�lov� obr�zek (n�zev souboru v�etn� cesty)</param>
		/// <param name="width">c�lov� ���ka</param>
		/// <param name="height">c�lov� v��ka</param>
		/// <param name="resizeMode"><see cref="ResizeMode"/> mo�nost ur�uj�c� re�im</param>
		/// <returns><see cref="Size"/> s rozm�ry v�sledn�ho obr�zku</returns>
		public static Size Resize(string sourceFilename, string destinationFilename, int width, int height, ResizeMode resizeMode)
		{
			return Resize(sourceFilename, destinationFilename, width, height, resizeMode, 75);
		}

		/// <summary>
		/// Zm�n� rozm�ry obr�zku.
		/// Pozor, pou��v� default-quality 75.
		/// </summary>
		/// <param name="filename">obr�zek (n�zev souboru v�etn� cesty)</param>
		/// <param name="width">c�lov� ���ka</param>
		/// <param name="height">c�lov� v��ka</param>
		/// <param name="resizeMode"><see cref="ResizeMode"/> mo�nost ur�uj�c� re�im</param>
		/// <returns><see cref="Size"/> s rozm�ry v�sledn�ho obr�zku</returns>
		public static Size Resize(string filename, int width, int height, ResizeMode resizeMode)
		{
			return Resize(filename, filename, width, height, resizeMode, 75);
		}
		#endregion
	}
}
