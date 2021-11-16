// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Webp.Lossy;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;

namespace SixLabors.ImageSharp.Tests.Formats.Webp
{
    [Trait("Format", "Webp")]
    public class YuvConversionTests
    {
        [Theory]
        [WithFile(TestImages.Webp.Yuv, PixelTypes.Rgba32)]
        public void ConvertRgbToYuv_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            // arrange
            using Image<TPixel> image = provider.GetImage();
            Configuration config = image.GetConfiguration();
            MemoryAllocator memoryAllocator = config.MemoryAllocator;
            int pixels = image.Width * image.Height;
            int uvWidth = (image.Width + 1) >> 1;
            using System.Buffers.IMemoryOwner<byte> yBuffer = memoryAllocator.Allocate<byte>(pixels, AllocationOptions.Clean);
            using System.Buffers.IMemoryOwner<byte> uBuffer = memoryAllocator.Allocate<byte>(uvWidth * image.Height, AllocationOptions.Clean);
            using System.Buffers.IMemoryOwner<byte> vBuffer = memoryAllocator.Allocate<byte>(uvWidth * image.Height, AllocationOptions.Clean);
            Span<byte> y = yBuffer.GetSpan();
            Span<byte> u = uBuffer.GetSpan();
            Span<byte> v = vBuffer.GetSpan();
            byte[] expectedY =
            {
                82, 82, 82, 82, 128, 135, 134, 129, 167, 179, 176, 172, 192, 201, 200, 204, 188, 172, 175, 177, 168,
                151, 154, 154, 153, 152, 151, 151, 152, 160, 160, 160, 160, 82, 82, 82, 82, 140, 137, 135, 116, 174,
                183, 176, 162, 196, 199, 199, 210, 188, 166, 176, 181, 170, 145, 155, 154, 153, 154, 151, 151, 150,
                162, 159, 160, 160, 82, 82, 83, 82, 142, 139, 137, 117, 176, 184, 177, 164, 195, 199, 198, 210, 188,
                165, 175, 180, 169, 145, 155, 154, 153, 154, 152, 151, 150, 163, 160, 160, 160, 82, 82, 82, 82, 124,
                122, 120, 101, 161, 171, 165, 151, 197, 209, 208, 210, 197, 174, 183, 189, 175, 148, 158, 158, 155,
                151, 148, 148, 147, 159, 156, 156, 159, 128, 140, 142, 124, 189, 185, 183, 167, 201, 199, 198, 209,
                179, 165, 171, 179, 160, 145, 151, 152, 151, 154, 152, 151, 153, 164, 160, 160, 160, 170, 170, 170,
                169, 135, 137, 139, 122, 185, 182, 180, 165, 201, 200, 199, 210, 180, 166, 173, 180, 162, 145, 153,
                153, 151, 154, 151, 150, 152, 164, 160, 159, 159, 170, 170, 170, 170, 134, 135, 137, 120, 184, 180,
                177, 164, 200, 198, 196, 210, 181, 167, 174, 181, 163, 146, 155, 155, 153, 154, 152, 150, 152, 163,
                160, 159, 159, 167, 167, 167, 168, 129, 116, 117, 101, 167, 166, 164, 149, 205, 210, 209, 210, 191,
                177, 184, 191, 170, 149, 158, 159, 153, 151, 148, 146, 148, 159, 155, 155, 155, 170, 169, 170, 170,
                167, 174, 175, 161, 201, 201, 200, 204, 178, 173, 174, 185, 159, 148, 155, 158, 152, 152, 151, 150,
                153, 162, 159, 158, 160, 170, 169, 169, 168, 109, 122, 120, 129, 179, 183, 184, 171, 199, 200, 198,
                210, 172, 166, 170, 179, 155, 145, 150, 152, 149, 155, 152, 150, 155, 164, 161, 159, 162, 170, 170,
                170, 170, 92, 111, 109, 115, 176, 176, 177, 165, 198, 198, 196, 209, 174, 170, 173, 183, 159, 148,
                155, 156, 152, 154, 152, 150, 154, 163, 160, 158, 159, 166, 166, 168, 169, 98, 117, 116, 117, 172,
                162, 164, 152, 209, 210, 210, 210, 184, 179, 183, 192, 164, 151, 157, 159, 150, 150, 148, 146, 150,
                159, 155, 154, 157, 170, 169, 170, 170, 117, 136, 134, 123, 192, 196, 196, 197, 179, 180, 180, 191,
                159, 155, 159, 164, 153, 151, 152, 150, 154, 160, 157, 155, 160, 170, 166, 167, 165, 120, 134, 135,
                139, 69, 87, 86, 90, 201, 199, 199, 208, 165, 166, 167, 177, 148, 145, 148, 151, 150, 155, 153, 150,
                157, 165, 162, 159, 165, 170, 169, 170, 166, 84, 107, 108, 111, 49, 66, 64, 71, 200, 199, 198, 208,
                171, 173, 174, 184, 155, 150, 155, 157, 152, 153, 153, 149, 156, 163, 160, 157, 162, 167, 165, 169,
                167, 97, 121, 121, 125, 60, 77, 75, 76, 204, 210, 210, 210, 179, 180, 181, 191, 158, 152, 156, 159,
                150, 150, 149, 146, 152, 159, 156, 153, 160, 170, 169, 170, 170, 112, 135, 136, 138, 71, 88, 86, 79,
                188, 188, 188, 197, 160, 162, 163, 170, 152, 150, 152, 151, 154, 157, 156, 152, 160, 167, 164, 164,
                161, 135, 146, 150, 143, 77, 98, 99, 103, 51, 62, 60, 62, 172, 166, 165, 174, 145, 145, 145, 150,
                152, 155, 154, 150, 160, 165, 163, 159, 168, 170, 168, 170, 151, 80, 104, 109, 101, 44, 63, 63, 66,
                55, 52, 53, 51, 175, 176, 175, 183, 151, 153, 155, 158, 151, 152, 152, 148, 157, 161, 160, 156, 164,
                168, 165, 169, 156, 100, 122, 126, 118, 60, 79, 79, 81, 54, 52, 52, 51, 177, 181, 180, 188, 153,
                153, 155, 159, 149, 150, 150, 146, 155, 159, 157, 153, 164, 170, 169, 170, 170, 109, 131, 136, 127,
                66, 86, 86, 87, 46, 43, 43, 47, 168, 170, 169, 175, 151, 151, 152, 153, 153, 155, 154, 150, 160,
                164, 162, 160, 161, 151, 157, 165, 144, 88, 109, 114, 105, 55, 69, 68, 67, 62, 56, 56, 59, 151, 145,
                145, 148, 154, 154, 154, 150, 162, 164, 163, 159, 170, 170, 167, 170, 135, 80, 100, 110, 89, 41, 61,
                64, 59, 56, 53, 50, 50, 94, 85, 86, 79, 154, 155, 155, 158, 152, 152, 152, 148, 159, 161, 160, 155,
                166, 169, 165, 169, 146, 104, 122, 131, 110, 61, 80, 83, 75, 53, 53, 48, 47, 84, 74, 75, 75, 154,
                154, 154, 158, 151, 150, 150, 146, 158, 159, 158, 154, 167, 170, 169, 170, 153, 108, 127, 136, 113,
                63, 83, 87, 78, 48, 46, 43, 41, 81, 71, 72, 74, 153, 153, 153, 155, 153, 152, 152, 148, 160, 161,
                159, 157, 165, 165, 166, 170, 143, 101, 118, 127, 104, 60, 75, 78, 70, 56, 51, 48, 46, 85, 76, 77,
                81, 152, 154, 154, 151, 164, 164, 163, 159, 170, 170, 167, 170, 121, 84, 98, 114, 78, 44, 60, 68,
                52, 56, 53, 48, 56, 96, 85, 85, 83, 107, 105, 106, 100, 151, 151, 152, 148, 160, 160, 160, 155, 169,
                170, 166, 169, 134, 108, 121, 135, 98, 63, 79, 87, 69, 53, 53, 46, 50, 85, 73, 73, 71, 104, 95, 96,
                97, 151, 151, 151, 148, 160, 159, 159, 155, 169, 170, 170, 170, 137, 108, 121, 136, 99, 63, 78, 87,
                67, 51, 48, 43, 48, 85, 73, 72, 71, 105, 96, 97, 98, 152, 150, 150, 147, 160, 159, 159, 155, 169,
                170, 169, 170, 140, 111, 125, 139, 102, 67, 81, 87, 67, 50, 47, 41, 46, 83, 71, 71, 70, 103, 96, 96,
                98, 160, 162, 163, 159, 170, 170, 167, 170, 109, 91, 98, 117, 70, 49, 60, 72, 49, 55, 54, 46, 62,
                95, 84, 81, 85, 107, 104, 105, 103, 96, 98, 97, 100, 160, 159, 160, 156, 170, 170, 167, 169, 122,
                111, 118, 136, 87, 66, 77, 88, 62, 52, 53, 43, 56, 85, 74, 71, 76, 105, 95, 96, 96, 98, 100, 100,
                100, 160, 160, 160, 156, 170, 170, 167, 170, 120, 109, 116, 134, 86, 64, 75, 86, 60, 53, 51, 43, 56,
                86, 75, 72, 77, 106, 96, 97, 96, 97, 100, 100, 100, 160, 160, 160, 159, 169, 170, 168, 170, 129,
                115, 117, 123, 90, 71, 76, 79, 62, 51, 51, 47, 59, 79, 75, 74, 81, 100, 97, 98, 98, 100, 100, 100,
                100
            };
            byte[] expectedU =
            {
                90, 90, 59, 63, 36, 38, 23, 20, 34, 35, 47, 48, 70, 82, 104, 121, 121, 90, 90, 61, 69, 37, 42, 22,
                18, 33, 32, 47, 47, 67, 75, 97, 113, 120, 59, 61, 30, 37, 22, 20, 38, 36, 50, 50, 78, 83, 113, 122,
                142, 166, 164, 63, 69, 37, 43, 20, 18, 34, 32, 48, 47, 70, 73, 102, 110, 136, 166, 166, 36, 37, 22,
                20, 38, 35, 50, 49, 80, 80, 116, 119, 145, 165, 185, 197, 193, 38, 42, 20, 18, 35, 32, 48, 47, 72,
                72, 106, 108, 142, 165, 184, 191, 194, 23, 22, 38, 34, 50, 48, 81, 77, 117, 115, 150, 160, 184, 194,
                212, 220, 217, 20, 18, 36, 32, 49, 47, 76, 71, 111, 108, 148, 164, 185, 190, 208, 217, 219, 34, 33,
                50, 48, 80, 73, 116, 111, 150, 154, 184, 190, 213, 217, 226, 232, 232, 35, 32, 49, 47, 80, 72, 115,
                107, 154, 164, 187, 189, 211, 216, 228, 237, 235, 47, 46, 77, 70, 115, 106, 149, 148, 184, 187, 213,
                214, 226, 230, 227, 223, 224, 48, 47, 83, 73, 119, 108, 159, 164, 190, 189, 214, 216, 229, 236, 229,
                222, 220, 70, 67, 113, 101, 145, 142, 184, 185, 213, 211, 226, 229, 226, 226, 218, 211, 212, 82, 75,
                122, 110, 165, 165, 193, 190, 217, 216, 231, 236, 226, 222, 214, 208, 207, 104, 97, 142, 136, 186,
                184, 212, 208, 227, 228, 227, 229, 218, 214, 196, 185, 188, 121, 113, 166, 166, 197, 191, 220, 217,
                232, 237, 223, 222, 211, 208, 185, 173, 172, 121, 120, 164, 166, 193, 194, 217, 219, 232, 235, 224,
                220, 212, 207, 188, 172, 172
            };
            byte[] expectedV =
            {
                240, 240, 201, 206, 172, 174, 136, 136, 92, 90, 55, 50, 37, 30, 26, 23, 23, 240, 240, 204, 213, 173,
                179, 141, 141, 96, 98, 56, 54, 38, 31, 27, 25, 23, 201, 204, 164, 172, 129, 135, 82, 87, 46, 47, 33,
                29, 25, 23, 20, 16, 16, 206, 213, 172, 180, 137, 141, 93, 99, 54, 54, 36, 31, 26, 25, 21, 17, 16,
                172, 173, 129, 138, 81, 89, 45, 49, 32, 30, 24, 24, 19, 16, 42, 55, 51, 174, 179, 136, 141, 89, 99,
                51, 55, 35, 31, 26, 25, 21, 17, 39, 48, 52, 136, 141, 82, 92, 45, 51, 31, 32, 24, 24, 19, 17, 43,
                51, 74, 85, 81, 136, 141, 87, 99, 49, 55, 32, 32, 25, 25, 20, 17, 41, 46, 69, 81, 83, 92, 96, 46,
                53, 32, 34, 24, 25, 18, 18, 44, 47, 76, 81, 103, 117, 116, 90, 97, 48, 54, 30, 31, 24, 25, 18, 17,
                43, 46, 74, 80, 103, 118, 122, 55, 57, 33, 36, 24, 26, 19, 20, 44, 43, 76, 77, 102, 111, 138, 159,
                157, 50, 54, 30, 31, 24, 25, 17, 17, 47, 46, 77, 79, 106, 118, 143, 164, 168, 37, 38, 25, 26, 19,
                21, 43, 41, 75, 73, 103, 106, 138, 152, 174, 195, 194, 30, 31, 23, 25, 16, 17, 51, 46, 81, 79, 111,
                118, 151, 164, 188, 205, 206, 26, 27, 20, 21, 43, 39, 74, 69, 103, 102, 138, 143, 174, 188, 204,
                216, 218, 23, 25, 16, 17, 55, 48, 85, 81, 117, 118, 159, 164, 195, 205, 216, 227, 227, 23, 23, 16,
                16, 51, 52, 81, 83, 116, 122, 157, 168, 194, 206, 218, 227, 227
            };

