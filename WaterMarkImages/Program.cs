using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Drawing.Processing;
using System;
using System.IO;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace WatermarkApp
{
    public class Watermarker
    {
        private string _watermarkText;
        private float _fontSize;
        private FontStyle _fontStyle;
        private string _fontName;

        public Watermarker(string watermarkText, float fontSize, FontStyle fontStyle, string fontName = "Arial")
        {
            _watermarkText = watermarkText;
            _fontSize = fontSize;
            _fontStyle = fontStyle;
            _fontName = fontName;
        }

        public void AddWatermark(Image image, PointF location)
        {
            var fontCollection = new FontCollection();
            var fontFamily = fontCollection.Add(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), $"{_fontName}.ttf"));
            var font = fontFamily.CreateFont(_fontSize, _fontStyle);

            image.Mutate(ctx => ctx.DrawText(_watermarkText, font, Color.White, location));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: WatermarkApp <inputDirectory> <outputDirectory> <copyrightText>");
                return;
            }

            string inputDir = args[0];
            string outputDir = args[1];
            string copyrightText = args[2];
            float fontSize = 24; // Example font size, adjust as needed

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            Watermarker watermarker = new Watermarker(copyrightText, fontSize, FontStyle.Italic);

            foreach (var imagePath in Directory.EnumerateFiles(inputDir))
            {
                using (var image = Image.Load(imagePath))
                {
                    var location = new PointF(10, image.Height - 30); // Example position, adjust as needed
                    watermarker.AddWatermark(image, location);
                    var outputPath = Path.Combine(outputDir, Path.GetFileName(imagePath));
                    image.SaveAsJpeg(outputPath); // Adjust format as needed
                }
            }
        }
    }
}
