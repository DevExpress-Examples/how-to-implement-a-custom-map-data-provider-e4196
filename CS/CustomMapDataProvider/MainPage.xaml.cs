﻿using System;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Map;
using System.Globalization;

namespace CustomMapDataProvider {
    public partial class MainPage : UserControl {
        public MainPage() {
            InitializeComponent();
            imageTilesLayer.DataProvider = new CustomMapDataProvider();
        }

        public class CustomMapDataProvider : MapDataProviderBase {
            readonly SphericalMercatorProjection projection = new SphericalMercatorProjection();

            public override IProjection Projection { get { return projection; } }

            public CustomMapDataProvider() {
                SetTileSource(new CustomTileSource());
            }
            protected override MapDependencyObject CreateObject() {
                return new CustomMapDataProvider();
            }
            public override Size GetMapSizeInPixels(double zoomLevel) {
                return new Size(Math.Pow(2.0, zoomLevel) * OpenStreetMapTileSource.tileSize,
                    Math.Pow(2.0, zoomLevel) * OpenStreetMapTileSource.tileSize);
            }
        }

        public class CustomTileSource : MapTileSourceBase {
            const string roadUrlTemplate =
                @"http://{subdomain}.tile.openstreetmap.org/{tileLevel}/{tileX}/{tileY}.png";
            public const int maxZoomLevel = 20;
            public const int tileSize = 256;

            static int imageWidth = (int)Math.Pow(2.0, maxZoomLevel) * tileSize;
            static int imageHeight = (int)Math.Pow(2.0, maxZoomLevel) * tileSize;
            static string[] subdomains = new string[] { "a", "b", "c" };

            public CustomTileSource()
                : base(imageWidth, imageHeight, tileSize, tileSize) {

            }

            public override Uri GetTileByZoomLevel(int zoomLevel, int tilePositionX, int tilePositionY) {
                string url = roadUrlTemplate;
                url = url.Replace("{tileX}", tilePositionX.ToString(CultureInfo.InvariantCulture));
                url = url.Replace("{tileY}", tilePositionY.ToString(CultureInfo.InvariantCulture));
                url = url.Replace("{tileLevel}", zoomLevel.ToString(CultureInfo.InvariantCulture));
                url = url.Replace("{subdomain}", subdomains[GetSubdomainIndex(subdomains.Length)]);
                return new Uri(url);
            }
        }
    }
}