            // act
            YuvConversion.ConvertRgbToYuv(image, config, memoryAllocator, y, u, v);

            // assert
            Assert.True(expectedY.AsSpan().SequenceEqual(y));
            Assert.True(expectedU.AsSpan().SequenceEqual(u.Slice(0, expectedU.Length)));
            Assert.True(expectedV.AsSpan().SequenceEqual(v.Slice(0, expectedV.Length)));
        }

        [Theory]
        [WithFile(TestImages.Png.TestPattern31x31HalfTransparent, PixelTypes.Rgba32)]
        public void ConvertRgbToYuv_WithAlpha_Works<TPixel>(TestImageProvider<TPixel> provider)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            // arrange
            using Image<TPixel> image = provider.GetImage();
            Configuration config = image.GetConfiguration();
            MemoryAllocator memoryAllocator = config.MemoryAllocator;
            int pixels = image.Width * image.Height;
            int uvWidth = (image.Width + 1) >> 1;
            using System.Buffers.IMemoryOwner<byte> yBuffer = memoryAllocator.Allocate<byte>(pixels, AllocationOptions.Clean);
            using System.Buffers.IMemoryOwner<byte> uBuffer = memoryAllocator.Allocate<byte>(uvWidth * image.Height, AllocationOptions.Clean);
            using System.Buffers.IMemoryOwner<byte> vBuffer = memoryAllocator.Allocate<byte>(uvWidth * image.Height, AllocationOptions.Clean);

