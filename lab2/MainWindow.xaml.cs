using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Windows.Media;
using MetadataExtractor;

namespace lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFolder(object sender, RoutedEventArgs e)
        {
            string[] formats = { ".jpg", ".gif", ".tif", ".bmp", ".png", ".pcx" };

            string path = "";
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = folderBrowserDialog.SelectedPath;
            }

            List<ImageInfo> infos = new List<ImageInfo>();
            foreach (string filename in System.IO.Directory.GetFiles(path))
            {
                FileInfo fileInfo = new FileInfo(filename);
                if (formats.Contains(fileInfo.Extension))
                {
                    Image img = Image.FromFile(filename);
                    string name = fileInfo.Name;
                    string size = $"{img.Width}X{img.Height}";
                    string DPI = $"{img.HorizontalResolution}X{img.VerticalResolution}";
                    string colorDepth = Image.GetPixelFormatSize(img.PixelFormat).ToString();
                    string compression = "";
                    IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(filename);
                    foreach (MetadataExtractor.Directory directory in directories)
                    {
                        foreach (Tag tag in directory.Tags)
                        {
                            if (tag.Name.Equals("Compression"))
                            {
                                compression = tag.Description;
                            }
                        }
                    }
                    infos.Add(new ImageInfo()
                    {
                        Name = name,
                        Size = size,
                        DPI = DPI,
                        ColorDepth = colorDepth,
                        Compression = compression
                    });
                }
            }
            InfoGrid.ItemsSource = infos;
        }
    }
}
