// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Pbm;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Qoi;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Webp;

namespace SixLabors.ImageSharp;

/// <summary>
/// Provides a lazily initialized configuration which allows altering default behaviour or extending the library.
/// </summary>
internal static class LazyConfiguration
{
    /// <summary>
    /// A lazily initialized configuration default instance.
    /// </summary>
    internal static readonly Lazy<Configuration> Lazy = new(CreateDefaultInstance);

    /// <summary>
    /// Creates the default instance with the following <see cref="IImageFormatConfigurationModule"/>s preregistered:
    /// <see cref="PngConfigurationModule"/>
    /// <see cref="JpegConfigurationModule"/>
    /// <see cref="GifConfigurationModule"/>
    /// <see cref="BmpConfigurationModule"/>.
    /// <see cref="PbmConfigurationModule"/>.
    /// <see cref="TgaConfigurationModule"/>.
    /// <see cref="TiffConfigurationModule"/>.
    /// <see cref="WebpConfigurationModule"/>.
    /// <see cref="QoiConfigurationModule"/>.
    /// </summary>
    /// <returns>The default configuration of <see cref="Configuration"/>.</returns>
    internal static Configuration CreateDefaultInstance() => new(
            new PngConfigurationModule(),
            new JpegConfigurationModule(),
            new GifConfigurationModule(),
            new BmpConfigurationModule(),
            new PbmConfigurationModule(),
            new TgaConfigurationModule(),
            new TiffConfigurationModule(),
            new WebpConfigurationModule(),
            new QoiConfigurationModule());
}