            Span<byte> y = yBuffer.GetSpan();
            Span<byte> u = uBuffer.GetSpan();
            Span<byte> v = vBuffer.GetSpan();
            byte[] expectedY =
            {
                16, 16, 16, 16, 16, 235, 235, 235, 235, 235, 16, 16, 16, 16, 16, 152, 41, 41, 152, 152, 41, 41, 152,
                152, 41, 41, 152, 152, 41, 41, 152, 16, 16, 16, 16, 16, 235, 235, 235, 235, 235, 16, 16, 16, 16, 16,
                152, 41, 41, 152, 152, 41, 41, 152, 152, 41, 41, 152, 152, 41, 41, 152, 16, 16, 16, 16, 16, 235,
                235, 235, 235, 235, 16, 16, 16, 16, 16, 152, 41, 41, 152, 152, 41, 41, 152, 152, 41, 41, 152, 152,
                41, 41, 152, 16, 16, 16, 16, 16, 235, 235, 235, 235, 235, 16, 16, 16, 16, 16, 152, 41, 41, 152, 152,
                41, 41, 152, 152, 41, 41, 152, 152, 41, 41, 152, 16, 16, 16, 16, 16, 235, 235, 235, 235, 235, 16,
                16, 16, 16, 16, 152, 41, 41, 152, 152, 41, 41, 152, 152, 41, 41, 152, 152, 41, 41, 152, 235, 235,
                235, 235, 235, 16, 16, 16, 16, 16, 235, 235, 235, 235, 235, 152, 41, 41, 152, 152, 41, 41, 152, 152,
                41, 41, 152, 152, 41, 41, 152, 235, 235, 235, 235, 235, 16, 16, 16, 16, 16, 235, 235, 235, 235, 235,
                152, 41, 41, 152, 152, 41, 41, 152, 152, 41, 41, 152, 152, 41, 41, 152, 235, 235, 235, 235, 235, 16,
                16, 16, 16, 16, 235, 235, 235, 235, 235, 152, 41, 41, 152, 152, 41, 41, 152, 152, 41, 41, 152, 152,
                41, 41, 152, 235, 235, 235, 235, 235, 16, 16, 16, 16, 16, 235, 235, 235, 235, 235, 152, 41, 41, 152,
                152, 41, 41, 152, 152, 41, 41, 152, 152, 41, 41, 152, 235, 235, 235, 235, 235, 16, 16, 16, 16, 16,
                235, 235, 235, 235, 235, 152, 41, 41, 152, 152, 41, 41, 152, 152, 41, 41, 152, 152, 41, 41, 152, 16,
                16, 16, 16, 16, 235, 235, 235, 235, 235, 16, 16, 16, 16, 16, 152, 41, 41, 152, 152, 41, 41, 152,
                152, 41, 41, 152, 152, 41, 41, 152, 16, 16, 16, 16, 16, 235, 235, 235, 235, 235, 16, 16, 16, 16, 16,
                152, 41, 41, 152, 152, 41, 41, 152, 152, 41, 41, 152, 152, 41, 41, 152, 16, 16, 16, 16, 16, 235,
                235, 235, 235, 235, 16, 16, 16, 16, 16, 152, 41, 41, 152, 152, 41, 41, 152, 152, 41, 41, 152, 152,
                41, 41, 152, 16, 16, 16, 16, 16, 235, 235, 235, 235, 235, 16, 16, 16, 16, 16, 152, 41, 41, 152, 152,
                41, 41, 152, 152, 41, 41, 152, 152, 41, 41, 152, 16, 16, 16, 16, 16, 235, 235, 235, 235, 235, 16,
                16, 16, 16, 16, 152, 41, 41, 152, 152, 41, 41, 152, 152, 41, 41, 152, 152, 41, 41, 152, 82, 82, 82,
                82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 81, 158, 170, 118, 130, 182, 65, 142, 220, 103, 155,
                167, 115, 127, 204, 127, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 145, 157, 106,
                118, 170, 118, 130, 207, 90, 142, 154, 103, 114, 192, 115, 127, 82, 82, 82, 82, 82, 82, 82, 82, 82,
                82, 82, 82, 82, 82, 82, 145, 93, 105, 157, 105, 117, 195, 78, 130, 142, 90, 102, 179, 102, 114, 192,
                82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 80, 92, 170, 93, 105, 182, 65, 142, 129,
                77, 155, 167, 90, 102, 179, 62, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 82, 80, 157,
                80, 92, 170, 52, 130, 117, 65, 142, 154, 102, 89, 166, 49, 127, 82, 82, 82, 82, 82, 82, 82, 82, 82,
                82, 82, 82, 82, 82, 82, 145, 197, 80, 157, 169, 117, 104, 181, 130, 142, 90, 77, 154, 37, 114, 191,
                81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 209, 67, 144, 156, 105, 117, 169, 117,
                129, 206, 64, 141, 153, 102, 179, 166, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81,
                55, 132, 144, 92, 169, 156, 104, 116, 194, 77, 129, 141, 89, 166, 178, 101, 81, 81, 81, 81, 81, 81,
                81, 81, 81, 81, 81, 81, 81, 81, 81, 119, 131, 80, 157, 144, 92, 104, 181, 64, 116, 193, 76, 154,
                166, 89, 101, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 119, 67, 144, 156, 79, 91,
                169, 52, 104, 181, 64, 141, 153, 76, 88, 165, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81,
                81, 81, 183, 132, 144, 196, 79, 156, 39, 116, 168, 51, 129, 141, 89, 76, 153, 101, 81, 81, 81, 81,
                81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 81, 119, 131, 183, 66, 143, 155, 104, 156, 168, 116, 128,
                205, 63, 140, 218, 101, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 119, 196, 54,
                131, 208, 91, 143, 155, 103, 115, 193, 51, 128, 205, 88, 165, 41, 41, 41, 41, 41, 41, 41, 41, 41,
                41, 41, 41, 41, 41, 41, 183, 41, 118, 196, 79, 156, 143, 91, 103, 180, 63, 115, 193, 75, 153, 165,
                41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 29, 106, 183, 66, 143, 130, 78, 90, 168,
                116, 103, 180, 63, 140, 152, 75, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 41, 93,
                171, 53, 131, 118, 66, 78, 155, 103, 90, 167, 50, 128, 140, 63, 75
            };
            byte[] expectedU =
            {
                128, 128, 128, 128, 128, 128, 128, 139, 240, 139, 240, 139, 240, 139, 240, 139, 128, 128, 128, 128,
                128, 128, 128, 139, 240, 139, 240, 139, 240, 139, 240, 139, 128, 128, 128, 128, 128, 128, 128, 139,
                240, 139, 240, 139, 240, 139, 240, 139, 128, 128, 128, 128, 128, 128, 128, 139, 240, 139, 240, 139,
                240, 139, 240, 139, 128, 128, 128, 128, 128, 128, 128, 139, 240, 139, 240, 139, 240, 139, 240, 139,
                128, 128, 128, 128, 128, 128, 128, 139, 240, 139, 240, 139, 240, 139, 240, 139, 128, 128, 128, 128,
                128, 128, 128, 139, 240, 139, 240, 139, 240, 139, 240, 139, 112, 112, 108, 106, 106, 112, 112, 139,
                229, 146, 204, 132, 199, 131, 204, 135, 90, 90, 90, 90, 90, 90, 90, 100, 161, 92, 116, 141, 99, 155,
                113, 97, 90, 90, 90, 90, 90, 90, 90, 173, 145, 114, 173, 122, 133, 127, 96, 170, 96, 96, 96, 96, 96,
                96, 96, 148, 98, 134, 122, 113, 139, 93, 169, 85, 91, 91, 91, 91, 91, 91, 91, 108, 134, 130, 112,
                149, 105, 139, 146, 110, 91, 91, 91, 91, 91, 91, 91, 107, 164, 117, 149, 127, 128, 166, 107, 129,
                159, 159, 159, 159, 159, 159, 159, 161, 112, 113, 138, 87, 143, 112, 88, 161, 240, 240, 240, 240,
                240, 240, 240, 137, 110, 162, 110, 140, 158, 104, 159, 137, 240, 240, 240, 240, 240, 240, 240, 109,
                150, 108, 140, 161, 80, 157, 162, 128
            };
            byte[] expectedV =
            {
                128, 128, 128, 128, 128, 128, 128, 189, 110, 189, 110, 189, 110, 189, 110, 189, 128, 128, 128, 128,
                128, 128, 128, 189, 110, 189, 110, 189, 110, 189, 110, 189, 128, 128, 128, 128, 128, 128, 128, 189,
                110, 189, 110, 189, 110, 189, 110, 189, 128, 128, 128, 128, 128, 128, 128, 189, 110, 189, 110, 189,
                110, 189, 110, 189, 128, 128, 128, 128, 128, 128, 128, 189, 110, 189, 110, 189, 110, 189, 110, 189,
                128, 128, 128, 128, 128, 128, 128, 189, 110, 189, 110, 189, 110, 189, 110, 189, 128, 128, 128, 128,
                128, 128, 128, 189, 110, 189, 110, 189, 110, 189, 110, 189, 175, 175, 186, 193, 193, 175, 175, 188,
                109, 172, 108, 164, 119, 152, 92, 189, 240, 240, 240, 240, 240, 240, 240, 104, 121, 131, 142, 135,
                109, 92, 146, 115, 240, 240, 240, 240, 240, 240, 240, 122, 136, 148, 137, 113, 157, 155, 121, 130,
                155, 155, 155, 155, 155, 155, 155, 109, 135, 96, 88, 142, 136, 105, 138, 116, 81, 81, 81, 81, 81,
                81, 81, 143, 104, 148, 150, 111, 138, 128, 116, 141, 81, 81, 81, 81, 81, 81, 81, 63, 147, 133, 119,
                141, 165, 126, 147, 173, 101, 101, 101, 101, 101, 101, 101, 135, 109, 129, 122, 124, 107, 108, 128,
                138, 110, 110, 110, 110, 110, 110, 110, 117, 137, 151, 127, 114, 131, 139, 142, 120, 110, 110, 110,
                110, 110, 110, 110, 142, 156, 119, 137, 167, 141, 151, 66, 85
            };

            // act
            YuvConversion.ConvertRgbToYuv(image, config, memoryAllocator, y, u, v);

            // assert
            Assert.True(expectedY.AsSpan().SequenceEqual(y));
            Assert.True(expectedU.AsSpan().SequenceEqual(u.Slice(0, expectedU.Length)));
            Assert.True(expectedV.AsSpan().SequenceEqual(v.Slice(0, expectedV.Length)));
        }
    }
}